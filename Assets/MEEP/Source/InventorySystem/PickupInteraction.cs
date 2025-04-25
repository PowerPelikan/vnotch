using MEEP.EquipmentSystem;
using MEEP.InteractionSystem;
using UnityEngine;
using UnityEngine.Events;

namespace MEEP.Inventories
{
    [RequireComponent(typeof(Inventory))]
    public class PickupInteraction : MonoBehaviour
    {
        private Inventory linkedInventory;

        public UnityEvent OnPickup;

        public UnityEvent OnDropOff;

        private void Start()
        {
            linkedInventory = GetComponent<Inventory>();
        }


        /// <summary>
        /// Perform a transfer between the linked inventory and the instigator's active inventory,
        /// depending on whether the linked inventory has an item to transfer.
        /// </summary>
        public void PickupOrDropoff(InteractionHandle handle)
        {
            if (linkedInventory.HasItems)
            {
                PickUp(handle);
            }
            else
            {
                Dropoff(handle);
            }
        }


        /// <summary>
        /// Places an item from the linked inventory into the instigator's active inventory.
        /// </summary>
        public void PickUp(InteractionHandle handle)
        {
            var instigator = handle.Instigator?.GetComponentInChildren<EquipmentUser>(true)?.ActiveInventory;

            if (instigator == null)
            {
                handle.Cancel();
                return;
            }

            var successfulTransfer = linkedInventory.TryTransferItem(0, instigator);

            if (successfulTransfer)
            {
                handle.Complete();
                OnPickup?.Invoke();
            }
            else
                handle.Cancel();
        }

        /// <summary>
        /// Places an item from the instigator's active inventory into the linked inventory
        /// </summary>
        public void Dropoff(InteractionHandle handle)
        {
            var instigator = handle.Instigator?.GetComponentInChildren<EquipmentUser>(true)?.ActiveInventory;

            if (instigator == null)
            {
                handle.Cancel();
                return;
            }

            var successfulTransfer = instigator.TryTransferItem(0, linkedInventory);

            if (successfulTransfer)
            {
                handle.Complete();
                OnDropOff?.Invoke();
            }
            else
                handle.Cancel();
        }

    }

}