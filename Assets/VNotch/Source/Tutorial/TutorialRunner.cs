using MEEP;
using MEEP.Objectives;
using MEEP.OnScreenMessages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(MessagePusher))]
[DefaultExecutionOrder(10000)]
public class TutorialRunner : MonoBehaviour
{

    [Serializable]
    public struct TutorialStep
    {
        [Tooltip("The label of the current step. For debugging")]
        public string name;

        [Min(0)]
        public float delayBetweenMessages;

        [Tooltip("One or more messages to display once this step is reached")]
        public List<OnScreenMessageConfig> messages;

        [Space]

        [Tooltip("The objectives that need to be completed for the next step to be reached")]
        public List<ObjectiveDefinition> objectivesToNext;

        [Space]

        [Tooltip("One or more messages to display once this step is completed (and before the next starts)")]
        public List<OnScreenMessageConfig> completionMessages;

        [Space]

        public UnityEvent onStepReached;

        public UnityEvent OnStepCompleted;
    }


    private MessagePusher messagePusher;

    private Coroutine nextMessageCoroutine;

    private int currentMessageIndex;

    private int currentStepIndex;

    [SerializeField]
    private StartupBehaviour startupBehaviour = StartupBehaviour.OnStart;

    [Space]

    [SerializeField]
    private UnityEvent OnTutorialStart;

    [SerializeField]
    private List<TutorialStep> steps = new List<TutorialStep>();

    [SerializeField]
    private UnityEvent OnTutorialCompleted;


    private void Awake()
    {
        messagePusher = GetComponent<MessagePusher>();
    }

    private void Start()
    {
        if(startupBehaviour == StartupBehaviour.OnStart)
            StartTutorial();
    }

    public void StartTutorial()
    {
        ResetAllObjectives();
        currentStepIndex = 0;

        OnTutorialStart?.Invoke();
        StartStep();
    }

    /// <summary>
    /// Completes the current tutorial step,
    /// starting the next one.
    /// </summary>
    private void CompleteStep()
    {
        steps[currentStepIndex].OnStepCompleted?.Invoke();

        currentStepIndex++;

        if (currentStepIndex >= steps.Count)
        {
            Debug.Log("Tutorial finished!");
            messagePusher.ClearMessageDisplay();
            OnTutorialCompleted?.Invoke();
        }
        else
        {
            StartStep();
        }
    }

    private void StartStep()
    {
        // stop previous step's messages from displaying
        if (nextMessageCoroutine != null)
            StopCoroutine(nextMessageCoroutine);

        messagePusher.ClearMessageDisplay();

        // display message here
        currentMessageIndex = 0;
        nextMessageCoroutine = StartCoroutine(RunTutorialStep());

        // notify
        steps[currentStepIndex].onStepReached?.Invoke();
    }

    private IEnumerator RunTutorialStep()
    {

        //display messages
        while (currentMessageIndex < steps[currentStepIndex].messages.Count)
        {
            messagePusher.PushMessage(steps[currentStepIndex].messages[currentMessageIndex]);
            currentMessageIndex++;

            yield return new WaitForSeconds(steps[currentStepIndex].delayBetweenMessages);
        }

        // check completion
        ActivateCurrentObjectives();

        while (!CheckCurrentObjectivesComplete())
        {
            yield return new WaitForEndOfFrame();
        }

        // reset message index for completion messages
        currentMessageIndex = 0;

        //display messages
        while (currentMessageIndex < steps[currentStepIndex].completionMessages.Count)
        {
            messagePusher.PushMessage(steps[currentStepIndex].completionMessages[currentMessageIndex]);
            currentMessageIndex++;

            yield return new WaitForSeconds(steps[currentStepIndex].delayBetweenMessages);
        }

        // complete step
        CompleteStep();
    }

    private void ActivateCurrentObjectives()
    {
        // TODO work on resetting the objectives properly
        //for (int i = 0; i < steps[currentStepIndex].objectivesToNext.Count; i++)
        //{
        //    steps[currentStepIndex].objectivesToNext[i].StartObjective();
        //}
    }

    /// <summary>
    /// TODO FIXME This is a dirty hack to get around scriptable objects keeping changes between play sessions
    /// </summary>
    private void ResetAllObjectives()
    {
        for (int step = 0; step < steps.Count; step++)
        {
            for (int objective = 0; step < steps[step].objectivesToNext.Count; objective++)
            {
                steps[step].objectivesToNext[objective].ResetObjective();
            }
        }
    }

    private bool CheckCurrentObjectivesComplete()
    {
        for (int i = 0; i < steps[currentStepIndex].objectivesToNext.Count; i++)
        {
            Debug.LogFormat("StepCompleted: {0}", steps[currentStepIndex].objectivesToNext[i].HasBeenCompleted);

            if (!steps[currentStepIndex].objectivesToNext[i].HasBeenCompleted)
                return false;
        }

        return true;
    }
}
