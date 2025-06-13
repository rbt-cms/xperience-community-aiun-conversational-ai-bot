using Microsoft.Extensions.Configuration;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Constants
{
    public static class Constants
    {
        public static readonly string XApikey = GetConfigurationValue("AIUNSettings:XApikey", "GdcyRmTmraGFerfM-EoFBNDbcj0T-XS9Ietcd3ZemxY");
        public static readonly string AIUNBaseUrl = GetConfigurationValue("AIUNSettings:BaseUrl", "https://dev-api.aiun.ai/");
        public static readonly string UploadDocumentUrl = GetConfigurationValue("AIUNSettings:UploadDocumentUrl", "upload/urls");
        public static readonly string TokensURl = GetConfigurationValue("AIUNSettings:TokensURl", "users/tokens");
        public static readonly string GetIndexesUrl = GetConfigurationValue("AIUNSettings:GetIndexesUrl", "upload/uploaded/documents");

        private static string GetConfigurationValue(string key, string defaultValue)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            return config[key] ?? defaultValue;
        }
    }
}
