using MEEP.InteractionSystem;
using UnityEngine;

namespace MEEP.EquipmentSystem
{
    public class EquipmentPickUp : MonoBehaviour
    {

        [SerializeField]
        private GameObject equipmentPrefab;

        public void Equip(InteractionHandle handle)
        {
            var instigator = handle.Instigator?.GetComponentInChildren<EquipmentUser>();

            if (instigator != null)
            {
                var instance = Instantiate(equipmentPrefab, instigator.transform);
                instigator.AddEquipmentLayer(instance);
                handle.Complete();
            }
            else
            {
                handle.Cancel();
            }
        }

    }
}
