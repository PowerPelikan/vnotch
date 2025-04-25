using UnityEngine;

namespace MEEP.Experiment
{
    /// <summary>
    /// Single use component that plays one of three animations depending 
    /// on the sample being hit.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class SampleSimPlayback : MonoBehaviour
    {

        [SerializeField]
        private PendulumSimulation simulation;

        [SerializeField]
        private GameObject BrokenPrefab;

        [SerializeField]
        private GameObject BentPrefab;

        public void PlaySimulation()
        {
            var sample = simulation.Sample;

            // no playback if there is no sample
            if (sample == null)
                return;

            // spawn broken variant of sample
            // TODO

            // move along pre-calculated path
            if (sample.GetBrittleness() < 0.5F)
            {
                GetComponent<Animator>().SetTrigger("Simulate_Bent");

            }
            else
            {
                GetComponent<Animator>().SetTrigger("Simulate_Broken");

            }
        }

    }
}