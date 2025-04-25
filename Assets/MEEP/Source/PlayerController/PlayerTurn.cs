using MEEP.InputPipelines;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MEEP.PlayerController
{
    [RequireComponent(typeof(InputReader))]
    [RequireComponent(typeof(PlayerInput))]
    [AddComponentMenu("VNotch/Player/TurnController")]
    public class PlayerTurn : MonoBehaviour
    {

        [SerializeField]
        private LocomotionConfiguration configuration;

        /// <summary>
        /// The camera to use for looking around.
        /// </summary>
        [SerializeField]
        private Transform lookTarget;

        [Space]

        [Header("Input Processing")]

        [SerializeField]
        private PipedVector2Input lookInputPipe;


        // runtime state
        private Vector2 lookInput;

        private float _cinemachineTargetPitch;
        private float _rotationVelocity;

        private void OnEnable()
        {
            lookInputPipe.RegisterProcessor(ProcessLookInput, 100);
        }

        private void OnDisable()
        {
            lookInputPipe.UnregisterProcessor(ProcessLookInput);
        }

        private void LateUpdate()
        {
            Turn();
        }


        public void ProcessLookInput(ref ProcessedInputAction<Vector2> context)
        {
            lookInput = context.ProcessedValue;
        }

        private void Turn()
        {
            // don't turn unless there is input
            if (lookInput.sqrMagnitude <= 0.002f)
                return;

            var deltaTimeFactor = Time.deltaTime / Time.fixedDeltaTime;

            _cinemachineTargetPitch += lookInput.y * configuration.rotationSpeed * deltaTimeFactor;
            _rotationVelocity = lookInput.x * configuration.rotationSpeed * deltaTimeFactor;

            // clamp our pitch rotation
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, configuration.bottomClamp, configuration.topClamp);

            // Update Cinemachine camera target pitch
            lookTarget.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0f, 0f);

            // rotate the player left and right
            transform.Rotate(Vector3.up * _rotationVelocity);
        }

        private static float ClampAngle(float angle, float angleMin, float angleMax)
        {
            // map angle a 360 degree range while maintaining the sign/direction
            angle = (Mathf.Abs(angle) % 360) * Mathf.Sign(angle);
            return Mathf.Clamp(angle, angleMin, angleMax);
        }
    }
}
