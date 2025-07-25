﻿using CMS.Base;
using CMS.Core;

using Kentico.Xperience.Admin.Base;

using Microsoft.Extensions.DependencyInjection;

using XperienceCommunity.AIUN.ConversationalAIBot;


[assembly: CMS.AssemblyDiscoverable]
[assembly: CMS.RegisterModule(typeof(AiunChatbotModule))]

//// Adds a new application category 
//[assembly: UICategory(AIUNChatbotModule.CUSTOM_CATEGORY, "Custom", Icons.CustomElement, 100)]

namespace XperienceCommunity.AIUN.ConversationalAIBot
{
    public class AiunChatbotModule : AdminModule
    {
        public const string CUSTOM_CATEGORY = "acme.web.admin.category";
        private AiunChatbotModuleInstaller? installer;

        public AiunChatbotModule() : base(nameof(AiunChatbotModule))
        {
        }

        protected override void OnInit(ModuleInitParameters parameters)
        {
            base.OnInit(parameters);
            RegisterClientModule("XperienceCommunity", "aiun-chatbot");

            var services = parameters.Services;

            installer = services.GetService<AiunChatbotModuleInstaller>();

            if (installer != null)
            {
                ApplicationEvents.Initialized.Execute += InitializeModule;
            }
        }
        private void InitializeModule(object? sender, EventArgs e) =>
            installer?.Install();
    }
}

