using Kentico.Xperience.Admin.Base.FormAnnotations;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Providers;
using XperienceCommunity.AIUN.ConversationalAIBot.InfoClasses.AIUNConfigurationItem;


namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models
{
    public class AiunConfigurationItemModel
    {
        public readonly IEnumerable<AIUNConfigurationItemInfo> ItemList = [];
        public int Id { get; set; }
        [DropDownComponent(Label = "Channel Name", DataProviderType = typeof(ChannelListProvider), Order = 1)]
        [RequiredValidationRule]
        public string ChannelName { get; set; } = string.Empty;

        [TextInputComponent(Label = "Client ID", Order = 3)]
        [RequiredValidationRule]
        public string ClientID { get; set; } = string.Empty;
        [TextInputComponent(Label = "API Key", Order = 4)]
        [RequiredValidationRule]
        public string APIKey { get; set; } = string.Empty;
        [TextInputComponent(Label = "Base URL", Order = 5)]
        [RequiredValidationRule]
        public string BaseURL { get; set; } = string.Empty;


        public AiunConfigurationItemModel() { }
        public AiunConfigurationItemModel(string channelName, string clientID, string apiKey, string baseURL)
        {
            ChannelName = channelName;
            ClientID = clientID;
            APIKey = apiKey;
            BaseURL = baseURL;
        }
        public AiunConfigurationItemModel(AIUNConfigurationItemInfo aIUNConfigurationItemInfo)
        {
            Id = aIUNConfigurationItemInfo.AIUNConfigurationItemID;
            ChannelName = aIUNConfigurationItemInfo.ChannelName;
            ClientID = aIUNConfigurationItemInfo.ClientID;
            APIKey = aIUNConfigurationItemInfo.APIKey;
            BaseURL = aIUNConfigurationItemInfo.BaseURL;
        }

        public AiunConfigurationItemModel(IEnumerable<AIUNConfigurationItemInfo> enumerable) => ItemList = enumerable;

    }
}
