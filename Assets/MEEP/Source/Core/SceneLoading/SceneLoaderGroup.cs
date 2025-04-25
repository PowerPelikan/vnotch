using System.Collections.Generic;
using UnityEngine;

namespace MEEP
{
    /// <summary>
    /// Contains one or more scenes that should be loaded additively together.
    /// A scene may be member of one or more groups at once.
    /// </summary>
    [CreateAssetMenu(menuName = "MEEP/SceneLoaderGroup", fileName = "SceneLoaderGroup")]
    public class SceneLoaderGroup : ScriptableObject
    {
        /// <summary>
        /// the (unique) names of the scenes
        /// </summary>
        [SerializeField, HideInInspector]
        private string[] m_serializedSceneNames;

        /// <summary>
        /// The scenes contained within this scene group, identified by name.
        /// </summary>
        public string[] SerializedScenes => m_serializedSceneNames;

        /// <summary>
        /// The scene that is marked as active, identified by name.
        /// </summary>
        public string SerializedActiveScene => m_serializedSceneNames[m_serializedSceneNames.Length - 1];

#if UNITY_EDITOR

        /* 
         * These are editor specific fields, so we can drag and drop scene assets
         * Note that upon build, these classes no longer exist, so we need to reference
         * our scenes by strings.
         */

        [Space]

        [SerializeField]
        public List<UnityEditor.SceneAsset> LoadTarget = new();

        private void Awake()
        {
            OnValidate();
        }

        /// <summary>
        /// Makes sure the reference strings are set
        /// </summary>
        private void OnValidate()
        {
            m_serializedSceneNames = new string[LoadTarget.Count];

            for (int i = 0; i < LoadTarget.Count; i++)
            {
                if (LoadTarget[i] != null)
                    m_serializedSceneNames[i] = LoadTarget[i].name;
            }
        }

#endif

    }
}
