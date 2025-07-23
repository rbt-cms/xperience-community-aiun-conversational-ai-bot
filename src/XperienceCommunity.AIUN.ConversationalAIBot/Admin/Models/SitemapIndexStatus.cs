using Newtonsoft.Json;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models
{
    public class SitemapIndexStatus
    {
        [JsonProperty("sitemap_url")]
        public required string SitemapUrl { get; set; }

        [JsonProperty("total_urls")]
        public int TotalUrls { get; set; }

        [JsonProperty("completed")]
        public int Completed { get; set; }

        [JsonProperty("pending")]
        public int Pending { get; set; }

        [JsonProperty("processing")]
        public int Processing { get; set; }

        [JsonProperty("failed")]
        public int Failed { get; set; }

        [JsonProperty("overall_status")]
        public required string OverallStatus { get; set; }
    }

}
