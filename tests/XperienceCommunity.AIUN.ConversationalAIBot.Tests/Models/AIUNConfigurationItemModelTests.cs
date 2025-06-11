using System.Collections.Generic;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models;
using XperienceCommunity.AIUN.ConversationalAIBot.InfoClasses.AIUNConfigurationItem;

using Xunit;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Tests.Models
{
    // Test double to avoid Kentico IoC/container issues
    public class TestAIUNConfigurationItemInfo : AIUNConfigurationItemInfo
    {
        public override int AIUNConfigurationItemID { get; set; }
        public override string ChannelName { get; set; }
        public override string ClientID { get; set; }
        public override string APIKey { get; set; }
        public override string BaseURL { get; set; }
    }

    public class AIUNConfigurationItemModelTests
    {
        [Fact]
        public void Constructor_InitializesWithDefaultValues()
        {
            var model = new AIUNConfigurationItemModel();

            Assert.Equal(0, model.Id);
            Assert.Equal(string.Empty, model.ChannelName);
            Assert.Equal(string.Empty, model.ClientID);
            Assert.Equal(string.Empty, model.APIKey);
            Assert.Equal(string.Empty, model.BaseURL);
        }

        [Fact]
        public void Properties_SetAndGetValues()
        {
            var model = new AIUNConfigurationItemModel
            {
                Id = 42,
                ChannelName = "TestChannel",
                ClientID = "TestClientId",
                APIKey = "TestApiKey",
                BaseURL = "https://test.url"
            };

            Assert.Equal(42, model.Id);
            Assert.Equal("TestChannel", model.ChannelName);
            Assert.Equal("TestClientId", model.ClientID);
            Assert.Equal("TestApiKey", model.APIKey);
            Assert.Equal("https://test.url", model.BaseURL);
        }

        [Fact]
        public void Constructor_WithStringParameters_SetsProperties()
        {
            var model = new AIUNConfigurationItemModel("ChannelA", "ClientA", "KeyA", "https://a.url");

            Assert.Equal("ChannelA", model.ChannelName);
            Assert.Equal("ClientA", model.ClientID);
            Assert.Equal("KeyA", model.APIKey);
            Assert.Equal("https://a.url", model.BaseURL);
        }

        [Fact]
        public void Constructor_WithEnumerable_DoesNotThrow()
        {
            var list = new List<AIUNConfigurationItemInfo>();
            var model = new AIUNConfigurationItemModel(list);

            // No public property to assert, just ensure no exception
            Assert.NotNull(model);
        }

        [Fact]
        public void Constructor_WithAIUNConfigurationItemInfo_SetsProperties()
        {
            var info = new TestAIUNConfigurationItemInfo
            {
                AIUNConfigurationItemID = 7,
                ChannelName = "ChannelB",
                ClientID = "ClientB",
                APIKey = "KeyB",
                BaseURL = "https://b.url"
            };

            var model = new AIUNConfigurationItemModel(info);

            Assert.Equal(7, model.Id);
            Assert.Equal("ChannelB", model.ChannelName);
            Assert.Equal("ClientB", model.ClientID);
            Assert.Equal("KeyB", model.APIKey);
            Assert.Equal("https://b.url", model.BaseURL);
        }
    }
}
