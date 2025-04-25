using UnityEngine;

namespace MEEP
{
    /// <summary>
    /// This little script acts as a copy position + rotation constraint.
    /// Unlike Unity's regular constraints however, this one is supposed 
    /// to be evaluated after later update, thereby improving stability with
    /// objects that are updated on LateUpdate
    /// </summary>
    [DefaultExecutionOrder(1000)]
    public class DelayedConstraint : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        private void LateUpdate()
        {
            if (target == null)
                return;

            this.transform.position = target.transform.position;
            this.transform.rotation = target.transform.rotation;
        }

        public void SetTarget(Transform transform)
        {
            this.target = transform;
        }

    }
}
