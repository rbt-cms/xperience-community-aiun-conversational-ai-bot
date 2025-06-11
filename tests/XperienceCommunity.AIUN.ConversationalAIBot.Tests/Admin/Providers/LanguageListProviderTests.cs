using CMS.ContentEngine;
using CMS.DataEngine;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Providers;

using Moq;
using Moq.Protected;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Tests.Admin.Providers
{
    public class LanguageListProviderTests
    {
        private readonly Mock<IInfoProvider<ContentLanguageInfo>> mockContentLanguageInfoProvider;
        private readonly LanguageListProvider languageListProvider;

        public LanguageListProviderTests()
        {
            mockContentLanguageInfoProvider = new Mock<IInfoProvider<ContentLanguageInfo>>();
            languageListProvider = new LanguageListProvider(mockContentLanguageInfoProvider.Object);
        }

        [Fact]
        public async Task GetItemsAsync_ReturnsPagedItems()
        {
            // Arrange
            var mockData = new List<ContentLanguageInfo>
        {
            CreateStubContentLanguageInfo("en", "English"),
            CreateStubContentLanguageInfo("fr", "French")
        };

            var mockQuery = CreateMockQuery(mockData);

            mockContentLanguageInfoProvider
                .Setup(provider => provider.Get())
                .Returns(mockQuery.Object);

            // Act
            var result = await languageListProvider.GetItemsAsync(null, 0, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count());
            Assert.Contains(result.Items, x => x.Value == "en" && x.Text == "English");
        }

        [Fact]
        public async Task GetItemsAsync_FiltersBySearchTerm()
        {
            // Arrange
            var mockData = new List<ContentLanguageInfo>
    {
        CreateStubContentLanguageInfo("en", "English"),
        CreateStubContentLanguageInfo("fr", "French")
    };

            var filteredData = mockData.Where(x => x.ContentLanguageDisplayName.Contains("Eng")).ToList();

            var mockQuery = CreateMockQuery(filteredData);
            mockQuery.Setup(q => q.WhereStartsWith(It.IsAny<string>(), It.IsAny<string>())).Returns(mockQuery.Object);
            mockQuery.Setup(q => q.Page(It.IsAny<int>(), It.IsAny<int>())).Returns(mockQuery.Object);

            mockContentLanguageInfoProvider
                .Setup(provider => provider.Get())
                .Returns(mockQuery.Object);

            // Act
            var result = await languageListProvider.GetItemsAsync("Eng", 0, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal("en", result.Items.First().Value);
        }

        [Fact]
        public async Task GetSelectedItemsAsync_ReturnsSelectedItems()
        {
            // Arrange
            var mockData = new List<ContentLanguageInfo>
        {
            CreateStubContentLanguageInfo("en", "English"),
            CreateStubContentLanguageInfo("fr", "French")
        };

            var mockQuery = CreateMockQuery(mockData);
            mockQuery.Setup(q => q.Page(It.IsAny<int>(), It.IsAny<int>())).Returns(mockQuery.Object);

            mockContentLanguageInfoProvider
                .Setup(provider => provider.Get())
                .Returns(mockQuery.Object);

            var selectedValues = new List<string> { "en" };

            // Act
            var result = await languageListProvider.GetSelectedItemsAsync(selectedValues, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("en", result.First().Value);
        }

        // Helper to create a stub ContentLanguageInfo
        private static ContentLanguageInfo CreateStubContentLanguageInfo(string name, string displayName)
        {
            return new StubContentLanguageInfo
            {
                ContentLanguageNameValue = name,
                ContentLanguageDisplayNameValue = displayName
            };
        }

        private class StubContentLanguageInfo : ContentLanguageInfo
        {
            public string ContentLanguageNameValue { get; set; } = string.Empty;
            public string ContentLanguageDisplayNameValue { get; set; } = string.Empty;

            public override string ContentLanguageName
            {
                get => ContentLanguageNameValue;
                set => ContentLanguageNameValue = value;
            }

            public override string ContentLanguageDisplayName
            {
                get => ContentLanguageDisplayNameValue;
                set => ContentLanguageDisplayNameValue = value;
            }
        }

        private static Mock<ObjectQuery<ContentLanguageInfo>> CreateMockQuery(List<ContentLanguageInfo> items)
        {
            var mockQuery = new Mock<ObjectQuery<ContentLanguageInfo>>(MockBehavior.Strict);
            mockQuery.SetupProperty(q => q.ClassName, It.IsAny<string>());
            mockQuery.SetupProperty(q => q.QueryName, It.IsAny<string>());
            mockQuery.SetupProperty(q => q.ObjectType, It.IsAny<string>());
            mockQuery.Protected().Setup("TypeUpdated");
            mockQuery.Setup(q => q.GetEnumerableTypedResultAsync(
                It.IsAny<System.Data.CommandBehavior>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken?>()))
                .ReturnsAsync(items);

            mockQuery.Setup(q => q.IsPagedQuery).Returns(true);
            mockQuery.Setup(q => q.TotalRecords).Returns(items.Count);
            mockQuery.Setup(q => q.Offset).Returns(0);
            mockQuery.Setup(q => q.MaxRecords).Returns(0);

            // Add this line to allow .Page() to be called in tests
            mockQuery.Setup(q => q.Page(It.IsAny<int>(), It.IsAny<int>())).Returns(mockQuery.Object);

            return mockQuery;
        }
    }
}
