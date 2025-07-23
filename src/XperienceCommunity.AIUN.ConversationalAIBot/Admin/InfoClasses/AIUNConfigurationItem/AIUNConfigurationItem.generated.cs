using System;
using System.Data;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using XperienceCommunity.AIUN.ConversationalAIBot.InfoClasses.AIUNConfigurationItem;

[assembly: RegisterObjectType(typeof(AIUNConfigurationItemInfo), AIUNConfigurationItemInfo.OBJECT_TYPE)]

namespace XperienceCommunity.AIUN.ConversationalAIBot.InfoClasses.AIUNConfigurationItem
{
    /// <summary>
    /// Data container class for <see cref="AIUNConfigurationItemInfo"/>.
    /// </summary>
    public partial class AIUNConfigurationItemInfo : AbstractInfo<AIUNConfigurationItemInfo, IInfoProvider<AIUNConfigurationItemInfo>>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "RBT.AIUNConfigurationitem";


        /// <summary>
        /// Type information.
        /// </summary>

        public static readonly ObjectTypeInfo TYPEINFO = new(typeof(IInfoProvider<AIUNConfigurationItemInfo>), OBJECT_TYPE, "RBT.AIUNConfigurationitem", nameof(AIUNConfigurationItemID), null,null, nameof(ClientID), nameof(APIKey), nameof(BaseURL), null, null)
        {
            TouchCacheDependencies = true,
            ContinuousIntegrationSettings =
        {
            Enabled = true,
        },
        };


        /// <summary>
        /// AIUN configuration ID.
        /// </summary>
        [DatabaseField]
        public virtual int AIUNConfigurationItemID
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(AIUNConfigurationItemID)), 0);
            set => SetValue(nameof(AIUNConfigurationItemID), value);
        }


        /// <summary>
        /// Channel ID.
        /// </summary>
        [DatabaseField]
        public virtual string ChannelName
        {
            get => ValidationHelper.GetString(GetValue(nameof(ChannelName)), String.Empty);
            set => SetValue(nameof(ChannelName), value);
        }


        /// <summary>
        /// Client ID.
        /// </summary>
        [DatabaseField]
        public virtual string ClientID
        {
            get => ValidationHelper.GetString(GetValue(nameof(ClientID)), String.Empty);
            set => SetValue(nameof(ClientID), value);
        }


        /// <summary>
        /// Token.
        /// </summary>
        [DatabaseField]
        public virtual string APIKey
        {
            get => ValidationHelper.GetString(GetValue(nameof(APIKey)), String.Empty);
            set => SetValue(nameof(APIKey), value);
        }


        /// <summary>
        /// Base URL.
        /// </summary>
        [DatabaseField]
        public virtual string BaseURL
        {
            get => ValidationHelper.GetString(GetValue(nameof(BaseURL)), String.Empty);
            set => SetValue(nameof(BaseURL), value);
        }

        /// <summary>
        /// Base URL.
        /// </summary>
        [DatabaseField]
        public virtual string LastUpdated
        {
            get => ValidationHelper.GetString(GetValue(nameof(LastUpdated)), String.Empty);
            set => SetValue(nameof(LastUpdated), value);
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
        /// Creates an empty instance of the <see cref="AIUNConfigurationItemInfo"/> class.
        /// </summary>
        public AIUNConfigurationItemInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="AIUNConfigurationItemInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public AIUNConfigurationItemInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}
