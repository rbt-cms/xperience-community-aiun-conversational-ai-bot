using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models.AIUNIndexes;

using Xunit;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Models.AIUNIndexes
{
    public class IndexItemModelTests
    {
        [Fact]
        public void Constructor_InitializesWithNullProperties()
        {
            // Act
            var model = new IndexItemModel();

            // Assert
            Assert.Null(model.Id);
            Assert.Null(model.Name);
            Assert.Null(model.UploadedDate);
            Assert.Null(model.Title);
            Assert.Null(model.Status);
            Assert.Null(model.Category);
            Assert.Null(model.Department);
        }

        [Fact]
        public void Properties_SetAndGetValues()
        {
            // Arrange
            var model = new IndexItemModel
            {
                Id = "123",
                Name = "Test Name",
                UploadedDate = "2024-06-01",
                Title = "Test Title",
                Status = "Active",
                Category = "General",
                Department = "IT"
            };

            // Assert
            Assert.Equal("123", model.Id);
            Assert.Equal("Test Name", model.Name);
            Assert.Equal("2024-06-01", model.UploadedDate);
            Assert.Equal("Test Title", model.Title);
            Assert.Equal("Active", model.Status);
            Assert.Equal("General", model.Category);
            Assert.Equal("IT", model.Department);
        }
    }
}
