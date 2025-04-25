using MEEP.InputPipelines;
using MEEP.Inventories;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MEEP.EquipmentSystem
{
    /// <summary>
    /// Component that enables equipment use.
    /// </summary>
    public class EquipmentUser : MonoBehaviour
    {

        [SerializeField]
        private PipedEventInput pipe_scrollUp;

        [SerializeField]
        private PipedEventInput pipe_scrollDown;

        /// <summary>
        /// The selectable equipment
        /// </summary>
        [NonReorderable]
        [SerializeField]
        private List<EquipmentLayer> equipment;

        public IReadOnlyList<EquipmentLayer> Equipment => equipment;
        public EquipmentLayer ActiveLayer => (selectedIndex != -1) ? equipment[selectedIndex] : null;

        /// <summary>
        /// The Index of the selected piece of equipment.
        /// -1 indicates that no equipment is selected.
        /// </summary>
        [SerializeField]
        private int selectedIndex = -1;

        /// <summary>
        /// The inventory of the active piece of 
        /// equipment, if it has one.
        /// </summary>
        public Inventory ActiveInventory
        {
            get
            {
                if (selectedIndex == -1)
                    return null;
                else
                    return equipment[selectedIndex]?.GetComponentInChildren<Inventory>();
            }
        }

        public UnityEvent OnEquipmentUserUpdated;

        private void Start()
        {
            SelectEquipment(selectedIndex);
        }

        private void OnEnable()
        {
            pipe_scrollUp.RegisterProcessor(EquipmentValueUp, 100);
            pipe_scrollDown.RegisterProcessor(EquipmentValueDown, 100);
        }

        private void OnDisable()
        {
            pipe_scrollUp.UnregisterProcessor(EquipmentValueUp);
            pipe_scrollDown.UnregisterProcessor(EquipmentValueDown);
        }



        private void EquipmentValueUp(ref bool eventConsumedFlag)
        {
            UpdateSelectedIndex(selectedIndex + 1);
            eventConsumedFlag = true;
        }

        private void EquipmentValueDown(ref bool eventConsumedFlag)
        {
            UpdateSelectedIndex(selectedIndex - 1);
            eventConsumedFlag = true;
        }

        /// <summary>
        /// Equips the given piece of equipment
        /// </summary>
        public void SelectEquipment(int index)
        {
            // toggle equipment layers so that only the current one is visible
            for (int i = 0; i < equipment.Count; i++)
            {
                equipment[i].gameObject.SetActive(i == index);
            }
        }

        public void UpdateSelectedIndex(int newValue)
        {
            if (newValue < 0)
                newValue = equipment.Count - 1;
            if (newValue > equipment.Count - 1)
                newValue = 0;

            selectedIndex = newValue;
            SelectEquipment(selectedIndex);
            OnEquipmentUserUpdated?.Invoke();
        }

        // Input Tests

        public void OnEquipmentUpInput()
        {
            UpdateSelectedIndex(selectedIndex + 1);
        }

        public void OnEquipmentDownInput()
        {
            UpdateSelectedIndex(selectedIndex - 1);
        }

        // Add

        public void AddEquipmentLayer(GameObject equipmentLayer)
        {
            equipment.Add(equipmentLayer.GetComponent<EquipmentLayer>());
            UpdateSelectedIndex(equipment.Count - 1);


            // propagate inventory events

            var equipmentInventory = equipment[equipment.Count - 1].GetComponent<Inventory>();

            if(equipmentInventory != null)
            {
                equipmentInventory.OnInventoryChange?.AddListener(() => { OnEquipmentUserUpdated?.Invoke(); });
            }
        }

    }

}