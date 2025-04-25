using UnityEngine;

namespace MEEP.Core
{
    public class MainMenuUtilities : MonoBehaviour
    {

        [SerializeField]
        private SceneLoader launchLoader;

        public void LaunchExperiment()
        {
            launchLoader.PerformLoad();
        }

        public void OpenSettingsMenu()
        {

        }

        public void CloseSettingsMenu()
        {

        }

        public void ExitApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

    }
}
