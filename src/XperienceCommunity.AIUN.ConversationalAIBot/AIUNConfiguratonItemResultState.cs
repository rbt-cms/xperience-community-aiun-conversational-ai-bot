using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



#nullable enable
namespace XperienceCommunity.AIUN.ConversationalAIBot
{

    public enum ModificationResultState
    {
        Success,
        Failure
    }

    public class ModificationResult(ModificationResultState resultState, string? message = null)
    {
        public ModificationResultState ModificationResultState { get; set; } = resultState;
        public string? Message { get; set; } = message;
    }


}
