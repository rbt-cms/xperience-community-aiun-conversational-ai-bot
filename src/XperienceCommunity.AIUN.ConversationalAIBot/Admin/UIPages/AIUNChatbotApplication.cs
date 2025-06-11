using Kentico.Xperience.Admin.Base.UIPages;
using Kentico.Xperience.Admin.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Membership;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages;

[assembly:
    UIApplication(
    identifier: AIUNChatbotApplication.IDENTIFIER,
    type: typeof(AIUNChatbotApplication),
    slug: "aiunchatbot",
    name: "AIUN Chatbot",
    category: BaseApplicationCategories.CONFIGURATION,
    icon: Icons.Earth,
    templateName: TemplateNames.SECTION_LAYOUT)]
namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages
{
    [UIPermission(SystemPermissions.VIEW)]
    public class AIUNChatbotApplication : ApplicationPage
    {
        public const string IDENTIFIER = "XperienceCommunity.AIUN.ConversationalAIBot";
    }
}
