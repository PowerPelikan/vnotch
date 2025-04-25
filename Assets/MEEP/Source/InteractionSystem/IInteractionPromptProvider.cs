using UnityEngine.Localization;

namespace MEEP.InteractionSystem
{

    /// <summary>
    /// Defines a method to retrieve a prompt which the interaction point will display
    /// </summary>
    public interface IInteractionPromptProvider
    {
        public LocalizedString GetInteractionPromptText();
    }

}
