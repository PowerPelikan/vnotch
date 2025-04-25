using UnityEngine;

// Adapted from the StarterAssetsInput class of the StarterAsset package under the UnityCompanionLicence

namespace MEEP.PlayerController
{
    /// <summary>
    /// Contains a configuration of the locomotion controller.
    /// </summary>
    [CreateAssetMenu(menuName = "VNotch/LocomotionConfig", fileName = "PlayerLocomotion")]
    public class LocomotionConfiguration : ScriptableObject
    {
        [Header("Locomotion")]

        [Tooltip("Move speed of the character in m/s")]
        public float moveSpeed = 4.0f;
        [Tooltip("Sprint speed of the character in m/s")]
        public float sprintSpeed = 6.0f;

        [Tooltip("Acceleration and deceleration when switching between walking and sprinting")]
        public float speedChangeRate = 10.0f;


        [Space(10)]


        [Header("Grounding")]

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float gravity = -15.0f;

        [Tooltip("What layers the character treats as ground")]
        public LayerMask groundLayers;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float fallTimeout = 0.15f;

        [Tooltip("Simulates the feet being a little lower than usual when checking ground. Useful for rough ground.")]
        public float groundedOffset = -0.14f;


        [Space(10)]


        [Header("Turning")]

        [Tooltip("Rotation speed of the character")]
        public float rotationSpeed = 1.0f;

        [Range(0, 89)]
        [Tooltip("How far in degrees can you move the camera up")]
        public float topClamp = 89.0f;

        [Range(-89, 0)]
        [Tooltip("How far in degrees can you move the camera down")]
        public float bottomClamp = -89.0f;

    }
}
