using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MEEP.MachineSystem
{
    [Serializable]
    public struct MachinePartTransition
    {

        public enum FinishMethod
        {
            Animator = 0,
            Callback = 1,
            Immediate = 2,
        }

        public FinishMethod finishMethod;

        public string animationClipName;

        public MachinePartState next;

        [Space]

        [Tooltip("MachineParts that are blocked from change by this transition")]
        public List<MachinePart> partsBlockedByThis;

        [Space]

        public UnityEvent OnEnter;

        public UnityEvent OnExit;



        public void EnterTransition() => OnEnter?.Invoke();

        public void ExitTransition() => OnExit?.Invoke();

    }
}


