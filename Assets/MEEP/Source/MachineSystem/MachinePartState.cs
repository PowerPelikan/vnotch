using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace MEEP.MachineSystem
{
    public class MachinePartState : ScriptableObject
    {
        public string stateName;

        [Space]

        [Tooltip("MachineParts that are blocked from change by this state")]
        public List<MachinePart> partsBlockedByThis = new();

        [Space]

        public List<MachinePartTransition> transitions = new();

        [Space]

        [Header("State Events")]
        [Space]

        public UnityEvent OnStateEnter;

        public UnityEvent OnStateExit;



        private bool HasNext()
        {
            return transitions.Count > 0;
        }


        /// <summary>
        /// Find a transition to another state.
        /// </summary>
        public MachinePartTransition? FindTransitionToState(MachinePartState to)
        {
            foreach (var transition in this.transitions)
            {
                if (transition.next == to)
                    return transition;
            }

            return null;
        }


#if UNITY_EDITOR //Editor Only Validation Methods

        private void OnValidate()
        {
            SanitizeName();
            ValidateTransitions();
        }

        /// <summary>
        /// Sanitizes the state name so it can be used as a file name
        /// </summary>
        private void SanitizeName()
        {
            // filter non alphanumerics (name needs to be a valid path)
            stateName = Regex.Replace(stateName, "[^0-9a-zA-Z\\[\\]]", "");
            this.name = stateName;
        }

        /// <summary>
        /// Warns users if they attempt to place transitions of other graphs
        /// in their transitions.
        /// </summary>
        private void ValidateTransitions()
        {
            MachinePartState nextState;

            // check if transition references are part of the same asset and not of the same type
            foreach (var transition in transitions)
            {
                nextState = transition.next;
                bool isSamePath = AssetDatabase.GetAssetPath(nextState).Equals(AssetDatabase.GetAssetPath(this));

                if (nextState != null && !isSamePath)
                {
                    Debug.LogErrorFormat("Cannot transition to state {0} because it's not in the same graph", nextState);
                }
            }
        }

#endif

    }

}

