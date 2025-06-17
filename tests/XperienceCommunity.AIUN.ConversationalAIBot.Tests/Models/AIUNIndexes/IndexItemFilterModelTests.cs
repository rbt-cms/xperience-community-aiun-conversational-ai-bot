using NUnit.Framework;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models.AIUNIndexes;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Models.AIUNIndexes
{
    [TestFixture]
    public class IndexItemFilterModelTests
    {
        [Test]
        public void Constructor_InitializesWithDefaultValues()
        {
            // Act
            var model = new IndexItemFilterModel();

            // Assert
            Assert.That(model.Page, Is.EqualTo(1));
            Assert.That(model.PageSize, Is.EqualTo(10));
            Assert.That(model.SearchTerm, Is.EqualTo(string.Empty));
            Assert.That(model.SortBy, Is.EqualTo("uploaded_date"));
            Assert.That(model.SortDirection, Is.EqualTo("desc"));
            Assert.That(model.TypeFilter, Is.EqualTo("All"));
            Assert.That(model.Channel, Is.EqualTo(string.Empty));
        }

        [Test]
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
            Assert.That(model.Page, Is.EqualTo(3));
            Assert.That(model.PageSize, Is.EqualTo(25));
            Assert.That(model.SearchTerm, Is.EqualTo("test search"));
            Assert.That(model.SortBy, Is.EqualTo("name"));
            Assert.That(model.SortDirection, Is.EqualTo("asc"));
            Assert.That(model.TypeFilter, Is.EqualTo("Documents"));
            Assert.That(model.Channel, Is.EqualTo("Web"));
        }
    }
}
