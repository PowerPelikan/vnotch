using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace MEEP.InputPipelines
{
    [CreateAssetMenu(menuName = "MEEP/Input/PipedEvent", fileName = "pipedEvent")]
    public class PipedEventInput : PipedInput
    {
        protected Dictionary<IInputEventProcessor, int> processorChain = new();

        protected List<IInputEventProcessor> sortedProcessorList = new();

        public UnityEvent OnInputRaised;

        private void Awake()
        {
            // init
            processorChain = new();
            sortedProcessorList = new();
        }


        public void RegisterProcessor(IInputEventProcessor processor, int priority)
        {
            processorChain.Add(processor, priority);
            SortUsers();
        }

        public void UnregisterProcessor(IInputEventProcessor processor)
        {
            processorChain.Remove(processor);
            SortUsers();
        }

        public void UpdatePriority(IInputEventProcessor processor, int newPriority)
        {
            processorChain[processor] = newPriority;
            SortUsers();
        }


        /// <summary>
        /// Returns the pipeline users in order of their priority;
        /// </summary>
        private void SortUsers()
        {
            sortedProcessorList = processorChain.Keys.ToList();
            sortedProcessorList = sortedProcessorList.OrderByDescending(user => processorChain[user]).ToList();
        }


        protected void ProcessEvent()
        {
            bool eventConsumedFlag = true;

            for (int i = 0; i < sortedProcessorList.Count; i++)
            {
                sortedProcessorList[i].Invoke(ref eventConsumedFlag);

                //abort if input was consumed
                if (eventConsumedFlag)
                    return;
            }
        }

        public override void UpdateInput(object newValue)
        {
            ProcessEvent();
        }
    }
}
