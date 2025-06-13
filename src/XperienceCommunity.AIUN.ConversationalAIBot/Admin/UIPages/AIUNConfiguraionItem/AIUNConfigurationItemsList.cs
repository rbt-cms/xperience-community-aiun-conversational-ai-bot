using CMS.Core;
using CMS.DataEngine;

using Kentico.Xperience.Admin.Base;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.IManagers;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages.AIUNConfiguraionItem;
using XperienceCommunity.AIUN.ConversationalAIBot.InfoClasses.AIUNConfigurationItem;


[assembly: UIPage(
    parentType: typeof(AiunChatbotApplication),
    slug: "list",
    uiPageType: typeof(AiunConfigurationItemsList),
    name: "Configuration",
    templateName: TemplateNames.LISTING,
    order: UIPageOrder.NoOrder)]
namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages.AIUNConfiguraionItem
{
    public class AiunConfigurationItemsList : ListingPage
    {
        protected override string ObjectType => AIUNConfigurationItemInfo.OBJECT_TYPE;


        private readonly IDefaultChatbotManager defaultChatbotManager;
        private readonly IInfoProvider<AIUNConfigurationItemInfo> aiUNConfigurationItemInfoProvider;
        private readonly IHttpContextAccessor httpContextAccessor;

        private readonly IMemoryCache memoryCache;



        public AiunConfigurationItemsList(IDefaultChatbotManager defaultChatbotManagerParam, IInfoProvider<AIUNConfigurationItemInfo> aIUNConfigurationItemInfoProviderParam, IMemoryCache memoryCacheParam, IHttpContextAccessor httpContextAccessorParam)
        {
            defaultChatbotManager = defaultChatbotManagerParam;
            aiUNConfigurationItemInfoProvider = aIUNConfigurationItemInfoProviderParam;
            memoryCache = memoryCacheParam;
            httpContextAccessor = httpContextAccessorParam;
        }

        public override Task ConfigurePage()
        {
            _ = PageConfiguration.ColumnConfigurations
            .AddColumn(nameof(AIUNConfigurationItemInfo.AIUNConfigurationItemID), "ID")
            .AddColumn(nameof(AIUNConfigurationItemInfo.ChannelName), "Channel Name")
            .AddColumn(nameof(AIUNConfigurationItemInfo.ClientID), "ClientID")
            .AddColumn(nameof(AIUNConfigurationItemInfo.APIKey), "API Key")
            .AddColumn(nameof(AIUNConfigurationItemInfo.BaseURL), "Base URL");

            _ = PageConfiguration.HeaderActions.AddLink<AiunConfigurationItemCreate>("Create");
            _ = PageConfiguration.AddEditRowAction<AiunConfigurationItemEdit>();
            _ = PageConfiguration.TableActions.AddCommand("Index Published Pages", nameof(Index), icon: Icons.ArrowSend);
            _ = PageConfiguration.TableActions.AddDeleteAction("Delete");

            return base.ConfigurePage();
        }
        [PageCommand]
        public override Task<ICommandResponse<RowActionResult>> Delete(int id) => base.Delete(id);

        [PageCommand]
        public ICommandResponse<RowActionResult> Index(int id, CancellationToken cancellationToken)
        {
            var result = new RowActionResult(false);

            if (memoryCache.TryGetValue("IndexInProgress", out bool isIndexInProgress) && isIndexInProgress)
            {
                return ResponseFrom(result)
                    .AddErrorMessage("Indexing is already in progress. Please try again later.");
            }
            _ = memoryCache.Set("IndexInProgress", true, TimeSpan.FromMinutes(10));

            var request = httpContextAccessor.HttpContext?.Request;
            string scheme = request?.Scheme ?? string.Empty;
            var host = request?.Host ?? new HostString();

            _ = Task.Run(async () =>
            {
                try
                {
                    var AIUNConfigurationItem = aiUNConfigurationItemInfoProvider.Get().WithID(id).FirstOrDefault() ?? new AIUNConfigurationItemInfo();
                    var websiteChannels = await defaultChatbotManager.GetAllWebsiteChannels();
                    var (channelName, websiteChannelID) = websiteChannels
                        .Where(c => c.ChannelName == AIUNConfigurationItem.ChannelName)
                        .Select(c => (c.ChannelName, c.WebsiteChannelID))
                        .FirstOrDefault();

                    _ = await defaultChatbotManager.IndexInternal(websiteChannelID, channelName, AIUNConfigurationItem.ClientID, cancellationToken, scheme, host);
                    _ = memoryCache.Set("IndexInProgress", false);
                }
                catch (Exception ex)
                {
                    _ = memoryCache.Set("IndexInProgress", false);
                    EventLogService.LogException(nameof(AiunConfigurationItemsList), nameof(Index), ex);
                }
            });

            return ResponseFrom(result)
                .AddInfoMessage("Published Pages - Indexing is In-Progress");
        }
    }
}
