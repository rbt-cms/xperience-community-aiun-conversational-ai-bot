using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models;

using Xunit;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Models
{
    public class WebsiteChannelModelTests
    {
        [Fact]
        public void Constructor_InitializesWithEmptyStringProperties()
        {
            var model = new WebsiteChannelModel();

            Assert.Equal(string.Empty, model.ClientId);
            Assert.Equal(string.Empty, model.ChannelName);
        }

        [Fact]
        public void Properties_SetAndGetValues()
        {
            var model = new WebsiteChannelModel
            {
                ClientId = "client-123",
                ChannelName = "MainChannel"
            };

            Assert.Equal("client-123", model.ClientId);
            Assert.Equal("MainChannel", model.ChannelName);
        }
    }
}
