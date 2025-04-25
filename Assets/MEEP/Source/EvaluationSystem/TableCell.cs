using UnityEngine;

namespace MEEP.EvaluationSystem
{
    public abstract class TableCell : MonoBehaviour
    {
    }

    public abstract class TableCell<T> : TableCell
    {
        public T Value;
    }
}
