using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models;

using Xunit;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Models
{
    // Test double for AIUNSettingsKeyInfo
    public class TestAiunSettingsKeyInfo
    {
        public string SettingsKey { get; set; } = string.Empty;
    }

    public class AIUNSettingKeyTests
    {
        [Fact]
        public void Constructor_InitializesWithDefaultValues()
        {
            var model = new AiunSettingKey();

            Assert.Equal(string.Empty, model.SettingKey);
        }

        [Fact]
        public void Properties_SetAndGetValues()
        {
            var model = new AiunSettingKey
            {
                SettingKey = "TestKey"
            };

            Assert.Equal("TestKey", model.SettingKey);
        }

        [Fact]
        public void Constructor_WithStringParameter_SetsProperty()
        {
            var model = new AiunSettingKey("MySettingKey");

            Assert.Equal("MySettingKey", model.SettingKey);
        }

        [Fact]
        public void Constructor_WithAIUNSettingsKeyInfo_SetsProperty()
        {
            // If you have the real AIUNSettingsKeyInfo, use it here. Otherwise, use a test double.  
            var info = new TestAiunSettingsKeyInfo { SettingsKey = "InfoKey" };

            // You may need to use a mock or a real instance if available.  
            var model = new AiunSettingKey(info.SettingsKey);

            Assert.Equal("InfoKey", model.SettingKey);
        }
    }
}
