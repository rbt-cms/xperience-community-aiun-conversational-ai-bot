using CMS.DataEngine;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages.AIUNConfiguraionItem;
using XperienceCommunity.AIUN.ConversationalAIBot.InfoClasses.AIUNConfigurationItem;

using IFormItemCollectionProvider = Kentico.Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider;

[assembly: UIPage(
    parentType: typeof(AiunConfigurationItemsList),
    slug: "create",
    uiPageType: typeof(AiunConfigurationItemCreate),
    name: "Create Configuration Item",
    templateName: TemplateNames.EDIT,
    order: 2)]
namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages.AIUNConfiguraionItem
{
    internal class AiunConfigurationItemCreate(IFormItemCollectionProvider formItemCollectionProvider,
            IFormDataBinder formDataBinder,
            IInfoProvider<AIUNConfigurationItemInfo> aIUNConfigurationItemInfoProvider,
            IPageLinkGenerator pageLinkGenerator) : AiunConfigurationItemBaseEditPage(
    formItemCollectionProvider,
    formDataBinder,
#pragma warning disable CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
                aIUNConfigurationItemInfoProvider)
#pragma warning restore CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
    {
        private AiunConfigurationItemModel? model = null;
        protected override AiunConfigurationItemModel Model
        {
            get
            {
                model ??= new AiunConfigurationItemModel(aIUNConfigurationItemInfoProvider.Get().GetEnumerableTypedResult());
                return model;
            }
        }

        protected override Task<ICommandResponse> ProcessFormData(AiunConfigurationItemModel model, ICollection<IFormItem> formItems)
        {
            var result = ValidateAndProcess(model, updateExisting: false);

            if (result.ModificationResultState == ModificationResultState.Success)
            {
                var successResponse = NavigateTo(pageLinkGenerator.GetPath<AiunConfigurationItemsList>())
                    .AddSuccessMessage("Item created!");

                return Task.FromResult<ICommandResponse>(successResponse);
            }

            var errorResponse = ResponseFrom(new FormSubmissionResult(FormSubmissionStatus.ValidationFailure))
                .AddErrorMessage(result.Message);

            return Task.FromResult<ICommandResponse>(errorResponse);
        }


    }
}
