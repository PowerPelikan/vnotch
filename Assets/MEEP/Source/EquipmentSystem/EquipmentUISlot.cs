using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MEEP.EquipmentSystem
{
    public class EquipmentUISlot : MonoBehaviour
    {

        public Image slotIcon;

        public Image itemIndicator;

        public Image selectionIndicator;

        public void SetSelectedIndicator(bool status)
        {
            selectionIndicator.gameObject.SetActive(status);
        }

        public void SetSlotIcon(EquipmentDefinition equipment)
        {
            slotIcon.sprite = equipment.EquipmentIcon;
        }

        public void SetItemIndicator(bool hasItem)
        {
            itemIndicator.gameObject.SetActive(hasItem);
        }


        public void UpdateEquipmentSlotUI(EquipmentLayer equipmentLayer, bool isSelected)
        {
            SetSlotIcon(equipmentLayer.EquipmentDef);

            var inventory = equipmentLayer.GetComponent<Inventories.Inventory>();

            if (inventory != null)
                SetItemIndicator(inventory.HasItems);
            else
                SetItemIndicator(false);

            SetSelectedIndicator(isSelected);
        }

    }
}
