using UnityEngine;

namespace MEEP.OnScreenMessages
{
    /// <summary>
    /// A scriptable factory to instantiate OnScreenMessage objects
    /// </summary>
    [CreateAssetMenu(menuName = "MEEP/OnScreenMessages/MessageConfig", fileName = "myMessageConfig")]
    public class OnScreenMessageConfig : ScriptableObject
    {

        [SerializeField]
        private OnScreenMessage serializedMessage;

        /// <summary>
        /// Creates a clone of the serializedMessage
        /// </summary>
        /// <returns></returns>
        public OnScreenMessage FromConfig()
        {
            return serializedMessage.Clone() as OnScreenMessage;
        }
    }
}
