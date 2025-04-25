using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MEEP
{
    /// <summary>
    /// Fires its event when an AnimationEvent is received,
    /// allowing objects other than the one containing the animator to reference it.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class SerializedAnimationEvent : MonoBehaviour
    {

        public UnityEvent OnEventFired;

        public void FireAnimationEvent()
        {
            OnEventFired?.Invoke();
        }

    }
}
