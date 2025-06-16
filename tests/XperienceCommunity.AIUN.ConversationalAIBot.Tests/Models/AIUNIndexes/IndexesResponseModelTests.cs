using NUnit.Framework;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models.AIUNIndexes;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Models.AIUNIndexes
{
    [TestFixture]
    public class IndexesResponseModelTests
    {
        [Test]
        public void Constructor_InitializesWithDefaultValues()
        {
            // Act
            var model = new IndexesResponseModel();

            // Assert
            Assert.That(model.Items, Is.Not.Null);
            Assert.That(model.Items, Is.Empty);
            Assert.That(model.Total, Is.EqualTo(0));
            Assert.That(model.Page, Is.EqualTo(1));
            Assert.That(model.Size, Is.EqualTo(50));
        }

        [Test]
        public void Properties_SetAndGetValues()
        {
            // Arrange
            var items = new List<IndexItemModel>
            {
                new() { Id = "1", Name = "Test" }
            };
            var model = new IndexesResponseModel
            {
                Items = items,
                Total = 10,
                Page = 2,
                Size = 25
            };

            // Assert
            Assert.That(model.Items, Is.EqualTo(items));
            Assert.That(model.Total, Is.EqualTo(10));
            Assert.That(model.Page, Is.EqualTo(2));
            Assert.That(model.Size, Is.EqualTo(25));
        }
    }
}
