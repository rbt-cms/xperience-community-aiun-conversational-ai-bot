using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models.AIUNIndexes;

using System.Collections.Generic;

using Xunit;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Tests.Models.AIUNIndexes
{
    public class IndexesResponseModelTests
    {
        [Fact]
        public void Constructor_InitializesWithDefaultValues()
        {
            // Act
            var model = new IndexesResponseModel();

            // Assert
            Assert.NotNull(model.Items);
            Assert.Empty(model.Items);
            Assert.Equal(0, model.Total);
            Assert.Equal(1, model.Page);
            Assert.Equal(50, model.Size);
        }

        [Fact]
        public void Properties_SetAndGetValues()
        {
            // Arrange
            var items = new List<IndexItemModel>
            {
                new IndexItemModel { Id = "1", Name = "Test" }
            };
            var model = new IndexesResponseModel
            {
                Items = items,
                Total = 10,
                Page = 2,
                Size = 25
            };

            // Assert
            Assert.Equal(items, model.Items);
            Assert.Equal(10, model.Total);
            Assert.Equal(2, model.Page);
            Assert.Equal(25, model.Size);
        }
    }
}
