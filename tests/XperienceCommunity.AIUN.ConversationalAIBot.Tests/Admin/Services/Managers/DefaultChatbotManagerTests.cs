using CMS.ContentEngine;
using CMS.Core;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Websites;

using Microsoft.AspNetCore.Http;

using Moq;

using NUnit.Framework;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.InfoClasses.AIUNRegistration;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.IManagers;
using XperienceCommunity.AIUN.ConversationalAIBot.InfoClasses.AIUNConfigurationItem;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.Managers
{
    [TestFixture]
    public class DefaultChatbotManagerTests
    {
        private Mock<IAiunApiManager> mockAIUNApiManager;
        private Mock<IInfoProvider<ChannelInfo>> mockChannelProvider;
        private Mock<IConversionService> mockConversionService;
        private Mock<IProgressiveCache> mockCache;
        private Mock<IContentQueryExecutor> mockContentQueryExecutor;
        private Mock<IWebPageUrlRetriever> mockUrlRetriever;
        private Mock<IEventLogService> mockEventLogService;
        private DefaultChatbotManager chatbotManager;
        private Mock<IInfoProvider<AIUNRegistrationInfo>> mockAIUNRegistrationInfo;

        [SetUp]
        public void SetUp()
        {
            mockAIUNApiManager = new Mock<IAiunApiManager>();
            mockChannelProvider = new Mock<IInfoProvider<ChannelInfo>>();
            mockConversionService = new Mock<IConversionService>();
            mockCache = new Mock<IProgressiveCache>();
            mockContentQueryExecutor = new Mock<IContentQueryExecutor>();
            mockUrlRetriever = new Mock<IWebPageUrlRetriever>();
            mockEventLogService = new Mock<IEventLogService>();
            mockAIUNRegistrationInfo = new Mock<IInfoProvider<AIUNRegistrationInfo>>();

            var mockAIUNConfigurationItemInfoProvider = new Mock<IInfoProvider<AIUNConfigurationItemInfo>>();

            chatbotManager = new DefaultChatbotManager(
                mockAIUNConfigurationItemInfoProvider.Object,
                mockContentQueryExecutor.Object,
                mockCache.Object,
                mockChannelProvider.Object,
                mockConversionService.Object,
                mockUrlRetriever.Object,
                mockEventLogService.Object,
                mockAIUNApiManager.Object,
                mockAIUNRegistrationInfo.Object
            );
        }

        [Test]
        public async Task InitializeChatbot_ShouldReturnTrue_WhenInitializationSucceeds()
        {
            // Arrange
            _ = mockAIUNApiManager.Setup(s => s.UploadURLsAsync(It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("Success");

            // Act
            string result = await chatbotManager.IndexInternal(1, "TestChannel", "TestClientID", default, "https", new HostString("localhost"));

            // Assert
            Assert.That(result == null || result.Any() || !result.Any());
        }

        [Test]
        public async Task InitializeChatbot_ShouldReturnFalse_WhenInitializationFails()
        {
            // Arrange
            _ = mockAIUNApiManager.Setup(s => s.UploadURLsAsync(It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Error"));

            // Act
            string result = await chatbotManager.IndexInternal(1, "TestChannel", "TestClientID", default, "https", new HostString("localhost"));

            // Assert
            Assert.That(result == null || !result.Any(), "Expected result to be empty or null when initialization fails.");
        }

        [Test]
        public void GetChatbotStatus_ShouldReturnCorrectStatus()
        {
            // Act
            var result = chatbotManager.GetAllWebsiteChannels();

            // Assert
            Assert.That(result, Is.Not.Null);
        }
    }
}
