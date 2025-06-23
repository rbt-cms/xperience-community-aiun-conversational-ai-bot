﻿using System.Net;
using System.Net.Sockets;

using CMS;
using CMS.Base;
using CMS.ContentEngine;
using CMS.Core;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.Websites;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using XperienceCommunity.AIUN.ConversationalAIBot;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.InfoClasses.AIUNRegistration;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.IManagers;

[assembly: RegisterModule(typeof(ContentChangeEventHandler))]

namespace XperienceCommunity.AIUN.ConversationalAIBot
{
    public class ContentChangeEventHandler : Module
    {
        private IServiceProvider services = null!;
        public ContentChangeEventHandler()
            : base(nameof(ContentChangeEventHandler))
        {
        }

        protected override void OnInit(ModuleInitParameters parameters)
        {
            services = parameters.Services;
            base.OnInit();

            // Attach to relevant events  
            WebPageEvents.Publish.Execute += (sender, e) => HandleWebPagePublish(e);
        }

        private void HandleWebPagePublish(CMSEventArgs e)
        {
            if (e is not WebPageEventArgsBase pageEvent)
            {
                return;
            }
            using var scope = services.CreateScope();
            var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
            var eventLogService = scope.ServiceProvider.GetRequiredService<IEventLogService>();

            // Capture request context early (inside the current thread where HttpContext exists)  
            var request = httpContextAccessor.HttpContext?.Request;
            string scheme = request?.Scheme ?? string.Empty;
            var host = request?.Host ?? new();
            string websiteChannelName = pageEvent.WebsiteChannelName;
            string pagePath = pageEvent.TreePath;
            string culture = pageEvent.ContentLanguageName;
            int userId = MembershipContext.AuthenticatedUser?.UserID ?? 0;
            string userName = MembershipContext.AuthenticatedUser?.UserName ?? "Unknown";
            string ipAddress = RequestContext.UserHostAddress ?? "Unknown";

            try
            {
                var handler = scope.ServiceProvider.GetRequiredService<ContentChangeEventHandlerWorker>();
                handler.ProcessAsync(websiteChannelName, pagePath, culture, scheme, host, userId, userName, ipAddress);
            }
            catch (Exception ex)
            {
                eventLogService.LogEvent(new EventLogData(EventTypeEnum.Error, nameof(ContentChangeEventHandler),
                   nameof(HandleWebPagePublish))
                {
                    EventDescription = $"Failed to upload URLs at {DateTime.Now}.\n" +
                                      $"Exception: {ex.Message}\n",
                    UserID = userId,
                    UserName = userName,
                    IPAddress = ipAddress
                });
            }
        }
        public static IEnumerable<string> GetWebPageRelativeUrls(IEnumerable<int> pageIdentifiers, string languageName, CancellationToken cancellationToken)
        {

            using var scope = Service.Resolve<IServiceScopeFactory>().CreateScope();
            var _urlRetriever = scope.ServiceProvider.GetRequiredService<IWebPageUrlRetriever>();
            var relativeUrls = new List<string>();

            try
            {
                foreach (int pageIdentifier in pageIdentifiers)
                {

                    var webPageUrl = _urlRetriever.Retrieve(pageIdentifier, languageName, false, cancellationToken).GetAwaiter().GetResult();
                    relativeUrls.Add(webPageUrl.RelativePath.TrimStart('~'));
                }
            }
            catch (Exception ex)
            {
                var _eventLogService = scope.ServiceProvider.GetRequiredService<IEventLogService>();
                _eventLogService.LogException(nameof(ContentChangeEventHandler), nameof(GetWebPageRelativeUrls), ex, null);
            }

            return relativeUrls;
        }
    }

    public class ContentChangeEventHandlerWorker
    {
        private readonly IAiunApiManager syncLogs;
        private readonly IDefaultChatbotManager chatbotManager;
        private readonly IEventLogService eventLog;
        private readonly IInfoProvider<AIUNRegistrationInfo> aIUNRegistrationInfo;
        public ContentChangeEventHandlerWorker(
            IAiunApiManager syncLogs,
            IDefaultChatbotManager chatbotManager,
            IEventLogService eventLog,
            IInfoProvider<AIUNRegistrationInfo> aIUNRegistrationInfo
             )
        {
            this.syncLogs = syncLogs;
            this.chatbotManager = chatbotManager;
            this.eventLog = eventLog;
            this.aIUNRegistrationInfo = aIUNRegistrationInfo;
        }


        public void ProcessAsync(string websiteChannelName, string pagePath, string culture, string scheme, HostString hostString, int userId, string userName, string ipAddress)
        {
            try
            {
                using var scope = Service.Resolve<IServiceScopeFactory>().CreateScope();
                if (ipAddress is "::1" or "127.0.0.1")
                {
                    try
                    {
                        var host = Dns.GetHostEntryAsync(Dns.GetHostName()).GetAwaiter().GetResult();
                        foreach (var ip in host.AddressList)
                        {
                            if (ip.AddressFamily == AddressFamily.InterNetwork)
                            {
                                ipAddress = ip.ToString();
                                break;
                            }
                        }
                    }
                    catch
                    {
                        ipAddress = "localhost";
                    }
                }
                ipAddress ??= "Unknown";

                var builder = new ContentItemQueryBuilder();
                _ = builder.ForContentTypes(p => p.ForWebsite(websiteChannelName, PathMatch.Single(pagePath)));
                var contentQueryExecutor = scope.ServiceProvider.GetRequiredService<IContentQueryExecutor>();
                var urlRetriever = scope.ServiceProvider.GetRequiredService<IWebPageUrlRetriever>();
                var pageIdentifiers = contentQueryExecutor.GetWebPageResult(
                    builder, i => i.WebPageItemID, new ContentQueryExecutionOptions(), CancellationToken.None).GetAwaiter().GetResult();

                var relativeUrls = new List<string>();
                foreach (int id in pageIdentifiers)
                {
                    var url = urlRetriever.Retrieve(id, culture, false).GetAwaiter().GetResult();
                    relativeUrls.Add(url.RelativePath.TrimStart('~'));
                }

                var absoluteUrls = chatbotManager.GetAbsoluteUrls(relativeUrls, scheme, hostString).GetAwaiter().GetResult();
                string clientID = chatbotManager.GetClientIDWIthChannelName(websiteChannelName);
                string? securityToken = aIUNRegistrationInfo.Get()?.FirstOrDefault()?.APIKey ?? string.Empty;
                _ = syncLogs.UploadURLsAsync(absoluteUrls.ToList(), clientID, securityToken ?? string.Empty);
                eventLog.LogEvent(new EventLogData(EventTypeEnum.Information, "ContentChangeEventHandler", "UploadURLsAsync")
                {
                    EventDescription = $"Uploaded URLs successfully at {DateTime.Now}.\n" +
                                       $"URLs: {string.Join("\n", absoluteUrls.ToList())}\n\n",
                    UserID = userId,
                    UserName = userName,
                    IPAddress = ipAddress
                });
            }
            catch (Exception ex)
            {
                eventLog.LogEvent(new EventLogData(EventTypeEnum.Error, nameof(ContentChangeEventHandler),
                    "Upload Failed")
                {
                    EventDescription = $"Failed to upload URLs at {DateTime.Now}.\n" +
                                       $"Exception: {ex.Message}\n",
                    UserID = userId,
                    UserName = userName,
                    IPAddress = ipAddress
                });
            }
        }
    }
}


