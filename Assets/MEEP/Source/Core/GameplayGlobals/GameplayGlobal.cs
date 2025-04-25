using UnityEngine;
using UnityEngine.Events;

namespace MEEP
{
    /// <summary>
    /// Defines a global runtime reference. 
    /// These can be used to decouple Scripts that don't exist on the same prefab
    /// </summary>
    [CreateAssetMenu(menuName = "VNotch/Core/GameplayGlobal", fileName = "myGameplayGlobal")]
    public class GameplayGlobal : ScriptableObject
    {
        private GameObject registeredTarget;

        public bool IsReady => registeredTarget != null;

        public UnityEvent OnGlobalRegistered;

        private void Awake()
        {
            OnGlobalRegistered = new UnityEvent();
        }

        public void RegisterObject(GameObject obj)
        {
            if (registeredTarget != null && !registeredTarget.Equals(obj))
                throw new System.Exception(string.Format("GameplayGlobal {0} cannot be registered twice", obj.name));

            registeredTarget = obj;
            OnGlobalRegistered?.Invoke();
        }

        public GameObject GetTarget()
        {
            return registeredTarget;
        }

        public T GetComponent<T>() where T : Component
        {
            if (registeredTarget == null)
                return null;
            else
                return registeredTarget.GetComponent<T>();
        }
    }

}