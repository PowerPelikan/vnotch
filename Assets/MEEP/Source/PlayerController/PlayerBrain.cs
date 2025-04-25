using MEEP.InteractionSystem;
using UnityEngine;

namespace MEEP.PlayerController
{
    /// <summary>
    /// Manages the different player controller behaviours.
    /// </summary>
    [AddComponentMenu("VNotch/Player/PlayerBrain")]
    [RequireComponent(typeof(PlayerLocomotion))]
    [RequireComponent(typeof(PlayerTurn))]
    [RequireComponent(typeof(InteractionRaycaster))]
    public class PlayerBrain : MonoBehaviour
    {

        public enum PlayerState { FirstPerson, ZoomView }

        /// <summary>
        /// The current state of the player
        /// </summary>
        public PlayerState state = PlayerState.FirstPerson;

        // movement components
        private PlayerLocomotion locomotionController;
        private PlayerTurn turnController;

        private InteractionRaycaster InteractionRaycaster;



        private void Awake()
        {
            locomotionController = GetComponent<PlayerLocomotion>();
            turnController = GetComponent<PlayerTurn>();
            InteractionRaycaster = GetComponent<InteractionRaycaster>();
        }

        /// <summary>
        /// Set up the zoom view state
        /// </summary>
        public void EnterZoomView()
        {
            this.state = PlayerState.ZoomView;

            locomotionController.enabled = false;
            turnController.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void ExitZoomView()
        {
            this.state = PlayerState.FirstPerson;

            locomotionController.enabled = true;
            turnController.enabled = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }

}