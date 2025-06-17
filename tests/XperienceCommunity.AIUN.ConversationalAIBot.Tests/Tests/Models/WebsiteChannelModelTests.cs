using NUnit.Framework;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Models
{
    [TestFixture]
    public class WebsiteChannelModelTests
    {
        [Test]
        public void Constructor_InitializesWithEmptyStringProperties()
        {
            var model = new WebsiteChannelModel();

            Assert.That(model.ClientId, Is.EqualTo(string.Empty));
            Assert.That(model.ChannelName, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Properties_SetAndGetValues()
        {
            var model = new WebsiteChannelModel
            {
                ClientId = "client-123",
                ChannelName = "MainChannel"
            };

            Assert.That(model.ClientId, Is.EqualTo("client-123"));
            Assert.That(model.ChannelName, Is.EqualTo("MainChannel"));
        }
    }
}
