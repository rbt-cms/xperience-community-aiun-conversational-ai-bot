using CMS.DataEngine;
using CMS.FormEngine;
using CMS.Modules;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.InfoClasses.AIUNRegistration;
using XperienceCommunity.AIUN.ConversationalAIBot.InfoClasses.AIUNConfigurationItem;

namespace XperienceCommunity.AIUN.ConversationalAIBot
{
    public class AiunChatbotModuleInstaller(IInfoProvider<ResourceInfo> resourceProvider)
    {
        public void Install()
        {
            var resource = resourceProvider.Get("XperienceCommunity.AIUN.ConversationalAIBot") ?? new ResourceInfo();

            _ = InitializeResource(resource);
            InstallAIUNConfiguartionItemInfo(resource);
            InstallAIUNRegistrationInfo(resource);
        }



        public ResourceInfo InitializeResource(ResourceInfo resource)
        {
            resource.ResourceDisplayName = "AIUN Chatbot";

            resource.ResourceName = "XperienceCommunity.AIUN.ConversationalAIBot";
            resource.ResourceDescription = "AIUN Chatbot module data";
            resource.ResourceIsInDevelopment = false;
            if (resource.HasChanged)
            {
                resourceProvider.Set(resource);
            }

            return resource;
        }

        private void InstallAIUNConfiguartionItemInfo(ResourceInfo resource)
        {
            var info = DataClassInfoProvider.GetDataClassInfo(AIUNConfigurationItemInfo.OBJECT_TYPE) ?? DataClassInfo.New(AIUNConfigurationItemInfo.OBJECT_TYPE);

            info.ClassName = AIUNConfigurationItemInfo.TYPEINFO.ObjectClassName;
            info.ClassTableName = AIUNConfigurationItemInfo.TYPEINFO.ObjectClassName.Replace(".", "_");
            info.ClassDisplayName = "AIUN Configuration Item";
            info.ClassType = ClassType.OTHER;
            info.ClassResourceID = resource.ResourceID;

            var formInfo = FormHelper.GetBasicFormDefinition(nameof(AIUNConfigurationItemInfo.AIUNConfigurationItemID));

            var formItem = new FormFieldInfo
            {
                Name = nameof(AIUNConfigurationItemInfo.ChannelName),
                AllowEmpty = false,
                Visible = true,
                Precision = 0,
                DataType = "text",
                Enabled = true,
            };
            formInfo.AddFormItem(formItem);



            formItem = new FormFieldInfo
            {
                Name = nameof(AIUNConfigurationItemInfo.ClientID),
                AllowEmpty = false,
                Visible = true,
                Precision = 0,
                Size = 100,
                DataType = "text",
                Enabled = true
            };
            formInfo.AddFormItem(formItem);

            formItem = new FormFieldInfo
            {
                Name = nameof(AIUNConfigurationItemInfo.APIKey),
                AllowEmpty = false,
                Visible = true,
                Precision = 0,
                Size = 200,
                DataType = "text",
                Enabled = true
            };
            formInfo.AddFormItem(formItem);

            formItem = new FormFieldInfo
            {
                Name = nameof(AIUNConfigurationItemInfo.BaseURL),
                AllowEmpty = false,
                Visible = true,
                Precision = 0,
                Size = 100,
                DataType = "text",
                Enabled = true
            };
            formInfo.AddFormItem(formItem);



            SetFormDefinition(info, formInfo);

            if (info.HasChanged)
            {
                DataClassInfoProvider.SetDataClassInfo(info);
            }

        }

        private void InstallAIUNRegistrationInfo(ResourceInfo resource)
        {
            var info = DataClassInfoProvider.GetDataClassInfo(AIUNRegistrationInfo.OBJECT_TYPE) ?? DataClassInfo.New(AIUNRegistrationInfo.OBJECT_TYPE);

            info.ClassName = AIUNRegistrationInfo.TYPEINFO.ObjectClassName;
            info.ClassTableName = AIUNRegistrationInfo.TYPEINFO.ObjectClassName.Replace(".", "_");
            info.ClassDisplayName = "AIUN Registration";
            info.ClassType = ClassType.OTHER;
            info.ClassResourceID = resource.ResourceID;

            var formInfo = FormHelper.GetBasicFormDefinition(nameof(AIUNRegistrationInfo.AIUNRegistrationItemID));

            var formItem = new FormFieldInfo
            {
                Name = nameof(AIUNRegistrationInfo.FirstName),
                AllowEmpty = false,
                Visible = true,
                Precision = 0,
                Size = 50,
                DataType = "text",
                Enabled = true,
            };
            formInfo.AddFormItem(formItem);



            formItem = new FormFieldInfo
            {
                Name = nameof(AIUNRegistrationInfo.LastName),
                AllowEmpty = false,
                Visible = true,
                Precision = 0,
                Size = 50,
                DataType = "text",
                Enabled = true
            };
            formInfo.AddFormItem(formItem);

            formItem = new FormFieldInfo
            {
                Name = nameof(AIUNRegistrationInfo.Email),
                AllowEmpty = false,
                Visible = true,
                Precision = 0,
                Size = 100,
                DataType = "text",
                Enabled = true
            };
            formInfo.AddFormItem(formItem);

            formItem = new FormFieldInfo
            {
                Name = nameof(AIUNRegistrationInfo.APIKey),
                AllowEmpty = false,
                Visible = true,
                Precision = 0,
                Size = 400,
                DataType = "text",
                Enabled = true
            };
            formInfo.AddFormItem(formItem);

            SetFormDefinition(info, formInfo);

            if (info.HasChanged)
            {
                DataClassInfoProvider.SetDataClassInfo(info);
            }

        }

        /// <summary>
        /// Ensure that the form is upserted with any existing form
        /// </summary>
        /// <param name="info"></param>
        /// <param name="form"></param>
        private static void SetFormDefinition(DataClassInfo info, FormInfo form)
        {
            if (info.ClassID > 0)
            {
                var existingForm = new FormInfo(info.ClassFormDefinition);
                existingForm.CombineWithForm(form, new());
                info.ClassFormDefinition = existingForm.GetXmlDefinition();
            }
            else
            {
                info.ClassFormDefinition = form.GetXmlDefinition();
            }
        }
    }
}
