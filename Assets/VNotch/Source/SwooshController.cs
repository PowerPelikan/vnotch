using UnityEngine;

namespace MEEP.Experiment
{
    public class SwooshController : MonoBehaviour
    {
        [SerializeField] private PendulumSimulation m_Simulation;
        [SerializeField] private AudioSource m_SwingAudio;

        [Tooltip("The velocity range in wich volume will be modulated.")]
        [SerializeField] private Vector2 m_VolumeVelocityMinMax = new Vector2(0, 1);
        [Tooltip("The range the velocity will be modulated to.")]
        [SerializeField] private Vector2 m_VolumeMinMax = new Vector2(0, 1);

        [Tooltip("The pitch range in wich volume will be modulated.")]
        [SerializeField] private Vector2 m_PitchVelocityMinMax = new Vector2(0, 1);
        [Tooltip("The range the pitch will be modulated to.")]
        [SerializeField] private Vector2 m_PitchMinMax = new Vector2(0.5f, 1);


        private void Start()
        {
            UpdateAudio(float.PositiveInfinity);
        }
        private void Update()
        {
            UpdateAudio(Time.deltaTime);
        }

        private void UpdateAudio(float deltaTime)
        {
            float volumeInput = Mathf.InverseLerp(m_VolumeVelocityMinMax.x, m_VolumeVelocityMinMax.y, Mathf.Abs(m_Simulation.CurrentVelocity));
            float volumeOutput = Mathf.Lerp(m_VolumeMinMax.x, m_VolumeMinMax.y, volumeInput);
            m_SwingAudio.volume = Mathf.Lerp(m_SwingAudio.volume, volumeOutput, 30 * deltaTime);

            float pitchInput = Mathf.InverseLerp(m_PitchVelocityMinMax.x, m_PitchVelocityMinMax.y, Mathf.Abs(m_Simulation.CurrentVelocity));
            float pitchOutput = Mathf.Lerp(m_PitchMinMax.x, m_PitchMinMax.y, pitchInput);
            m_SwingAudio.pitch = Mathf.Lerp(m_SwingAudio.pitch, pitchOutput, 30 * deltaTime);
        }
    }

}