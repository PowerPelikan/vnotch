using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MEEP.EquipmentSystem
{
    public class EquipmentUI : MonoBehaviour
    {

        [SerializeField]
        private GameplayGlobal playerGlobal;
        private EquipmentUser displayedUser;

        [SerializeField]
        private GameObject slotPrefab;

        /// <summary>
        /// The size of the pre-made ui slot pool.
        /// This avoids having to create GameObjects at runtime,
        /// but risks exceeding the pool if the user is carrying too many equipment pieces.
        /// </summary>
        [SerializeField]
        private int poolSize = 5;

        private List<GameObject> uiSlots;


        private void Start()
        {
            uiSlots = new();
            AddSlots(poolSize);

            if (playerGlobal.IsReady)
                FindDisplayedUser();

            playerGlobal.OnGlobalRegistered.AddListener(FindDisplayedUser);
        }

        private void FindDisplayedUser()
        {
            displayedUser = playerGlobal.GetTarget().GetComponentInChildren<EquipmentUser>(true);
            displayedUser.OnEquipmentUserUpdated.AddListener(UpdateEquipmentUI);
        }

        private void UpdateEquipmentUI()
        {
            for (int i = 0; i < poolSize; i++)
            {
                var hasEquipmentPartner = i < displayedUser.Equipment.Count;

                uiSlots[i].SetActive(hasEquipmentPartner);

                if (hasEquipmentPartner)
                {
                    var layer = displayedUser.Equipment[i];
                    var isSelected = displayedUser.ActiveLayer == displayedUser.Equipment[i];
                    uiSlots[i].GetComponent<EquipmentUISlot>().UpdateEquipmentSlotUI(layer, isSelected);
                }
            }
        }

        private void AddSlots(int count)
        {
            for (int i = 0; i < count; i++)
            {
                uiSlots.Add(GameObject.Instantiate(slotPrefab, transform));
                uiSlots[i].SetActive(false);
            }
        }
    }
}
