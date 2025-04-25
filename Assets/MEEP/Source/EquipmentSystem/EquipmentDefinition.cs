using UnityEngine;
using UnityEngine.Localization;

namespace MEEP.EquipmentSystem
{
    /// <summary>
    /// Defines a type of equipment, identifiable between scenes.
    /// Multiple instances of this equipment may be used.
    /// </summary>
    [CreateAssetMenu(menuName = "MEEP/Equipment/New Equipment Definition", fileName = "MyEquipment")]
    public class EquipmentDefinition : ScriptableObject
    {
        /// <summary>
        /// The name of the equipment piece.
        /// </summary>
        public LocalizedString EquipmentName;

        public Sprite EquipmentIcon;
    }
}
