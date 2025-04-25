using MEEP.InputPipelines;
using UnityEngine;

namespace MEEP.InteractionSystem
{

    /// <summary>
    /// Class that requests interactions with Interaction Points.
    /// 
    /// While the mouse is held, fill up the interaction meter of a selected interaction point 
    /// ( identified through raycasting ). Once the interaction point is filled, this Behaviour 
    /// receives a callback that the interaction has been successful.
    /// 
    /// </summary>
    [AddComponentMenu("VNotch/Core/InteractionRaycaster")]
    public class InteractionRaycaster : MonoBehaviour
    {

        /// <summary>
        /// Defines the behaviour of the ray on mouse movement. 
        /// </summary>
        public enum RaycastMode { FirstPerson, Locked }


        [Header("Inputs")]

        [SerializeField]
        private PipedVector2Input pipe_cursorPosition;

        [SerializeField]
        private PipedBoolInput pipe_interact;

        [Space]

        [SerializeField]
        private GameplayGlobal global_camera;

        [Space]

        /// <summary>
        /// The max distance the ray can travel
        /// </summary>
        [SerializeField]
        private float maxRayDistance = 100;

        /// <summary>
        /// The layer(s) the raycast "can see"
        /// </summary>
        [SerializeField]
        private LayerMask targetLayer;


        public RaycastMode controlMode;


        #region runtime variables

        /// <summary>
        /// is the interact button pressed
        /// </summary>
        private bool _isInteracting = false;

        /// <summary>
        /// the current internal cursor position in screen coords
        /// </summary>
        private Vector2 _cursorPosition;

        /// <summary>
        /// The camera rendering
        /// </summary>
        private Camera _camera;

        /// <summary>
        /// The ray shot to identify interaction points.
        /// Cached to save on memory.
        /// </summary>
        private Ray _cameraRay;

        /// <summary>
        /// Caches Raycast hits.
        /// </summary>
        public RaycastHit[] _hits = new RaycastHit[5];


        private PointInteractionHandle currentInteraction;

        #endregion cached variables

        private void Start()
        {
            _camera = global_camera.GetComponent<Camera>();

            if (_camera == null)
                global_camera.OnGlobalRegistered.AddListener(RegisterCamera);
        }


        private void OnEnable()
        {
            pipe_interact.RegisterProcessor(UpdateInteract, 100);
            pipe_cursorPosition.RegisterProcessor(UpdateCursorPosition, 100);
        }

        private void OnDisable()
        {
            pipe_interact.UnregisterProcessor(UpdateInteract);
            pipe_cursorPosition.UnregisterProcessor(UpdateCursorPosition);
        }



        private void UpdateInteract(ref ProcessedInputAction<bool> processedInput)
        {
            _isInteracting = processedInput.ProcessedValue;
            processedInput.Consume();
        }

        private void UpdateCursorPosition(ref ProcessedInputAction<Vector2> processedInput)
        {
            _cursorPosition = processedInput.ProcessedValue;
        }



        private void Update()
        {
            AccumulateInteraction();
        }

        private void RegisterCamera()
        {
            _camera = global_camera.GetComponent<Camera>();
            global_camera.OnGlobalRegistered.RemoveListener(RegisterCamera);
        }

        /// <summary>
        /// Shoot a raycast in camera direction. On hit, try filling the interaction point's
        /// interaction bar. 
        /// </summary>
        public void AccumulateInteraction()
        {
            if (_camera == null)
                return;

            // perform raycast
            UpdateRay();
            var hitcount = Physics.RaycastNonAlloc(_cameraRay, _hits, maxRayDistance, targetLayer);

            // find InteractionPointCollider in results
            var interactionTarget = FindComponentInHits<InteractionPointCollider>(hitcount);

            if (interactionTarget != null)
                interactionTarget.Owner.HandleActivationRaycast(this, _isInteracting);
        }

        /// <summary>
        /// Updates the Raycast's direction to represent changes in camera position and direction.
        /// </summary>
        private void UpdateRay()
        {
            _cameraRay = _camera.ScreenPointToRay(_cursorPosition, Camera.MonoOrStereoscopicEye.Mono);
        }

        /// <summary>
        /// Find InteractionPoint objects in the current raycast results
        /// </summary>
        private ComponentType FindComponentInHits<ComponentType>(int hitcount) where ComponentType : MonoBehaviour
        {
            ComponentType interactionTarget;

            for (int i = 0; i < hitcount; i++)
            {
                // break on empty entries (gameObjects always have a transform attached)
                if (_hits[i].transform == null)
                    continue;

                //NOTE: it is important to use the explicit collider here, as otherwise a parent object may be selected in some cases
                interactionTarget = _hits[i].collider.gameObject.GetComponent<ComponentType>();

                // TODO consider order in case of multiple hits
                if (interactionTarget != null)
                    return interactionTarget;
            }

            // TODO if there are hits that dont contain interaction points, there should probably be a warning

            return null;
        }


        public void GiveInteractionObject(PointInteractionHandle interaction)
        {
            currentInteraction = interaction;
        }

        public void OnInteractionCanceled()
        {
            currentInteraction = null;
        }

    }

}