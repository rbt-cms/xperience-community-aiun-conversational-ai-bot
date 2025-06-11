using CMS.DataEngine;
using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;
using XperienceCommunity.AIUN.ConversationalAIBot.InfoClasses.AIUNConfigurationItem;
using IFormItemCollectionProvider = Kentico.Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages.AIUNConfiguraionItem;

[assembly: UIPage(
    parentType: typeof(AIUNConfigurationItemsList),
    slug: PageParameterConstants.PARAMETERIZED_SLUG,
    uiPageType: typeof(AIUNConfigurationItemEdit),
    name: "Edit",
    templateName: TemplateNames.EDIT,
    order: 1)]
namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages.AIUNConfiguraionItem
{
    internal class AIUNConfigurationItemEdit(
    IFormItemCollectionProvider formItemCollectionProvider,
    IFormDataBinder formDataBinder,
    IPageLinkGenerator pageLinkGenerator,
    IInfoProvider<AIUNConfigurationItemInfo> aIUNConfigurationItemProvider
      ) : AIUNConfigurationItemBaseEditPage(formItemCollectionProvider, formDataBinder, aIUNConfigurationItemProvider)
    {
        private readonly IInfoProvider<AIUNConfigurationItemInfo> aIUNConfigurationInfoProvider = aIUNConfigurationItemProvider;

        [PageParameter(typeof(IntPageModelBinder))]
        public int AIUNConfigurationItemIdentifier { get; set; }
        private AIUNConfigurationItemModel? model = null;

        protected override AIUNConfigurationItemModel Model
        {
            get
            {
                var settings = aIUNConfigurationItemProvider
                    .Get()
                    .WithID(AIUNConfigurationItemIdentifier)
                    .FirstOrDefault() ?? throw new InvalidOperationException("Specified key does not exist");
                model ??= new AIUNConfigurationItemModel(settings);
                return model;
            }
        }
        public override Task ConfigurePage()
        {
            PageConfiguration.Headline = LocalizationService.GetString("Edit the configuration Item");
            return base.ConfigurePage();
        }

        protected override Task<ICommandResponse> ProcessFormData(AIUNConfigurationItemModel model, ICollection<IFormItem> formItems)
        {
            var result = ValidateAndProcess(model, updateExisting: true);

            if (result.ModificationResultState == ModificationResultState.Success)
            {
                var successResponse = NavigateTo(pageLinkGenerator.GetPath<AIUNConfigurationItemsList>())
                    .AddSuccessMessage("Item edited");

                return Task.FromResult<ICommandResponse>(successResponse);
            }

            var errorResponse = ResponseFrom(new FormSubmissionResult(FormSubmissionStatus.ValidationFailure))
                .AddErrorMessage(result.Message);

            return Task.FromResult<ICommandResponse>(errorResponse);
        }

    }
}
