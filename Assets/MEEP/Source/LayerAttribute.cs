using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MEEP
{
    /// <summary>
    /// Attribute to select a single layer.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class LayerDropdownAttribute : PropertyAttribute
    {
    }

}
