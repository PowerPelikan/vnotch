using UnityEngine;
using UnityEngine.Events;

namespace MEEP.Inventories
{
    /// <summary>
    /// Defines an inventory for this game object.
    /// Can be used to store picked up items
    /// </summary>
    public class Inventory : MonoBehaviour
    {

        /// <summary>
        /// Defines how items stored in this inventory should be displayed
        /// </summary>
        public enum InventoryDisplay { UIOnly, AttachToPoints }

        /// <summary>
        /// Defines item slots.
        /// The size of the array defines the limit of items that can be held
        /// </summary>
        [SerializeField]
        private PickupItem[] slots;

        [Space]

        [SerializeField]
        private InventoryDisplay displayMode;

        [SerializeField]
        private Transform[] attachPoints;

        public bool HasItems
        {
            get
            {
                for (int i = 0; i < slots.Length; i++)
                {
                    if (slots[i] != null)
                        return true;
                }

                return false;
            }
        }

        public UnityEvent OnInventoryChange;

        public PickupItem GetItem(int index)
        {
            return slots[index];
        }

        /// <summary>
        /// Tries to store the given item
        /// </summary>
        public bool TryStoreItem(PickupItem item)
        {
            var nextFreeSlot = GetNextFreeSlotIndex();

            if (!item.IsAvailable || nextFreeSlot == -1)
                return false;

            Debug.LogFormat("Storing Item {0}", item.name);

            item.SetHolder(this);
            slots[nextFreeSlot] = item;
            if (displayMode == InventoryDisplay.AttachToPoints)
            {
                AttachItemToPoint(nextFreeSlot);
            }

            OnInventoryChange?.Invoke();
            return true;
        }

        /// <summary>
        /// Tries to pass the given item to another inventory
        /// </summary>
        public bool TryTransferItem(PickupItem item, Inventory other)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].Equals(item))
                {
                    return TryTransferItem(i, other);
                }
            }
            return false;
        }

        public bool TryTransferItem(int itemIndex, Inventory other)
        {
            var item = slots[itemIndex];
            item.SetHolder(null);

            var stored = other.TryStoreItem(item);

            if (stored)
            {
                // remove item from this
                slots[itemIndex] = null;
                OnInventoryChange?.Invoke();
                return true;
            }
            else
            {
                // put item back
                item.SetHolder(this);
                return false;
            }
        }

        private int GetNextFreeSlotIndex()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == null)
                    return i;
            }
            return -1;
        }

        private void AttachItemToPoint(int index)
        {
            slots[index].transform.SetParent(attachPoints[index], true);
            slots[index].transform.localPosition = Vector3.zero;
            slots[index].transform.localRotation = Quaternion.identity;

            // we need to reset the scale, otherwise the model will be stretched with repeated interaction
            slots[index].transform.localScale = Vector3.one;
        }

    }

}