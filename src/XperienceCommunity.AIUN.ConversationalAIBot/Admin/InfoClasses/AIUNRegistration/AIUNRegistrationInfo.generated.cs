using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CMS;
using CMS.DataEngine;

using CMS.Helpers;

using XperienceCommunity.AIUN.ConversationalAIBot.Admin.InfoClasses.AIUNRegistration;


[assembly: RegisterObjectType(typeof(AIUNRegistrationInfo), AIUNRegistrationInfo.OBJECT_TYPE)]
namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.InfoClasses.AIUNRegistration
{
    public class AIUNRegistrationInfo : AbstractInfo<AIUNRegistrationInfo, IAIUNRegistrationInfoProvider>, IInfoWithId
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "rbt.aiunregistration";

        /// <summary>
        /// Type information.
        /// </summary>

        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(AIUNRegistrationInfoProvider), OBJECT_TYPE, "RBT.AIUNRegistration", "AIUNRegistrationItemID", null, null, null, null, null, null, null)
        {
            TouchCacheDependencies = true,
            ContinuousIntegrationSettings =
        {
            Enabled = true,
        },
        };


        /// <summary>
        /// AIUN Registration Item ID.
        /// </summary>
        [DatabaseField]
        public virtual int AIUNRegistrationItemID
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(AIUNRegistrationItemID)), 0);
            set => SetValue(nameof(AIUNRegistrationItemID), value);
        }


        /// <summary>
        /// First Name.
        /// </summary>
        [DatabaseField]
        public virtual string FirstName
        {
            get => ValidationHelper.GetString(GetValue(nameof(FirstName)), String.Empty);
            set => SetValue(nameof(FirstName), value);
        }


        /// <summary>
        /// Last Name.
        /// </summary>
        [DatabaseField]
        public virtual string LastName
        {
            get => ValidationHelper.GetString(GetValue(nameof(LastName)), String.Empty);
            set => SetValue(nameof(LastName), value);
        }


        /// <summary>
        /// Token.
        /// </summary>
        [DatabaseField]
        public virtual string Email
        {
            get => ValidationHelper.GetString(GetValue(nameof(Email)), String.Empty);
            set => SetValue(nameof(Email), value);
        }


        /// <summary>
        /// API Key.
        /// </summary>
        [DatabaseField]
        public virtual string APIKey
        {
            get => ValidationHelper.GetString(GetValue(nameof(APIKey)), String.Empty);
            set => SetValue(nameof(APIKey), value);
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
        /// Creates an empty instance of the <see cref="AIUNRegistrationInfo"/> class.
        /// </summary>
        public AIUNRegistrationInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="AIUNRegistrationInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public AIUNRegistrationInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}
