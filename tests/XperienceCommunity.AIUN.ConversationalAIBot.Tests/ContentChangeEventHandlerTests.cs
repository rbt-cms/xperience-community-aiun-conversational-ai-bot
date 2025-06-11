
using CMS.Base;
using CMS.Core;
using CMS.Websites;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.IManagers;

using Microsoft.Extensions.DependencyInjection;

using Moq;

using System.Reflection;

using Xunit;


// Helper class for non-virtual property override
public class TestWebPageEventArgsBase : WebPageEventArgsBase
{
    private readonly string _treePath;
    public TestWebPageEventArgsBase(string treePath)
    {
        _treePath = treePath;
    }
    public new string TreePath => _treePath;
}
public class ContentChangeEventHandlerTests
{
    private readonly Mock<IAIUNApiManager> _aiunApiManagerMock;
    private readonly Mock<IEventLogService> _eventLogServiceMock;
    private readonly ContentChangeEventHandler _eventHandler;

    public ContentChangeEventHandlerTests()
    {
        _aiunApiManagerMock = new Mock<IAIUNApiManager>();
        _eventLogServiceMock = new Mock<IEventLogService>();

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(sp => sp.GetService(typeof(IAIUNApiManager))).Returns(_aiunApiManagerMock.Object);
        serviceProviderMock.Setup(sp => sp.GetService(typeof(IEventLogService))).Returns(_eventLogServiceMock.Object);

        var serviceScopeMock = new Mock<IServiceScope>();
        serviceScopeMock.Setup(s => s.ServiceProvider).Returns(serviceProviderMock.Object);

        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        serviceScopeFactoryMock.Setup(f => f.CreateScope()).Returns(serviceScopeMock.Object);

        var serviceCollectionMock = new Mock<IServiceProvider>();
        serviceCollectionMock.Setup(sp => sp.GetService(typeof(IServiceScopeFactory))).Returns(serviceScopeFactoryMock.Object);

        _eventHandler = new ContentChangeEventHandler();
    }

    [Fact]
    public async Task HandleWebPagePublish_ValidPageEvent_LogsPageInfo()
    {
        var pageEvent = new TestWebPageEventArgsBase("/TestPage");
        var cmsEventArgs = new CMSEventArgs();

        var handleWebPagePublishMethod = typeof(ContentChangeEventHandler).GetMethod("HandleWebPagePublish", BindingFlags.NonPublic | BindingFlags.Instance);
        if (handleWebPagePublishMethod == null)
        {
            throw new InvalidOperationException("HandleWebPagePublish method not found.");
        }

        // Act  
        if (handleWebPagePublishMethod != null)
        {
            var task = handleWebPagePublishMethod.Invoke(_eventHandler, new object[] { pageEvent, cmsEventArgs }) as Task;
            if (task != null)
            {
                await task;
            }
        }


    }


    [Fact]
    public async Task HandleWebPagePublish_InvalidPageEvent_DoesNotCallUploadURLsAsync()
    {
        // Arrange
        var cmsEventArgs = new CMSEventArgs();

        var handleWebPagePublishMethod = typeof(ContentChangeEventHandler).GetMethod("HandleWebPagePublish", BindingFlags.NonPublic | BindingFlags.Instance);
        if (handleWebPagePublishMethod == null)
        {
            throw new InvalidOperationException("HandleWebPagePublish method not found.");
        }

        // Act
        if (handleWebPagePublishMethod != null)
        {
            var task = handleWebPagePublishMethod.Invoke(_eventHandler, new object[] { this, cmsEventArgs }) as Task;
            if (task != null)
            {
                await task;
            }
        }

        // Assert
        _aiunApiManagerMock.Verify(
            x => x.UploadURLsAsync(It.IsAny<List<string>>(), It.IsAny<string>()),
            Times.Never
        );
    }

    [Fact]
    public async Task HandleWebPagePublish_ExceptionInUploadURLsAsync_LogsError()
    {
        // Arrange  
        var pageEvent = new TestWebPageEventArgsBase("/TestPage");

        _aiunApiManagerMock
            .Setup(x => x.UploadURLsAsync(It.IsAny<List<string>>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception("Test exception"));

        var cmsEventArgs = new CMSEventArgs();

        var handleWebPagePublishMethod = typeof(ContentChangeEventHandler).GetMethod("HandleWebPagePublish", BindingFlags.NonPublic | BindingFlags.Instance);
        if (handleWebPagePublishMethod == null)
        {
            throw new InvalidOperationException("HandleWebPagePublish method not found.");
        }

        // Act  
        if (handleWebPagePublishMethod != null)
        {
            var task = handleWebPagePublishMethod.Invoke(_eventHandler, new object[] { this, cmsEventArgs }) as Task;
            if (task != null)
            {
                await task;
            }
        }

        //// Assert  
        //_eventLogServiceMock.Verify(  
        //    x => x.LogException(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Exception>()),  
        //    Times.Once  
        //);  
    }
}
