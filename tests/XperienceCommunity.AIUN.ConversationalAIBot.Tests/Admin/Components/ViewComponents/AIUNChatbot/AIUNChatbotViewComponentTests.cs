using CMS.Core;
using CMS.DataEngine;
using CMS.Websites.Routing;

using Microsoft.AspNetCore.Mvc.ViewComponents;

using Moq;

using XperienceCommunity.AIUN.ConversationalAIBot.InfoClasses.AIUNConfigurationItem;

using Xunit;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Components.ViewComponents.AIUNChatbot
{
    public class AIUNChatbotViewComponentTests
    {
        [Fact]
        public async Task InvokeAsync_ReturnsViewEvenWithNoConfig()
        {
            // Arrange  
            var mockAIUNConfigurationItemInfoProvider = new Mock<IInfoProvider<AIUNConfigurationItemInfo>>();
            var mockWebsiteChannelContext = new Mock<IWebsiteChannelContext>();
            var mockEventLogService = new Mock<IEventLogService>();

            _ = mockWebsiteChannelContext.Setup(x => x.WebsiteChannelName).Returns("TestChannel");
            _ = mockAIUNConfigurationItemInfoProvider
                .Setup(x => x.Get())
                .Returns(new ObjectQuery<AIUNConfigurationItemInfo>());

            var component = new AiunChatbotViewComponent(
                mockAIUNConfigurationItemInfoProvider.Object,
                mockWebsiteChannelContext.Object,
                mockEventLogService.Object
            );

            // Act  
            var result = await component.InvokeAsync();

            // Assert  
            var viewResult = Assert.IsType<ViewViewComponentResult>(result);
            Assert.NotNull(viewResult.ViewData?.Model);
            // Optionally, check for empty or fallback content  
        }

        [Fact]
        public async Task InvokeAsync_ReturnsViewWithValidConfig()
        {
            // Arrange  
            var mockAIUNConfigurationItemInfoProvider = new Mock<IInfoProvider<AIUNConfigurationItemInfo>>();
            var mockWebsiteChannelContext = new Mock<IWebsiteChannelContext>();
            var mockEventLogService = new Mock<IEventLogService>();

            _ = mockWebsiteChannelContext.Setup(x => x.WebsiteChannelName).Returns("TestChannel");
            _ = mockAIUNConfigurationItemInfoProvider
                .Setup(x => x.Get())
                .Returns(new ObjectQuery<AIUNConfigurationItemInfo>().WhereEquals("ChannelName", "TestChannel"));

            var component = new AiunChatbotViewComponent(
                mockAIUNConfigurationItemInfoProvider.Object,
                mockWebsiteChannelContext.Object,
                mockEventLogService.Object
            );

            // Act  
            var result = await component.InvokeAsync();

            // Assert  
            var viewResult = Assert.IsType<ViewViewComponentResult>(result);
            Assert.NotNull(viewResult.ViewData?.Model);
            // Optionally, validate the model content  
        }

        [Fact]
        public async Task InvokeAsync_LogsErrorWhenExceptionOccurs()
        {
            // Arrange  
            var mockAIUNConfigurationItemInfoProvider = new Mock<IInfoProvider<AIUNConfigurationItemInfo>>();
            var mockWebsiteChannelContext = new Mock<IWebsiteChannelContext>();
            var mockEventLogService = new Mock<IEventLogService>();

            _ = mockWebsiteChannelContext.Setup(x => x.WebsiteChannelName).Returns("TestChannel");
            _ = mockAIUNConfigurationItemInfoProvider
                .Setup(x => x.Get())
                .Throws(new Exception("Test exception"));

            var component = new AiunChatbotViewComponent(
                mockAIUNConfigurationItemInfoProvider.Object,
                mockWebsiteChannelContext.Object,
                mockEventLogService.Object
            );

            // Act  
            var result = await component.InvokeAsync();

            // Assert  
            mockEventLogService.Verify(x => x.LogEvent(It.IsAny<EventLogData>()), Times.Once);
            var viewResult = Assert.IsType<ViewViewComponentResult>(result);
            Assert.NotNull(viewResult.ViewData?.Model);
            // Optionally, check for fallback content  
        }
    }
}

