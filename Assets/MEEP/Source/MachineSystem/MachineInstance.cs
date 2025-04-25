using MEEP.InteractionSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MEEP.MachineSystem
{
    /// <summary>
    /// Defines an abstract machine, with parts
    /// </summary>
    public class MachineInstance : MonoBehaviour
    {

        [System.Serializable]
        public struct MachinePartStateOverride
        {
            public MachinePart part;
            public MachinePartState state;
        }

        #region Fields

        [SerializeField]
        private Machine machineDefinition;

        /// <summary>
        /// The animator visualizing this machine
        /// </summary>
        [SerializeReference]
        private Animator animator;

        [Space]

        [Tooltip("Allows initialization of parts with other states")]
        [SerializeField]
        private List<MachinePartStateOverride> initOverrides = new();

        private Dictionary<MachinePart, MachinePartState> partStates;

        private Dictionary<MachinePart, MachinePartTransition> partTransitions;

        #endregion Fields

        public IReadOnlyList<MachinePart> Parts
            => machineDefinition.Parts;

        public IReadOnlyDictionary<MachinePart, MachinePartState> PartStates
            => partStates;

        public IReadOnlyDictionary<MachinePart, MachinePartTransition> PartTransitions
             => partTransitions;


        private void Awake()
        {
            InitializeStateMachine();
        }


        #region Interaction Flow

        /// <summary>
        /// Performs an interaction with this machine if conditions are met without an preexisting handle.
        /// This is useful for interactions that happen automatically, such as a simulation ending.
        /// </summary>
        /// <returns>true if the interaction was started</returns>
        public void TryPerformMachineInteraction(MachineInteraction interaction)
        {
            var virtualHandle = new InteractionHandle();

            //hook in events
            virtualHandle.Events.OnStarted.AddListener(interaction.OnPerformed.Invoke);
            virtualHandle.Events.OnCompleted.AddListener(interaction.OnFinished.Invoke);

            var started = TryPerformMachineInteraction(interaction, virtualHandle);

            if (started)
                virtualHandle.Events.InvokeStarted(virtualHandle);
        }

        /// <summary>
        /// Performs an interaction with this machine if conditions are met
        /// </summary>
        /// <returns>true if the interaction was started</returns>
        public bool TryPerformMachineInteraction(MachineInteraction interaction, InteractionHandle handle)
        {
            if (!interaction.CheckPreConditions())
            {
                Debug.LogWarningFormat(this.gameObject, "Attempted interaction's precondition not met");
                return false;
            }

            if (!interaction.CheckAllFreeToMove())
            {
                Debug.LogWarningFormat(this.gameObject, "One of the required parts is blocked by another part");
                return false;
            }

            // preconditions are met, we may perform the operation
            StartCoroutine(StartMachineInteraction(interaction, handle));
            return true;
        }

        private IEnumerator StartMachineInteraction(MachineInteraction interaction, InteractionHandle handle)
        {
            var coroutineHandles = GenerateTransitionCoroutines(ref interaction.postCondition);

            // wait for all coroutines to finish
            for (int i = 0; i < coroutineHandles.Count; i++)
            {
                yield return coroutineHandles[i];
            }

            // once all transitions have finished, notify that this interaction is finished
            ApplyPostCondition(interaction);
            handle.Complete();
        }

        #endregion Interaction Flow



        #region Coroutine Generator

        /// <summary>
        /// Generates a coroutine for every part that needs to move 
        /// from its current state to the post condition state.
        /// </summary>
        private List<Coroutine> GenerateTransitionCoroutines(ref List<MachineInteraction.PartCondition> desiredPostConditions)
        {
            List<Coroutine> transitionRoutines = new();

            foreach (var desiredPostCondition in desiredPostConditions)
            {
                var transition = GenerateTransitionCoroutine(desiredPostCondition);
                if (transition != null)
                    transitionRoutines.Add(transition);
            }

            return transitionRoutines;
        }

        /// <summary>
        /// Attempt to generate a transition coroutine between a part's current state
        /// and the desired post condition.
        /// </summary>
        private Coroutine GenerateTransitionCoroutine(MachineInteraction.PartCondition desiredPostCondition)
        {
            var currentState = partStates[desiredPostCondition.part];

            if (currentState == desiredPostCondition.state)
                return null; // no transition necessary

            // look for a transition
            var transitionInfo = currentState.FindTransitionToState(desiredPostCondition.state);
            if (transitionInfo != null)
            {
                return GenerateTransitionCoroutine(desiredPostCondition.part, (MachinePartTransition)transitionInfo);
            }
            else
            {
                Debug.LogErrorFormat("No valid transition found between states {0} and {1}"
                    , currentState.stateName, desiredPostCondition.state.stateName);
                return null;
            }
        }

        /// <summary>
        /// Attempt to generate a transition coroutine between a part's current state
        /// and the desired post condition.
        /// </summary>
        private Coroutine GenerateTransitionCoroutine(MachinePart part, MachinePartTransition transition)
        {
            switch (transition.finishMethod)
            {
                case MachinePartTransition.FinishMethod.Animator:
                    return StartCoroutine(WaitForAnimationClip(part, transition));
                case MachinePartTransition.FinishMethod.Callback:
                    return StartCoroutine(WaitForFinishNotification(part, transition));
                case MachinePartTransition.FinishMethod.Immediate:
                    return null; // no transition necessary
                default:
                    throw new System.NotImplementedException("no behaviour defined");
            }
        }

        #endregion Coroutine Generator



        #region WaitRoutines

        private IEnumerator WaitForAnimationClip(MachinePart part, MachinePartTransition transition)
        {
            var layer = animator.GetLayerIndex(part.animatorLayerName);

            // check if the requested state exists
            if (!animator.HasState(layer, Animator.StringToHash(transition.animationClipName)))
                throw new System.ArgumentException(
                    string.Format("The requested animation clip ({0}) does not exist.", transition.animationClipName));

            // start playback
            animator.Play(transition.animationClipName, layer);
            partTransitions.Add(part, transition);

            // capture current state
            var state = animator.GetCurrentAnimatorStateInfo(layer);

            // we have entered our target state. wait for the animation to finish
            // (NOTE: we need to wait at least 1 frame, to give the animator a chance to update the time tracker)
            do
            {
                yield return new WaitForEndOfFrame();
                state = animator.GetCurrentAnimatorStateInfo(layer);
                //Debug.LogFormat("Waiting for animation... {0}", state.normalizedTime);
            } while (state.normalizedTime < 0.999F);

            partTransitions.Remove(part);
        }

        private IEnumerator WaitForFinishNotification(MachinePart part, MachinePartTransition transition)
        {
            partTransitions.Add(part, transition);

            while (partTransitions.ContainsKey(part))
                yield return new WaitForEndOfFrame();
        }

        /// <summary>
        /// Notify the machine that the transition running on a given part has finished.
        /// </summary>
        public void NotifyTransitionFinished(MachinePart part)
        {
            if (partTransitions.ContainsKey(part))
                partTransitions.Remove(part);
        }

        #endregion



        #region Interaction Checks

        /// <summary>
        /// Check if a given part is currently blocked by the machine's state
        /// or transitions.
        /// </summary>
        public bool CheckIsFreeToMove(MachinePart part)
        {
            // a running transition on a part always blocks that part
            if (partTransitions.ContainsKey(part))
                return false;

            // check if the part is blocked by other parts' transitions
            foreach (var (key, value) in partTransitions)
            {
                if (value.partsBlockedByThis.Contains(part))
                    return false;
            }

            // check if the part is blocked by other parts' states
            foreach (var (key, value) in partStates)
            {
                if (value.partsBlockedByThis.Contains(part))
                    return false;
            }

            return true;
        }

        #endregion



        #region Set Up

        private void InitializeStateMachine()
        {
            partStates = new();
            partTransitions = new();

            foreach (var part in machineDefinition.Parts)
            {
                var initialState = part.states[0];
                // apply override
                var stateOverrideIdx = initOverrides.FindIndex(x => x.part.Equals(part));
                if (stateOverrideIdx != -1)
                    initialState = initOverrides[stateOverrideIdx].state;

                partStates.Add(part, initialState);
            }
        }

        #endregion Set Up



        #region Apply Conditions

        private void ApplyPostCondition(MachineInteraction interaction)
        {
            foreach (var postCondition in interaction.postCondition)
            {
                partStates[postCondition.part] = postCondition.state;
            }
        }

        #endregion Apply Conditions


    }

}