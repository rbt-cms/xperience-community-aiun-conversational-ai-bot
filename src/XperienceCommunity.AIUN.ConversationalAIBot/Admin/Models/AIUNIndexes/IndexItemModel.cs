using System.Text.Json.Serialization;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models.AIUNIndexes
{
    /// <summary>
    /// Deserialize Indexes items from API response
    /// </summary>
    public class IndexItemModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("uploaded_date")]
        public string UploadedDate { get; set; } = string.Empty;
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;
        [JsonPropertyName("department")]
        public string Department { get; set; } = string.Empty;
    }
}
