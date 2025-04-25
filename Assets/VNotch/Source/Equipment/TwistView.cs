using MEEP.InputPipelines;
using UnityEngine;

namespace VNotch.Equipment
{

    /// <summary>
    /// Spinns a given object based on horizontal mouse input
    /// </summary>
    public class TwistView : MonoBehaviour
    {

        [SerializeField]
        private PipedVector2Input pipe_look;

        private float _mouseVelocity;

        [SerializeField]
        public float min, max;

        [SerializeField]
        public float softBounds;

        public float targetSpin;

        private void OnEnable()
        {
            pipe_look.RegisterProcessor(UpdateLookValue, 110);
        }

        private void OnDisable()
        {
            pipe_look.UnregisterProcessor(UpdateLookValue);
        }

        private void UpdateLookValue(ref ProcessedInputAction<Vector2> input)
        {
            _mouseVelocity = input.ProcessedValue.x;
            input.Consume();
        }

        private void LateUpdate()
        {
            UpdateSpin();
            var spin = Quaternion.AngleAxis(targetSpin, transform.up);
            transform.rotation = spin * transform.rotation;
        }

        private void UpdateSpin()
        {
            targetSpin += _mouseVelocity;

            if (targetSpin > max)
                targetSpin = max;

            if (targetSpin < min)
                targetSpin = min;
        }

    }
}

