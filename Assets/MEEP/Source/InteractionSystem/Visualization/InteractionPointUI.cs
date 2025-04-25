using UnityEngine;
using UnityEngine.UI;

namespace MEEP.InteractionSystem.Visualization
{
    /// <summary>
    /// Animates the visuals of interaction points.
    /// </summary>
    public class InteractionPointUI : MonoBehaviour
    {

        [SerializeReference]
        private InteractionPoint point;

        private ActivationTimer timer;

        [Space]

        [SerializeReference]
        private Animator animator;

        [SerializeReference]
        private Image progressBar;

        private void Start()
        {
            timer = point.gameObject.GetComponent<ActivationTimer>();
        }

        private void Update()
        {
            progressBar.fillAmount = timer.RelativeCompletionTime;

            //update animator states
            animator.SetBool("IsRunning", point.IsRunning);
            animator.SetBool("IsHighlighted", point.IsHighlighted);
            animator.SetBool("IsAvailable", point.IsAvailable);
        }

    }

}