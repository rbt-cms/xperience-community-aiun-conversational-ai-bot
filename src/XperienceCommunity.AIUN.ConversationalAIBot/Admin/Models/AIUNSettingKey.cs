using Kentico.Xperience.Admin.Base.FormAnnotations;


namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models
{
    public class AiunSettingKey
    {
        [TextInputComponent(Label = "Setting Key", Order = 1)]
        public string SettingKey { get; set; } = string.Empty;
        public AiunSettingKey() { }
        public AiunSettingKey(string settingkey) => SettingKey = settingkey;
        public AiunSettingKey(AIUNSettingsKeyInfo aIUNSettingsKeyInfo) => SettingKey = aIUNSettingsKeyInfo.SettingsKey;
    }
}
