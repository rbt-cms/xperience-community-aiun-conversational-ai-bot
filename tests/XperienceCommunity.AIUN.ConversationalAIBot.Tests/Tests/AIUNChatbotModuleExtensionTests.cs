using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using Moq;

using NUnit.Framework;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.InfoClasses.AIUNRegistration;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.IManagers;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Tests
{
    [TestFixture]
    public class AIUNChatbotModuleExtensionTests
    {
        [Test]
        public void AddKenticoXperienceAIUNChatbot_RegistersRequiredServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Register mocks for all required dependencies
            var mockResourceInfoProvider = new Mock<CMS.DataEngine.IInfoProvider<CMS.Modules.ResourceInfo>>();
            _ = services.AddSingleton(mockResourceInfoProvider.Object);

            var mockChannelInfoProvider = new Mock<CMS.DataEngine.IInfoProvider<CMS.ContentEngine.ChannelInfo>>();
            _ = services.AddSingleton(mockChannelInfoProvider.Object);

            var mockConversionService = new Mock<CMS.Core.IConversionService>();
            _ = services.AddSingleton(mockConversionService.Object);

            var mockProgressiveCache = new Mock<CMS.Helpers.IProgressiveCache>();
            _ = services.AddSingleton(mockProgressiveCache.Object);

            var mockConfigItemInfoProvider = new Mock<CMS.DataEngine.IInfoProvider<InfoClasses.AIUNConfigurationItem.AIUNConfigurationItemInfo>>();
            _ = services.AddSingleton(mockConfigItemInfoProvider.Object);

            var mockContentQueryExecutor = new Mock<CMS.ContentEngine.IContentQueryExecutor>();
            _ = services.AddSingleton(mockContentQueryExecutor.Object);

            var mockWebPageUrlRetriever = new Mock<CMS.Websites.IWebPageUrlRetriever>();
            _ = services.AddSingleton(mockWebPageUrlRetriever.Object);

            var mockEventLogService = new Mock<CMS.Core.IEventLogService>();
            _ = services.AddSingleton(mockEventLogService.Object);

            var mockAIUNRegistrationInfoProvider = new Mock<CMS.DataEngine.IInfoProvider<AIUNRegistrationInfo>>();
            _ = services.AddSingleton(mockAIUNRegistrationInfoProvider.Object);

            // Act
            _ = services.AddKenticoXperienceAIUNChatbot();
            var provider = services.BuildServiceProvider();

            // Assert
            Assert.That(provider.GetService<AiunChatbotModuleInstaller>(), Is.Not.Null);
            Assert.That(provider.GetService<IDefaultChatbotManager>(), Is.Not.Null);
            Assert.That(provider.GetService<IAiunApiManager>(), Is.Not.Null);
            Assert.That(provider.GetService<IHttpContextAccessor>(), Is.Not.Null);
        }
    }
}
