using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Drawing;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace MEEP
{
    [CustomEditor(typeof(SceneLoaderGroup))]
    [CanEditMultipleObjects]
    public class SceneLoaderGroupEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var active = serializedObject.FindProperty("m_serializedSceneNames");

            if(active != null && active.arraySize > 0)
            {
                var guiContent = new GUIContent();
                guiContent.image = EditorGUIUtility.IconContent("d_SceneAsset Icon").image;
                guiContent.text = active.GetArrayElementAtIndex(active.arraySize - 1).stringValue;

                EditorGUILayout.Space();

                // show active scene target
                EditorGUILayout.BeginHorizontal();
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.PrefixLabel("Active Scene of Group:");
                    EditorGUILayout.LabelField(guiContent, EditorStyles.objectField);
                    EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                // button to load in editor
                if (GUILayout.Button("Load scenes in Editor"))
                {
                    LoadScenesInEditor((SceneLoaderGroup)target);
                }
            }
        }

        /// <summary>
        /// Load the scenes of this group in the editor
        /// </summary>
        public void LoadScenesInEditor(SceneLoaderGroup group)
        {

            Debug.Log("Opening scenes...");

            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

            var scenesToLoad = group.LoadTarget;
            var oldScenes = new List<UnityEngine.SceneManagement.Scene>();

            // collect old scenes
            for (int i = 0; i < EditorSceneManager.sceneCount; i++)
            {
                var scene = EditorSceneManager.GetSceneAt(i);
                if (!scenesToLoad.Find((x) => x.name.Equals(scene.name)))
                {
                    oldScenes.Add(scene);
                }
            }

            // open new scenes
            for (int i = 0; i < scenesToLoad.Count; i++)
            {
                var scenePath = AssetDatabase.GetAssetPath(scenesToLoad[i].GetInstanceID());
                var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);

                if (i < scenesToLoad.Count - 1)
                    continue;

                if (scene.isLoaded)
                    SetActiveScene(scene, LoadSceneMode.Additive);
                else
                    EditorSceneManager.sceneLoaded += SetActiveScene;
            }

            // close old scenes
            for (int i = 0; i < oldScenes.Count; i++)
            {
                EditorSceneManager.CloseScene(oldScenes[i], true);
            }

        }

        private void SetActiveScene(Scene scene, LoadSceneMode mode)
        {
            EditorSceneManager.SetActiveScene(scene);
            EditorSceneManager.sceneLoaded -= SetActiveScene;

            Debug.Log("Set Active Scene!");
        }

    }
}
