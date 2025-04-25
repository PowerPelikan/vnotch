using System;
using UnityEngine;
using UnityEngine.Localization;

namespace MEEP.OnScreenMessages
{
    [Serializable]
    public class OnScreenMessage : ICloneable
    {

        public enum MessageMode { Timed, Manual };

        public LocalizedString msgText;

        /// <summary>
        /// The amount of time that this message can survive in the queue
        /// (before it is considered irrelevant)
        /// </summary>
        public float queueLifetime = 2;

        [Space]

        public MessageMode messageMode;

        /// <summary>
        /// The amount of time that this message should be displayed
        /// </summary>
        public float displayLifetime = 2;

        public OnScreenMessage(LocalizedString msgText, float displayLifetime, float queueLifetime)
        {
            this.msgText = msgText;
            this.messageMode = MessageMode.Timed;
            this.displayLifetime = displayLifetime;
            this.queueLifetime = queueLifetime;
        }

        public OnScreenMessage(LocalizedString msgText, MessageMode messageMode, float displayLifetime, float queueLifetime)
        {
            this.msgText = msgText;
            this.messageMode = messageMode;
            this.displayLifetime = displayLifetime;
            this.queueLifetime = queueLifetime;
        }

        public object Clone()
        {
            return new OnScreenMessage(this.msgText, this.messageMode, this.queueLifetime, this.displayLifetime);
        }
    }
}
