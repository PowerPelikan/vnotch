using UnityEngine;


/// <summary>
/// Accumulation Behaviours subdivide the typical time steps of 
/// Frames/Update and Physics/FixedUpdate. 
/// This ensures frame-rate independent and accurate simulations.
/// NOTE: Currently, behaviours are not synced in any way. TODO
/// </summary>
public abstract class AccumulationBehaviour : MonoBehaviour
{

    /// <summary>
    /// The required time step as ticks per second
    /// </summary>
    [Min(10)]
    public float ticksPerSecond = 60;

    private protected float timeAccumulator = 0;

    /// <summary>
    /// The required time step in ms
    /// </summary>
    protected float TickRate => 1 / ticksPerSecond;


    public void Update()
    {
        // Accumulate frames
        timeAccumulator += Time.deltaTime;

        // subdivide time step
        while (TickRate > 0 && timeAccumulator >= TickRate)
        {
            timeAccumulator -= TickRate;
            Tick();
        }
    }

    public abstract void Tick();

}
