
using CMS;
using CMS.Base;
using CMS.Core;
using CMS.DataEngine;
using CMS.Websites;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.IManagers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using CMS.Membership;
using CMS.Helpers;
using CMS.ContentEngine;


[assembly: RegisterModule(typeof(ContentChangeEventHandler))]

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
        //ContentItemEvents.Publish.Execute += HandleContentItemPublish;
    }

    private async void HandleWebPagePublish(object? sender, CMSEventArgs e)
    {
        if (e is not WebPageEventArgsBase pageEvent)
        {
            return;
        }

        LogPageInfo(pageEvent);
        //await ProcessContentChangeAsync(pageEvent.ID, pageEvent.ContentLanguageName);
    }


    private async Task LogPageInfo(WebPageEventArgsBase pageEvent)
    {
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
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
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
            using var scope = Service.Resolve<IServiceScopeFactory>().CreateScope();
            var syncLogs = scope.ServiceProvider.GetRequiredService<IAIUNApiManager>();
            var chatbotManager = scope.ServiceProvider.GetRequiredService<IDefaultChatbotManager>();
            var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
            var _contentQueryExecutor = scope.ServiceProvider.GetRequiredService<IContentQueryExecutor>();


            if (!string.IsNullOrWhiteSpace(pagePath))
            {
                var relativeUrls = new List<string>();
                var options = new ContentQueryExecutionOptions
                {
                    ForPreview = false,
                    IncludeSecuredItems = false,

                };

                var builder = new ContentItemQueryBuilder();
                builder.ForContentTypes(parameters =>
                    parameters.ForWebsite(
                        websiteChannelName: websiteChannelName,
                        pathMatch: PathMatch.Single(pagePath)
                    ));

                var cancellationToken = CancellationToken.None;
                var pageIdentifiers = await _contentQueryExecutor.GetWebPageResult(builder, i => i.WebPageItemID, options, cancellationToken);
                var languageUrls = await GetWebPageRelativeUrls(pageIdentifiers, "en", cancellationToken);

                relativeUrls.AddRange(languageUrls);


                var request = httpContextAccessor.HttpContext?.Request;
                string scheme = request?.Scheme ?? string.Empty;
                var host = request?.Host ?? new();
                var absoluteUrls = await chatbotManager.GetAbsoluteUrls(relativeUrls, scheme, host);
                string clientID = chatbotManager.GetClientIDWIthChannelName(websiteChannelName);

                await syncLogs.UploadURLsAsync(absoluteUrls.ToList(), clientID);

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

    public async Task<IEnumerable<string>> GetWebPageRelativeUrls(IEnumerable<int> pageIdentifiers, string languageName, CancellationToken cancellationToken)
    {

        using var scope = Service.Resolve<IServiceScopeFactory>().CreateScope();
        var _urlRetriever = scope.ServiceProvider.GetRequiredService<IWebPageUrlRetriever>();
        var relativeUrls = new List<string>();

        try
        {
            foreach (int pageIdentifier in pageIdentifiers)
            {

                var webPageUrl = await _urlRetriever.Retrieve(pageIdentifier, languageName, false, cancellationToken);
                relativeUrls.Add(webPageUrl.RelativePath.TrimStart('~'));
            }
        }
        catch (Exception ex)
        {
            // _eventLogService.LogException(nameof(DefaultChatbotManager), nameof(GetWebPageRelativeUrls), ex, null);
        }

        return relativeUrls;
    }

}
