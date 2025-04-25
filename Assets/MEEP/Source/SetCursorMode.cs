using UnityEngine;

namespace MEEP
{
    public class SetCursorMode : MonoBehaviour
    {

        [SerializeField]
        private bool onStart = true;

        [SerializeField]
        private CursorLockMode targetMode;

        [SerializeField]
        private bool shouldBeVisible = true;

        void Start()
        {
            if (onStart)
                SetCursor();
        }

        public void SetCursor()
        {
            Cursor.lockState = targetMode;
            Cursor.visible = shouldBeVisible;
        }
    }
}
