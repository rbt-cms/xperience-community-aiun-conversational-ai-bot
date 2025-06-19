using Kentico.Xperience.Admin.Base;

using Newtonsoft.Json;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.IManagers;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages.TokensUsage;


[assembly: UIPage(
    parentType: typeof(AiunChatbotApplication),
    slug: "token-usage",
    uiPageType: typeof(AiunTokenUsageTemplate),
    name: "Token Usage",
    templateName: "@XperienceCommunity/aiun-chatbot/TokensUsageLayout",
    order: UIPageOrder.NoOrder)]

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages.TokensUsage
{
    // Main page class for Token Usage
    internal class AiunTokenUsageTemplate : Page<AiunTokenUsageLayoutProperties>
    {
        private readonly IAiunApiManager aIUNApiManager;
        private readonly IDefaultChatbotManager defaultChatbotManager;

        public AiunTokenUsageTemplate(IAiunApiManager aIUNApiManagerParam, IDefaultChatbotManager defaultChatbotManagerParam)
        {
            aIUNApiManager = aIUNApiManagerParam;
            defaultChatbotManager = defaultChatbotManagerParam;
        }


        // Called on page load to populate template props
        public override async Task<AiunTokenUsageLayoutProperties> ConfigureTemplateProperties(AiunTokenUsageLayoutProperties properties)
        {
            properties.OverallUsage = new OverallUsage();
            properties.Clients = [];

            var apiData = await aIUNApiManager.GetTokenUsageAsync();

            if (apiData != null)
            {

                properties.OverallUsage.TokenLimit = apiData.OverallUsage.TokenLimit;
                properties.OverallUsage.TokenUsed = apiData.OverallUsage.TokenUsed;
                apiData.Clients.ForEach(x =>
                {
                    string clientName = defaultChatbotManager.GetChannelNameWithClientID(x.ClientId);
                    if (!string.IsNullOrEmpty(clientName))
                    {

                        properties.Clients.Add(new TokenUsageClient
                        {
                            ClientId = x.ClientId,
                            ClientName = clientName,
                            TokensUsed = x.TokensUsed
                        });
                    }
                });
            }
            return properties;
        }
    }

    // Template props sent to React component
    public class AiunTokenUsageLayoutProperties : TemplateClientProperties
    {
        [JsonProperty("overall")]
        public OverallUsage OverallUsage { get; set; } = new OverallUsage();
        public List<TokenUsageClient> Clients { get; set; } = [];
    }

    // Client-specific token data
    public class TokenUsageClient
    {
        [JsonProperty("client_id")]
        public long ClientId { get; set; }

        [JsonProperty("client_name")]
        public string ClientName { get; set; } = string.Empty;

        [JsonProperty("tokens_used")]
        public int TokensUsed { get; set; } = 0;
    }

    public class OverallUsage
    {
        [JsonProperty("token_limit")]
        public int TokenLimit { get; set; } = 0;

        [JsonProperty("token_used")]
        public int TokenUsed { get; set; } = 0;
    }
}
