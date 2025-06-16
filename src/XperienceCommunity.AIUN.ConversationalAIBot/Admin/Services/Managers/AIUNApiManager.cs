using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using CMS.Core;
using CMS.DataEngine;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.InfoClasses.AIUNRegistration;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models.AIUNIndexes;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.IManagers;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages.TokensUsage;


namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.Managers
{
    public class AiunApiManager : IAiunApiManager
    {
        private readonly HttpClient httpClient;
        private readonly IEventLogService eventLogService;
        private readonly IInfoProvider<AIUNRegistrationInfo> aIUNRegistrationInfo;


        public AiunApiManager(HttpClient httpClientParam, IEventLogService eventLogServiceParam,
            IInfoProvider<AIUNRegistrationInfo> aIUNRegistrationInfoParam)
        {
            httpClient = httpClientParam;
            eventLogService = eventLogServiceParam;
            aIUNRegistrationInfo = aIUNRegistrationInfoParam;

        }

        /// <summary>
        /// Sign up for AIUN service
        /// </summary>
        /// <param name="aIUNRegistrationItemModel"></param>
        /// <returns></returns>
        public async Task<AIUNRegistrationItemModel> AIUNSignup(AIUNRegistrationItemModel aIUNRegistrationItemModel)
        {
            try
            {
                string requestUrl = Constants.Constants.AIUNBaseUrl + Constants.Constants.SignupUrl; // e.g. "http://13.84.47.183:5300/users/signup"

                // Set Authorization header with Bearer token and X-Api-Key
                string moduleKey = Constants.Constants.XApikey;


                // Prepare the payload
                var payload = new
                {
                    first_name = aIUNRegistrationItemModel.FirstName,
                    last_name = aIUNRegistrationItemModel.LastName,
                    email = aIUNRegistrationItemModel.Email
                };

                // Convert to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");


                httpClient.DefaultRequestHeaders.Add("X-Api-Key", moduleKey);

                // Send GET request
                var response = await httpClient.PostAsync(requestUrl, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var data = System.Text.Json.JsonSerializer.Deserialize<AIUNRegistrationItemModel>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return data ?? new AIUNRegistrationItemModel();
                }
                else
                {
                    string details = await response.Content.ReadAsStringAsync();
                    eventLogService.LogException(nameof(AiunApiManager), nameof(AIUNSignup), new Exception("API_Call_Failed"),
                     $"AIUN registration failed with status code {(int)response.StatusCode}: {response.ReasonPhrase}\nDetails: {details}");
                    return new AIUNRegistrationItemModel();
                }

            }
            catch (Exception ex)
            {
                eventLogService.LogException(nameof(AiunApiManager), nameof(AIUNSignup), ex,
                    $"AIUN registration failed:" + ex.Message);
                return new AIUNRegistrationItemModel();
            }
        }
        /// <summary>
        /// Upload URLs to the API
        /// </summary>
        /// <param name="websiteUrls"></param>
        /// <param name="clientID"></param>
        /// <param name="securityToken"></param>
        /// <returns></returns>
        public async Task<string> UploadURLsAsync(List<string> websiteUrls, string clientID, string? securityToken = "")
        {
            try
            {
                string requestUrl = Constants.Constants.AIUNBaseUrl + Constants.Constants.UploadDocumentUrl;

                // Prepare the payload
                var payload = new
                {
                    urls = websiteUrls,
                    department = clientID,
                };

                // Convert to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                // Set Authorization header with Bearer token: ModuleKey + SecurityToken
                string moduleKey = Constants.Constants.XApikey;
                if (string.IsNullOrEmpty(securityToken))
                {
                    securityToken = aIUNRegistrationInfo.Get()?.FirstOrDefault()?.APIKey ?? string.Empty;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{securityToken}");
                httpClient.DefaultRequestHeaders.Add("X-Api-Key", moduleKey);
                // Send POST request
                var response = await httpClient.PostAsync(requestUrl, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();

                    // Log success using LogInformation
                    eventLogService.LogInformation(
                        source: nameof(AiunApiManager),
                        eventCode: "Upload Success",
                        eventDescription: $"Uploaded URLs successfully at {DateTime.Now}. URLs: {string.Join(", ", websiteUrls)}"
                    );

                    return result;

                }
                // Optional: log or handle errors
                string error = await response.Content.ReadAsStringAsync();

                // Log failure using LogException
                var exception = new ApplicationException($"Upload API failed. Status: {response.StatusCode}, Error: {error}");
                eventLogService.LogException(
                    source: nameof(AiunApiManager),
                    eventCode: "Upload Failure",
                    ex: exception,
                    additionalMessage: $"Failed to upload URLs at {DateTime.Now}. URLs: {string.Join(", ", websiteUrls)}"
                );

                return error;
            }
            catch (Exception ex)
            {
                // Log unexpected exceptions
                eventLogService.LogException(
                    source: nameof(AiunApiManager),
                    eventCode: "Upload Exception",
                    ex: ex,
                    additionalMessage: $"Unexpected error at {DateTime.Now} while uploading URLs: {string.Join(", ", websiteUrls)}"
                );
            }
            return string.Empty;


        }
        /// <summary>
        /// Get Token Usage from API call
        /// </summary>
        /// <returns></returns>
        public async Task<AiunTokenUsageLayoutProperties> GetTokenUsageAsync()
        {
            try
            {
                string requestUrl = Constants.Constants.AIUNBaseUrl + Constants.Constants.TokensURl; // e.g. "http://13.84.47.183:5300/token-usage"

                // Set Authorization header with Bearer token and X-Api-Key
                string moduleKey = Constants.Constants.XApikey;
                string securityToken = aIUNRegistrationInfo.Get()?.FirstOrDefault()?.APIKey ?? string.Empty;
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", securityToken);
                httpClient.DefaultRequestHeaders.Add("X-Api-Key", moduleKey);

                var response = await httpClient.GetAsync(requestUrl);
                if (response.Content == null)
                {
                    return new AiunTokenUsageLayoutProperties();
                }

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(json))
                    {
                        var result = JsonConvert.DeserializeObject<AiunTokenUsageLayoutProperties>(json);
                        return result ?? new AiunTokenUsageLayoutProperties();
                    }
                }

                string error = await response.Content.ReadAsStringAsync();
                var exception = new ApplicationException($"Token Usage API failed. Status: {response.StatusCode}, Error: {error}");
                eventLogService.LogException(
                    source: nameof(AiunApiManager),
                    eventCode: "Token Usage Failure",
                    ex: exception,
                    additionalMessage: $"Token Usage API failed at {DateTime.Now}"
                );
            }
            catch (Exception ex)
            {
                eventLogService.LogException(nameof(AiunApiManager), nameof(GetTokenUsageAsync), ex);
            }
            return new AiunTokenUsageLayoutProperties();
        }

        /// <summary>
        /// Get Indexes from API call based on filter params
        /// </summary>
        /// <param name="indexItemFilterModel"></param>
        /// <returns></returns>
        public async Task<IndexesResponseModel> GetIndexesAsync(IndexItemFilterModel indexItemFilterModel)
        {
            try
            {
                string requestUrl = Constants.Constants.AIUNBaseUrl + Constants.Constants.GetIndexesUrl; // e.g. "http://13.84.47.183:5300/upload/uploaded/documents"

                // Set Authorization header with Bearer token and X-Api-Key
                string moduleKey = Constants.Constants.XApikey;

                // This hardcoded need to replace with fetch key from database ......
                string securityToken = aIUNRegistrationInfo.Get()?.FirstOrDefault()?.APIKey ?? string.Empty;

                string filterParams = string.Empty;

                if (indexItemFilterModel != null)
                {
                    indexItemFilterModel.TypeFilter = indexItemFilterModel.TypeFilter switch
                    {
                        "URL" => "true",
                        "Documents" => "false",
                        _ => string.Empty
                    };

                    string departmentParam = !string.IsNullOrEmpty(indexItemFilterModel.Channel) ? "&department_id=" + indexItemFilterModel.Channel : "";
                    string typeFilterParam = !string.IsNullOrEmpty(indexItemFilterModel.TypeFilter) ? "&is_url=" + indexItemFilterModel.TypeFilter : "";
                    filterParams = $"?page={indexItemFilterModel.Page}&size={indexItemFilterModel.PageSize}&search={indexItemFilterModel.SearchTerm}{departmentParam}{typeFilterParam}";
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{securityToken}");
                httpClient.DefaultRequestHeaders.Add("X-Api-Key", moduleKey);

                // Send GET request
                var response = await httpClient.GetAsync(requestUrl + filterParams);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var data = System.Text.Json.JsonSerializer.Deserialize<IndexesResponseModel>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return data ?? new IndexesResponseModel();
                }
                else
                {
                    string details = await response.Content.ReadAsStringAsync();
                    eventLogService.LogException("AIUNApiManager", "GetIndexes", new Exception("API_Call_Failed"),
                     $"API call failed with status code {(int)response.StatusCode}: {response.ReasonPhrase}\nDetails: {details}");
                    return new IndexesResponseModel();
                }
            }
            catch (Exception ex)
            {
                eventLogService.LogException(nameof(AiunApiManager), nameof(GetIndexesAsync), ex);
            }

            return new IndexesResponseModel();
        }

    }
}
