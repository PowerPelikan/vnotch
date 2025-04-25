using System.Collections.Generic;
using UnityEngine;

namespace MEEP.MachineSystem
{
    /// <summary>
    /// Defines an independent part of an instrument/machine.
    /// This allows for multiple interactions on a single machine,
    /// while also keeping states consistent.
    /// These parts are also referenced by the Animator via Animation Layers
    /// </summary>
    [CreateAssetMenu(fileName = "My Part", menuName = "VNotch/Machine/Part")]
    public class MachinePart : ScriptableObject
    {

        public string animatorLayerName = "";

        public List<MachinePartState> states = new();

    }
}
