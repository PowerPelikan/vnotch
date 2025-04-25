using MEEP.InteractionSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace MEEP.MachineSystem
{
    /// <summary>
    /// Represents a machine interaction.
    /// Allows one to specify a desired post condition of a machine interaction.
    /// 
    /// NOTE: Interactions are currently limited to states that have direct transitions to 
    /// each other.
    /// 
    /// </summary>
    public class MachineInteraction : MonoBehaviour
    {
        [System.Serializable]
        public struct PartCondition
        {
            /// <summary>
            /// Overrides the targeted machine for this interaction.
            /// Defaults to the target Machine of the MachineInteraction component.
            /// </summary>
            public MachineInstance targetMachineOverride;

            [Space]

            public MachinePart part;
            public MachinePartState state;
        }

        [Space]

        [SerializeField]
        [Tooltip("name of this interaction. Not used in code. Only for debugging")]
        private new string name;

        [SerializeField]
        [Tooltip("the short description to be displayed on the interaction point UI")]
        public LocalizedString interactionDescription;

        [Space]

        public MachineInstance targetMachine;

        [Space]

        public List<PartCondition> preCondition;

        public List<PartCondition> postCondition;

        [Space]

        public UnityEvent<InteractionHandle> OnPerformed;

        public UnityEvent<InteractionHandle> OnFinished;

        /// <summary>
        /// Checks the preconditions and then excutes the appropriate step.
        /// </summary>
        public void TryPerformInteraction(InteractionHandle handle)
        {
            // ask the machine to animate transitions, then set the post condition
            bool wasStarted = targetMachine.TryPerformMachineInteraction(this, handle);
            if (wasStarted)
            {
                OnPerformed?.Invoke(handle);
                handle.Events.OnCompleted.AddListener(OnFinished.Invoke);
            }

        }


        /// <summary>
        /// Returns the Machine Instance on which to perform a conditions checks.
        /// </summary>
        public MachineInstance GetMachineOfCondition(PartCondition condition)
        {
            if (condition.targetMachineOverride != null)
                return condition.targetMachineOverride;
            else
                return targetMachine;
        }


        /// <summary>
        /// Check if the current machine state represents a valid pre-condition.
        /// </summary>
        public bool CheckPreConditions()
        {
            Dictionary<MachinePart, bool> validatingParts = new();

            // check requirements
            for (int i = 0; i < preCondition.Count; i++)
            {
                var precondition = preCondition[i];
                var activeMachine = GetMachineOfCondition(precondition);

                var partToCheck = precondition.part;
                var currentStateOfPart = activeMachine.PartStates[partToCheck];

                if (!validatingParts.ContainsKey(partToCheck))
                    validatingParts.Add(partToCheck, currentStateOfPart == precondition.state);

                // if the precondition defines a part multiple times,
                // either state is valid. So we only need to update if the check is not already valid
                if (validatingParts[partToCheck] == false)
                    validatingParts[partToCheck] = (currentStateOfPart == precondition.state);
            }

            return !validatingParts.ContainsValue(false);
        }

        /// <summary>
        /// Check if all parts of a given interaction are free to move.
        /// </summary>
        public bool CheckAllFreeToMove()
        {
            foreach (var partCondition in postCondition)
            {
                var activeMachine = GetMachineOfCondition(partCondition);

                if (!activeMachine.CheckIsFreeToMove(partCondition.part))
                {
                    return false;
                }
            }
            return true;
        }


#if UNITY_EDITOR // Editor exclusive validation methods
        private void OnValidate()
        {
            DetectDuplicateParts();
        }

        private void DetectDuplicateParts()
        {
            var partsUsed = new HashSet<MachinePart>();
            var duplicateConditions = new HashSet<PartCondition>();

            if (postCondition == null)
                postCondition = new();

            for (int i = 0; i < postCondition.Count; i++)
            {
                if (partsUsed.Contains(postCondition[i].part))
                    duplicateConditions.Add(postCondition[i]);
                else if (postCondition[i].part != null)
                    partsUsed.Add(postCondition[i].part);
            }

            if (duplicateConditions.Count > 0)
                Debug.LogWarning("The post condition contains multiple definitions for the same part." +
                    " Only the last one will be applied", this.gameObject);
        }

#endif
    }

}