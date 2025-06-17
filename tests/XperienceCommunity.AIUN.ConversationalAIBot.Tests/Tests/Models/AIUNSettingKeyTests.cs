using NUnit.Framework;

using XperienceCommunity.AIUN.ConversationalAIBot.Tests.Admin.Constants;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Tests.Models
{
    // Test double for AIUNSettingsKeyInfo
    public class TestAiunSettingsKeyInfo
    {
        public string SettingsKey { get; set; } = string.Empty;
    }

    [TestFixture]
    public class AIUNSettingKeyTests
    {
        [Test]
        public void Constructor_InitializesWithDefaultValues()
        {
            var model = new AiunSettingKey();

            Assert.That(model.SettingKey, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Properties_SetAndGetValues()
        {
            var model = new AiunSettingKey
            {
                SettingKey = "TestKey"
            };

            Assert.That(model.SettingKey, Is.EqualTo("TestKey"));
        }

        [Test]
        public void Constructor_WithStringParameter_SetsProperty()
        {
            var model = new AiunSettingKey("MySettingKey");

            Assert.That(model.SettingKey, Is.EqualTo("MySettingKey"));
        }

        [Test]
        public void Constructor_WithAIUNSettingsKeyInfo_SetsProperty()
        {
            var info = new TestAiunSettingsKeyInfo { SettingsKey = "InfoKey" };

            var model = new AiunSettingKey(info.SettingsKey);

            Assert.That(model.SettingKey, Is.EqualTo("InfoKey"));
        }
    }
}
