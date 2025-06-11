using XperienceCommunity.AIUN.ConversationalAIBot;
using Kentico.Xperience.Admin.Base;
using CMS.Base;
using CMS.Core;
using Microsoft.Extensions.DependencyInjection;


[assembly: CMS.AssemblyDiscoverable]
[assembly: CMS.RegisterModule(typeof(AIUNChatbotModule))]

//// Adds a new application category 
//[assembly: UICategory(AIUNChatbotModule.CUSTOM_CATEGORY, "Custom", Icons.CustomElement, 100)]

namespace XperienceCommunity.AIUN.ConversationalAIBot
{
    public class AIUNChatbotModule : AdminModule
    {
        public const string CUSTOM_CATEGORY = "acme.web.admin.category";
        private AIUNChatbotModuleInstaller? installer;

        public AIUNChatbotModule() : base(nameof(AIUNChatbotModule))
        {
        }

        protected override void OnInit(ModuleInitParameters parameters)
        {
            base.OnInit(parameters);
            RegisterClientModule("rbt", "aiun-chatbot");

            var services = parameters.Services;

            installer = services.GetService<AIUNChatbotModuleInstaller>();

            if (installer != null)
            {
                ApplicationEvents.Initialized.Execute += InitializeModule;
            }
        }

        private void InitializeModule(object? sender, EventArgs e) =>
            installer?.Install();
    }
}

