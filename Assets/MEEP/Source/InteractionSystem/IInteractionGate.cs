namespace MEEP.InteractionSystem
{
    /// <summary>
    /// Gate intended to check whether an interaction point may be interacted with.
    /// </summary>
    public interface IInteractionGate
    {
        public bool CheckInteractionRequest();
    }
}