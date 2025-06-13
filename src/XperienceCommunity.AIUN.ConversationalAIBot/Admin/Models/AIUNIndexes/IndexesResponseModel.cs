
using System.Text.Json.Serialization;


namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models.AIUNIndexes
{
    /// <summary>
    /// Deserialize Indexes API response
    /// </summary>
    public class IndexesResponseModel
    {
        [JsonPropertyName("items")]
        public List<IndexItemModel> Items { get; set; } = [];
        [JsonPropertyName("total")]
        public int Total { get; set; } = 0;
        [JsonPropertyName("page")]
        public int Page { get; set; } = 1;
        [JsonPropertyName("size")]
        public int Size { get; set; } = 50;
    }
}
