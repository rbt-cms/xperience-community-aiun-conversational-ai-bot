using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.IManagers;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.Managers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XperienceCommunity.AIUN.ConversationalAIBot
{
    public static class AIUNChatbotModuleExtension
    {
        /// <summary>
        /// Adds all required services for AIUN Chatbot functionality
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddKenticoXperienceAIUNChatbot(this IServiceCollection services)
        {

            services.AddHttpContextAccessor()
                .AddSingleton<AIUNChatbotModuleInstaller>()
                .AddScoped<IDefaultChatbotManager, DefaultChatbotManager>()
                .AddScoped<IAIUNApiManager, AIUNApiManager>()
                .AddHttpClient<IAIUNApiManager, AIUNApiManager>();




            return services;
        }
    }
}
