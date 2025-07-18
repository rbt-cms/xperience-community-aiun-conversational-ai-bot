using System.Net;

using System.Text;
using System.Text.Json;

using CMS.Core;
using CMS.DataEngine;

using Moq;
using Moq.Protected;

using NUnit.Framework;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.InfoClasses.AIUNRegistration;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models.AIUNIndexes;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages.TokensUsage;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.Managers
{
    [TestFixture]
    public class AIUNApiManagerTests
    {
        private Mock<HttpMessageHandler> httpMessageHandlerMock;
        private Mock<IEventLogService> eventLogServiceMock;
        private Mock<IInfoProvider<AIUNRegistrationInfo>> aIUNRegistrationInfoMock;
        private HttpClient httpClient;
        private AiunApiManager apiManager;

        [SetUp]
        public void SetUp()
        {
            httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            eventLogServiceMock = new Mock<IEventLogService>();
            aIUNRegistrationInfoMock = new Mock<IInfoProvider<AIUNRegistrationInfo>>();
            httpClient = new HttpClient(httpMessageHandlerMock.Object);
            apiManager = new AiunApiManager(httpClient, eventLogServiceMock.Object, aIUNRegistrationInfoMock.Object);
        }

        [Test]
        public async Task ValidateChatbotConfiguration_ReturnsEmptyString_OnSuccess()
        {
            // Arrange
            var model = new AiunConfigurationItemModel { ClientID = "cid", APIKey = "key", BaseURL = "url" };
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            string result = await apiManager.ValidateChatbotConfiguration(model);

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public async Task ValidateChatbotConfiguration_ReturnsErrorMessage_OnFailure()
        {
            // Arrange
            var model = new AiunConfigurationItemModel { ClientID = "cid", APIKey = "key", BaseURL = "url" };
            var error = new AiunRegistrationModel { ErrorMessage = "Invalid config" };
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonSerializer.Serialize(error), Encoding.UTF8, "application/json")
            };
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            string result = await apiManager.ValidateChatbotConfiguration(model);

            // Assert
            Assert.That(result, Is.EqualTo("Invalid config"));
        }

        [Test]
        public async Task ValidateChatbotConfiguration_ReturnsGenericError_OnException()
        {
            // Arrange
            var model = new AiunConfigurationItemModel { ClientID = "cid", APIKey = "key", BaseURL = "url" };
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new Exception("fail"));

            // Act
            string result = await apiManager.ValidateChatbotConfiguration(model);

            // Assert
            Assert.That(result, Does.Contain("AIUN chatbot config validation failed"));
        }

        [Test]
        public async Task AIUNSignup_ReturnsModel_OnSuccess()
        {
            // Arrange
            var input = new AiunRegistrationModel { FirstName = "A", LastName = "B", Email = "e@e.com" };
            var expected = new AiunRegistrationModel { FirstName = "A", LastName = "B", Email = "e@e.com" };
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(expected), Encoding.UTF8, "application/json")
            };
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await apiManager.AIUNSignup(input);

            // Assert
            Assert.That(result.FirstName, Is.EqualTo("A"));
            Assert.That(result.LastName, Is.EqualTo("B"));
            Assert.That(result.Email, Is.EqualTo("e@e.com"));
        }

        [Test]
        public async Task AIUNSignup_ReturnsErrorModel_OnBadRequest()
        {
            // Arrange
            var input = new AiunRegistrationModel { FirstName = "A", LastName = "B", Email = "e@e.com" };
            var error = new AiunRegistrationModel { ErrorMessage = "Email exists" };
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonSerializer.Serialize(error), Encoding.UTF8, "application/json")
            };
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await apiManager.AIUNSignup(input);

            // Assert
            Assert.That(result.ErrorMessage, Is.EqualTo("Email exists"));
        }

        [Test]
        public async Task AIUNSignup_ReturnsEmptyModel_OnOtherFailure()
        {
            // Arrange
            var input = new AiunRegistrationModel { FirstName = "A", LastName = "B", Email = "e@e.com" };
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("fail", Encoding.UTF8, "application/json")
            };
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await apiManager.AIUNSignup(input);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task AIUNSignup_ReturnsEmptyModel_OnException()
        {
            // Arrange
            var input = new AiunRegistrationModel { FirstName = "A", LastName = "B", Email = "e@e.com" };
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new Exception("fail"));

            // Act
            var result = await apiManager.AIUNSignup(input);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task UploadURLsAsync_ReturnsResult_OnSuccess()
        {
            // Arrange
            var urls = new List<string> { "https://a.com" };
            string clientId = "dept";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("success", Encoding.UTF8, "application/json")
            };
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var mockQuery = new Mock<ObjectQuery<AIUNRegistrationInfo>>();
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.Provider).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.AsQueryable().Provider);
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.Expression).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.AsQueryable().Expression);
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.ElementType).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.AsQueryable().ElementType);
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.GetEnumerator()).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.ToList().GetEnumerator());
            aIUNRegistrationInfoMock.Setup(x => x.Get()).Returns(mockQuery.Object);

            // Act
            string result = await apiManager.UploadURLsAsync(urls, clientId);

            // Assert
            Assert.That(result, Is.EqualTo("success"));
        }

        [Test]
        public async Task UploadURLsAsync_ReturnsError_OnFailure()
        {
            // Arrange
            var urls = new List<string> { "https://a.com" };
            string clientId = "dept";
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("fail", Encoding.UTF8, "application/json")
            };
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var mockQuery = new Mock<ObjectQuery<AIUNRegistrationInfo>>();
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.Provider).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.AsQueryable().Provider);
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.Expression).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.AsQueryable().Expression);
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.ElementType).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.AsQueryable().ElementType);
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.GetEnumerator()).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.ToList().GetEnumerator());
            aIUNRegistrationInfoMock.Setup(x => x.Get()).Returns(mockQuery.Object);

            // Act
            string result = await apiManager.UploadURLsAsync(urls, clientId);

            // Assert
            Assert.That(result, Is.EqualTo("fail"));
        }

        [Test]
        public async Task UploadURLsAsync_ReturnsEmptyString_OnException()
        {
            // Arrange
            var urls = new List<string> { "https://a.com" };
            string clientId = "dept";
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new Exception("fail"));

            var mockQuery = new Mock<ObjectQuery<AIUNRegistrationInfo>>();
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.Provider).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.AsQueryable().Provider);
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.Expression).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.AsQueryable().Expression);
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.ElementType).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.AsQueryable().ElementType);
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.GetEnumerator()).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.ToList().GetEnumerator());
            aIUNRegistrationInfoMock.Setup(x => x.Get()).Returns(mockQuery.Object);

            // Act
            string result = await apiManager.UploadURLsAsync(urls, clientId);

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public async Task GetTokenUsageAsync_ReturnsModel_OnSuccess()
        {
            // Arrange
            var expected = new AiunTokenUsageLayoutProperties { Clients = [] };
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(expected), Encoding.UTF8, "application/json")
            };
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var mockQuery = new Mock<ObjectQuery<AIUNRegistrationInfo>>();
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.Provider).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.AsQueryable().Provider);
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.Expression).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.AsQueryable().Expression);
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.ElementType).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.AsQueryable().ElementType);
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.GetEnumerator()).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.ToList().GetEnumerator());
            aIUNRegistrationInfoMock.Setup(x => x.Get()).Returns(mockQuery.Object);

            // Act
            var result = await apiManager.GetTokenUsageAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetTokenUsageAsync_ReturnsEmptyModel_OnFailure()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("fail", Encoding.UTF8, "application/json")
            };
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var mockQuery = new Mock<ObjectQuery<AIUNRegistrationInfo>>();
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.Provider).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.AsQueryable().Provider);
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.Expression).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.AsQueryable().Expression);
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.ElementType).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.AsQueryable().ElementType);
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.GetEnumerator()).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.ToList().GetEnumerator());
            aIUNRegistrationInfoMock.Setup(x => x.Get()).Returns(mockQuery.Object);

            // Act
            var result = await apiManager.GetTokenUsageAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetTokenUsageAsync_ReturnsEmptyModel_OnException()
        {
            // Arrange
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new Exception("fail"));

            var mockQuery = new Mock<ObjectQuery<AIUNRegistrationInfo>>();
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.Provider).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.AsQueryable().Provider);
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.Expression).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.AsQueryable().Expression);
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.ElementType).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.AsQueryable().ElementType);
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.GetEnumerator()).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.ToList().GetEnumerator());
            aIUNRegistrationInfoMock.Setup(x => x.Get()).Returns(mockQuery.Object);

            // Act
            var result = await apiManager.GetTokenUsageAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetIndexesAsync_ReturnsIndexesResponseModel_WhenApiCallIsSuccessful()
        {
            // Arrange  
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.BaseAddress = new Uri("https://test-api.aiun.ai/");

            var indexItemFilterModel = new IndexItemFilterModel
            {
                Page = 1,
                PageSize = 10,
                SearchTerm = "test",
                Channel = "channel1",
                TypeFilter = "URL"
            };

            var expectedResponse = new IndexesResponseModel
            {
                Total = 1,
                Page = 1,
                Size = 10,
                Items =
               [
                   new()
                   {
                       Id = "1",
                       Name = "Test Index",
                       UploadedDate = null,
                       Title = null,
                       Status = null,
                       Category = null,
                       Department = null
                   }
               ]
            };

            string jsonResponse = JsonSerializer.Serialize(
                expectedResponse,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
            );

            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
            };

            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage)
                .Verifiable();

            aIUNRegistrationInfoMock.Setup(x => x.Get())
                .Returns(() =>
                {
                    var mockQuery = new Mock<ObjectQuery<AIUNRegistrationInfo>>();
                    _ = mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                        .Setup(q => q.Provider).Returns(new[] { new FakeAiunSettingsKeyInfo("dummy-token") }.AsQueryable().Provider);
                    _ = mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                        .Setup(q => q.Expression).Returns(new[] { new FakeAiunSettingsKeyInfo("dummy-token") }.AsQueryable().Expression);
                    _ = mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                        .Setup(q => q.ElementType).Returns(new[] { new FakeAiunSettingsKeyInfo("dummy-token") }.AsQueryable().ElementType);
                    _ = mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                        .Setup(q => q.GetEnumerator()).Returns(new[] { new FakeAiunSettingsKeyInfo("dummy-token") }.ToList().GetEnumerator());
                    return mockQuery.Object;
                });

            // Act  
            var result = await apiManager.GetIndexesAsync(indexItemFilterModel);

            // Assert  
            httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Items, Is.Not.Null);
            Assert.That(result.Items, Has.Count.EqualTo(1));
            Assert.That(result.Items[0].Id, Is.EqualTo(expectedResponse.Items[0].Id));
            Assert.That(result.Items[0].Name, Is.EqualTo(expectedResponse.Items[0].Name));
            Assert.That(result.Total, Is.EqualTo(expectedResponse.Total));
            Assert.That(result.Page, Is.EqualTo(expectedResponse.Page));
            Assert.That(result.Size, Is.EqualTo(expectedResponse.Size));
        }

        [Test]
        public async Task GetIndexesAsync_ReturnsEmptyModel_OnFailure()
        {
            // Arrange
            var indexItemFilterModel = new IndexItemFilterModel { Page = 1, PageSize = 10 };
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("fail", Encoding.UTF8, "application/json")
            };
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var mockQuery = new Mock<ObjectQuery<AIUNRegistrationInfo>>();
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.Provider).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.AsQueryable().Provider);
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.Expression).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.AsQueryable().Expression);
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.ElementType).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.AsQueryable().ElementType);
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.GetEnumerator()).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.ToList().GetEnumerator());
            aIUNRegistrationInfoMock.Setup(x => x.Get()).Returns(mockQuery.Object);

            // Act
            var result = await apiManager.GetIndexesAsync(indexItemFilterModel);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetIndexesAsync_ReturnsEmptyModel_OnException()
        {
            // Arrange
            var indexItemFilterModel = new IndexItemFilterModel { Page = 1, PageSize = 10 };
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new Exception("fail"));

            var mockQuery = new Mock<ObjectQuery<AIUNRegistrationInfo>>();
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.Provider).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.AsQueryable().Provider);
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.Expression).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.AsQueryable().Expression);
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.ElementType).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.AsQueryable().ElementType);
            mockQuery.As<IQueryable<AIUNRegistrationInfo>>()
                .Setup(q => q.GetEnumerator()).Returns(new[] { new FakeAiunSettingsKeyInfo("token") }.ToList().GetEnumerator());
            aIUNRegistrationInfoMock.Setup(x => x.Get()).Returns(mockQuery.Object);

            // Act
            var result = await apiManager.GetIndexesAsync(indexItemFilterModel);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        // Helper class
        public class FakeAiunSettingsKeyInfo : AIUNRegistrationInfo
        {
            public FakeAiunSettingsKeyInfo(string settingsKey) => APIKey = settingsKey;
            public override string APIKey { get; set; }
        }
    }
}
