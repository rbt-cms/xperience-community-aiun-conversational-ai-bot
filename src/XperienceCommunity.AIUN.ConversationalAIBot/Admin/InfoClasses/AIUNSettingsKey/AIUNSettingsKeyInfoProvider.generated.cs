using CMS.DataEngine;

namespace XperienceCommunity.AIUN.ConversationalAIBot
{
    /// <summary>
    /// Class providing <see cref="AIUNSettingsKeyInfo"/> management.
    /// </summary>
    [ProviderInterface(typeof(IAIUNSettingsKeyInfoProvider))]
    public partial class AIUNSettingsKeyInfoProvider : AbstractInfoProvider<AIUNSettingsKeyInfo, AIUNSettingsKeyInfoProvider>, IAIUNSettingsKeyInfoProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AIUNSettingsKeyInfoProvider"/> class.
        /// </summary>
        public AIUNSettingsKeyInfoProvider()
            : base(AIUNSettingsKeyInfo.TYPEINFO)
        {
        }
    }
}
