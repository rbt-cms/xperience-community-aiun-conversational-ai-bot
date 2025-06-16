using System.Reflection;

using CMS.Base;
using CMS.Core;
using CMS.Websites;

using Microsoft.Extensions.DependencyInjection;

using Moq;

using NUnit.Framework;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.IManagers;

namespace XperienceCommunity.AIUN.ConversationalAIBot
{
    // Helper class for non-virtual property override
    public class TestWebPageEventArgsBase : WebPageEventArgsBase
    {
        public TestWebPageEventArgsBase(string treePath) => TreePath = treePath;
        public new string TreePath { get; }
    }

    [TestFixture]
    public class ContentChangeEventHandlerTests
    {
        private Mock<IAiunApiManager> aiunApiManagerMock;
        private Mock<IEventLogService> eventLogServiceMock;
        private ContentChangeEventHandler eventHandler;

        [SetUp]
        public void SetUp()
        {
            aiunApiManagerMock = new Mock<IAiunApiManager>();
            eventLogServiceMock = new Mock<IEventLogService>();

            var serviceProviderMock = new Mock<IServiceProvider>();
            _ = serviceProviderMock.Setup(sp => sp.GetService(typeof(IAiunApiManager))).Returns(aiunApiManagerMock.Object);
            _ = serviceProviderMock.Setup(sp => sp.GetService(typeof(IEventLogService))).Returns(eventLogServiceMock.Object);

            var serviceScopeMock = new Mock<IServiceScope>();
            _ = serviceScopeMock.Setup(s => s.ServiceProvider).Returns(serviceProviderMock.Object);

            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            _ = serviceScopeFactoryMock.Setup(f => f.CreateScope()).Returns(serviceScopeMock.Object);

            var serviceCollectionMock = new Mock<IServiceProvider>();
            _ = serviceCollectionMock.Setup(sp => sp.GetService(typeof(IServiceScopeFactory))).Returns(serviceScopeFactoryMock.Object);

            eventHandler = new ContentChangeEventHandler();
        }

        [Test]
        public async Task HandleWebPagePublish_InvalidPageEvent_DoesNotCallUploadURLsAsync()
        {
            // Arrange  
            var cmsEventArgs = new CMSEventArgs();

            var handleWebPagePublishMethod = typeof(ContentChangeEventHandler).GetMethod("HandleWebPagePublish", BindingFlags.NonPublic | BindingFlags.Instance)
                ?? throw new InvalidOperationException("HandleWebPagePublish method not found.");

            // Act  
            if (handleWebPagePublishMethod.Invoke(eventHandler, new object[] { cmsEventArgs }) is Task task)
            {
                await task;
            }

            // Assert  
            aiunApiManagerMock.Verify(
                x => x.UploadURLsAsync(It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Never
            );
        }
    }
}
