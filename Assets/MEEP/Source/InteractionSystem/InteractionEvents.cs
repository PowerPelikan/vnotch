using System;
using UnityEngine;
using UnityEngine.Events;

namespace MEEP.InteractionSystem
{
    [Serializable]
    public class InteractionEvents
    {
        [Header("Handle Events")]
        [Space]

        /// <summary>
        /// Event fired when the activation bar has been filled up
        /// </summary>
        [Tooltip("Event fired when the activation bar has been filled up")]
        [SerializeField]
        public UnityEvent<InteractionHandle> OnStarted;

        /// <summary>
        /// Event fired when a started interaction has been canceled
        /// </summary>
        [SerializeField]
        public UnityEvent<InteractionHandle> OnCanceled;

        /// <summary>
        /// Event fired once the current interaction has been completed
        /// </summary>
        [SerializeField]
        public UnityEvent<InteractionHandle> OnCompleted;

        public InteractionEvents()
        {
            OnStarted = new UnityEvent<InteractionHandle>();
            OnCanceled = new UnityEvent<InteractionHandle>();
            OnCompleted = new UnityEvent<InteractionHandle>();
        }

        public void InvokeStarted(InteractionHandle handle)
        {
            //Debug.LogFormat("Started Action at {0}.", gameObject.name);
            OnStarted?.Invoke(handle);
        }

        public void InvokeCanceled(InteractionHandle handle)
        {
            //Debug.LogFormat("Canceled Action at {0}.", gameObject.name);
            OnCanceled?.Invoke(handle);
        }

        public void InvokeCompleted(InteractionHandle handle)
        {
            //Debug.LogFormat("Completed Action at {0}.", gameObject.name);
            OnCompleted?.Invoke(handle);
        }
    }

}
