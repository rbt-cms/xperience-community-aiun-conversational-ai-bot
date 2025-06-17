using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CMS.DataEngine;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.InfoClasses.AIUNRegistration
{
    /// <summary>
    /// Class providing <see cref="AIUNRegistrationInfo"/> management.
    /// </summary>
    [ProviderInterface(typeof(IAIUNRegistrationInfoProvider))]
    public partial class AIUNRegistrationInfoProvider : AbstractInfoProvider<AIUNRegistrationInfo, AIUNRegistrationInfoProvider>, IAIUNRegistrationInfoProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AIUNRegistrationInfoProvider"/> class.
        /// </summary>
        public AIUNRegistrationInfoProvider()
            : base(AIUNRegistrationInfo.TYPEINFO)
        {
        }
    }
}
