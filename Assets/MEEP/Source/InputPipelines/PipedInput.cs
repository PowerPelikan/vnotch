using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MEEP.InputPipelines
{
    public abstract class PipedInput : ScriptableObject
    {

        [SerializeField]
        protected InputActionReference actionReference;

        public InputAction associatedAction => actionReference.action;

        public abstract void UpdateInput(object newValue);
    }

    public abstract class PipedValueInput : PipedInput
    {
        [NonSerialized]
        protected List<MonoBehaviour> inputUsers = new();

        public IReadOnlyList<MonoBehaviour> GetUsers()
        {
            return inputUsers.AsReadOnly();
        }
    }

    public abstract class PipedValueInput<InputValueType> : PipedValueInput where InputValueType : notnull
    {

        protected Dictionary<IInputProcessor<InputValueType>, int> processorChain = new();

        [NonSerialized]
        protected List<IInputProcessor<InputValueType>> sortedProcessorList = new();


        private void Awake()
        {
            // init
            processorChain = new();
            sortedProcessorList = new();
            inputUsers = new();
        }


        public void RegisterProcessor(IInputProcessor<InputValueType> processor, int priority)
        {
            processorChain.Add(processor, priority);
            SortUsers();
        }

        public void UnregisterProcessor(IInputProcessor<InputValueType> processor)
        {
            processorChain.Remove(processor);
            SortUsers();
        }

        public void UpdatePriority(IInputProcessor<InputValueType> processor, int newPriority)
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

            inputUsers.Clear();

            for (int i = 0; i < sortedProcessorList.Count; i++)
            {
                inputUsers.Add((sortedProcessorList[i].Target as MonoBehaviour));
            }
        }

        protected void UpdateInput(InputValueType newValue)
        {
            var processedInput = new ProcessedInputAction<InputValueType>(newValue);

            for (int i = 0; i < sortedProcessorList.Count; i++)
            {
                sortedProcessorList[i].Invoke(ref processedInput);
            }
        }

        public override void UpdateInput(object newValue)
        {
            if (newValue != null)
            {
                try
                {
                    UpdateInput((InputValueType)newValue);
                }
                catch
                {
                    Debug.LogFormat("Could not cast {0} to {1}", newValue.GetType(), typeof(InputValueType));
                }
            }
            else
                UpdateInput(default(InputValueType));
        }
    }

}
