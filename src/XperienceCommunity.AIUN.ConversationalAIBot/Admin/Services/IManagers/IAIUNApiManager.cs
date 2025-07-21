
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models.AIUNIndexes;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages.TokensUsage;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.IManagers
{
    public interface IAiunApiManager
    {
        public Task<string> UploadURLsAsync(List<string> websiteUrls, string clientID, string securityToken = "");
        public Task<AiunTokenUsageLayoutProperties> GetTokenUsageAsync();
        public Task<IndexesResponseModel> GetIndexesAsync(IndexItemFilterModel indexItemFilterModel);
        public Task<AiunRegistrationModel> AIUNSignup(AiunRegistrationModel aIUNRegistrationItemModel);

        public Task<string> ValidateChatbotConfiguration(AiunConfigurationItemModel aiunConfigurationItemModel);
    }
}
