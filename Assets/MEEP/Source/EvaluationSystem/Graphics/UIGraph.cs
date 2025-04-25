using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MEEP
{
    /// <summary>
    /// Creates a line graph visualization of a set of data
    /// </summary>
    [ExecuteInEditMode]
    public class UIGraph : Graphic
    {
        [SerializeField]
        private float thickness = 0.1F;

        [SerializeField]
        public List<Vector2> dataPoints;

        [SerializeField]
        public Vector2 dataLowerBound;

        [SerializeField]
        public Vector2 dataUpperBound;


        private UIVertex BaseVert
        {
            get
            {
                UIVertex vert = UIVertex.simpleVert;
                vert.color = this.color;
                return vert;
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {

            vh.Clear();

            for (int i = 0; i < dataPoints.Count - 1; i++)
            {
                var mappedDataA = RemapDataPoint(dataPoints[i]);
                var mappedDataB = RemapDataPoint(dataPoints[i + 1]);

                var verts = CreateSegment(mappedDataA, mappedDataB);
                vh.AddUIVertexQuad(verts);

                if (i < dataPoints.Count - 2)
                {
                    var mappedDataC = RemapDataPoint(dataPoints[i + 2]);
                    var miter = CreateMiter(mappedDataA, mappedDataB, mappedDataC);
                    vh.AddUIVertexQuad(miter);
                }
            }
        }

        private UIVertex[] CreateSegment(Vector2 a, Vector2 b)
        {
            Vector2 tangent = (b - a).normalized;
            Vector2 normal = new Vector2(tangent.y, -tangent.x);

            UIVertex vA = BaseVert;
            vA.position = a + (normal * thickness);

            UIVertex vB = BaseVert;
            vB.position = a - (normal * thickness);

            UIVertex vC = BaseVert;
            vC.position = b - (normal * thickness);

            UIVertex vD = BaseVert;
            vD.position = b + (normal * thickness);

            return new UIVertex[] { vA, vB, vC, vD };
        }

        private UIVertex[] CreateMiter(Vector2 a, Vector2 b, Vector2 c)
        {
            Vector2 tangentA = (b - a).normalized;
            Vector2 normalA = new Vector2(tangentA.y, -tangentA.x);

            Vector2 tangentB = (c - b).normalized;
            Vector2 normalB = new Vector2(tangentB.y, -tangentB.x);

            UIVertex vA = BaseVert;
            vA.position = b + (normalA * thickness);

            UIVertex vB = BaseVert;
            vB.position = b - (normalA * thickness);

            UIVertex vC = BaseVert;
            vC.position = b - (normalB * thickness);

            UIVertex vD = BaseVert;
            vD.position = b + (normalB * thickness);

            return new UIVertex[] { vC, vA, vD, vB };
        }

        /// <summary>
        /// Maps data point to fit the rect, as well as the given value range.
        /// </summary>
        private Vector2 RemapDataPoint(Vector2 dataPoint)
        {
            var rangeMapped = new Vector2(
                Mathf.InverseLerp(dataLowerBound.x, dataUpperBound.x, dataPoint.x),
                Mathf.InverseLerp(dataLowerBound.y, dataUpperBound.y, dataPoint.y)
                );

            var rectMin = rectTransform.rect.min;
            var rectMax = rectTransform.rect.max;

            return new Vector2(
                Mathf.Lerp(rectMin.x, rectMax.x, rangeMapped.x),
                Mathf.Lerp(rectMin.y, rectMax.y, rangeMapped.y)
                );
        }


    }
}
