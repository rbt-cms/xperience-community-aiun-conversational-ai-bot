using System.Text.Json.Serialization;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.InfoClasses.AIUNRegistration;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models
{
    public class AiunRegistrationModel
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

        [JsonPropertyName("detail")]
        public string ErrorMessage { get; set; } = string.Empty;
        public AiunRegistrationModel() { }

        public AiunRegistrationModel(string detail) => ErrorMessage = detail;
        public AiunRegistrationModel(string firstName, string lastName, string email, string apiKey)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            APIKey = apiKey;
        }

        public AiunRegistrationModel(IEnumerable<AIUNRegistrationInfo> enumerable) => ItemList = enumerable;

        public AiunRegistrationModel(AIUNRegistrationInfo aIUNRegistrationInfo)
        {
            FirstName = aIUNRegistrationInfo.FirstName;
            LastName = aIUNRegistrationInfo.LastName;
            Email = aIUNRegistrationInfo.Email;
            APIKey = aIUNRegistrationInfo.APIKey;
        }
    }
}
