using UnityEngine;

namespace MEEP
{
    /// <summary>
    /// Pushes animation curve data to a ui graph
    /// </summary>
    [RequireComponent(typeof(UIGraph))]
    public class CurveToGraph : MonoBehaviour
    {

        [SerializeField]
        private AnimationCurve curve;

        [SerializeField]
        [Min(1F)]
        private float sampleRate = 0.1F;

#if UNITY_EDITOR

        private void OnValidate()
        {
            var graph = GetComponent<UIGraph>();

            var timeMin = graph.dataLowerBound.x;
            var timeMax = graph.dataUpperBound.x;

            graph.dataPoints.Clear();

            for (float i = timeMin; i < timeMax; i += sampleRate)
            {
                graph.dataPoints.Add(new Vector2(i, curve.Evaluate(i)));
            }

            graph.dataPoints.Add(new Vector2(timeMax, curve.Evaluate(timeMax)));

            graph.SetVerticesDirty();
        }

#endif

    }
}
