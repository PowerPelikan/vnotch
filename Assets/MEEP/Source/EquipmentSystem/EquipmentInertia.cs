using UnityEngine;

namespace MEEP.EquipmentSystem
{
    /// <summary>
    /// Adds a bit of inertia to an eqipment layer,
    /// making it feel more "held".
    /// </summary>
    public class EquipmentInertia : MonoBehaviour
    {

        /// <summary>
        /// The base rotation to track
        /// </summary>
        [SerializeField]
        private Transform trackingTarget;

        private Quaternion _prevFacing;

        private Quaternion velocity;

        public void Update()
        {
            UpdateVelocity();

            //TODO
        }

        private void UpdateVelocity()
        {
            velocity = Quaternion.FromToRotation(_prevFacing.eulerAngles, trackingTarget.rotation.eulerAngles);
            _prevFacing = trackingTarget.rotation;
        }

    }
}
