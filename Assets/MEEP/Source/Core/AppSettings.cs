using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace MEEP.Core
{
    /// <summary>
    /// Simple wrapper for settings menu related configurations.
    /// Uses PlayerPrefs to store values (which is not ideal, but at least compatible with WebGL)
    /// </summary>
    public class AppSettings
    {
        private static AppSettings instance;

        public static event Action OnAudioSettingsChanged;

        public static event Action OnLocSettingsChanged;

        public static event Action OnGraphicsSettingsChanged;

        public static event Action OnAccessiblitySettingsChanged;



        #region Available Settings

        //language settings
        private LocaleIdentifier selectedLocale;

        //sound settings

        private float volume_main;
        private float volume_sfx;
        private float volume_music;

        /// <summary> if anabled, tells the app to display subtitles for sound effects. </summary>
        private bool accessibleSound;

        //graphics settings

        /// <summary> determines the quality level of the graphics. 0 being the best. </summary>
        private int qualityLevel;

        #endregion



        #region Properties

        public static bool IsInitialized => instance != null;

        public static LocaleIdentifier SelectedLocale => instance.selectedLocale;

        public static float VolumeMain => instance.volume_main;
        public static float VolumeSound => instance.volume_sfx;
        public static float VolumeMusic => instance.volume_music;

        public static bool AccessibleSound => instance.accessibleSound;

        public static int QualityLevel => instance.qualityLevel;

        #endregion



        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void InitializeSettings()
        {
            Debug.Log("Initializing Settings...");

            instance = LoadSettings();

            Debug.Log("Settings Loaded.");
        }


        /// <summary>
        /// Create a settings instance from the player prefs or default values
        /// </summary>
        public static AppSettings LoadSettings()
        {
            var instance = new AppSettings();

            Debug.Log("Loading PlayerPrefs...");

            instance.volume_main = PlayerPrefs.GetFloat(nameof(volume_main), 0.5F);
            instance.volume_sfx = PlayerPrefs.GetFloat(nameof(volume_sfx), 0.5F);
            instance.volume_music = PlayerPrefs.GetFloat(nameof(volume_music), 0.5F);

            instance.accessibleSound = PlayerPrefs.GetInt(nameof(accessibleSound), 1) > 0;

            instance.qualityLevel = PlayerPrefs.GetInt(nameof(qualityLevel), 0);

            return instance;
        }


        #region Apply and Save

        /// <summary>
        /// Applies all settings defined in the app settings
        /// </summary>
        public static void ApplyAllSettings()
        {
            ApplyAudioSettings();
            ApplyLocalizationSettings();
            ApplyGraphicsSettings();
            ApplyAccessiblitySettings();
        }


        /// <summary>
        /// Applies and saves the audio settings.
        /// </summary>
        public static void ApplyAudioSettings()
        {
            PlayerPrefs.SetFloat(nameof(volume_main), instance.volume_main);
            PlayerPrefs.SetFloat(nameof(volume_sfx), instance.volume_sfx);
            PlayerPrefs.SetFloat(nameof(volume_music), instance.volume_music);

            PlayerPrefs.Save();
            OnAudioSettingsChanged?.Invoke();
        }

        /// <summary>
        /// Applies and saves the localization settings.
        /// </summary>
        public static void ApplyLocalizationSettings()
        {
            PlayerPrefs.SetString(nameof(selectedLocale), instance.selectedLocale.Code);
            LocalizationSettings.SelectedLocale.Identifier = instance.selectedLocale;

            PlayerPrefs.Save();
            OnLocSettingsChanged?.Invoke();
        }

        /// <summary>
        /// Applies and saves the graphics settings.
        /// </summary>
        public static void ApplyGraphicsSettings()
        {
            PlayerPrefs.SetInt(nameof(qualityLevel), instance.qualityLevel);
            QualitySettings.SetQualityLevel(instance.qualityLevel, true);

            PlayerPrefs.Save();
            OnGraphicsSettingsChanged?.Invoke();
        }

        /// <summary>
        /// Applies and saves the accessiblity settings.
        /// </summary>
        public static void ApplyAccessiblitySettings()
        {
            PlayerPrefs.SetInt(nameof(accessibleSound), instance.accessibleSound ? 1 : 0);

            PlayerPrefs.Save();
            OnAccessiblitySettingsChanged?.Invoke();
        }

        #endregion Apply and Save
    }
}
