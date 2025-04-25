using MEEP.InputPipelines;
using UnityEngine;
using UnityEngine.Events;

namespace MEEP.EquipmentSystem
{
    /// <summary>
    /// Defines an equipment layer, an instance of a type of equipment in the players hand.
    /// Note that this instance doesn't need to match its pickup form.
    /// </summary>
    public class EquipmentLayer : MonoBehaviour
    {

        [SerializeField]
        private PipedEventInput pipe_focusModeToggle;

        [SerializeField]
        private Animator animator;

        [Space]

        /// <summary>
        /// The associated equipment definition
        /// </summary>
        [SerializeField]
        private EquipmentDefinition equipmentDef;

        [Space]

        [SerializeField]
        private EquipmentMode currentMode;

        [Space]

        public UnityEvent<EquipmentLayer> OnEnableLayer;

        public UnityEvent<EquipmentLayer> OnDisableLayer;

        public UnityEvent<EquipmentLayer> OnEnterFocusMode;

        public UnityEvent<EquipmentLayer> OnExitFocusMode;


        public EquipmentDefinition EquipmentDef => equipmentDef;


        /// <summary>
        /// Called when the layer has become active
        /// </summary>
        public void OnEnable()
        {
            pipe_focusModeToggle.RegisterProcessor(ToggleFocusMode, 100);

            // init in basic mode
            currentMode = EquipmentMode.Basic;

            OnEnableLayer?.Invoke(this);
        }

        /// <summary>
        /// Called when the layer becomes inactive
        /// </summary>
        public void OnDisable()
        {
            pipe_focusModeToggle.UnregisterProcessor(ToggleFocusMode);
            OnDisableLayer?.Invoke(this);
        }

        public void ToggleFocusMode(ref bool eventConsumedFlag)
        {
            ToggleFocusMode();
            eventConsumedFlag = true;
        }


        public void ToggleFocusMode()
        {
            if (currentMode == EquipmentMode.Basic)
                EnterFocusMode();
            else
                ExitFocusMode();
        }

        /// <summary>
        /// Called once Focus Mode is activated
        /// </summary>
        public void EnterFocusMode()
        {
            currentMode = EquipmentMode.Focussed;
            animator.SetBool("IsFocussed", true);
            OnEnterFocusMode?.Invoke(this);
        }

        /// <summary>
        /// Called when Focus Mode is deactivated
        /// </summary>
        public void ExitFocusMode()
        {
            currentMode = EquipmentMode.Basic;
            animator.SetBool("IsFocussed", false);
            OnExitFocusMode?.Invoke(this);
        }

    }
}