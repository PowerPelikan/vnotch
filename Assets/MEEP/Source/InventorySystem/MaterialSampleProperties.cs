using UnityEngine;

namespace MEEP.Experiment
{

    /// <summary>
    /// Models the material properties of a given sample
    /// </summary>
    [CreateAssetMenu(fileName = "MaterialSample", menuName = "VNotch/Experiment/MaterialSample")]
    public class MaterialSampleProperties : ScriptableObject
    {

        public string materialName;

        /// <summary>
        /// Characterizes the material. Defines the impact work of the material at a given temperature.
        /// (in Joule / °C)
        /// </summary>
        public AnimationCurve impactWorkByTemp;

        /// <summary>
        /// Defines the texture of the post experiment surface.
        /// </summary>
        [Range(0, 1)]
        public float brittleness;

        /// <summary>
        /// How much the sample should expand when impacted (in cm)
        /// </summary>
        public float expansionRate;

    }

}