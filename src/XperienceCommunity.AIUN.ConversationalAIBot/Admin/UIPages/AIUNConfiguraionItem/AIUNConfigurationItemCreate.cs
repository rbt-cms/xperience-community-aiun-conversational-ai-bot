using CMS.DataEngine;
using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;
using XperienceCommunity.AIUN.ConversationalAIBot.InfoClasses.AIUNConfigurationItem;
using IFormItemCollectionProvider = Kentico.Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages.AIUNConfiguraionItem;


[assembly: UIPage(
    parentType: typeof(AIUNConfigurationItemsList),
    slug: "create",
    uiPageType: typeof(AIUNConfigurationItemCreate),
    name: "Create Configuration Item",
    templateName: TemplateNames.EDIT,
    order: 2)]
namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages.AIUNConfiguraionItem
{
    internal class AIUNConfigurationItemCreate(IFormItemCollectionProvider formItemCollectionProvider,
            IFormDataBinder formDataBinder,
            IInfoProvider<AIUNConfigurationItemInfo> aIUNConfigurationItemInfoProvider,
            IPageLinkGenerator pageLinkGenerator) : AIUNConfigurationItemBaseEditPage(
                formItemCollectionProvider,
                formDataBinder,
                aIUNConfigurationItemInfoProvider)
    {
        private AIUNConfigurationItemModel? model = null;
        protected override AIUNConfigurationItemModel Model
        {
            get
            {
                var configurationItemInfo = aIUNConfigurationItemInfoProvider.Get().FirstOrDefault() ?? throw new InvalidOperationException("No AIUNConfigurationItemInfo found.");

                model ??= new AIUNConfigurationItemModel(configurationItemInfo);
                return model;
            }
        }

        protected override Task<ICommandResponse> ProcessFormData(AIUNConfigurationItemModel model, ICollection<IFormItem> formItems)
        {
            var result = ValidateAndProcess(model, updateExisting: false);

            if (result.ModificationResultState == ModificationResultState.Success)
            {
                var successResponse = NavigateTo(pageLinkGenerator.GetPath<AIUNConfigurationItemsList>())
                    .AddSuccessMessage("Item created!");

                return Task.FromResult<ICommandResponse>(successResponse);
            }

            var errorResponse = ResponseFrom(new FormSubmissionResult(FormSubmissionStatus.ValidationFailure))
                .AddErrorMessage(result.Message);

            return Task.FromResult<ICommandResponse>(errorResponse);
        }


    }
}
