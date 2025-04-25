using UnityEngine;
using UnityEngine.Events;

namespace MEEP.Inventories
{
    /// <summary>
    /// Defines an item that can be picked up or dropped of.
    /// </summary>
    public class PickupItem : MonoBehaviour
    {

        /// <summary>
        /// The current holder of this item.
        /// None if the item exists in the world without being held
        /// </summary>
        [SerializeField]
        private Inventory holder;
        public Inventory Holder => holder;

        public bool IsAvailable => holder == null;

        /// <summary>
        /// Defines a piece of equipment that is required to pick up
        /// this item.
        /// </summary>
        [SerializeField]
        private EquipmentSystem.EquipmentDefinition requiredEquipment;

        [Space]

        public UnityEvent<Inventory> OnPickedUp;

        public UnityEvent OnDropped;



        public void SetHolder(Inventory target)
        {
            if (target == null)
            {
                holder = null;
                OnDropped?.Invoke();
            }
            else if (holder == null)
            {
                holder = target;
                OnPickedUp?.Invoke(target);
            }
            else
                Debug.LogError("The item is already being held");
        }

    }

}