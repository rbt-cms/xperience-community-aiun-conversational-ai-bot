using CMS.ContentEngine;
using CMS.DataEngine;

using Moq;
using Moq.Protected;

using NUnit.Framework;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Providers
{
    public class LanguageListProviderTests
    {
        private Mock<IInfoProvider<ContentLanguageInfo>> mockContentLanguageInfoProvider;
        private LanguageListProvider languageListProvider;

        [SetUp]
        public void SetUp()
        {
            mockContentLanguageInfoProvider = new Mock<IInfoProvider<ContentLanguageInfo>>();
            languageListProvider = new LanguageListProvider(mockContentLanguageInfoProvider.Object);
        }

        [Test]
        public async Task GetItemsAsync_ReturnsPagedItems()
        {
            // Arrange
            var mockData = new List<ContentLanguageInfo>
            {
                CreateStubContentLanguageInfo("en", "English"),
                CreateStubContentLanguageInfo("fr", "French")
            };

            var mockQuery = CreateMockQuery(mockData);

            _ = mockContentLanguageInfoProvider
                .Setup(provider => provider.Get())
                .Returns(mockQuery.Object);

            // Act
            var result = await languageListProvider.GetItemsAsync(null, 0, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Items.Count(), Is.EqualTo(2));
            Assert.That(result.Items, Has.Some.Matches<dynamic>(x => x.Value == "en" && x.Text == "English"));
        }

        [Test]
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
            _ = mockQuery.Setup(q => q.WhereStartsWith(It.IsAny<string>(), It.IsAny<string>())).Returns(mockQuery.Object);
            _ = mockQuery.Setup(q => q.Page(It.IsAny<int>(), It.IsAny<int>())).Returns(mockQuery.Object);

            _ = mockContentLanguageInfoProvider
                .Setup(provider => provider.Get())
                .Returns(mockQuery.Object);

            // Act
            var result = await languageListProvider.GetItemsAsync("Eng", 0, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Items.Count(), Is.EqualTo(1));
            Assert.That(result.Items.First().Value, Is.EqualTo("en"));
        }

        [Test]
        public async Task GetSelectedItemsAsync_ReturnsSelectedItems()
        {
            // Arrange
            var mockData = new List<ContentLanguageInfo>
            {
                CreateStubContentLanguageInfo("en", "English"),
                CreateStubContentLanguageInfo("fr", "French")
            };

            var mockQuery = CreateMockQuery(mockData);
            _ = mockQuery.Setup(q => q.Page(It.IsAny<int>(), It.IsAny<int>())).Returns(mockQuery.Object);

            _ = mockContentLanguageInfoProvider
                .Setup(provider => provider.Get())
                .Returns(mockQuery.Object);

            var selectedValues = new List<string> { "en" };

            // Act
            var result = await languageListProvider.GetSelectedItemsAsync(selectedValues, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Value, Is.EqualTo("en"));
        }

        // Helper to create a stub ContentLanguageInfo
        private static ContentLanguageInfo CreateStubContentLanguageInfo(string name, string displayName) => new StubContentLanguageInfo
        {
            ContentLanguageNameValue = name,
            ContentLanguageDisplayNameValue = displayName
        };

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
            _ = mockQuery.SetupProperty(q => q.ClassName, It.IsAny<string>());
            _ = mockQuery.SetupProperty(q => q.QueryName, It.IsAny<string>());
            _ = mockQuery.SetupProperty(q => q.ObjectType, It.IsAny<string>());
            _ = mockQuery.Protected().Setup("TypeUpdated");
            _ = mockQuery.Setup(q => q.GetEnumerableTypedResultAsync(
                It.IsAny<System.Data.CommandBehavior>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken?>()))
                .ReturnsAsync(items);

            _ = mockQuery.Setup(q => q.IsPagedQuery).Returns(true);
            _ = mockQuery.Setup(q => q.TotalRecords).Returns(items.Count);
            _ = mockQuery.Setup(q => q.Offset).Returns(0);
            _ = mockQuery.Setup(q => q.MaxRecords).Returns(0);

            // Add this line to allow .Page() to be called in tests
            _ = mockQuery.Setup(q => q.Page(It.IsAny<int>(), It.IsAny<int>())).Returns(mockQuery.Object);

            return mockQuery;
        }
    }
}
