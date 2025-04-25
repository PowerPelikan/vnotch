using UnityEngine;

namespace MEEP.Experiment
{

    /// <summary>
    /// The in world instance of a material sample. 
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class MaterialSampleInstance : MonoBehaviour
    {

        /// <summary>
        /// The internal label of this material sample
        /// </summary>
        [SerializeField]
        private string label;

        /// <summary>
        /// The underlying properties of the sample
        /// </summary>
        [SerializeField]
        private MaterialSampleProperties properties;

        /// <summary>
        /// The currentTemperature of the sample (in °C)
        /// </summary>
        [SerializeField]
        private float temperature;


        public string GetLabel()
        {
            return label;
        }

        /// <summary>
        /// Return the impact work of the sample (based on its current temperature)
        /// in Joule Charpy
        /// </summary>
        public float GetImpactWork()
        {
            return properties.impactWorkByTemp.Evaluate(temperature);
        }

        public float GetTemperature()
        {
            return temperature;
        }

        public float GetBrittleness()
        {
            return properties.brittleness;
        }
    }


}