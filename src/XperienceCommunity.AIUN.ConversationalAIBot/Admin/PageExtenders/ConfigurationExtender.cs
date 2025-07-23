using CMS.DataEngine;

using Kentico.Xperience.Admin.Base;

using Microsoft.AspNetCore.Http;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.PageExtenders;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.IManagers;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages.AIUNConfiguraionItem;
using XperienceCommunity.AIUN.ConversationalAIBot.InfoClasses.AIUNConfigurationItem;


[assembly: PageExtender(typeof(ConfigurationItemListExtender))]
namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.PageExtenders
{
    public class ConfigurationItemListExtender : PageExtender<AiunConfigurationItemsList>
    {
        private readonly IAiunApiManager apiManager;
        private readonly IInfoProvider<AIUNConfigurationItemInfo> aiUNConfigurationItemInfoProvider;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ConfigurationItemListExtender(IAiunApiManager apiManager, IInfoProvider<AIUNConfigurationItemInfo> aIUNConfigurationItemInfoProviderParam, IHttpContextAccessor httpContextAccessorParam)
        {
            this.apiManager = apiManager;
            aiUNConfigurationItemInfoProvider = aIUNConfigurationItemInfoProviderParam;
            httpContextAccessor = httpContextAccessorParam;
        }

        public override Task ConfigurePage()
        {
            //if (isConfigured == 1)
            //{
            //    return Task.CompletedTask;
            //}

            var AIUNConfigurationItem = aiUNConfigurationItemInfoProvider.Get().FirstOrDefault() ?? new AIUNConfigurationItemInfo();
            var request = httpContextAccessor.HttpContext?.Request;
            string clientUrl = " ";
            if (request != null)
            {
                clientUrl = $"{request.Scheme}://{request.Host}";
            }
            var indexStatusobj = apiManager.GetSitemapIndexAsync(AIUNConfigurationItem.ClientID, clientUrl, null).Result;
            string status = string.Empty;
            if (indexStatusobj != null && indexStatusobj.Count > 0)
            {
                status = indexStatusobj[0].OverallStatus;
            }

            // Escape single quotes to avoid SQL injection or syntax error
            string escapedStatus = status.Replace("'", "''");

            // Inject the dynamic status value into the query
            string statusTableSql = $"""
            (SELECT '{escapedStatus}' AS Status) AS StatusTable
        """;

            Page.PageConfiguration.QueryModifiers.AddModifier((query, _) =>
                query.Source(source =>
                    source.LeftJoin(
                        sourceExpression: statusTableSql,
                        condition: "1 = 1"
                    )
                )
                .AddColumn(new QueryColumn("StatusTable.Status") { ColumnAlias = "Status" })
            );

            Page.PageConfiguration.ColumnConfigurations
                .AddColumn("Status", caption: "Status");

            // isConfigured = 1;
            return base.ConfigurePage();
        }
    }

}
