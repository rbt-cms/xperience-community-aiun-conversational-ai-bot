using Xunit;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Constants
{
    public class ConstantsTests
    {
        [Fact]
        public void Constants_Values_AreAsExpected()
        {
            Assert.Equal("GdcyRmTmraGFerfM-EoFBNDbcj0T-XS9Ietcd3ZemxY", Constants.XApikey);
            Assert.Equal("http://13.84.47.183:5300/", Constants.AIUNBaseUrl);
            Assert.Equal("upload/urls", Constants.UploadDocumentUrl);
            Assert.Equal("users/tokens", Constants.TokensURl);
            Assert.Equal("upload/uploaded/documents", Constants.GetIndexesUrl);
        }
    }
}
