﻿using CMS.ContentEngine;
using CMS.DataEngine;

using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.Providers
{
    internal class ChannelListProvider : IDropDownOptionsProvider
    {
        private readonly IInfoProvider<ChannelInfo> channelInfoProvider;
        public ChannelListProvider(IInfoProvider<ChannelInfo> channelInfoProvider) => this.channelInfoProvider = channelInfoProvider;

        public async Task<IEnumerable<DropDownOptionItem>> GetOptionItems() =>
            (await channelInfoProvider.Get()
                .WhereEquals(nameof(ChannelInfo.ChannelType), nameof(ChannelType.Website))
                .GetEnumerableTypedResultAsync())
                .Select(x => new DropDownOptionItem()
                {
                    Value = x.ChannelName,
                    Text = x.ChannelDisplayName
                });
    }
}
