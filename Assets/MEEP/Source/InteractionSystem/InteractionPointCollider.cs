using UnityEngine;

namespace MEEP.InteractionSystem
{
    /// <summary>
    /// Tags a collider as belonging to a certain interaction point
    /// </summary>
    [DisallowMultipleComponent]
    public class InteractionPointCollider : MonoBehaviour
    {

        private InteractionPoint owner;

        private new Collider collider;


        public InteractionPoint Owner => owner;

        public Collider Collider => collider;


        /// <summary>
        /// Initializes an instance of this component on the given game object
        /// </summary>
        public static void TagGameObject(Collider collider, InteractionPoint owner)
        {
            var compInstance = collider.gameObject.AddComponent<InteractionPointCollider>();
            compInstance.owner = owner;
            compInstance.collider = collider;
        }

    }
}
