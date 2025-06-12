
using System.Net;
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

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.IManagers;


[assembly: RegisterModule(typeof(XperienceCommunity.AIUN.ConversationalAIBot.ContentChangeEventHandler))]


namespace XperienceCommunity.AIUN.ConversationalAIBot
{
    public class ContentChangeEventHandler : Module
    {
        public ContentChangeEventHandler()
            : base(nameof(ContentChangeEventHandler))
        {
        }

        protected override void OnInit()
        {
            base.OnInit();

            // Attach to relevant events
            WebPageEvents.Publish.Execute += HandleWebPagePublish;

        }

        private void HandleWebPagePublish(object? sender, CMSEventArgs e)
        {
            if (e is not WebPageEventArgsBase pageEvent)
            {
                return;
            }

            _ = LogPageInfo(pageEvent);

        }


        private async Task LogPageInfo(WebPageEventArgsBase pageEvent)
        {
            using var scope = Service.Resolve<IServiceScopeFactory>().CreateScope();
            string pagePath = pageEvent.TreePath;
            string websiteChannelName = pageEvent.WebsiteChannelName;

            // Fetch user and request info
            var currentUser = MembershipContext.AuthenticatedUser;
            int userId = currentUser?.UserID ?? 0;
            string userName = currentUser?.UserName ?? "Unknown";
            string ipAddress = RequestContext.UserHostAddress ?? "Unknown";

            if (ipAddress is "::1" or "127.0.0.1")
            {
                try
                {
                    var host = await Dns.GetHostEntryAsync(Dns.GetHostName());
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

            try
            {

                var syncLogs = scope.ServiceProvider.GetRequiredService<IAiunApiManager>();
                var chatbotManager = scope.ServiceProvider.GetRequiredService<IDefaultChatbotManager>();
                var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
                var contentQueryExecutor = scope.ServiceProvider.GetRequiredService<IContentQueryExecutor>();


                if (!string.IsNullOrWhiteSpace(pagePath))
                {
                    var relativeUrls = new List<string>();
                    var options = new ContentQueryExecutionOptions
                    {
                        ForPreview = false,
                        IncludeSecuredItems = false,

                    };

                    var builder = new ContentItemQueryBuilder();
                    _ = builder.ForContentTypes(parameters =>
                        parameters.ForWebsite(
                            websiteChannelName: websiteChannelName,
                            pathMatch: PathMatch.Single(pagePath)
                        ));

                    var cancellationToken = CancellationToken.None;
                    var pageIdentifiers = await contentQueryExecutor.GetWebPageResult(builder, i => i.WebPageItemID, options, cancellationToken);
                    var languageUrls = await GetWebPageRelativeUrls(pageIdentifiers, "en", cancellationToken);

                    relativeUrls.AddRange(languageUrls);


                    var request = httpContextAccessor.HttpContext?.Request;
                    string scheme = request?.Scheme ?? string.Empty;
                    var host = request?.Host ?? new();
                    var absoluteUrls = await chatbotManager.GetAbsoluteUrls(relativeUrls, scheme, host);
                    string clientID = chatbotManager.GetClientIDWIthChannelName(websiteChannelName);

                    _ = await syncLogs.UploadURLsAsync(absoluteUrls.ToList(), clientID);

                    // Log with user details
                    Service.Resolve<IEventLogService>().LogEvent(new EventLogData(
                        EventTypeEnum.Information,
                        "ContentChangeEventHandler",
                        "Upload Success")
                    {
                        EventDescription = $"Uploaded URLs successfully at {DateTime.Now}.\n" +
                                           $"URLs: {absoluteUrls}\n\n",
                        UserID = userId,
                        UserName = userName,
                        IPAddress = ipAddress
                    });
                }
            }
            catch (Exception ex)
            {
                Service.Resolve<IEventLogService>().LogException("ContentChangeEventHandler", "Logging_Page_Info_Failed", ex);

                // Log with user details even on exception
                Service.Resolve<IEventLogService>().LogEvent(new EventLogData(
                    EventTypeEnum.Error,
                    "ContentChangeEventHandler",
                    "Upload Failed")
                {
                    EventDescription = $"Failed to upload URLs at {DateTime.Now}.\n" +
                                       $"Exception: {ex.Message}\n" +
                                       $"URLs: {pagePath.ToLower()}\n\n",
                    UserID = userId,
                    UserName = userName,
                    IPAddress = ipAddress
                });
            }
        }

        public static async Task<IEnumerable<string>> GetWebPageRelativeUrls(IEnumerable<int> pageIdentifiers, string languageName, CancellationToken cancellationToken)
        {
            using var scope = Service.Resolve<IServiceScopeFactory>().CreateScope();
            var urlRetriever = scope.ServiceProvider.GetRequiredService<IWebPageUrlRetriever>();
            var eventLogService = scope.ServiceProvider.GetRequiredService<IEventLogService>();
            var relativeUrls = new List<string>();

            try
            {
                foreach (int pageIdentifier in pageIdentifiers)
                {
                    var webPageUrl = await urlRetriever.Retrieve(pageIdentifier, languageName, false, cancellationToken);
                    relativeUrls.Add(webPageUrl.RelativePath.TrimStart('~'));
                }
            }
            catch (Exception ex)
            {
                eventLogService.LogException(nameof(ContentChangeEventHandler), nameof(GetWebPageRelativeUrls), ex, null);
            }

            return relativeUrls;
        }

    }

}
