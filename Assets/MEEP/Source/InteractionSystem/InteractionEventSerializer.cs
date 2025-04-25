using UnityEngine;

namespace MEEP.InteractionSystem
{
    /// <summary>
    /// Provides a serialized version of the interaction events
    /// </summary>
    public class InteractionEventSerializer : MonoBehaviour
    {
        /// <summary>
        /// The events object itself.
        /// </summary>
        [SerializeField]
        private PointInteractionEvents eventsObject = new PointInteractionEvents();

        public PointInteractionEvents EventsObject => eventsObject;



#if UNITY_EDITOR

        private void OnValidate()
        {
            if (eventsObject == null || eventsObject.GetType() != typeof(PointInteractionEvents))
                eventsObject = new PointInteractionEvents();
        }

#endif

    }
}