using Xunit;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Constants
{
    public class ConstantsTests
    {
        [Fact]
        public void XApikey_ShouldHaveDefaultOrConfiguredValue() =>
            // If appsettings.json is missing or does not contain the key, default is used
            Assert.False(string.IsNullOrWhiteSpace(Constants.XApikey));

        [Fact]
        public void AIUNBaseUrl_ShouldHaveDefaultOrConfiguredValue() => Assert.False(string.IsNullOrWhiteSpace(Constants.AIUNBaseUrl));

        [Fact]
        public void UploadDocumentUrl_ShouldHaveDefaultOrConfiguredValue() => Assert.False(string.IsNullOrWhiteSpace(Constants.UploadDocumentUrl));

        [Fact]
        public void TokensURl_ShouldHaveDefaultOrConfiguredValue() => Assert.False(string.IsNullOrWhiteSpace(Constants.TokensURl));

        [Fact]
        public void GetIndexesUrl_ShouldHaveDefaultOrConfiguredValue() => Assert.False(string.IsNullOrWhiteSpace(Constants.GetIndexesUrl));
    }
}
