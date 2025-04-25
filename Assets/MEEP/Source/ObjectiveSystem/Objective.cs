using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace MEEP.Objectives
{

    /// <summary>
    /// Defines an objective handle. Objectives are completable tasks in-game, 
    /// and offer events for when an objective has been completed
    /// </summary>
    [CreateAssetMenu(menuName = "MEEP/Objective", fileName = "myObjective")]
    public class ObjectiveDefinition : ScriptableObject
    {

        public LocalizedString objectiveText;

        [SerializeField]
        private bool hasBeenCompleted = false;

        private bool isActive = false;

        private int timesCompleted = 0;


        public bool HasBeenCompleted => hasBeenCompleted;

        public bool IsActive => isActive;

        public int TimesCompleted => timesCompleted;

        public UnityEvent OnStarted;

        public UnityEvent OnCompletedFirstTime;

        public UnityEvent<int> OnCompletedMultiple;

        public UnityEvent OnUnCompleted;


        public void ResetObjective()
        {
            isActive = true;
            hasBeenCompleted = false;
            timesCompleted = 0;

            OnStarted?.Invoke();
        }

        public void SetInactive()
        {
            isActive = true;
        }

        public bool Complete()
        {
            hasBeenCompleted = true;
            timesCompleted++;

            if (timesCompleted == 1)
                OnCompletedFirstTime?.Invoke();
            else
                OnCompletedMultiple?.Invoke(timesCompleted);

            return true;
        }


        /// <summary>
        /// Completes the objective, but serializable
        /// </summary>
        public void CompleteByInspector()
        {
            Complete();
        }

        public void UnComplete()
        {
            hasBeenCompleted = false;
            OnUnCompleted?.Invoke();
        }

    }
}
