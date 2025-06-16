using CMS.Core;
using CMS.DataEngine;
using CMS.Websites.Routing;

using Microsoft.AspNetCore.Mvc.ViewComponents;

using Moq;

using NUnit.Framework;

using XperienceCommunity.AIUN.ConversationalAIBot.InfoClasses.AIUNConfigurationItem;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Components.ViewComponents.AIUNChatbot
{
    public class AIUNChatbotViewComponentTests
    {
        [Test]
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
            Assert.That(result, Is.InstanceOf<ViewViewComponentResult>()); // Updated to use NUnit's `Assert.That` with `Is.InstanceOf`
            var viewResult = (ViewViewComponentResult)result;
            Assert.That(viewResult.ViewData?.Model, Is.Not.Null);
            // Optionally, check for empty or fallback content  
        }

        [Test]
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
            Assert.That(result, Is.InstanceOf<ViewViewComponentResult>()); // Updated to use NUnit's `Assert.That` with `Is.InstanceOf`
            var viewResult = (ViewViewComponentResult)result;
            Assert.That(viewResult.ViewData?.Model, Is.Not.Null);
            // Optionally, validate the model content  
        }

        [Test]
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
            Assert.That(result, Is.InstanceOf<ViewViewComponentResult>()); // Updated to use NUnit's `Assert.That` with `Is.InstanceOf`
            var viewResult = (ViewViewComponentResult)result;
            Assert.That(viewResult.ViewData?.Model, Is.Not.Null);
            // Optionally, check for fallback content  
        }
    }
}
