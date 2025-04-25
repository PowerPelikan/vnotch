using UnityEngine;
using UnityEngine.Events;

namespace MEEP.OnScreenMessages
{
    public class MessagePusher : MonoBehaviour
    {

        [SerializeField]
        private OnScreenMessageConfig defaultMessage;

        [SerializeField]
        private GameplayGlobal messageSystemObj;

        private OnScreenMessageReceiver messageReceiver;

        private UnityAction FindReceiver =>
            () => { messageReceiver = messageSystemObj.GetComponent<OnScreenMessageReceiver>(); };

        private void Start()
        {
            if (messageSystemObj.IsReady)
            {
                FindReceiver();
            }
            else
            {
                messageSystemObj.OnGlobalRegistered.AddListener(FindReceiver);
            }
        }

        [ContextMenu("PushMessage")]
        public void PushMessage()
        {
            PushMessage(defaultMessage);
        }

        public void PushMessage(OnScreenMessageConfig messageConfig)
        {
            if (messageReceiver == null)
                return;

            messageReceiver.ReceiveMessage(messageConfig.FromConfig());
        }

        public void PushMessage(OnScreenMessage messageInstance)
        {
            if (messageReceiver == null)
                return;

            messageReceiver.ReceiveMessage(messageInstance);
        }

        public void ClearMessageDisplay()
        {
            if (messageReceiver == null)
                return;

            messageReceiver.ClearDisplay();
        }

    }
}
