using Microsoft.Extensions.DependencyInjection;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.IManagers;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.Managers;

namespace XperienceCommunity.AIUN.ConversationalAIBot
{
    public static class AiunChatbotModuleExtension
    {
        /// <summary>
        /// Adds all required services for AIUN Chatbot functionality
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddKenticoXperienceAIUNChatbot(this IServiceCollection services)
        {

            _ = services.AddHttpContextAccessor()
                .AddSingleton<AiunChatbotModuleInstaller>()
                .AddScoped<IDefaultChatbotManager, DefaultChatbotManager>()
                .AddScoped<IAiunApiManager, AiunApiManager>()
                .AddHttpClient<IAiunApiManager, AiunApiManager>();




            return services;
        }
    }
}
