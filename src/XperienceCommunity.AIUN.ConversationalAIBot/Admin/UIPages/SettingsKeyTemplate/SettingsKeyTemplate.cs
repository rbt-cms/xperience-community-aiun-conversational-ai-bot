using CMS.Core;
using CMS.DataEngine;

using Kentico.Xperience.Admin.Base;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages.SettingsKeyTemplate;


[assembly: UIPage(

    parentType: typeof(AiunChatbotApplication), // Your module root

    slug: "settings-key", // Unique slug for the page

    uiPageType: typeof(SettingsKeyTemplate), // Template for Settings Key

    name: "API Key", // Page Name

    templateName: "@rbt/aiun-chatbot/SettingsKeyLayout", // React component

    order: 1)] // No specific order

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages.SettingsKeyTemplate
{

    // Page for creating or updating the Settings Key

    internal class SettingsKeyTemplate : Page<SettingsKeyProperties>
    {
        private readonly IInfoProvider<AIUNSettingsKeyInfo> settingsKeyProvider;
        public SettingsKeyTemplate(IInfoProvider<AIUNSettingsKeyInfo> settingsKeyProviderParam) => settingsKeyProvider = settingsKeyProviderParam;

        // Configure the template properties (retrieve the key)

        public override Task<SettingsKeyProperties> ConfigureTemplateProperties(SettingsKeyProperties properties)
        {
            try
            {
                var existingKey = settingsKeyProvider.Get()?.FirstOrDefault();
                properties.KeyValue = existingKey?.SettingsKey ?? ""; // Set default if key not found
            }
            catch (Exception ex)
            {
                Service.Resolve<IEventLogService>().LogException(nameof(SettingsKeyTemplate), nameof(ConfigureTemplateProperties), ex);
            }
            return Task.FromResult(properties);
        }

        // PageCommand for saving the key value

        [PageCommand]
        public Task<SettingsKeyResult> SaveKeyValue(string keyValue)
        {
            try
            {
                var existingKey = settingsKeyProvider.Get()?.FirstOrDefault();
                if (existingKey != null)
                {
                    existingKey.SettingsKey = keyValue;
                    settingsKeyProvider.Set(existingKey);
                }
                else
                {
                    if (keyValue != null)
                    {
                        var newKey = new AIUNSettingsKeyInfo
                        {
                            SettingsKey = keyValue
                        };
                        settingsKeyProvider.Set(newKey);
                    }
                    else
                    {
                        return Task.FromResult(new SettingsKeyResult("Please input API key"));
                    }
                }
                return Task.FromResult(new SettingsKeyResult("Saved successfully."));
            }
            catch (Exception ex)
            {
                Service.Resolve<IEventLogService>().LogException(nameof(SettingsKeyTemplate), nameof(SaveKeyValue), ex);
                return Task.FromResult(new SettingsKeyResult("Something went wrong. Please try again later."));
            }
        }
    }

    // Properties to be passed to the template (this will be used in the UI)

    internal class SettingsKeyProperties : TemplateClientProperties
    {
        public string KeyValue { get; set; } = string.Empty;
    }

    // Result returned after saving the key value

    internal readonly record struct SettingsKeyResult(string Message);

}
