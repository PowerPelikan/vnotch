using UnityEngine;

namespace MEEP.InputPipelines
{
    [CreateAssetMenu(menuName = "MEEP/Input/PipedBool", fileName = "pipedBool")]
    public class PipedBoolInput : PipedValueInput<bool>
    {
        public override void UpdateInput(object newValue)
        {
            if (newValue != null)
            {
                UpdateInput((float)newValue > 0.01F);
            }
            else
                UpdateInput(default(bool));
        }
    }
}
