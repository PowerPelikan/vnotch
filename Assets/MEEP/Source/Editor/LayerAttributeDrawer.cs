using MEEP;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


// source: https://discussions.unity.com/t/type-for-layer-selection/91723/3

[CustomPropertyDrawer(typeof(LayerDropdownAttribute))]
class LayerAttributeEditor : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // makes a nice dropdown for our int
        property.intValue = EditorGUI.LayerField(position, label, property.intValue);
    }
}