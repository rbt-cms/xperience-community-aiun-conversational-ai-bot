using CMS.DataEngine;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Models;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.Services.IManagers;
using XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages.AIUNConfiguraionItem;
using XperienceCommunity.AIUN.ConversationalAIBot.InfoClasses.AIUNConfigurationItem;

using IFormItemCollectionProvider = Kentico.Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider;

[assembly: UIPage(
    parentType: typeof(AiunConfigurationItemsList),
    slug: PageParameterConstants.PARAMETERIZED_SLUG,
    uiPageType: typeof(AiunConfigurationItemEdit),
    name: "Edit",
    templateName: TemplateNames.EDIT,
    order: 1)]
namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.UIPages.AIUNConfiguraionItem
{
    internal class AiunConfigurationItemEdit : AiunConfigurationItemBaseEditPage
    {
        private readonly IPageLinkGenerator pageLinkGenerator;
        private readonly IInfoProvider<AIUNConfigurationItemInfo> aIUNConfigurationItemProvider; // Fix: Add missing field declaration
        private readonly IAiunApiManager aiUNApiManager;

        [PageParameter(typeof(IntPageModelBinder))]
        public int AIUNConfigurationItemIdentifier { get; set; }
        private AiunConfigurationItemModel? model = null;

        public AiunConfigurationItemEdit(IAiunApiManager aiUNApiManagerParam,
           IFormItemCollectionProvider formItemCollectionProvider,
           IFormDataBinder formDataBinder,
           IPageLinkGenerator pageLinkGenerator,
           IInfoProvider<AIUNConfigurationItemInfo> aIUNConfigurationItemProvider
        ) : base(formItemCollectionProvider, formDataBinder, aIUNConfigurationItemProvider)
        {
            aiUNApiManager = aiUNApiManagerParam;
            this.pageLinkGenerator = pageLinkGenerator;
            this.aIUNConfigurationItemProvider = aIUNConfigurationItemProvider; // Fix: Initialize the missing field
        }

        protected override AiunConfigurationItemModel Model
        {
            get
            {
                var settings = aIUNConfigurationItemProvider
                    .Get()
                    .WithID(AIUNConfigurationItemIdentifier)
                    .FirstOrDefault() ?? throw new InvalidOperationException("Specified key does not exist");
                model ??= new AiunConfigurationItemModel(settings);
                return model;
            }
        }

        public override Task ConfigurePage()
        {
            PageConfiguration.Headline = LocalizationService.GetString("Edit the configuration Item");
            return base.ConfigurePage();
        }

        protected override async Task<ICommandResponse> ProcessFormData(AiunConfigurationItemModel model, ICollection<IFormItem> formItems)
        {
            string error = await aiUNApiManager.ValidateChatbotConfiguration(model);
            if (!string.IsNullOrEmpty(error))
            {
                var validationerrorResponse = ResponseFrom(new FormSubmissionResult(FormSubmissionStatus.ValidationFailure))
                    .AddErrorMessage(error);
                return validationerrorResponse;
            }

            var result = ValidateAndProcess(model, updateExisting: true);

            if (result.ModificationResultState == ModificationResultState.Success)
            {
                var successResponse = NavigateTo(pageLinkGenerator.GetPath<AiunConfigurationItemsList>())
                    .AddSuccessMessage("Item edited");

                return successResponse;
            }

            var errorResponse = ResponseFrom(new FormSubmissionResult(FormSubmissionStatus.ValidationFailure))
                .AddErrorMessage(result.Message);

            return errorResponse;
        }
    }
}
