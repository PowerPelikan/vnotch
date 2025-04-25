using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MEEP
{
    /// <summary>
    /// Defines Startup behavior. 
    /// OnStart loads the specified scenes automatically once the object is loaded itself.
    /// Manual loads the specified scenes on demand (for example, when a button is pressed).
    /// </summary>
    public enum StartupBehaviour { OnStart, Manual }
}
