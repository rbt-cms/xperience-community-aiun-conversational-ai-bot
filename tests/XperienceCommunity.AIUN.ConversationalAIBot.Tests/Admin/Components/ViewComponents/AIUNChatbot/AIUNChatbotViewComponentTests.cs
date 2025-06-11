using CMS.Core;
using CMS.DataEngine;
using CMS.Websites.Routing;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Components.ViewComponents.AIUNChatbot;
using XperienceCommunity.AIUN.ConversationalAIBot.InfoClasses.AIUNConfigurationItem;

using Microsoft.AspNetCore.Mvc.ViewComponents;

using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Tests.Admin.Components.ViewComponents.AIUNChatbot
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

            mockWebsiteChannelContext.Setup(x => x.WebsiteChannelName).Returns("TestChannel");
            mockAIUNConfigurationItemInfoProvider
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

            mockWebsiteChannelContext.Setup(x => x.WebsiteChannelName).Returns("TestChannel");
            mockAIUNConfigurationItemInfoProvider
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

            mockWebsiteChannelContext.Setup(x => x.WebsiteChannelName).Returns("TestChannel");
            mockAIUNConfigurationItemInfoProvider
                .Setup(x => x.Get())
                .Throws(new System.Exception("Test exception"));

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

