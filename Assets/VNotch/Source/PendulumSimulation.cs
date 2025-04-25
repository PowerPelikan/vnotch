using MEEP.Inventories;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace MEEP.Experiment
{

    /// <summary>
    /// Simple pendulum simulation
    /// </summary>
    [RequireComponent(typeof(Inventory))]
    public class PendulumSimulation : MonoBehaviour
    {

        private float GRAVITY => Physics.gravity.magnitude;

        #region Properties

        [Space]

        [SerializeField]
        private HammerConfiguration hammerConfig;

        [Tooltip("The inventory holding the currently inserted sample")]
        [SerializeField]
        private Inventory sampleSlot;

        [Header("Phyiscs Settings")]

        [Min(0)]
        [Tooltip("The amount of drag the pendulum experiences.")]
        [SerializeField] private float drag = 0f;

        [Min(0)]
        [Tooltip("The accuracy of the simulation. Lower values are more accurate.")]
        [SerializeField] private float tickRate = 0.005f;

        [Min(0)]
        [Tooltip("The speed of the simulation.")]
        [SerializeField] private float speedMultiplier = 1;

        [Space]

        [Tooltip("The apex of the pendulum was reached and it begun to swing the other way.")]
        [SerializeField] private UnityEvent OnApexReached;
        [Tooltip("The pendulum has crossed the center and swung from one side to the other.")]
        [SerializeField] private UnityEvent OnCrossedCenter;
        [Tooltip("The pendulum has broken the probe. This is called immediately after the first swing.")]
        [SerializeField] private UnityEvent OnSampleBroken;
        [Tooltip("The pendulum has come to a stop.")]
        [SerializeField] private UnityEvent OnStoppedSwinging;

        public event Action OnSimulationTick;

        public MaterialSampleInstance Sample => sampleSlot?.GetItem(0)?.GetComponentInChildren<MaterialSampleInstance>(true);

        /// <summary> Position of the pendulum in radians </summary>
        public float CurrentAngle => currentAngle;

        /// <summary> A world space vector of the current end position of the pendulum </summary>
        public Vector3 PendulumVector => transform.TransformPoint(Quaternion.AngleAxis(currentAngle * Mathf.Rad2Deg, Vector3.forward) * Vector3.down * hammerConfig.ArmLength);

        /// <summary> A world space vector of the current velocity of the pendulum </summary>
        public Vector3 PendulumVelocityVector => transform.TransformVector(new Vector3(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle), 0)) * currentVelocity;

        /// <summary> Velocity of the pendulum in radians/second </summary>
        public float CurrentVelocity => currentVelocity;

        /// <summary> Number of times the pendulum has crossed its resting position </summary>
        public int PendulumSwings => swingsDone;

        #endregion Properties

        /// <summary>Is the pendulum currently simulating</summary>
        private bool isSimulating = false;

        /// <summary> Position of the pendulum in radians </summary>
        private float currentAngle;

        /// <summary> Velocity of the pendulum in radians/second </summary>
        private float currentVelocity;

        /// <summary> Number of times the pendulum has crossed its resting position </summary>
        private int swingsDone;

        /// <summary></summary>
        private float timeAccumulator;



        private void Start()
        {
            sampleSlot = GetComponent<Inventory>();

            if (hammerConfig == null)
                throw new System.ArgumentException("The hammer needs a configuration in order to function");

            StopSimulation();
        }


        private void Update()
        {
            if (!isSimulating || hammerConfig == null)
                return;

            // Accumulate frames and simulate
            timeAccumulator += Time.deltaTime;

            while (tickRate > 0 && timeAccumulator >= tickRate)
            {
                timeAccumulator -= tickRate;

                float oldVel = currentVelocity;
                int oldSwings = swingsDone;

                TickSimulation(tickRate * speedMultiplier, ref currentAngle, ref currentVelocity, ref swingsDone);
                OnSimulationTick?.Invoke();

                // On apex reached
                if (oldVel != 0 && Mathf.Sign(oldVel) != Mathf.Sign(currentVelocity))
                {
                    OnApexReached?.Invoke();
                }

                // On swung around
                if (oldSwings != swingsDone)
                {
                    OnCrossedCenter?.Invoke();
                    if (swingsDone == 1)
                        OnSampleBroken?.Invoke();
                }
            }
        }

        #region Playback

        /// <summary>
        /// Reset simulation state to the beginning
        /// </summary>
        /// <remarks>Does not pause/stop the simulation!</remarks>
        [ContextMenu(nameof(ResetSimulation))]
        public void ResetSimulation()
        {
            currentAngle = hammerConfig.InitialAngle;
            currentVelocity = 0;
            swingsDone = 0;

            timeAccumulator = 0;
        }

        /// <summary>
        /// Allows for simulation to occur
        /// </summary>
        [ContextMenu(nameof(StartSimulation))]
        public void StartSimulation()
        {
            isSimulating = true;
        }

        /// <summary>
        /// Prevents simulation from occurring
        /// </summary>
        [ContextMenu(nameof(PauseSimulation))]
        public void PauseSimulation()
        {
            isSimulating = false;
        }

        /// <summary>
        /// Pauses and resets the simulation
        /// </summary>
        [ContextMenu(nameof(StopSimulation))]
        public void StopSimulation()
        {
            PauseSimulation();
            ResetSimulation();
        }

        #endregion Playback

        private float CalculateRisingHeight()
        {
            // no sample, no loss
            if (Sample == null)
                return hammerConfig.InitialHeight;

            // get impact work K from sample
            var impactWork = Sample.GetImpactWork();

            // calculate how high the hammer should go
            return hammerConfig.InitialHeight * (1 - Mathf.Abs(impactWork / hammerConfig.LaborCapacity));
        }

        private float CalculateRemainingEnergyFactor()
        {
            // no sample, no loss
            if (Sample == null)
                return 1;

            // finally, convert this height into a factor
            return (CalculateRisingHeight()) / hammerConfig.InitialHeight;
        }

        /// <summary>
        /// Simulates a tick of a pendulum simulation
        /// </summary>
        /// <param name="dt">The time step of the simulation.</param>
        /// <param name="angle">The current position of the pendulum in radians.</param>
        /// <param name="velocity">The current velocity of the simulation in radians/second.</param>
        /// <param name="swings">The number of times the pendulum has swung from one side to another.</param>
        /// <remarks>The simulation state can be fed in as parameters while the simulation setup is taken from the owning component.</remarks>
        public void TickSimulation(float dt, ref float angle, ref float velocity, ref int swings)
        {
            if (hammerConfig == null)
                return;

            //check if the pendulum has stopped
            if (Mathf.Abs(velocity) <= 0.001F && Mathf.Abs(angle) <= 0.001F)
            {
                Debug.Log("Pendulum has stopped");
                StopSimulation();
                OnStoppedSwinging.Invoke();
            }


            float force = GRAVITY * Mathf.Sin(angle);
            float acceleration = -force / hammerConfig.ArmLength;
            velocity += acceleration * dt;
            velocity *= 1 - (drag * dt);
            float newPendulumAng = angle + velocity * dt;

            if (Mathf.Sign(newPendulumAng) != Mathf.Sign(angle))
            {
                swings++;

                if (swings == 1)
                {
                    var newLinVelocity = Mathf.Sqrt((2 * (hammerConfig.LaborCapacity * CalculateRemainingEnergyFactor()) / hammerConfig.Mass));
                    velocity = newLinVelocity / hammerConfig.ArmLength;
                }
            }

            angle = newPendulumAng;
        }

        public void UpdateDrag(float drag)
        {
            this.drag = Mathf.Clamp01(drag);
        }

        public void PrintImpactWork()
        {
            Debug.LogFormat("Measured Sample {0} at {1} C with Impact Work: {2}", Sample?.GetLabel(), Sample?.GetTemperature(), Sample?.GetImpactWork());
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (hammerConfig == null)
                return;


            var initialDirection = Quaternion.AngleAxis(Mathf.Rad2Deg * hammerConfig.InitialAngle, transform.forward);
            var predictionDirection = Quaternion.AngleAxis(Mathf.Rad2Deg * hammerConfig.HeightToAngle(CalculateRisingHeight()), transform.forward);
            var currentDirection = Quaternion.AngleAxis(Mathf.Rad2Deg * currentAngle, transform.forward);

            // draw initial angle
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, initialDirection * -transform.up);

            // draw predicted apex
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, predictionDirection * -transform.up);

            // draw simulation body
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, currentDirection * -transform.up);

        }

#endif


    }
}