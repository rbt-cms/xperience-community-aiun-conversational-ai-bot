using Xunit;

namespace XperienceCommunity.AIUN.ConversationalAIBot
{
    public class AIUNConfiguratonItemResultStateTests
    {
        [Fact]
        public void ModificationResult_SuccessState_SetsPropertiesCorrectly()
        {
            // Arrange
            var result = new ModificationResult(ModificationResultState.Success, "Operation completed");

            // Assert
            Assert.Equal(ModificationResultState.Success, result.ModificationResultState);
            Assert.Equal("Operation completed", result.Message);
        }

        [Fact]
        public void ModificationResult_FailureState_SetsPropertiesCorrectly()
        {
            // Arrange
            var result = new ModificationResult(ModificationResultState.Failure, "Error occurred");

            // Assert
            Assert.Equal(ModificationResultState.Failure, result.ModificationResultState);
            Assert.Equal("Error occurred", result.Message);
        }

        [Fact]
        public void ModificationResult_DefaultMessage_IsNull()
        {
            // Arrange
            var result = new ModificationResult(ModificationResultState.Success);

            // Assert
            Assert.Equal(ModificationResultState.Success, result.ModificationResultState);
            Assert.Null(result.Message);
        }
    }
}
