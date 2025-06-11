using Kentico.Xperience.Admin.Base.FormAnnotations;


namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models
{
    public class AIUNSettingKey
    {
        private readonly IEnumerable<AIUNSettingsKeyInfo> enumerable = Enumerable.Empty<AIUNSettingsKeyInfo>();

        [TextInputComponent(Label = "Setting Key", Order = 1)]
        public string SettingKey { get; set; } = string.Empty;
        public AIUNSettingKey() { }
        public AIUNSettingKey(string settingkey) => SettingKey = settingkey;
        public AIUNSettingKey(IEnumerable<AIUNSettingsKeyInfo> enumerable) => this.enumerable = enumerable;
        public AIUNSettingKey(AIUNSettingsKeyInfo aIUNSettingsKeyInfo) => SettingKey = aIUNSettingsKeyInfo.SettingsKey;
    }
}
