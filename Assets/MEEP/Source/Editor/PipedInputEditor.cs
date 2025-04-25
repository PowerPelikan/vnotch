using UnityEditor;
using UnityEngine;

namespace MEEP.InputPipelines
{
    [CustomEditor(typeof(PipedValueInput), true)]
    public class PipedInputEditor : Editor
    {

        PipedValueInput Target => (PipedValueInput)target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var users = Target.GetUsers();

            EditorGUILayout.BeginFoldoutHeaderGroup(true, "Sorted User Queue");
            EditorGUI.indentLevel++;

            for (int i = 0; i < users.Count; i++)
            {
                try
                {
                    EditorGUILayout.ObjectField(users[i], typeof(MonoBehaviour), true);
                }
                catch (System.Exception)
                {
                    EditorGUILayout.LabelField("none");
                }
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

        }

    }
}
