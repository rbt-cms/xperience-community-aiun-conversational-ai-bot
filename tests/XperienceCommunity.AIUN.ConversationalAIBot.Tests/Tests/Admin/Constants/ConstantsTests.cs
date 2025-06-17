using NUnit.Framework;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Tests.Admin.Constants
{
    public class ConstantsTests
    {
        [Test]
        public void XApikey_ShouldHaveDefaultOrConfiguredValue() =>
            Assert.That(Constants.XApikey, Is.Not.Null.And.Not.Empty.And.Not.EqualTo(" ").And.Not.EqualTo(""));

        [Test]
        public void AIUNBaseUrl_ShouldHaveDefaultOrConfiguredValue() =>
            Assert.That(Constants.AIUNBaseUrl, Is.Not.Null.And.Not.Empty.And.Not.EqualTo(" ").And.Not.EqualTo(""));

        [Test]
        public void UploadDocumentUrl_ShouldHaveDefaultOrConfiguredValue() =>
            Assert.That(Constants.UploadDocumentUrl, Is.Not.Null.And.Not.Empty.And.Not.EqualTo(" ").And.Not.EqualTo(""));

        [Test]
        public void TokensURl_ShouldHaveDefaultOrConfiguredValue() =>
            Assert.That(Constants.TokensURl, Is.Not.Null.And.Not.Empty.And.Not.EqualTo(" ").And.Not.EqualTo(""));

        [Test]
        public void GetIndexesUrl_ShouldHaveDefaultOrConfiguredValue() =>
            Assert.That(Constants.GetIndexesUrl, Is.Not.Null.And.Not.Empty.And.Not.EqualTo(" ").And.Not.EqualTo(""));
    }
}
