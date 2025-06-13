
using CMS.ContentEngine;
using CMS.Core;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Websites;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.IManagers;
using XperienceCommunity.AIUN.ConversationalAIBot.InfoClasses.AIUNConfigurationItem;


namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.Managers
{
    public class DefaultChatbotManager : IDefaultChatbotManager
    {
        private readonly IInfoProvider<AIUNConfigurationItemInfo> aiUNConfigurationItemInfoProvider;
        private readonly IContentQueryExecutor contentQueryExecutor;
        private readonly IProgressiveCache cache;
        private readonly IInfoProvider<ChannelInfo> channelProvider;
        private readonly IConversionService conversionService;
        private readonly IWebPageUrlRetriever urlRetriever;
        private readonly IAiunApiManager aiUNApiManager;
        private readonly IEventLogService eventLogService;

        public DefaultChatbotManager(IInfoProvider<AIUNConfigurationItemInfo> aIUNConfigurationItemInfoProvider,
        IContentQueryExecutor executor,
        IProgressiveCache cacheParam,
          IInfoProvider<ChannelInfo> channelProviderParam,
           IConversionService conversionServiceParam,
            IWebPageUrlRetriever urlRetrieverParam,
        IEventLogService eventLogServiceParam,
        IAiunApiManager aiUNApiManagerParam
        )
        {
            contentQueryExecutor = executor;
            aiUNConfigurationItemInfoProvider = aIUNConfigurationItemInfoProvider;
            cache = cacheParam;
            channelProvider = channelProviderParam;
            conversionService = conversionServiceParam;
            urlRetriever = urlRetrieverParam;
            eventLogService = eventLogServiceParam;
            aiUNApiManager = aiUNApiManagerParam;
        }

        /// <summary>
        /// Get all website channels.
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<(int WebsiteChannelID, string ChannelName)>> GetAllWebsiteChannels() =>
           cache.LoadAsync(async cs =>
           {

               var results = await channelProvider.Get()
                   .Source(s => s.Join<WebsiteChannelInfo>(nameof(ChannelInfo.ChannelID), nameof(WebsiteChannelInfo.WebsiteChannelChannelID)))
                   .Columns(nameof(WebsiteChannelInfo.WebsiteChannelID), nameof(ChannelInfo.ChannelName))
                   .GetDataContainerResultAsync();

               cs.GetCacheDependency = () => CacheHelper.GetCacheDependency(new[] { $"{ChannelInfo.OBJECT_TYPE}|all", $"{WebsiteChannelInfo.OBJECT_TYPE}|all" });

               var items = new List<(int WebsiteChannelID, string ChannelName)>();

               foreach (var item in results)
               {
                   if (item.TryGetValue(nameof(WebsiteChannelInfo.WebsiteChannelID), out object channelID) && item.TryGetValue(nameof(ChannelInfo.ChannelName), out object channelName))
                   {
                       items.Add(new(conversionService.GetInteger(channelID, 0), conversionService.GetString(channelName, string.Empty)));
                   }
               }

               return items.AsEnumerable();
           }, new CacheSettings(5, nameof(DefaultChatbotManager), nameof(GetAllWebsiteChannels)));

        /// <summary>
        /// Index internal method to upload URLs to AIUN API.
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="channelName"></param>
        /// <param name="clientID"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="scheme"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        public async Task<string> IndexInternal(int channelId, string channelName, string clientID, CancellationToken cancellationToken, string scheme, HostString host)
        {
            try
            {
                var options = new ContentQueryExecutionOptions
                {
                    ForPreview = false,
                    IncludeSecuredItems = false,
                };

                var relativeUrls = new List<string>();

                var builder = new ContentItemQueryBuilder().ForContentTypes(p => p.ForWebsite(channelName))
                                                           .InLanguage("en")
                                                           .Parameters(p => p.Columns(nameof(IWebPageContentQueryDataContainer.WebPageItemID)));

                var pageIdentifiers = await contentQueryExecutor.GetWebPageResult(builder, i => i.WebPageItemID, options, cancellationToken);
                var languageUrls = await GetWebPageRelativeUrls(pageIdentifiers, "en", cancellationToken);

                relativeUrls.AddRange(languageUrls);
                var absoluteUrls = GetAbsoluteUrls(relativeUrls, scheme, host);
                _ = await aiUNApiManager.UploadURLsAsync(absoluteUrls.Distinct().ToList(), clientID);
            }
            catch (Exception ex)
            {
                eventLogService.LogException(nameof(DefaultChatbotManager), nameof(IndexInternal), ex, null);
            }
            return string.Empty;
        }

        /// <summary>
        /// Get web page relative urls.
        /// </summary>
        /// <param name="pageIdentifiers"></param>
        /// <param name="languageName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetWebPageRelativeUrls(IEnumerable<int> pageIdentifiers, string languageName, CancellationToken cancellationToken)
        {

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
                eventLogService.LogException(nameof(DefaultChatbotManager), nameof(GetWebPageRelativeUrls), ex, null);
            }

            return relativeUrls;
        }

        /// <summary>
        /// Get absolute urls.
        /// </summary>
        /// <param name="relativeUrls"></param>
        /// <param name="scheme"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        public IEnumerable<string> GetAbsoluteUrls(IEnumerable<string> relativeUrls, string scheme, HostString host)
        {
            try
            {
                return relativeUrls.Select(i => UriHelper.BuildAbsolute(scheme, host, path: i)).OrderBy(i => i);
            }
            catch (Exception ex)
            {
                eventLogService.LogException(nameof(DefaultChatbotManager), nameof(GetAbsoluteUrls), ex, null);
            }
            return Enumerable.Empty<string>();
        }
        /// <summary>
        /// Get client ID with channel name.
        /// </summary>
        /// <param name="channelName"></param>
        /// <returns></returns>
        public string GetClientIDWIthChannelName(string channelName)
        {
            try
            {
                var item = aiUNConfigurationItemInfoProvider
                    .Get()
                    .WhereEquals(nameof(AIUNConfigurationItemInfo.ChannelName), channelName)
                    .Column(nameof(AIUNConfigurationItemInfo.ClientID))
                    .FirstOrDefault();

                return item?.ClientID ?? string.Empty;
            }
            catch (Exception ex)
            {
                eventLogService.LogException(nameof(DefaultChatbotManager), nameof(GetClientIDWIthChannelName), ex, null);
            }
            return string.Empty;
        }

        /// <summary>
        /// Get channel name with client ID.
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public string GetChannelNameWithClientID(long clientID)
        {
            try
            {
                var item = aiUNConfigurationItemInfoProvider
                    .Get()
                    .WhereEquals(nameof(AIUNConfigurationItemInfo.ClientID), clientID)
                    .Column(nameof(AIUNConfigurationItemInfo.ChannelName))
                    .FirstOrDefault();

                return item?.ChannelName ?? string.Empty;
            }
            catch (Exception ex)
            {
                eventLogService.LogException(nameof(DefaultChatbotManager), nameof(GetChannelNameWithClientID), ex, null);
            }
            return string.Empty;
        }

        Task<IEnumerable<string>> IDefaultChatbotManager.GetAbsoluteUrls(IEnumerable<string> relativeUrls, string scheme, HostString host) => throw new NotImplementedException();
    }

}
