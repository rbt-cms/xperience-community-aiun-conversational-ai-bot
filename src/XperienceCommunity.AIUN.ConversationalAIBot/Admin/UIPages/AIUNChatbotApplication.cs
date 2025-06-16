using CMS.Membership;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.UIPages;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages;

[assembly:
    UIApplication(
    identifier: AiunChatbotApplication.IDENTIFIER,
    type: typeof(AiunChatbotApplication),
    slug: "aiunchatbot",
    name: "AIUN Chatbot",
    category: BaseApplicationCategories.CONFIGURATION,
    icon: Icons.Bubbles,
    templateName: TemplateNames.SECTION_LAYOUT)]
namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages
{
    [UIPermission(SystemPermissions.VIEW)]
    public class AiunChatbotApplication : ApplicationPage
    {
        public const string IDENTIFIER = "XperienceCommunity.AIUN.ConversationalAIBot";
    }
}
