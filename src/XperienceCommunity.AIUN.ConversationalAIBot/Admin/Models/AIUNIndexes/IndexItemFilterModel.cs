using System.Text.Json.Serialization;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models.AIUNIndexes
{
    /// <summary>
    /// Filter model for Indexes page
    /// </summary>
    public class IndexItemFilterModel
    {
        [JsonPropertyName("page")]
        public int Page { get; set; } = 1;
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; } = 10;
        [JsonPropertyName("searchTerm")]
        public string SearchTerm { get; set; } = string.Empty;
        [JsonPropertyName("sortBy")]
        public string SortBy { get; set; } = "uploaded_date";
        [JsonPropertyName("sortDirection")]
        public string SortDirection { get; set; } = "desc"; // or "asc"

        public string TypeFilter { get; set; } = "All";// All, URL, Documents
        public string Channel { get; set; } = string.Empty;
    }
}
