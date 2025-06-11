using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using XperienceCommunity.AIUN.ConversationalAIBot;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.IManagers;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.Managers;
using Microsoft.AspNetCore.Http;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Tests
{
    public class AIUNChatbotModuleExtensionTests
    {
        [Fact]
        public void AddKenticoXperienceAIUNChatbot_RegistersRequiredServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Register mocks for all required dependencies
            var mockResourceInfoProvider = new Moq.Mock<CMS.DataEngine.IInfoProvider<CMS.Modules.ResourceInfo>>();
            services.AddSingleton(mockResourceInfoProvider.Object);

            var mockChannelInfoProvider = new Moq.Mock<CMS.DataEngine.IInfoProvider<CMS.ContentEngine.ChannelInfo>>();
            services.AddSingleton(mockChannelInfoProvider.Object);

            var mockConversionService = new Moq.Mock<CMS.Core.IConversionService>();
            services.AddSingleton(mockConversionService.Object);

            var mockProgressiveCache = new Moq.Mock<CMS.Helpers.IProgressiveCache>();
            services.AddSingleton(mockProgressiveCache.Object);

            var mockConfigItemInfoProvider = new Moq.Mock<CMS.DataEngine.IInfoProvider<XperienceCommunity.AIUN.ConversationalAIBot.InfoClasses.AIUNConfigurationItem.AIUNConfigurationItemInfo>>();
            services.AddSingleton(mockConfigItemInfoProvider.Object);

            var mockContentQueryExecutor = new Moq.Mock<CMS.ContentEngine.IContentQueryExecutor>();
            services.AddSingleton(mockContentQueryExecutor.Object);

            var mockWebPageUrlRetriever = new Moq.Mock<CMS.Websites.IWebPageUrlRetriever>();
            services.AddSingleton(mockWebPageUrlRetriever.Object);

            var mockEventLogService = new Moq.Mock<CMS.Core.IEventLogService>();
            services.AddSingleton(mockEventLogService.Object);

            // Register missing getundex
            var mockSettingsKeyInfoProvider = new Moq.Mock<CMS.DataEngine.IInfoProvider<XperienceCommunity.AIUN.ConversationalAIBot.AIUNSettingsKeyInfo>>();
            services.AddSingleton(mockSettingsKeyInfoProvider.Object);

            // Act
            services.AddKenticoXperienceAIUNChatbot();
            var provider = services.BuildServiceProvider();

            // Assert
            Assert.NotNull(provider.GetService<AIUNChatbotModuleInstaller>());
            Assert.NotNull(provider.GetService<IDefaultChatbotManager>());
            Assert.NotNull(provider.GetService<IAIUNApiManager>());
            Assert.NotNull(provider.GetService<IHttpContextAccessor>());
        }

    }
}

