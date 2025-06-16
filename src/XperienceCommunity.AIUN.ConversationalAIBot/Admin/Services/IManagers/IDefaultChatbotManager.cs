using Microsoft.AspNetCore.Http;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.IManagers
{
    public interface IDefaultChatbotManager
    {
        public Task<IEnumerable<(int WebsiteChannelID, string ChannelName)>> GetAllWebsiteChannels();
        public Task<string> IndexInternal(int channelId, string channelName, string clientID, CancellationToken cancellationToken, string scheme, HostString host);
        public Task<IEnumerable<string>> GetWebPageRelativeUrls(IEnumerable<int> pageIdentifiers, string languageName, CancellationToken cancellationToken);
        public string GetClientIDWIthChannelName(string channelName);
        public string GetChannelNameWithClientID(long clientID);
        public Task<IEnumerable<string>> GetAbsoluteUrls(IEnumerable<string> relativeUrls, string scheme, HostString host);
        public AIUNRegistrationItemModel GetExistingRegistration();
        public Task<object> StoreOrUpdate(AIUNRegistrationItemModel data);

    }
}
