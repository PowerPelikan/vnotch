using MEEP.MachineSystem;
using System.Collections;
using UnityEngine;

namespace VNotch.Experiment
{
    /// <summary>
    /// Controls the dial of the pendulum machine
    /// </summary>
    public class DialPointerResetAnimation : MonoBehaviour
    {

        [SerializeField]
        private MachinePart dialPartRef;

        [SerializeField]
        private MachineInstance machineRef;

        [Space]

        /// <summary>
        /// Transition time in sec/deg
        /// </summary>
        [SerializeField]
        private float transitionTime;

        /// <summary>
        /// Curve defining the speed of interpolation
        /// </summary>
        [SerializeField]
        private AnimationCurve interpolationCurve;

        [Space]

        /// <summary>
        /// The angle towards which the dial should point in its rest position
        /// </summary>
        [SerializeField]
        private float targetAngle;

        /// <summary>
        /// The transform of the obstacle pointer
        /// </summary>
        [SerializeField]
        private Transform obstacleTransform;

        /// <summary>
        /// Should the direction of the obstacle transform be inverted?
        /// </summary>
        [SerializeField]
        private bool invertObstacle;

        /// <summary>
        /// The angle of the obstacle that should not be passed while resetting
        /// </summary>
        private float ObstacleAngle
        {
            get
            {
                if (obstacleTransform == null)
                    return 0;

                float invert = invertObstacle ? -1 : 1;
                return Vector3.SignedAngle(Vector3.up, invert * obstacleTransform.up, transform.forward);
            }
        }


        /// <summary>
        /// The current angle of the dial
        /// </summary>
        private float currentAngle;


        private void Awake()
        {
            this.enabled = false;
        }

        /// <summary>
        /// Reset the angle of the dial with an animated transition.
        /// </summary>
        [ContextMenu("Reset Dial")]
        public void ResetDial()
        {
            currentAngle = Vector3.SignedAngle(Vector3.up, transform.up, transform.forward);
            StartCoroutine(DialTransition());
        }

        private IEnumerator DialTransition()
        {
            //Enable transform updates
            this.enabled = true;
            GetComponent<DialPointerFollowAnimation>().enabled = false;

            // button to pushed position (0.3 sec)
            for (float i = 0; i < 0.3F; i += Time.deltaTime)
            {
                yield return new WaitForEndOfFrame();
            }

            var startAngle = currentAngle;
            var fullTime = transitionTime * Mathf.Abs(targetAngle - currentAngle);
            // rotate dial (depends on rotation angle)
            for (float currentTime = 0; currentTime < fullTime; currentTime += Time.deltaTime)
            {
                var relativeTime = currentTime / fullTime;
                currentAngle = Mathf.Lerp(
                    startAngle,
                    targetAngle,
                    interpolationCurve.Evaluate(interpolationCurve.Evaluate(relativeTime)));

                yield return new WaitForEndOfFrame();
            }
            // set pointer precisely
            currentAngle = targetAngle;

            // button to rest position
            for (float i = 0; i < 0.3F; i += Time.deltaTime)
            {
                yield return new WaitForEndOfFrame();
            }

            //Disable transform updates
            this.enabled = false;
            GetComponent<DialPointerFollowAnimation>().enabled = true;
        }

        private void LateUpdate()
        {
            var lookRotation = Quaternion.LookRotation(transform.forward, Quaternion.AngleAxis(currentAngle, transform.forward) * Vector3.up);
            transform.rotation = lookRotation;
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(currentAngle, transform.forward) * Vector3.up);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(targetAngle, transform.forward) * Vector3.up);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(ObstacleAngle, transform.forward) * Vector3.up);
        }

    }

}