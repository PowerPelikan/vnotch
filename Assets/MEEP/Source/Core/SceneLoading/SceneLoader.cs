using MEEP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MEEP
{

    public class SceneLoader : MonoBehaviour
    {

        [Tooltip("Defines load behavior. " +
            "OnStart loads the specified scenes automatically once the object is loaded itself." +
            "Manual loads the specified scenes on demand (for example, when a button is pressed).")]
        [SerializeField]
        private StartupBehaviour loadBehaviour;

        [Space]

        /// <summary>
        /// the (unique) names of the scenes we need to load
        /// </summary>
        [SerializeField]
        private SceneLoaderGroup loaderGroup;

        /// <summary>
        /// the last scene that is supposed to be loaded
        /// </summary>
        private Scene lastLoadingCall;

        private Scene payloadScene;


        private List<Scene> scenesToUnload;

        private List<string> remainingScenesToLoad;

        private void Start()
        {
            if (loadBehaviour != StartupBehaviour.OnStart)
                return;

            if (LoadingScreen.IsReady)
                PerformLoad();
            else
                LoadingScreen.onIsReady += OnLoadingScreenInitialized;
        }


        private void OnLoadingScreenInitialized()
        {
            PerformLoad();
            LoadingScreen.onIsReady -= OnLoadingScreenInitialized;
        }


        public void PerformLoad()
        {
            this.transform.parent = null;
            DontDestroyOnLoad(this.gameObject);

            // save payload data
            FlushPayloadToTempScene();

            CollectScenesToLoad();

            LoadingScreen.Show();

            // unload the scenes we don't need
            foreach (var scene in scenesToUnload)
                SceneManager.UnloadSceneAsync(scene);

            // start loading specified scenes
            for (int i = 0; i < remainingScenesToLoad.Count; i++)
            {
                SceneManager.LoadScene(remainingScenesToLoad[i], LoadSceneMode.Additive);
            }

            // register loading call we wait for
            lastLoadingCall = SceneManager.GetSceneByName(remainingScenesToLoad[remainingScenesToLoad.Count - 1]);

            StartCoroutine(WaitForLoadOrTime(1));
        }


        /// <summary>
        /// "saves" added payload data in a temporary scene, 
        /// which can then be accessed by the newly loaded set of scenes
        /// </summary>
        private void FlushPayloadToTempScene()
        {
            if (GetComponent<SceneLoaderPayload>() == null)
                return;

            // create temp scene

            payloadScene = SceneManager.CreateScene("PayloadData");
            var containerObj = new GameObject("PayloadData");

            SceneManager.MoveGameObjectToScene(containerObj, payloadScene);

            // copy data
            var payloadData = GetComponents<SceneLoaderPayload>();

            for (int i = 0; i < payloadData.Length; i++)
            {
                var payload = containerObj.AddComponent<SceneLoaderPayload>();
                // TODO optimize casting
                payload.AddToPayload(payloadData[i].PayloadObjects.ToArray());
            }

        }

        private void CollectScenesToLoad()
        {
            var loadTarget = loaderGroup.SerializedScenes;

            scenesToUnload = new();
            remainingScenesToLoad = new(loadTarget);

            // survey loaded scenes
            for (int i = 0; i < SceneManager.loadedSceneCount; i++)
            {
                var currentScene = SceneManager.GetSceneAt(i);

                if (loadTarget.Contains(currentScene.name))
                    remainingScenesToLoad.Remove(currentScene.name);
                else if (currentScene != LoadingScreen.LoadingScreenScene && currentScene != payloadScene)
                    scenesToUnload.Add(currentScene);
            }
        }

        private IEnumerator WaitForLoadOrTime(float timeToWait)
        {
            yield return new WaitForSecondsRealtime(timeToWait);

            while (!lastLoadingCall.isLoaded)
            {
                yield return new WaitForEndOfFrame();
            }

            HandleLastSceneLoaded();
        }

        private void HandleLastSceneLoaded()
        {
            Debug.Log("Finished Loading Scenes!");
            SceneManager.SetActiveScene(lastLoadingCall);
            LoadingScreen.Hide();

            Destroy(this.gameObject);
        }

    }
}
