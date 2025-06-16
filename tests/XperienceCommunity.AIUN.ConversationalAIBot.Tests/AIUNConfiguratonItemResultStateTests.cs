using NUnit.Framework;

namespace XperienceCommunity.AIUN.ConversationalAIBot
{
    [TestFixture]
    public class AIUNConfiguratonItemResultStateTests
    {
        [Test]
        public void ModificationResult_SuccessState_SetsPropertiesCorrectly()
        {
            // Arrange
            var result = new ModificationResult(ModificationResultState.Success, "Operation completed");

            // Assert
            Assert.That(result.ModificationResultState, Is.EqualTo(ModificationResultState.Success));
            Assert.That(result.Message, Is.EqualTo("Operation completed"));
        }

        [Test]
        public void ModificationResult_FailureState_SetsPropertiesCorrectly()
        {
            // Arrange
            var result = new ModificationResult(ModificationResultState.Failure, "Error occurred");

            // Assert
            Assert.That(result.ModificationResultState, Is.EqualTo(ModificationResultState.Failure));
            Assert.That(result.Message, Is.EqualTo("Error occurred"));
        }

        [Test]
        public void ModificationResult_DefaultMessage_IsNull()
        {
            // Arrange
            var result = new ModificationResult(ModificationResultState.Success);

            // Assert
            Assert.That(result.ModificationResultState, Is.EqualTo(ModificationResultState.Success));
            Assert.That(result.Message, Is.Null);
        }
    }
}
