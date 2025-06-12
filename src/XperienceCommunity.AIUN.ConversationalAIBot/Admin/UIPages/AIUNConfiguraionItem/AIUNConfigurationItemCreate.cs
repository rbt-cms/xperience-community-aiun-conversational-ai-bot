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
    internal class AiunConfigurationItemCreate : AiunConfigurationItemBaseEditPage
    {
        private readonly IInfoProvider<AIUNConfigurationItemInfo> aIUNConfigurationItemInfoProvider;
        private readonly IPageLinkGenerator pageLinkGenerator;
        private AiunConfigurationItemModel? model = null;

        public AiunConfigurationItemCreate(
            IFormItemCollectionProvider formItemCollectionProvider,
            IFormDataBinder formDataBinder,
            IInfoProvider<AIUNConfigurationItemInfo> aIUNConfigurationItemInfoProvider,
            IPageLinkGenerator pageLinkGenerator)
            : base(formItemCollectionProvider, formDataBinder, aIUNConfigurationItemInfoProvider)
        {
            this.aIUNConfigurationItemInfoProvider = aIUNConfigurationItemInfoProvider;
            this.pageLinkGenerator = pageLinkGenerator;
        }

        protected override AiunConfigurationItemModel Model
        {
            get
            {
                var configurationItemInfo = aIUNConfigurationItemInfoProvider.Get().FirstOrDefault() ?? throw new InvalidOperationException("No AIUNConfigurationItemInfo found.");

                model ??= new AiunConfigurationItemModel(configurationItemInfo);
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
