using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.InfoClasses.AIUNRegistration;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.IManagers;

using Xunit;

namespace XperienceCommunity.AIUN.ConversationalAIBot
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
            _ = services.AddSingleton(mockResourceInfoProvider.Object);

            var mockChannelInfoProvider = new Moq.Mock<CMS.DataEngine.IInfoProvider<CMS.ContentEngine.ChannelInfo>>();
            _ = services.AddSingleton(mockChannelInfoProvider.Object);

            var mockConversionService = new Moq.Mock<CMS.Core.IConversionService>();
            _ = services.AddSingleton(mockConversionService.Object);

            var mockProgressiveCache = new Moq.Mock<CMS.Helpers.IProgressiveCache>();
            _ = services.AddSingleton(mockProgressiveCache.Object);

            var mockConfigItemInfoProvider = new Moq.Mock<CMS.DataEngine.IInfoProvider<InfoClasses.AIUNConfigurationItem.AIUNConfigurationItemInfo>>();
            _ = services.AddSingleton(mockConfigItemInfoProvider.Object);

            var mockContentQueryExecutor = new Moq.Mock<CMS.ContentEngine.IContentQueryExecutor>();
            _ = services.AddSingleton(mockContentQueryExecutor.Object);

            var mockWebPageUrlRetriever = new Moq.Mock<CMS.Websites.IWebPageUrlRetriever>();
            _ = services.AddSingleton(mockWebPageUrlRetriever.Object);

            var mockEventLogService = new Moq.Mock<CMS.Core.IEventLogService>();
            _ = services.AddSingleton(mockEventLogService.Object);

            var mockAIUNRegistrationInfoProvider = new Moq.Mock<CMS.DataEngine.IInfoProvider<AIUNRegistrationInfo>>();
            _ = services.AddSingleton(mockAIUNRegistrationInfoProvider.Object);

            // Act
            _ = services.AddKenticoXperienceAIUNChatbot();
            var provider = services.BuildServiceProvider();

            // Assert
            Assert.NotNull(provider.GetService<AiunChatbotModuleInstaller>());
            Assert.NotNull(provider.GetService<IDefaultChatbotManager>());
            Assert.NotNull(provider.GetService<IAiunApiManager>());
            Assert.NotNull(provider.GetService<IHttpContextAccessor>());
        }

    }
}

