using CMS.DataEngine;

using Kentico.Xperience.Admin.Base;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models.AIUNIndexes;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.IManagers;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages.AIUNIndexes;
using XperienceCommunity.AIUN.ConversationalAIBot.InfoClasses.AIUNConfigurationItem;

[assembly: UIPage(
    parentType: typeof(AiunChatbotApplication),
    slug: "indexes",
    uiPageType: typeof(AiunIndexesTemplate),
    name: "Indexes",
    templateName: "@rbt/aiun-chatbot/AIUNIndexesLayout",
    order: UIPageOrder.NoOrder)]
namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages.AIUNIndexes
{
    /// <summary>
    /// Template class
    /// </summary>
    public class AiunIndexesTemplate : Page<AiunIndexesLayoutProperties>
    {
        private readonly IAiunApiManager aIUNApiManager;
        private readonly IInfoProvider<AIUNConfigurationItemInfo> aIUNConfigurationItemInfoProvider;
        public AiunIndexesTemplate(IAiunApiManager aiUNApiManagerParam, IInfoProvider<AIUNConfigurationItemInfo> aIUNConfigurationItemInfoProviderParam)
        {
            aIUNApiManager = aiUNApiManagerParam;
            aIUNConfigurationItemInfoProvider = aIUNConfigurationItemInfoProviderParam;
        }

        /// <summary>
        /// Sets default property values for the client template(AIUNIndexesTemplate.tsx)
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public override async Task<AiunIndexesLayoutProperties> ConfigureTemplateProperties(AiunIndexesLayoutProperties properties)
        {
            var filter = new IndexItemFilterModel();// Default filter on load
            properties.IndexesResponse = await aIUNApiManager.GetIndexesAsync(filter);
            var configItems = aIUNConfigurationItemInfoProvider.Get();
            properties.WebsiteChannels = (configItems != null)
                    ? configItems.Select(c => new WebsiteChannelModel
                    {
                        ClientId = c.ClientID,
                        ChannelName = c.ChannelName
                    })
                    .ToList() : [];

            return await Task.FromResult(properties);
        }


        /// <summary>
        /// Registers the 'GetIndexes' page command that can be invoked form the client template
        /// </summary>
        /// <param name="indexItemFilterModel"></param>
        /// <returns></returns>
        [PageCommand]
        public async Task<IndexesResponseModel> GetIndexes(IndexItemFilterModel indexItemFilterModel)
        {
            var result = await aIUNApiManager.GetIndexesAsync(indexItemFilterModel);
            return result;
        }
    }

    /// <summary>
    /// Defines the properties for passing to client template (AIUNIndexesLayoutTemplate.tsx)
    /// </summary>
    public class AiunIndexesLayoutProperties : TemplateClientProperties
    {
        public IndexesResponseModel IndexesResponse { get; set; } = new IndexesResponseModel();
        public List<WebsiteChannelModel> WebsiteChannels { get; set; } = [];
    }
}
