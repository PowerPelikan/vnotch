using UnityEngine;

namespace VNotch.Experiment
{
    /// <summary>
    /// Defines an Offset constraint.
    /// </summary>
    public class DialPointerFollowAnimation : AccumulationBehaviour
    {
        /// <summary>
        /// The Amount by which to offset the pointer
        /// </summary>
        [Space]
        [SerializeField]
        [Range(0, 45)]
        private float angleOffset;

        /// <summary>
        /// the transform being followed
        /// </summary>
        [SerializeField]
        private Transform targetTransform;

        /// <summary>
        /// The angle measured on the previous tick
        /// </summary>
        private float previousAngle;

        private Quaternion targetRotation;

        private void OnEnable()
        {
            targetRotation = transform.rotation;
        }

        private void LateUpdate()
        {
            transform.rotation = targetRotation;
        }


        public override void Tick()
        {
            var angle = Vector3.SignedAngle(-targetTransform.up, transform.up, targetTransform.forward);

            //if the sign changes between frames, the pointer probably passed through (unless it's really fast)
            if (!Mathf.Sign(previousAngle).Equals(Mathf.Sign(angle)) && Mathf.Abs(angle) <= 90)
            {
                // push according to previous angle
                targetRotation = CalculateFollowRotation(previousAngle < 0);
                transform.rotation = targetRotation;
                angle = Vector3.SignedAngle(-targetTransform.up, transform.up, targetTransform.forward); // TODO incorporate this into the Update Loop
            }
            else if (Mathf.Abs(angle) <= angleOffset)
            {
                targetRotation = CalculateFollowRotation(angle < 0);
                transform.rotation = targetRotation;
            }


            previousAngle = angle;
        }

        /// <summary>
        /// 
        /// </summary>
        private Quaternion CalculateFollowRotation(bool pushLeft)
        {
            var direction = pushLeft ? -1 : 1;
            // base of followed pointer
            var rotation = Quaternion.AngleAxis(180, targetTransform.forward) * targetTransform.rotation;
            // offset
            return Quaternion.AngleAxis(direction * angleOffset, targetTransform.forward) * rotation;
        }


        private void OnDrawGizmosSelected()
        {
            //Gizmos.color = Color.white;
            //Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(currentAngle, transform.forward) * Vector3.up);
            //Gizmos.color = Color.green;
            //Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(TargetAngle, transform.forward) * Vector3.up);
        }
    }
}