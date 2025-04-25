using System.Collections.Generic;
using UnityEngine;

namespace MEEP.MachineSystem
{

    /// <summary>
    /// Defines an abstract machine, with parts
    /// </summary>
    [CreateAssetMenu(fileName = "My Machine", menuName = "VNotch/Machine/Machine")]
    public class Machine : ScriptableObject
    {

        /// <summary>
        /// The parts of this machine that may move indepentently.
        /// Note that adding a part to multiple machines or 
        /// to the same machine multiple times breaks may break stuff.
        /// </summary>
        [SerializeField]
        [Tooltip("Add parts belonging to this machine here \n" +
            "NOTE: Adding a single part to multiple machines or to the same machine multiple times breaks breaks stuff.")]
        private List<MachinePart> parts;

        public IReadOnlyList<MachinePart> Parts
            => parts.AsReadOnly();

        /// <summary>
        /// Check if the given part belongs to this machine.
        /// </summary>
        public bool BelongsToMachine(MachinePart part)
        {
            return parts.Contains(part);
        }

    }

}