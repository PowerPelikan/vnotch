using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MEEP.MachineSystem
{
    [CustomEditor(typeof(MachinePart))]
    public class MachinePartEditor : Editor
    {

        MachinePart Target => (MachinePart)target;

        string Path => AssetDatabase.GetAssetPath(target);

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // asset options

            using (new GUILayout.VerticalScope())
            {
                GUILayout.Label("Asset Options", EditorStyles.boldLabel);

                if (GUILayout.Button("Refresh State Assets"))
                {
                    ValidateSubAssets();
                }
            }
        }


        private void OnValidate()
        {
            ValidateSubAssets();
        }

        private void ValidateSubAssets()
        {
            RemoveListDuplicates();

            RemoveDegenerateStates();

            SaveStateChanges();
        }

        /// <summary>
        /// Set duplicate state entries to null
        /// </summary>
        private void RemoveListDuplicates()
        {
            var counted = new HashSet<MachinePartState>();

            for (int i = 0; i < Target.states.Count; i++)
            {
                if (counted.Contains(Target.states[i]))
                {
                    Target.states[i] = null;
                }
                else
                {
                    counted.Add(Target.states[i]);
                }
            }
        }


        /// <summary>
        /// Remove states that are part of the asset, but not the state list
        /// </summary>
        private void RemoveDegenerateStates()
        {
            var subAssets = AssetDatabase.LoadAllAssetsAtPath(Path);

            var assetsToRemove = new List<Object>();

            // mark assets to remove
            for (int i = 0; i < subAssets.Length; i++)
            {
                if (AssetDatabase.IsSubAsset(subAssets[i]) && !Target.states.Contains(subAssets[i]))
                {
                    assetsToRemove.Add(subAssets[i]);
                }
            }

            // actually remove them
            for (int i = 0; i < assetsToRemove.Count; i++)
            {
                AssetDatabase.RemoveObjectFromAsset(assetsToRemove[i]);
            }
        }


        private void SaveStateChanges()
        {
            AssetDatabase.ImportAsset(Path);
            AssetDatabase.SaveAssetIfDirty(target);
            AssetDatabase.Refresh();
        }

    }

}