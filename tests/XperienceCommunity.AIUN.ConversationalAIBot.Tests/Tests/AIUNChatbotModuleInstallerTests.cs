using CMS.DataEngine;
using CMS.Modules;

using Moq;

using NUnit.Framework;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Tests
{
    [TestFixture]
    public class AIUNChatbotModuleInstallerTests
    {
        [Test]
        public void InitializeResource_CallsSet_WhenChanged()
        {
            // Arrange
            var resourceMock = new Mock<ResourceInfo>();
            _ = resourceMock.Setup(r => r.HasChanged).Returns(true);

            var resourceProviderMock = new Mock<IInfoProvider<ResourceInfo>>();
            resourceProviderMock.Setup(p => p.Set(resourceMock.Object)).Verifiable();

            var installer = new AiunChatbotModuleInstaller(resourceProviderMock.Object);

            // Act
            _ = installer.InitializeResource(resourceMock.Object);

            // Assert
            resourceProviderMock.Verify(p => p.Set(resourceMock.Object), Times.Once);
        }

        [Test]
        public void InitializeResource_DoesNotCallSet_WhenNotChanged()
        {
            // Arrange
            var resourceMock = new Mock<ResourceInfo>();
            _ = resourceMock.Setup(r => r.HasChanged).Returns(false);

            // Use reflection to set property values directly, since they are not virtual
            var resource = resourceMock.Object;
            typeof(ResourceInfo).GetProperty("ResourceDisplayName")?.SetValue(resource, null);
            typeof(ResourceInfo).GetProperty("ResourceName")?.SetValue(resource, null);
            typeof(ResourceInfo).GetProperty("ResourceDescription")?.SetValue(resource, null);
            typeof(ResourceInfo).GetProperty("ResourceIsInDevelopment")?.SetValue(resource, false);

            var resourceProviderMock = new Mock<IInfoProvider<ResourceInfo>>();
            var installer = new AiunChatbotModuleInstaller(resourceProviderMock.Object);

            // Act
            _ = installer.InitializeResource(resource);

            // Assert
            resourceProviderMock.Verify(p => p.Set(It.IsAny<ResourceInfo>()), Times.Never);
        }
    }
}
