using MEEP.InteractionSystem;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;

namespace MEEP.MachineSystem
{
    /// <summary>
    /// Selects between multiple MachineInteractions, depending on which is valid at the moment
    /// </summary>
    public class MachineInteractionSelector : MonoBehaviour, IInteractionGate, IInteractionPromptProvider
    {

        [SerializeField]
        private MachineInstance targetMachine;

        [SerializeField]
        private List<MachineInteraction> selectionPool = new List<MachineInteraction>();

        //TODO since this is required in a lot of places, it's better suited to be updated in the EarlyUpdate loop
        public MachineInteraction CurrentSelection
        {
            get
            {
                for (int i = 0; i < selectionPool.Count; i++)
                {
                    if (CheckPreconditions(selectionPool[i]))
                        return selectionPool[i];
                }
                return null;
            }
        }

        /// <summary>
        /// Checks the preconditions of the given interactions and selects
        /// a valid one.
        /// </summary>
        public void TryPerformInteraction(InteractionHandle handle)
        {
            CurrentSelection?.TryPerformInteraction(handle);
        }

        private bool CheckPreconditions(MachineInteraction interaction)
        {
            if (interaction.targetMachine != this.targetMachine)
                throw new System.ArgumentException("MachineInteraction in selection pool should operate on the same machine");

            return interaction.CheckPreConditions() && interaction.CheckAllFreeToMove();
        }


        // interface implementation


        public bool CheckInteractionRequest()
        {
            return CurrentSelection != null;
        }

        public LocalizedString GetInteractionPromptText()
        {
            return CurrentSelection?.interactionDescription;
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Application.isPlaying && this.isActiveAndEnabled)
            {
                var label = string.Format("CurrentSelection: \"{0}\"", CurrentSelection ? CurrentSelection.name : "None");
                Handles.Label(transform.position, label);
            }
        }
#endif

    }
}