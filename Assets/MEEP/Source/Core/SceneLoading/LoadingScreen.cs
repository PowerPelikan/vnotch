using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{

    // TODO this necessitates that the loading screen must be named as such, which is a bit crude
    private const string LOAD_SCREEN_NAME = "LoadingScreen";

    private static LoadingScreen instance;

    public static bool IsReady => instance != null;

    public static Scene LoadingScreenScene => instance.gameObject.scene;



    public static event Action onIsReady;



    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoLoad()
    {
        if (!SceneManager.GetSceneByName(LOAD_SCREEN_NAME).IsValid())
        {
            SceneManager.LoadScene(LOAD_SCREEN_NAME, LoadSceneMode.Additive);
            SceneManager.sceneLoaded += SetInstance;
        }
        else
        {
            instance = FindAnyObjectByType<LoadingScreen>(FindObjectsInactive.Include);
            onIsReady?.Invoke();
        }
    }

    private static void SetInstance(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals(LOAD_SCREEN_NAME))
        {
            instance = FindAnyObjectByType<LoadingScreen>(FindObjectsInactive.Include);
            SceneManager.sceneLoaded -= SetInstance;
            onIsReady?.Invoke();
        }
    }

    /// <summary>
    /// Shows the loading screen
    /// </summary>
    public static void Show()
    {
        instance.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides the loading screen
    /// </summary>
    public static void Hide()
    {
        instance.gameObject.SetActive(false);
    }

}
