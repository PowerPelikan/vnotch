using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TypeRestrictionAttribute), false)]
public class InterfaceReferenceAttributeDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUILayout.PropertyField(property);

        var attType = (attribute as TypeRestrictionAttribute).targetType;
        var propType = property.objectReferenceValue?.GetType();

        if (propType != null && !attType.IsAssignableFrom(propType))
        {
            Debug.LogWarningFormat("Behaviour {0} does not implement {1}", propType, attType);
            property.objectReferenceValue = null;
        }

    }

}
