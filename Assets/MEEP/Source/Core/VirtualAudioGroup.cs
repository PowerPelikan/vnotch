using UnityEngine;


namespace MEEP.Core
{
    /// <summary>
    /// Tags an AudioSource as being part of a virtual audio group.
    /// AudioGroups receive an event to adjust their volume, to bypass
    /// WebGL limitations regarding AudioMixers
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class VirtualAudioGroup : MonoBehaviour
    {

        public enum AudioGroup { SoundEffects, Background };

        /// <summary>
        /// The audio group this source belongs to
        /// </summary>
        [SerializeField]
        private AudioGroup audioGroup;


        /// <summary>
        /// How loud this source is relative to its other group members
        /// </summary>
        [SerializeField]
        [Range(0, 2)]
        private float volumeMultiplier = 1F;

        private void Awake()
        {
            AppSettings.OnAudioSettingsChanged -= UpdateVolume;
            AppSettings.OnAudioSettingsChanged += UpdateVolume;

            // update once at startup
            UpdateVolume();
        }

        private void UpdateVolume()
        {
            if (!AppSettings.IsInitialized)
            {
                GetComponent<AudioSource>().volume = volumeMultiplier;
                return;
            }

            float targetVolume = AppSettings.VolumeMain * volumeMultiplier;

            switch (audioGroup)
            {
                case AudioGroup.SoundEffects:
                    targetVolume *= AppSettings.VolumeSound;
                    break;
                case AudioGroup.Background:
                    targetVolume *= AppSettings.VolumeMusic;
                    break;
                default:
                    throw new System.ArgumentException("Audio Group not implemented in settings");
            }

            GetComponent<AudioSource>().volume = targetVolume;
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            UpdateVolume();
        }

#endif
    }

}