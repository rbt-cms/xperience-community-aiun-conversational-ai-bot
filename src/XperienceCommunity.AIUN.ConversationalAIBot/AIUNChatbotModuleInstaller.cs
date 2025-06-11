using CMS.DataEngine;
using CMS.FormEngine;
using CMS.Modules;
using XperienceCommunity.AIUN.ConversationalAIBot.InfoClasses.AIUNConfigurationItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XperienceCommunity.AIUN.ConversationalAIBot
{
    public class AIUNChatbotModuleInstaller(IInfoProvider<ResourceInfo> resourceProvider)
    {
        public void Install()
        {
            var resource = resourceProvider.Get("XperienceCommunity.AIUN.ConversationalAIBot") ?? new ResourceInfo();

            InitializeResource(resource);
           InstallAIUNConfiguartionItemInfo(resource);
           InstallAIUNSettingsKeyInfo(resource);
        }



        public ResourceInfo InitializeResource(ResourceInfo resource)
        {
            resource.ResourceDisplayName = "AIUN chatbot";

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

        private void InstallAIUNSettingsKeyInfo(ResourceInfo resource)
        {
            var info = DataClassInfoProvider.GetDataClassInfo(AIUNSettingsKeyInfo.OBJECT_TYPE) ?? DataClassInfo.New(AIUNSettingsKeyInfo.OBJECT_TYPE);
            info.ClassName = AIUNSettingsKeyInfo.TYPEINFO.ObjectClassName;
            info.ClassTableName = AIUNSettingsKeyInfo.TYPEINFO.ObjectClassName.Replace(".", "_");
            info.ClassDisplayName = "SettingsKey";
            info.ClassType = ClassType.OTHER;
            info.ClassResourceID = resource.ResourceID;

            var formInfo = FormHelper.GetBasicFormDefinition(nameof(AIUNSettingsKeyInfo.AIUNSettingsKeyID));

            var formItem = new FormFieldInfo
            {
                Name = nameof(AIUNSettingsKeyInfo.SettingsKey),
                AllowEmpty = false,
                Visible = true,
                Precision = 0,
                Size=1500,
                DataType = "text",
                Enabled = true,
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
