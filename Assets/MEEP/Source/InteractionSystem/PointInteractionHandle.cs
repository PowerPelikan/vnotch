using UnityEngine;

namespace MEEP.InteractionSystem
{
    /// <summary>
    /// Represents a contract between InteractionPartners. 
    /// Allows either party to notify the other in order to cancel further interaction.
    /// </summary>
    [System.Serializable]
    public class PointInteractionHandle : InteractionHandle
    {

        /// <summary>
        /// The interaction point that has been activated
        /// </summary>
        [SerializeReference]
        [HideInInspector]
        private InteractionPoint point;


        public InteractionRaycaster Controller => instigator;

        public InteractionPoint Point => point;


        public PointInteractionHandle(InteractionRaycaster issuer, InteractionPoint point)
        {
            this.instigator = issuer;
            this.point = point;
            this.events = point.GetComponent<InteractionEventSerializer>()?.EventsObject;

            events?.InvokeStarted(this);
        }

        /// <summary>
        /// Cancels the current interaction. 
        /// Self is used to notify the given partner.
        /// </summary>
        public override void Cancel()
        {
            point.OnInteractionCanceled();
            instigator.OnInteractionCanceled();

            base.Cancel();
        }

    }

}