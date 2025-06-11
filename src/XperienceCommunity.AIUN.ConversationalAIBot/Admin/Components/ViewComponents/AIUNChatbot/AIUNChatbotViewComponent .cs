using CMS.Core;
using CMS.DataEngine;
using CMS.Websites.Routing;
using XperienceCommunity.AIUN.ConversationalAIBot.InfoClasses.AIUNConfigurationItem;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Components.ViewComponents.AIUNChatbot
{
    public class AiunChatbotViewComponent : ViewComponent
    {
        private readonly IInfoProvider<AIUNConfigurationItemInfo> aIUNConfigurationItemInfoProvider;
        private readonly IWebsiteChannelContext websiteChannelContext;
        private readonly IEventLogService eventLogService;
        public AiunChatbotViewComponent(IInfoProvider<AIUNConfigurationItemInfo> aIUNConfigurationItemInfoProviderParam,
            IWebsiteChannelContext websiteChannelContextParam, IEventLogService eventLogServiceParam)
        {
            aIUNConfigurationItemInfoProvider = aIUNConfigurationItemInfoProviderParam;
            websiteChannelContext = websiteChannelContextParam;
            eventLogService = eventLogServiceParam;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IHtmlContent scriptHtml = HtmlString.Empty;

            try
            {
                var aiunConfigurationItem = aIUNConfigurationItemInfoProvider.Get()
                    .Where(a => a.ChannelName == websiteChannelContext.WebsiteChannelName)
                    .FirstOrDefault();

                if (aiunConfigurationItem != null)
                {
                    string script = aiunConfigurationItem.BaseURL.ToLower().Contains("</script>") ? aiunConfigurationItem.BaseURL : ("<script src='"
                                 + aiunConfigurationItem.BaseURL.TrimEnd('/') + "/chat.js?"
                                 + "key=" + aiunConfigurationItem.APIKey
                                 + "&client_id=" + aiunConfigurationItem.ClientID
                                 + "'></script>");
                    scriptHtml = new HtmlString(script);
                }
            }
            catch (Exception ex)
            {
                eventLogService.LogException("AIUNChatbotViewComponent", "Load script failed", ex);
            }
            return View("Components/AiunChatbot/Default.cshtml", scriptHtml);
        }
    }
}
