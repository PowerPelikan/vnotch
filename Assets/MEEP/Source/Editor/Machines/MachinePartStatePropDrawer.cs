using UnityEditor;
using UnityEngine;

namespace MEEP.MachineSystem
{
    [CustomPropertyDrawer(typeof(MachinePartState))]
    public class MachinePartStatePropDrawer : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.serializedObject.targetObject is MachinePart)
            {
                DrawMachinePartGUI(position, property, label);
            }
            else
            {
                DrawRegularGUI(position, property, label);
            }
        }


        #region RegularDrawer

        private void DrawRegularGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label);
        }

        #endregion RegularDrawer

        #region MachinePartDrawer
        // this drawer is only used when the property is owned by a machine part

        private void DrawMachinePartGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var rects = SplitRect(position);

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUI.PropertyField(rects[0], property, label);
            }

            if (property.objectReferenceValue == null && GUI.Button(rects[1], "+"))
            {
                property.objectReferenceValue = AddAsSubAsset(property.serializedObject);
            }
            else if (property.objectReferenceValue != null && GUI.Button(rects[1], "-"))
            {
                property.objectReferenceValue = null;
            }
        }

        private Rect[] SplitRect(Rect position)
        {
            Rect[] parts = new Rect[3];

            float margin = 4;
            float buttonWidth = position.height;
            float columnWidth = position.width - (1 * margin) - (1 * buttonWidth);

            parts[0] = new Rect(position.x, position.y, columnWidth, position.height);
            parts[1] = new Rect(parts[0].xMax + margin, position.y, buttonWidth, position.height);

            return parts;
        }

        private MachinePartState AddAsSubAsset(SerializedObject targetAsset)
        {
            var assetObj = targetAsset.targetObject;
            var instance = ScriptableObject.CreateInstance<MachinePartState>();
            instance.name = "New State";

            var assetPath = AssetDatabase.GetAssetPath(assetObj);
            AssetDatabase.AddObjectToAsset(instance, assetPath);
            AssetDatabase.SaveAssetIfDirty(assetObj);

            return instance;
        }

        private void DeleteSubAsset(SerializedObject targetAsset, Object subAssetTarget)
        {
            var assetObj = targetAsset.targetObject;

            AssetDatabase.RemoveObjectFromAsset(subAssetTarget);
            AssetDatabase.SaveAssetIfDirty(assetObj);
        }

        #endregion MachinePartDrawer

    }

}