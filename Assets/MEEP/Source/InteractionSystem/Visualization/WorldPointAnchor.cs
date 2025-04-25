using UnityEngine;

namespace MEEP.InteractionSystem.Visualization
{
    /// <summary>
    /// Places an overlay styled UI Element to appear at a world space position
    /// </summary>
    public class WorldPointAnchor : MonoBehaviour
    {
        public Transform target;

        [SerializeField]
        private GameplayGlobal global_camera;

        private Camera _camera;

        /// <summary>
        /// defines a position that is offscreen
        /// </summary>
        private Vector3 offscreenPosition = new Vector3(0, -100000, 0);

        // Start is called before the first frame update
        void Start()
        {
            _camera = global_camera.GetComponent<Camera>();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (_camera == null || target == null)
                return;


            if (IsInFrontOfCamera())
            {
                this.transform.position = _camera.WorldToScreenPoint(target.transform.position);
            }
            else
            {
                this.transform.position = _camera.WorldToScreenPoint(offscreenPosition);
            }
        }


        /// <summary>
        /// Checks if the target point is in front of the camera
        /// </summary>
        private bool IsInFrontOfCamera()
        {
            var direction = _camera.transform.position - target.transform.position;
            return Vector3.Dot(_camera.transform.forward, direction) < 0;
        }
    }
}
