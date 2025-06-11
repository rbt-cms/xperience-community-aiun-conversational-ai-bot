using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models;

using Xunit;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Tests.Models
{
    public class WebsiteChannelModelTests
    {
        [Fact]
        public void Constructor_InitializesWithNullProperties()
        {
            var model = new WebsiteChannelModel();

            Assert.Null(model.ClientId);
            Assert.Null(model.ChannelName);
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
