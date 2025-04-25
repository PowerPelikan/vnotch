using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MEEP.OnScreenMessages
{
    public class OnScreenMessageReceiver : MonoBehaviour
    {

        /// <summary>
        /// How many messages may be displayed at once
        /// </summary>
        public int MessageCapacity => messageDrawerPool.Length;


        /// <summary>
        /// UI elements able to draw messages
        /// </summary>
        private OnScreenMessageDrawer[] messageDrawerPool;

        /// <summary>
        /// Queues messages that cannot be displayed right away
        /// </summary>
        private List<OnScreenMessage> messageQueue;

        /// <summary>
        /// Keeps references to the time tracker of the messages in the queue
        /// </summary>
        private Dictionary<OnScreenMessage, Coroutine> messageQueueTracker;

        private void Awake()
        {
            messageQueue = new();
            messageQueueTracker = new();

            messageDrawerPool = GetComponentsInChildren<OnScreenMessageDrawer>(true);

            foreach (var messageDrawer in messageDrawerPool)
            {
                messageDrawer.OnMessageOver.AddListener(TryDequeueMessage);
            }

        }

        private void OnDisable()
        {
            StopAllCoroutines();
            messageQueue.Clear();
        }

        /// <summary>
        /// Force a message to appear on-screen.
        /// </summary>
        public void ForceDisplayMessage(OnScreenMessage message)
        {
            ClearDisplay();
            ReceiveMessage(message);
        }

        /// <summary>
        /// Remove all current and queued messages
        /// </summary>
        public void ClearDisplay()
        {
            StopAllCoroutines();
            messageQueue.Clear();
            messageQueueTracker.Clear();

            for (int i = 0; i < messageDrawerPool.Length; i++)
            {
                messageDrawerPool[i].ClearMessage();
            }

        }


        public void ReceiveMessage(OnScreenMessage message)
        {
            for (int i = 0; i < MessageCapacity; i++)
            {
                if (messageDrawerPool[i].canReceiveMessage)
                {
                    Debug.Log("Displaying new message");
                    messageDrawerPool[i].SetMessage(message);
                    return;
                }
            }

            Debug.Log("Enqueing message");
            EnqueueMessage(message);
        }

        /// <summary>
        /// Store a message for later, setting a timer
        /// to measure its lifetime
        /// </summary>
        private void EnqueueMessage(OnScreenMessage message)
        {
            var timeTracker = StartCoroutine(TimeMessageLifetime(message));

            messageQueue.Add(message);
            messageQueueTracker.Add(message, timeTracker);
        }

        private void TryDequeueMessage()
        {
            if (messageQueue.Count == 0)
                return;

            Debug.Log("Displaying enqued message");

            var message = messageQueue[0];

            StopCoroutine(messageQueueTracker[message]);

            messageQueue.Remove(message);
            messageQueueTracker.Remove(message);

            // Try displaying again
            ReceiveMessage(message);
        }

        private IEnumerator TimeMessageLifetime(OnScreenMessage message)
        {
            var lifetime = message.queueLifetime;

            while (lifetime > 0)
            {
                yield return new WaitForSeconds(0.2F);
                lifetime -= 0.2F;
            }

            // remove now irrelevant messages
            Debug.Log("Message died in queue");

            messageQueue.Remove(message);
            StopCoroutine(messageQueueTracker[message]);
            messageQueueTracker.Remove(message);
        }
    }
}
