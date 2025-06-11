using System;
using System.Data;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using XperienceCommunity.AIUN.ConversationalAIBot;

[assembly: RegisterObjectType(typeof(AIUNSettingsKeyInfo), AIUNSettingsKeyInfo.OBJECT_TYPE)]

namespace XperienceCommunity.AIUN.ConversationalAIBot
{
    /// <summary>
    /// Data container class for <see cref="AIUNSettingsKeyInfo"/>.
    /// </summary>
    public partial class AIUNSettingsKeyInfo : AbstractInfo<AIUNSettingsKeyInfo, IAIUNSettingsKeyInfoProvider>, IInfoWithId
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "rbt.aiunsettingskey";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(AIUNSettingsKeyInfoProvider), OBJECT_TYPE, "RBT.AIUNSettingsKey", "AIUNSettingsKeyID", null, null, null, null, null, null, null)
        {
            TouchCacheDependencies = true,
        };


        /// <summary>
        /// AIUN settings key ID.
        /// </summary>
        [DatabaseField]
        public virtual int AIUNSettingsKeyID
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(AIUNSettingsKeyID)), 0);
            set => SetValue(nameof(AIUNSettingsKeyID), value);
        }


        /// <summary>
        /// Settings key.
        /// </summary>
        [DatabaseField]
        public virtual string SettingsKey
        {
            get => ValidationHelper.GetString(GetValue(nameof(SettingsKey)), String.Empty);
            set => SetValue(nameof(SettingsKey), value, String.Empty);
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            Provider.Delete(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            Provider.Set(this);
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="AIUNSettingsKeyInfo"/> class.
        /// </summary>
        public AIUNSettingsKeyInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="AIUNSettingsKeyInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public AIUNSettingsKeyInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}
