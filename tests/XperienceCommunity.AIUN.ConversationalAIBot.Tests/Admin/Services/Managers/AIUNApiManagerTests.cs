using System.Net;
using System.Text;

using CMS.Core;
using CMS.DataEngine;

using Moq;
using Moq.Protected;

using NUnit.Framework;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models.AIUNIndexes;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.Managers
{
    [TestFixture]
    public class AIUNApiManagerTests
    {
        private Mock<HttpMessageHandler> httpMessageHandlerMock;
        private Mock<IEventLogService> eventLogServiceMock;
        private Mock<IInfoProvider<AIUNSettingsKeyInfo>> settingsKeyProviderMock;
        private HttpClient httpClient;
        private AiunApiManager apiManager;

        [SetUp]
        public void SetUp()
        {
            httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            eventLogServiceMock = new Mock<IEventLogService>();
            settingsKeyProviderMock = new Mock<IInfoProvider<AIUNSettingsKeyInfo>>();

            httpClient = new HttpClient(httpMessageHandlerMock.Object);
            apiManager = new AiunApiManager(httpClient, eventLogServiceMock.Object, settingsKeyProviderMock.Object);
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
                        .Setup(q => q.Provider).Returns(new[] { new FakeAiunSettingsKeyInfo("dummy-token") }.AsQueryable().Provider);
                    _ = mockQuery.As<IQueryable<AIUNSettingsKeyInfo>>()
                        .Setup(q => q.Expression).Returns(new[] { new FakeAiunSettingsKeyInfo("dummy-token") }.AsQueryable().Expression);
                    _ = mockQuery.As<IQueryable<AIUNSettingsKeyInfo>>()
                        .Setup(q => q.ElementType).Returns(new[] { new FakeAiunSettingsKeyInfo("dummy-token") }.AsQueryable().ElementType);
                    _ = mockQuery.As<IQueryable<AIUNSettingsKeyInfo>>()
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

        // Helper class
        public class FakeAiunSettingsKeyInfo : AIUNSettingsKeyInfo
        {
            public FakeAiunSettingsKeyInfo(string settingsKey) => SettingsKey = settingsKey;
            public override string SettingsKey { get; set; }
        }
    }
}
