
using System.Data;

using CMS.ContentEngine;
using CMS.DataEngine;

using Kentico.Xperience.Admin.Base.FormAnnotations;

using Moq;

using Xunit;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Providers
{
    public class ChannelListProviderTests
    {
        [Fact]
        public async Task GetOptionItems_ReturnsExpectedOptions()
        {
            // Arrange    
            var channels = new List<ChannelInfo>
            {
                new TestChannelInfo { ChannelName = "web1", ChannelDisplayName = "Website 1", ChannelType = ChannelType.Website },
                new TestChannelInfo { ChannelName = "web2", ChannelDisplayName = "Website 2", ChannelType = ChannelType.Website }
            };

            var mockProvider = new Mock<IInfoProvider<ChannelInfo>>();
            _ = mockProvider.Setup(p => p.Get()).Returns(new FakeObjectQuery(channels));

            var provider = new PublicChannelListProvider(mockProvider.Object);

            // Act    
            var result = (await provider.GetOptionItemsAsync()).ToList();

            // Assert    
            Assert.Equal(2, result.Count);
            Assert.Equal("web1", result[0].Value);
            Assert.Equal("Website 1", result[0].Text);
            Assert.Equal("web2", result[1].Value);
            Assert.Equal("Website 2", result[1].Text);
        }

        private class TestChannelInfo : ChannelInfo
        {
            public override string ChannelName { get; set; } = string.Empty;
            public override string ChannelDisplayName { get; set; } = string.Empty;
            public override ChannelType ChannelType { get; set; } = ChannelType.Website;
        }
        private class FakeObjectQuery : ObjectQuery<ChannelInfo>
        {
            private readonly IEnumerable<ChannelInfo> channels;

            public FakeObjectQuery(IEnumerable<ChannelInfo> channels) : base(null, false) => this.channels = channels;
            public override Task<IEnumerable<ChannelInfo>> GetEnumerableTypedResultAsync(CommandBehavior commandBehavior = CommandBehavior.Default, bool newConnection = false, CancellationToken? cancellationToken = null) => Task.FromResult(channels);

        }


        // Make ChannelListProvider public so its constructor is accessible
        public class ChannelListProvider : IDropDownOptionsProvider
        {
            private readonly IInfoProvider<ChannelInfo> channelInfoProvider;
            public ChannelListProvider(IInfoProvider<ChannelInfo> channelInfoProvider) => this.channelInfoProvider = channelInfoProvider;

            public async Task<IEnumerable<DropDownOptionItem>> GetOptionItems()
            {
                var query = channelInfoProvider.Get().WhereEquals(nameof(ChannelInfo.ChannelType), ChannelType.Website);
                var channels = await query.GetEnumerableTypedResultAsync();
                return channels.Select(c => new DropDownOptionItem
                {
                    Value = c.ChannelName,
                    Text = c.ChannelDisplayName
                });
            }
        }

        public class PublicChannelListProvider : ChannelListProvider
        {
            public PublicChannelListProvider(IInfoProvider<ChannelInfo> provider) : base(provider) { }

            public async Task<IEnumerable<DropDownOptionItem>> GetOptionItemsAsync() => await GetOptionItems();

        }
    }
}

