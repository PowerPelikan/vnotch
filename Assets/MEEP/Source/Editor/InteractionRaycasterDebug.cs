using UnityEditor;

namespace MEEP.InteractionSystem
{
    [CustomEditor(typeof(InteractionRaycaster))]
    public class InteractionRaycasterDebug : Editor
    {

        InteractionRaycaster Target => (InteractionRaycaster)target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginFoldoutHeaderGroup(true, "HITS");
            EditorGUI.indentLevel++;

            for (int i = 0; i < Target._hits.Length; i++)
            {
                try
                {
                    var hit = Target._hits[i].collider.name;
                    EditorGUILayout.LabelField(hit);
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