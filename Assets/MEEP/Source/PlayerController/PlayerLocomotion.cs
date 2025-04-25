using MEEP.InputPipelines;
using UnityEngine;

// Adapted from the StarterAssetsInput class of the StarterAsset package under the UnityCompanionLicence

namespace MEEP.PlayerController
{
    /// <summary>
    /// Allows the player transform to move around based on inputs given.
    /// </summary>
    [AddComponentMenu("VNotch/Player/LocomotionController")]
    public class PlayerLocomotion : MonoBehaviour
    {

        [SerializeReference]
        private LocomotionConfiguration configuration;

        [Space]

        [Header("Input Processing")]

        [SerializeField]
        private PipedVector2Input moveInputPipe;

        private CharacterController controller;


        private const float _terminalFallVelocity = 53f;


        // runtime state
        private Vector2 moveInput;

        private bool _grounded;
        private float _speed;

        private float _verticalVelocity;
        private float _fallTimeoutDelta;


        private void Start()
        {
            //find and cache components
            controller = GetComponent<CharacterController>();

            // reset the timeouts on start
            _fallTimeoutDelta = configuration.fallTimeout;
        }

        private void OnEnable()
        {
            moveInputPipe.RegisterProcessor(ProcessMoveInput, 100);
        }

        private void OnDisable()
        {
            moveInputPipe.UnregisterProcessor(ProcessMoveInput);
        }

        private void Update()
        {
            ApplyGravity();
            DoGroundCheck();
            Move();
        }



        public void ProcessMoveInput(ref ProcessedInputAction<Vector2> context)
        {
            moveInput = context.ProcessedValue;
        }



        #region locomotion logic

        /// <summary>
        /// Check if the player is touching solid ground
        /// </summary>
        private void DoGroundCheck()
        {
            // define a sphere at the "feet" of the player
            Vector3 groundSpherePosition = transform.position - new Vector3(0, configuration.groundedOffset, 0);

            _grounded = Physics.CheckSphere(
                groundSpherePosition,
                controller.radius,
                configuration.groundLayers,
                QueryTriggerInteraction.Ignore);
        }

        /// <summary>
        /// Apply horizontal movement
        /// </summary>
        private void Move()
        {
            float targetSpeed = DetermineTargetSpeed();

            float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0f, controller.velocity.z).magnitude;

            float speedMargin = 0.1f;

            // accelerate or decelerate to target speed
            if (Mathf.Abs(currentHorizontalSpeed - targetSpeed) < speedMargin)
            {
                // if the current speed is close enough, just set it to the exact target speed
                _speed = targetSpeed;
            }
            else
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Mathf.Clamp01(Time.deltaTime * configuration.speedChangeRate));
            }

            Vector3 inputDirection = new Vector3(moveInput.x, 0.0f, moveInput.y).normalized;

            // if there is a move input rotate player when the player is moving
            if (moveInput.sqrMagnitude > 0.01f)
            {
                inputDirection = (transform.right * moveInput.x) + (transform.forward * moveInput.y);
            }

            // move the player
            controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0f, _verticalVelocity, 0f) * Time.deltaTime);
        }

        /// <summary>
        /// Accellerate the player downwards unless they're touching the ground
        /// </summary>
        private void ApplyGravity()
        {
            if (_grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = configuration.fallTimeout;

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0f)
                    _verticalVelocity = -2f;
            }
            else
            {
                // tick down fall time out (but clamp to be at least 0)
                _fallTimeoutDelta = Mathf.Max(_fallTimeoutDelta - Time.deltaTime, 0);
                AccellerateDownwards();
            }
        }

        /// <summary>
        /// Calculate the horizontal target speed based on the player input
        /// </summary>
        private float DetermineTargetSpeed()
        {
            float inputMagnitude = Mathf.Clamp01(moveInput.magnitude);
            return configuration.moveSpeed * inputMagnitude;
        }

        private void AccellerateDownwards()
        {
            // accellerate downwards over time until terminal velocity is reached
            if (_verticalVelocity < _terminalFallVelocity)
                _verticalVelocity += configuration.gravity * Time.deltaTime;
            else
                _verticalVelocity = _terminalFallVelocity;
        }

        #endregion locomotion logic

    }
}