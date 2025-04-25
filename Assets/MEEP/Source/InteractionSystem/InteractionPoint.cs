using UnityEngine;

namespace MEEP.InteractionSystem
{
    /// <summary>
    /// Defines a single interaction point.
    /// </summary>
    [AddComponentMenu("VNotch/Core/InteractionPoint")]
    //[RequireComponent(typeof(SphereCollider))]
    //[RequireComponent(typeof(InteractionEventSerializer))]
    public class InteractionPoint : MonoBehaviour
    {
        [Header("Interaction Settings")]

        // general settings

        private GameplayGlobal global_camera;

        /// <summary>
        /// The component validating this point
        /// </summary>
        [SerializeField]
        [TypeRestriction(typeof(IInteractionGate))]
        private MonoBehaviour requestValidator;

        /// <summary>
        /// The furthest distance an interactor can be away from this waypoint
        /// and still activate the interaction point
        /// </summary>
        [SerializeField]
        private float maxInteractionDistance;

        /// <summary>
        /// The sphere at which the player must point in order to "fill up" 
        /// the interaction point
        /// </summary>
        [SerializeField]
        private new Collider collider;

        private ActivationTimer activationTimer;

        private PointInteractionEvents events;

        private InteractableVisuals visuals;

        // current interaction info
        public InteractionPhase CurrentPhase => GetInteractionPhase();

        /// <summary>
        /// Stores the transform of a potential interactor
        /// </summary>
        [SerializeReference]
        private PointInteractionHandle currentInteraction;



        #region Properties

        public float InteractionRange
        {
            get
            {
                // TODO Add an Anim Curve here
                var dist = Vector3.Distance(global_camera.GetTarget().transform.position, transform.position);
                return 1 - Mathf.Clamp01(dist - maxInteractionDistance);
            }
        }

        /// <summary>
        /// Is this point ready to be interacted with
        /// </summary>
        public bool IsAvailable => CheckInteractionRequest();

        public bool IsRunning => CurrentPhase == InteractionPhase.Running;

        public bool IsHighlighted;

        #endregion Properties



        private void Start()
        {
            events = GetComponent<InteractionEventSerializer>().EventsObject;
            activationTimer = GetComponent<ActivationTimer>();
            visuals = GetComponent<InteractableVisuals>();

            currentInteraction = null;

            InteractionPointCollider.TagGameObject(collider, this);
        }

        private void Update()
        {
            IsHighlighted = false;
        }

        /// <summary>
        /// Handle an incomming interaction raycast
        /// </summary>
        public void HandleActivationRaycast(InteractionRaycaster interactor, bool isInteracting = false)
        {
            visuals.ReceiveHit();
            IsHighlighted = true;

            // stop here if only hovering
            if (!isInteracting)
                return;

            if (!CheckIfInInteractionRange(interactor.transform.position))
            {
                Debug.Log("Interaction Point out of range");
                return;
            }

            // the raycaster is unknown (check prerequisites)
            if (activationTimer.Issuer != interactor && CheckInteractionRequest())
            {
                activationTimer.enabled = true;
                activationTimer.Issuer = interactor;
                events.InvokeValidRequest();
            }
            else if (activationTimer.Issuer != interactor)
                events.InvokeRequestDenied();

            // the raycaster is known (tick up)
            if (activationTimer.Issuer = interactor)
            {
                activationTimer.AddTime(Time.deltaTime);
            }
        }


        private bool CheckIfInInteractionRange(Vector3 wsPosition)
        {
            return Vector3.Distance(wsPosition, this.transform.position) <= maxInteractionDistance;
        }

        private bool CheckInteractionRequest()
        {
            // if there is no validator, just pass
            if (requestValidator == null)
                return true;

            if (requestValidator is not IInteractionGate)
                throw new System.ArgumentException("gate type does not implement InteractionGate!");

            return ((IInteractionGate)requestValidator).CheckInteractionRequest();
        }


        /// <summary>
        /// Start an interaction with a currently registered raycaster.
        /// </summary>
        public void StartInteraction(InteractionRaycaster interactor)
        {
            activationTimer.ResetTimer();

            currentInteraction = IssueInteractionHandle(interactor);
        }

        public InteractionPhase GetInteractionPhase()
        {
            InteractionPhase currentState;

            // does the point have a running handle?
            if (currentInteraction != null)
            {
                currentState = InteractionPhase.Running;
            }
            // is the timer currently running
            else if (activationTimer.RelativeCompletionTime > 0.001F)
            {
                currentState = InteractionPhase.Activating;
            }
            // is the point available at all?
            else if (CheckInteractionRequest())
            {
                currentState = InteractionPhase.Idle;
            }
            else
            {
                currentState = InteractionPhase.Blocked;
            }

            return currentState;
        }

        public void OnInteractionCanceled()
        {
            currentInteraction = null;
        }

        public PointInteractionHandle IssueInteractionHandle(InteractionRaycaster interactor)
        {
            currentInteraction = new PointInteractionHandle(interactor, this);
            return currentInteraction;
        }

    }
}
