using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models.AIUNIndexes;

using Xunit;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Tests.Models.AIUNIndexes
{
    public class IndexItemFilterModelTests
    {
        [Fact]
        public void Constructor_InitializesWithDefaultValues()
        {
            // Act
            var model = new IndexItemFilterModel();

            // Assert
            Assert.Equal(1, model.Page);
            Assert.Equal(10, model.PageSize);
            Assert.Equal(string.Empty, model.SearchTerm);
            Assert.Equal("uploaded_date", model.SortBy);
            Assert.Equal("desc", model.SortDirection);
            Assert.Equal("All", model.TypeFilter);
            Assert.Null(model.Channel);
        }

        [Fact]
        public void Properties_SetAndGetValues()
        {
            // Arrange
            var model = new IndexItemFilterModel
            {
                Page = 3,
                PageSize = 25,
                SearchTerm = "test search",
                SortBy = "name",
                SortDirection = "asc",
                TypeFilter = "Documents",
                Channel = "Web"
            };

            // Assert
            Assert.Equal(3, model.Page);
            Assert.Equal(25, model.PageSize);
            Assert.Equal("test search", model.SearchTerm);
            Assert.Equal("name", model.SortBy);
            Assert.Equal("asc", model.SortDirection);
            Assert.Equal("Documents", model.TypeFilter);
            Assert.Equal("Web", model.Channel);
        }
    }
}
