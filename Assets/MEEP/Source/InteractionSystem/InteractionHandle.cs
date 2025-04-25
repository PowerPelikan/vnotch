using UnityEngine;

namespace MEEP.InteractionSystem
{

    /// <summary>
    /// References an ongoing interaction.
    /// Exposes events that represent stages of that interaction
    /// </summary>
    public class InteractionHandle
    {

        /// <summary>
        /// The raycaster issuing the interaction
        /// </summary>
        [SerializeReference]
        protected InteractionRaycaster instigator;

        /// <summary>
        /// The event handler associated with this interaction point
        /// </summary>
        protected InteractionEvents events;

        public InteractionEvents Events => events;

        public InteractionRaycaster Instigator => instigator;

        public InteractionHandle()
        {
            this.events = new InteractionEvents();
            events.InvokeStarted(this);
        }

        /// <summary>
        /// Cancels the current interaction. 
        /// </summary>
        public virtual void Cancel()
        {
            events.InvokeCanceled(this);
            Debug.Log("Canceled Interaction!");
        }

        /// <summary>
        /// Completes the current interaction. 
        /// </summary>
        public virtual void Complete()
        {
            events.InvokeCompleted(this);
            Debug.Log("Finished Interaction!");
        }

    }
}
