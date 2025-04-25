using UnityEngine;

namespace MEEP.InteractionSystem
{
    /// <summary>
    /// An interaction activation timer tracks "how much"
    /// the point has been activated
    /// </summary>
    [RequireComponent(typeof(InteractionPoint))]
    public class ActivationTimer : MonoBehaviour
    {

        /// <summary>
        /// The raycaster issuing the interaction
        /// </summary>
        [SerializeReference]
        private InteractionRaycaster issuer;

        /// <summary>
        /// The interaction point that has been activated
        /// </summary>
        [SerializeReference]
        [HideInInspector]
        private InteractionPoint point;

        [Space]

        [SerializeField]
        private float timeToComplete = 1F;

        [SerializeField]
        private float currentTime = 0;


        private bool wasActiveThisFrame = false;


        public InteractionRaycaster Issuer { get => issuer; set => issuer = value; }

        public float RelativeCompletionTime => currentTime / timeToComplete;



        private void OnEnable()
        {
            point = GetComponent<InteractionPoint>();
            currentTime = 0;
            wasActiveThisFrame = true;
        }

        private void Update()
        {
            if (!wasActiveThisFrame && currentTime > 0.001F)
            {
                TickDown();
                return;
            }

            wasActiveThisFrame = false;
        }

        public void AddTime(float delta)
        {
            this.enabled = true;
            wasActiveThisFrame = true;
            currentTime += delta;

            if (currentTime > timeToComplete)
                Complete();
        }

        private void TickDown()
        {
            currentTime = Mathf.Max(0, currentTime - Time.deltaTime);

            if (currentTime < 0.001F)
            {
                ResetTimer();
                GetComponent<InteractionEventSerializer>().EventsObject.InvokeTimeout();
            }
        }

        public void ResetTimer()
        {
            this.issuer = null;
            currentTime = 0;
            this.enabled = false;
        }

        private void Complete()
        {
            point.StartInteraction(issuer);
            ResetTimer();
        }

    }

}