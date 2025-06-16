using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CMS.DataEngine;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Admin.InfoClasses.AIUNRegistration
{
    /// <summary>
    /// Declares members for <see cref="AIUNRegistrationInfo"/> management.
    /// </summary>
    public partial interface IAIUNRegistrationInfoProvider : IInfoProvider<AIUNRegistrationInfo>, IInfoByIdProvider<AIUNRegistrationInfo>
    {
    }
}
