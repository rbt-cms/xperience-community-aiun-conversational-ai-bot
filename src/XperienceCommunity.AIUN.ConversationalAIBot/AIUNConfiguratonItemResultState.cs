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
