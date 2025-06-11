using Xunit;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Constants;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Tests.Admin.Constants
{
    public class ConstantsTests
    {
        [Fact]
        public void Constants_Values_AreAsExpected()
        {
            Assert.Equal("GdcyRmTmraGFerfM-EoFBNDbcj0T-XS9Ietcd3ZemxY", XperienceCommunity.AIUN.ConversationalAIBot.Admin.Constants.Constants.XApikey);
            Assert.Equal("http://13.84.47.183:5300/", XperienceCommunity.AIUN.ConversationalAIBot.Admin.Constants.Constants.AIUNBaseUrl); // Fixed namespace
            Assert.Equal("upload/urls", XperienceCommunity.AIUN.ConversationalAIBot.Admin.Constants.Constants.UploadDocumentUrl);
            Assert.Equal("users/tokens", XperienceCommunity.AIUN.ConversationalAIBot.Admin.Constants.Constants.TokensURl);

            // Fixed the incorrect reference to GetIndexesUrl
            Assert.Equal("upload/uploaded/documents", XperienceCommunity.AIUN.ConversationalAIBot.Admin.Constants.Constants.GetIndexesUrl);
        }
    }
}
