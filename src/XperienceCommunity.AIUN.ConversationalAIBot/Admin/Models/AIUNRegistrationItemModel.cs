using System.Text.Json.Serialization;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.InfoClasses.AIUNRegistration;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models
{
    public class AIUNRegistrationItemModel
    {
        public readonly IEnumerable<AIUNRegistrationInfo> ItemList = [];
        public int Id { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("last_name")]
        public string LastName { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        public string APIKey { get; set; } = string.Empty;


        public AIUNRegistrationItemModel() { }
        public AIUNRegistrationItemModel(string firstName, string lastName, string email, string apiKey)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            APIKey = apiKey;
        }

        public AIUNRegistrationItemModel(IEnumerable<AIUNRegistrationInfo> enumerable) => ItemList = enumerable;

        public AIUNRegistrationItemModel(AIUNRegistrationInfo aIUNRegistrationInfo)
        {
            FirstName = aIUNRegistrationInfo.FirstName;
            LastName = aIUNRegistrationInfo.LastName;
            Email = aIUNRegistrationInfo.Email;
            APIKey = aIUNRegistrationInfo.APIKey;
        }
    }
}
