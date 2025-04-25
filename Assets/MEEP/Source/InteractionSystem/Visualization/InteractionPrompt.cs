using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace MEEP.InteractionSystem.Visualization
{

    /// <summary>
    /// Updates text for interaction prompts
    /// </summary>
    [RequireComponent(typeof(LocalizeStringEvent))]
    public class InteractionPrompt : MonoBehaviour
    {

        [SerializeField]
        private LocalizedString fallback;

        private LocalizeStringEvent localizeEvent;

        [SerializeField]
        [TypeRestriction(typeof(IInteractionPromptProvider))]
        private MonoBehaviour promptProvider;

        private void Start()
        {
            localizeEvent = GetComponent<LocalizeStringEvent>();
        }

        private void Update()
        {
            var provider = promptProvider as IInteractionPromptProvider;
            var currentSelection = provider.GetInteractionPromptText();

            localizeEvent.StringReference = currentSelection ?? fallback;
        }
    }

}