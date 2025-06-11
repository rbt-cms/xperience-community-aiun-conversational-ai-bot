
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages.TokensUsage;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models.AIUNIndexes;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.IManagers
{
    public interface IAIUNApiManager
    {
        public Task<string> UploadURLsAsync(List<string> websiteUrls, string clientID);
        public Task<AIUNTokenUsageLayoutProperties> GetTokenUsageAsync();
        public Task<IndexesResponseModel> GetIndexesAsync(IndexItemFilterModel indexItemFilterModel);
    }
}
