using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace MEEP.OnScreenMessages
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class OnScreenMessageDrawer : MonoBehaviour
    {

        private TextMeshProUGUI textComp;

        /// <summary>
        /// The visual element that is enabled/disabled
        /// based on this component's state.
        /// </summary>
        [SerializeField]
        private GameObject displayRoot;

        /// <summary>
        /// The message being displayed by this drawer
        /// </summary>
        private OnScreenMessage currentMessage;


        private float remainingLifetime;

        public bool canReceiveMessage;

        public UnityEvent OnMessageOver;

        private void Awake()
        {
            textComp = GetComponent<TextMeshProUGUI>();
            canReceiveMessage = true;
            displayRoot.SetActive(false);

        }

        private void Update()
        {
            if (currentMessage.messageMode == OnScreenMessage.MessageMode.Timed)
            {
                remainingLifetime -= Time.deltaTime;

                if (remainingLifetime < 0)
                {
                    ClearMessage();
                }
            }
        }

        public void SetMessage(OnScreenMessage message)
        {
            currentMessage = message;
            remainingLifetime = message.displayLifetime;

            textComp.text = message.msgText.GetLocalizedString();

            canReceiveMessage = false;
            displayRoot.SetActive(true);
        }

        public void ClearMessage()
        {
            displayRoot.SetActive(false);
            PrepareDrawerForNewMessage();
        }

        private void PrepareDrawerForNewMessage()
        {
            remainingLifetime = 0;
            currentMessage = null;
            canReceiveMessage = true;

            // move this display to the front
            transform.SetAsFirstSibling();

            OnMessageOver?.Invoke();
        }

    }
}
