using MEEP.InputPipelines;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MEEP.PlayerController
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput input;

        public List<PipedInput> pipedInputs = new List<PipedInput>();

        private Dictionary<InputAction, PipedInput> inputPipes;

        private void Awake()
        {
            BuildPipeDictionary();
            input.onActionTriggered += PassControllerInput;
        }

        private void BuildPipeDictionary()
        {
            inputPipes = new();

            // build dictionary
            for (int i = 0; i < pipedInputs.Count; i++)
            {
                inputPipes.Add(pipedInputs[i].associatedAction, pipedInputs[i]);
            }
        }

        private void PassControllerInput(InputAction.CallbackContext obj)
        {
            var action = obj.action;

            if (!inputPipes.ContainsKey(action))
                return;

            //Debug.LogFormat("Passing action {0} with Value {1}", action.name, action.ReadValueAsObject());

            // for events, we are only interested when they are performed
            if (inputPipes[action] is PipedEventInput && action.phase == InputActionPhase.Performed)
            {
                inputPipes[action].UpdateInput(action.ReadValueAsObject());
            }
            else if (inputPipes[action] is not PipedEventInput)
            {
                inputPipes[action].UpdateInput(action.ReadValueAsObject());
            }
        }

    }

}