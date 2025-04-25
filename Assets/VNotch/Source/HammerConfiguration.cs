using UnityEngine;

namespace MEEP.Experiment
{
    [CreateAssetMenu(fileName = "myHammerConfig", menuName = "VNotch/HammerConfiguration")]
    public class HammerConfiguration : ScriptableObject
    {

        [SerializeField]
        [Tooltip("The labor Capacity of the pendulum hammer in Joule")]
        private float laborCapacity = 300;

        [SerializeField]
        [Tooltip("length of the hammer's pendulum. Measured from the center to the impact surface." +
            "In meters")]
        private float armLength = 1;

        [SerializeField]
        [Range(-Mathf.PI, 0.01F)]
        [Tooltip("The angle at which the arm is released. " +
            "Acts as the reference value for the hammer's full capacity." +
            "In radians")]
        private float initialAngle = (-Mathf.PI / 4 * 3);



        public float LaborCapacity => laborCapacity;

        public float ArmLength => armLength;

        /// <summary>
        /// 
        /// </summary>
        public float InitialAngle => initialAngle;

        public float InitialHeight => AngleToHeight(initialAngle);

        /// <summary>
        /// Returns the mass of the hammer for calculations.
        /// Note that this is dependent on the initial angle and arm length.
        /// </summary>
        public float Mass => laborCapacity / (Physics.gravity.magnitude * AngleToHeight(initialAngle));



        /// <summary>
        /// Convert an angle on the pendulum into a height,
        /// measured from the lowest point in the arc.
        /// </summary>
        public float AngleToHeight(float radAngle)
        {
            return armLength * (1 - Mathf.Cos(radAngle));
        }

        public float HeightToAngle(float height)
        {
            return Mathf.Acos(1 - height / armLength);
        }

    }

}