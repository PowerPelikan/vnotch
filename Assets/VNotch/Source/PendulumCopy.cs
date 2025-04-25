using UnityEngine;

namespace MEEP.Experiment
{
    public class PendulumCopy : MonoBehaviour
    {

        [SerializeField]
        private PendulumSimulation simulation;

        /// <summary>
        /// The initial forward direction. 
        /// Using this instead of the current one prevents twisting.
        /// </summary>
        private Vector3 initialForward;


        private void Awake()
        {
            initialForward = transform.forward;
        }

        private void LateUpdate()
        {
            // It's ugly but it works
            Vector3 up = (simulation.PendulumVector - transform.position).normalized;
            Vector3 right = transform.right;
            this.transform.rotation = Quaternion.LookRotation(initialForward, up);
        }
    }
}
