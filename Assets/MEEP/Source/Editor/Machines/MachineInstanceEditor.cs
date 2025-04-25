using UnityEditor;
using UnityEngine;

namespace MEEP.MachineSystem
{

    [CustomEditor(typeof(MachineInstance), true)]
    public class MachineInstanceEditor : Editor
    {

        MachineInstance Target => (MachineInstance)target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // draw current state

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            if (Target.PartStates == null)
            {
                EditorGUILayout.HelpBox("State Info is only displayed in Play Mode", MessageType.Info);
                return;
            }

            DrawCurrentStates();
            DrawCurrentTransitions();
        }

        private void DrawCurrentStates()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.BeginFoldoutHeaderGroup(true, "State Info");

            var parts = Target.Parts;
            var states = Target.PartStates;

            using (new GUILayout.VerticalScope())
            {
                for (int i = 0; i < parts.Count; i++)
                {
                    using (new GUILayout.HorizontalScope())
                    {

                        GUILayout.Label(parts[i].name, GUILayout.Width(150));
                        GUILayout.Label(states[parts[i]].stateName, GUI.skin.button);
                    }
                }
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            GUILayout.EndVertical();
        }



        private void DrawCurrentTransitions()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.BeginFoldoutHeaderGroup(true, "Transition Info");

            var transitions = Target.PartTransitions;

            using (new GUILayout.VerticalScope())
            {
                foreach (var (part, transition) in transitions)
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label(part.name, GUILayout.Width(100));
                        GUILayout.Label("-> " + transition.next, GUI.skin.button);
                        GUILayout.Label(transition.finishMethod.ToString(), GUI.skin.button);
                    }
                }
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            GUILayout.EndVertical();
        }


    }

}