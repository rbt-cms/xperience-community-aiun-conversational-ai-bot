using NUnit.Framework;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models;
using XperienceCommunity.AIUN.ConversationalAIBot.InfoClasses.AIUNConfigurationItem;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Models
{
    public class TestAiunConfigurationItemInfo : AIUNConfigurationItemInfo
    {
        public override int AIUNConfigurationItemID { get; set; }
        public override string ChannelName { get; set; }
        public override string ClientID { get; set; }
        public override string APIKey { get; set; }
        public override string BaseURL { get; set; }
    }

    [TestFixture]
    public class AIUNConfigurationItemModelTests
    {
        [Test]
        public void Constructor_InitializesWithDefaultValues()
        {
            var model = new AiunConfigurationItemModel();

            Assert.That(model.Id, Is.EqualTo(0));
            Assert.That(model.ChannelName, Is.EqualTo(string.Empty));
            Assert.That(model.ClientID, Is.EqualTo(string.Empty));
            Assert.That(model.APIKey, Is.EqualTo(string.Empty));
            Assert.That(model.BaseURL, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Properties_SetAndGetValues()
        {
            var model = new AiunConfigurationItemModel
            {
                Id = 42,
                ChannelName = "TestChannel",
                ClientID = "TestClientId",
                APIKey = "TestApiKey",
                BaseURL = "https://test.url"
            };

            Assert.That(model.Id, Is.EqualTo(42));
            Assert.That(model.ChannelName, Is.EqualTo("TestChannel"));
            Assert.That(model.ClientID, Is.EqualTo("TestClientId"));
            Assert.That(model.APIKey, Is.EqualTo("TestApiKey"));
            Assert.That(model.BaseURL, Is.EqualTo("https://test.url"));
        }

        [Test]
        public void Constructor_WithStringParameters_SetsProperties()
        {
            var model = new AiunConfigurationItemModel("ChannelA", "ClientA", "KeyA", "https://a.url");

            Assert.That(model.ChannelName, Is.EqualTo("ChannelA"));
            Assert.That(model.ClientID, Is.EqualTo("ClientA"));
            Assert.That(model.APIKey, Is.EqualTo("KeyA"));
            Assert.That(model.BaseURL, Is.EqualTo("https://a.url"));
        }

        [Test]
        public void Constructor_WithEnumerable_DoesNotThrow()
        {
            var list = new List<AIUNConfigurationItemInfo>();
            var model = new AiunConfigurationItemModel(list);

            Assert.That(model, Is.Not.Null);
        }

        [Test]
        public void Constructor_WithAIUNConfigurationItemInfo_SetsProperties()
        {
            var info = new TestAiunConfigurationItemInfo
            {
                AIUNConfigurationItemID = 7,
                ChannelName = "ChannelB",
                ClientID = "ClientB",
                APIKey = "KeyB",
                BaseURL = "https://b.url"
            };

            var model = new AiunConfigurationItemModel(info);

            Assert.That(model.Id, Is.EqualTo(7));
            Assert.That(model.ChannelName, Is.EqualTo("ChannelB"));
            Assert.That(model.ClientID, Is.EqualTo("ClientB"));
            Assert.That(model.APIKey, Is.EqualTo("KeyB"));
            Assert.That(model.BaseURL, Is.EqualTo("https://b.url"));
        }
    }
}
