using System.Net;
using System.Text;

using CMS.Core;
using CMS.DataEngine;

using Moq;
using Moq.Protected;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models.AIUNIndexes;

using Xunit;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.Managers
{
    public class AIUNApiManagerTests
    {
        private readonly Mock<HttpMessageHandler> httpMessageHandlerMock;
        private readonly Mock<IEventLogService> eventLogServiceMock;
        private readonly Mock<IInfoProvider<AIUNSettingsKeyInfo>> settingsKeyProviderMock;
        private readonly HttpClient httpClient;
        private readonly AiunApiManager apiManager;

        public AIUNApiManagerTests()
        {
            httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            eventLogServiceMock = new Mock<IEventLogService>();
            settingsKeyProviderMock = new Mock<IInfoProvider<AIUNSettingsKeyInfo>>();

            httpClient = new HttpClient(httpMessageHandlerMock.Object);
            apiManager = new AiunApiManager(httpClient, eventLogServiceMock.Object, settingsKeyProviderMock.Object);
        }

        [Fact]
        public async Task GetIndexesAsync_ReturnsIndexesResponseModel_WhenApiCallIsSuccessful()
        {
            // Arrange  
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.BaseAddress = new Uri("https://test-api.aiun.ai/"); // Ensure BaseAddress is set

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

            string jsonResponse = System.Text.Json.JsonSerializer.Serialize(
                expectedResponse,
                new System.Text.Json.JsonSerializerOptions { PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase }
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

            _ = settingsKeyProviderMock.Setup(x => x.Get())
                .Returns(() =>
                {
                    var mockQuery = new Mock<ObjectQuery<AIUNSettingsKeyInfo>>();
                    _ = mockQuery.As<IQueryable<AIUNSettingsKeyInfo>>()
                        .Setup(q => q.Provider).Returns(new List<AIUNSettingsKeyInfo> { new FakeAiunSettingsKeyInfo("dummy-token") }.AsQueryable().Provider);
                    _ = mockQuery.As<IQueryable<AIUNSettingsKeyInfo>>()
                        .Setup(q => q.Expression).Returns(new List<AIUNSettingsKeyInfo> { new FakeAiunSettingsKeyInfo("dummy-token") }.AsQueryable().Expression);
                    _ = mockQuery.As<IQueryable<AIUNSettingsKeyInfo>>()
                        .Setup(q => q.ElementType).Returns(new List<AIUNSettingsKeyInfo> { new FakeAiunSettingsKeyInfo("dummy-token") }.AsQueryable().ElementType);
                    _ = mockQuery.As<IQueryable<AIUNSettingsKeyInfo>>()
                        .Setup(q => q.GetEnumerator()).Returns(new List<AIUNSettingsKeyInfo> { new FakeAiunSettingsKeyInfo("dummy-token") }.GetEnumerator());
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

            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            _ = Assert.Single(result.Items);
            Assert.Equal(expectedResponse.Items[0].Id, result.Items[0].Id);
            Assert.Equal(expectedResponse.Items[0].Name, result.Items[0].Name);
            Assert.Equal(expectedResponse.Total, result.Total);
            Assert.Equal(expectedResponse.Page, result.Page);
            Assert.Equal(expectedResponse.Size, result.Size);
        }


        // Other test methods remain unchanged  
    }

    // Place the helper class here, outside the test class
    public class FakeAiunSettingsKeyInfo : AIUNSettingsKeyInfo
    {
        public FakeAiunSettingsKeyInfo(string settingsKey) => SettingsKey = settingsKey;
        public override string SettingsKey { get; set; }
    }
}
