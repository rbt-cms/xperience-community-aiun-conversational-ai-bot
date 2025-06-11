using CMS.DataEngine;
using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;
using XperienceCommunity.AIUN.ConversationalAIBot.InfoClasses.AIUNConfigurationItem;
using IFormItemCollectionProvider = Kentico.Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages.AIUNConfiguraionItem
{
    internal abstract class AIUNConfigurationItemBaseEditPage(IFormItemCollectionProvider formItemCollectionProvider,
    IFormDataBinder formDataBinder,
    IInfoProvider<AIUNConfigurationItemInfo> aIUNConfigurationItemProvider
      ) : ModelEditPage<AIUNConfigurationItemModel>(formItemCollectionProvider, formDataBinder)
    {
        protected ModificationResult ValidateAndProcess(AIUNConfigurationItemModel configuration, bool updateExisting = false)
        {
            var aiUNConfigurationItemInfo = new AIUNConfigurationItemInfo();

            if (updateExisting)
            {
                aiUNConfigurationItemInfo = aIUNConfigurationItemProvider.Get().WithID(configuration.Id).FirstOrDefault();

                if (aiUNConfigurationItemInfo == null)
                {
                    string keyDoesNotExistErrorMessage = "Item does not exist";

                    return new ModificationResult(ModificationResultState.Failure, keyDoesNotExistErrorMessage);
                }
            }

            if ((aIUNConfigurationItemProvider.Get()
                .WhereEquals(nameof(aiUNConfigurationItemInfo.ClientID), configuration.ClientID)
                .WhereEquals(nameof(aiUNConfigurationItemInfo.ChannelName), configuration.ChannelName)
                .WhereNotEquals(nameof(aiUNConfigurationItemInfo.AIUNConfigurationItemID), configuration.Id)
                .Any() && updateExisting) ||
                (aIUNConfigurationItemProvider.Get()
                .WhereEquals(nameof(aiUNConfigurationItemInfo.ClientID), configuration.ClientID)
               .Or().WhereEquals(nameof(aiUNConfigurationItemInfo.ChannelName), configuration.ChannelName)
                .Any() && !updateExisting)
            )

            //    if (AIUNConfigurationItemProvider.Get()
            //    .WhereEquals(nameof(aiUNConfigurationItemInfo.ClientID), configuration.ClientID)
            //    .WhereNotEquals(nameof(aiUNConfigurationItemInfo.AIUNConfigurationItemID), configuration.Id)
            //    .Any()
            //)
            {
                string invalidKeyLanguageCombinationErrorMessage = "Item already exists.";

                return new ModificationResult(ModificationResultState.Failure,
                    invalidKeyLanguageCombinationErrorMessage);
            }
            aiUNConfigurationItemInfo.ChannelName = configuration.ChannelName;

            aiUNConfigurationItemInfo.ClientID = configuration.ClientID;
            aiUNConfigurationItemInfo.APIKey = configuration.APIKey;
            aiUNConfigurationItemInfo.BaseURL = configuration.BaseURL;

            if (updateExisting)
            {
                aiUNConfigurationItemInfo.Update();
            }
            else
            {
                aiUNConfigurationItemInfo.Insert();
                configuration.Id = aiUNConfigurationItemInfo.AIUNConfigurationItemID;
            }

            return new(ModificationResultState.Success);
        }
    }
}
