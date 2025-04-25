using System;
using UnityEngine;
using UnityEngine.Events;

namespace MEEP.InteractionSystem
{

    /// <summary>
    /// Variant of the interaction events class that provides addtional events
    /// pertaining to point activation
    /// </summary>
    [Serializable]
    public sealed class PointInteractionEvents : InteractionEvents
    {
        [Header("Point Events")]
        [Space]

        /// <summary>
        /// Event fired when the interactors request was denied
        /// </summary>
        [Tooltip("Event fired when the interactors request was denied")]
        [SerializeField]
        private UnityEvent OnRequestDenied;

        /// <summary>
        /// Event fired when the interactor's request was granted (tick up)
        /// </summary>
        [Tooltip("Event fired when the interactor's request was granted (tick up)")]
        [SerializeField]
        private UnityEvent OnValidRequest;

        /// <summary>
        /// Event fired when a previous activation has timed out
        /// </summary>
        [Tooltip("Event fired when a previous activation has timed out")]
        [SerializeField]
        private UnityEvent OnTimeout;

        public PointInteractionEvents() : base()
        {
            OnRequestDenied = new UnityEvent();
            OnValidRequest = new UnityEvent();
            OnTimeout = new UnityEvent();
        }

        public void InvokeRequestDenied()
        {
            OnRequestDenied?.Invoke();
        }

        public void InvokeValidRequest()
        {
            OnValidRequest?.Invoke();
        }

        public void InvokeTimeout()
        {
            OnTimeout?.Invoke();
        }

    }

}