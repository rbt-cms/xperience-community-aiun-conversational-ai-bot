using Kentico.Xperience.Admin.Base.FormAnnotations;


namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models
{
    public class AiunSettingKey
    {
#pragma warning disable IDE0052 // intentionally empty class
        private readonly IEnumerable<AIUNSettingsKeyInfo> enumerable = Enumerable.Empty<AIUNSettingsKeyInfo>();
#pragma warning restore
        [TextInputComponent(Label = "Setting Key", Order = 1)]
        public string SettingKey { get; set; } = string.Empty;
        public AiunSettingKey() { }
        public AiunSettingKey(string settingkey) => SettingKey = settingkey;
        public AiunSettingKey(IEnumerable<AIUNSettingsKeyInfo> enumerable) => this.enumerable = enumerable;
        public AiunSettingKey(AIUNSettingsKeyInfo aIUNSettingsKeyInfo) => SettingKey = aIUNSettingsKeyInfo.SettingsKey;
    }
}
