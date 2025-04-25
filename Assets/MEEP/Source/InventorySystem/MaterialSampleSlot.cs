using MEEP.InteractionSystem;
using UnityEngine;

namespace MEEP.Experiment
{
    /// <summary>
    /// Can hold a MaterialSampleInstance and pass it on to another MaterialSampleSlot
    /// </summary>
    public class MaterialSampleSlot : MonoBehaviour
    {

        [SerializeField]
        private MaterialSampleInstance content;

        public MaterialSampleInstance Content => content;

        [SerializeField]
        private bool simulateSamplePhysics = true;

        public bool SimulateSamplePhysics => simulateSamplePhysics;

        private void Start()
        {
        }


        /// <summary>
        /// Places the given MaterialSampleInstance into this slot
        /// </summary>
        public void Put(MaterialSampleInstance instance)
        {
            content = instance;
        }

        /// <summary>
        /// Passes the material sample onto the given slot
        /// </summary>
        public void Give(MaterialSampleSlot other)
        {
            if (other.IsInUse())
            {
                Debug.Log("The given slot is already in use");
                return;
            }


            if (!this.IsInUse())
            {
                Debug.Log("The slot has no sample to give");
                return;
            }

            other.content = content;
            content = null;
        }

        /// <summary>
        /// Drops the current MaterialSampleInstance
        /// </summary>
        public void Drop()
        {
            if (content == null)
                return;

            this.content = null;
        }

        /// <summary>
        /// Exchanges the sample in the slot (if there is one)
        /// with the sample in other's slot (if there is one)
        /// </summary>
        public void Exchange(MaterialSampleSlot other)
        {
            var sampleSelf = this.content;
            var sampleOther = other.content;

            this.content = sampleOther;
            other.content = sampleSelf;
        }


        /// <summary>
        /// Exchanges the sample in the slot (if there is one)
        /// with the sample in other's slot (if there is one)
        /// </summary>
        public void Exchange(PointInteractionHandle handle)
        {
            //TODO this makes the script really dependent on the transform hierarchy
            //of the player prefab, which is not not great. Find a more robust way of identifying the interaction partner
            var otherSlot = handle.Controller.transform.parent.GetComponentInChildren<MaterialSampleSlot>();

            if (otherSlot == this)
                Debug.LogWarningFormat("Echange should not be called from the player object");

            this.Exchange(otherSlot);
        }



        public bool IsInUse()
        {
            return content != null;
        }

    }

}