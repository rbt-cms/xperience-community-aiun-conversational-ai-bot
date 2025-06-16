using CMS.Core;
using CMS.Membership;

using Kentico.Xperience.Admin.Base;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.IManagers;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages.AIUNRegistration;

[assembly: UIPage(

    parentType: typeof(AiunChatbotApplication), // Your module root

    slug: "register-aiun", // Unique slug for the page

    uiPageType: typeof(AiunNRegistrationTemplate), // Template for AIUNRegistration

    name: "Register with AIUN", // Page Name

    templateName: "@rbt/aiun-chatbot/AIUNRegistrationLayout", // React component

    order: 1)] // No specific order

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages.AIUNRegistration
{
    public class AiunNRegistrationTemplate : Page<AiunRegistrationLayoutProperties>
    {
        private readonly IDefaultChatbotManager defaultChatbotManager;
        private readonly IEventLogService eventLogService;
        public AiunNRegistrationTemplate(
            IEventLogService eventLogServiceParam,
            IDefaultChatbotManager defaultChatbotManagerParam)
        {
            eventLogService = eventLogServiceParam;
            defaultChatbotManager = defaultChatbotManagerParam;
        }

        /// <summary>
        /// Sets default property values for the client template(AIUNRegistrationTemplate.tsx)
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public override async Task<AiunRegistrationLayoutProperties> ConfigureTemplateProperties(AiunRegistrationLayoutProperties properties)
        {
            try
            {
                var currentUser = MembershipContext.AuthenticatedUser;

                if (currentUser == null || !currentUser.IsAdministrator())
                {
                    properties.RegistrationItem = new AIUNRegistrationItemModel();
                    properties.IsRegistrationExist = false;
                    return await Task.FromResult(properties);
                }

                var existingInfo = defaultChatbotManager.GetExistingRegistration();

                if (existingInfo != null && !string.IsNullOrEmpty(existingInfo.Email))
                {
                    properties.RegistrationItem = existingInfo;
                    properties.IsRegistrationExist = true;
                }
                else
                {
                    properties.RegistrationItem = new AIUNRegistrationItemModel
                    {
                        FirstName = currentUser.FirstName ?? string.Empty,
                        LastName = currentUser.LastName ?? string.Empty,
                        Email = currentUser.Email ?? string.Empty,
                        APIKey = string.Empty
                    };
                    properties.IsRegistrationExist = false;
                }

            }
            catch (Exception ex)
            {
                eventLogService.LogException(
                       source: nameof(AiunNRegistrationTemplate),
                       eventCode: "Configure Template Properties",
                       ex
                   );
            }
            return await Task.FromResult(properties);
        }

        [PageCommand]
        public async Task<object> StoreOrUpdateAsync(AIUNRegistrationItemModel data)
        {

            try
            {
                return await defaultChatbotManager.StoreOrUpdate(data);
            }
            catch (Exception ex)
            {
                eventLogService.LogException(
                       source: nameof(AiunNRegistrationTemplate),
                       eventCode: "StoreOrUpdateAsync",
                       ex
                   );

                return new { success = false, message = ex.Message };
            }
        }
    }

    /// <summary>
    /// Defines the properties for passing to client template (AIUNRegistrationLayoutTemplate.tsx)
    /// </summary>
    public class AiunRegistrationLayoutProperties : TemplateClientProperties
    {
        public AIUNRegistrationItemModel RegistrationItem { get; set; } = new AIUNRegistrationItemModel();
        public bool IsRegistrationExist { get; set; }
    }
}
