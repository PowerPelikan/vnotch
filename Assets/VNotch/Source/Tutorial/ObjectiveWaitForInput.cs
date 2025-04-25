using MEEP.Objectives;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VNotch.Tutorial
{
    public class ObjectiveWaitForInput : MonoBehaviour
    {

        [SerializeField]
        private InputActionReference requiredAction;

        [SerializeField]
        private ObjectiveDefinition linkedObjective;

        [SerializeField]
        private int performThreshold = 100;

        [SerializeField]
        private int currentPerforms = 0;

        [SerializeField]
        private bool CanCompleteMultipleTimes = false;

        [SerializeField]
        private bool wasCompleted = false;



        private void OnEnable()
        {
            requiredAction.action.performed += IncrementObjective;
            currentPerforms = 0;
        }

        private void OnDisable()
        {
            requiredAction.action.performed -= IncrementObjective;
        }

        private void IncrementObjective(InputAction.CallbackContext context)
        {
            if (!context.performed || !this.isActiveAndEnabled)
                return;

            if (wasCompleted && !CanCompleteMultipleTimes)
                return;

            currentPerforms++;

            if (currentPerforms >= performThreshold)
                wasCompleted = linkedObjective.Complete() || wasCompleted;
        }
    }
}