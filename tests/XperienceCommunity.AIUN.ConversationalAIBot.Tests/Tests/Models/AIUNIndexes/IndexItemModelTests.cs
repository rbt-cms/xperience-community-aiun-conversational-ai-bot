using NUnit.Framework;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models.AIUNIndexes;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Models.AIUNIndexes
{
    [TestFixture]
    public class IndexItemModelTests
    {
        [Test]
        public void Constructor_InitializesWithEmptyStringProperties()
        {
            // Act
            var model = new IndexItemModel();

            // Assert
            Assert.That(model.Id, Is.EqualTo(string.Empty));
            Assert.That(model.Name, Is.EqualTo(string.Empty));
            Assert.That(model.UploadedDate, Is.EqualTo(string.Empty));
            Assert.That(model.Title, Is.EqualTo(string.Empty));
            Assert.That(model.Status, Is.EqualTo(string.Empty));
            Assert.That(model.Category, Is.EqualTo(string.Empty));
            Assert.That(model.Department, Is.EqualTo(string.Empty));
        }

        [Test]
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
            Assert.That(model.Id, Is.EqualTo("123"));
            Assert.That(model.Name, Is.EqualTo("Test Name"));
            Assert.That(model.UploadedDate, Is.EqualTo("2024-06-01"));
            Assert.That(model.Title, Is.EqualTo("Test Title"));
            Assert.That(model.Status, Is.EqualTo("Active"));
            Assert.That(model.Category, Is.EqualTo("General"));
            Assert.That(model.Department, Is.EqualTo("IT"));
        }
    }
}
