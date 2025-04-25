namespace MEEP.InteractionSystem
{
    /// <summary>
    /// Describes the phases of an interaction
    /// </summary>
    public enum InteractionPhase
    {
        Blocked,    // the point is blocked
        Idle,       // the point is available, but the raycaster is not hovering over it
        Hover,      // the raycaster is hovering over the point. The timer is 0
        Activating, // the raycaster is filling up or winding down
        Running,    // the point has been activated
    }

}