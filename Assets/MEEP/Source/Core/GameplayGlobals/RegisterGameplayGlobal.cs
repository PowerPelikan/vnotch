using UnityEngine;

namespace MEEP
{
    /// <summary>
    /// Registers an object as a Gameplay Global
    /// </summary>
    [AddComponentMenu("MEEP/GameplayGlobal")]
    public class RegisterGameplayGlobal : MonoBehaviour
    {

        [SerializeField]
        private GameplayGlobal target;

        private void Start()
        {
            target.RegisterObject(this.gameObject);
            Debug.LogFormat("Registered global object {0}", target.name);
        }
    }
}
