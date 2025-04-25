using MEEP.InputPipelines;
using UnityEngine;

namespace VNotch.Equipment
{
    public class ClipboardZoom : MonoBehaviour
    {

        [SerializeField]
        private PipedVector2Input pipe_cursor;

        [SerializeField]
        private PipedVector2Input pipe_look;


        private Vector2 cursorPosition;


        private void OnEnable()
        {
            pipe_cursor.RegisterProcessor(HandleCursorInput, 110);
            pipe_look.RegisterProcessor(HandleLookInput, 110);
        }

        private void OnDisable()
        {
            pipe_cursor.UnregisterProcessor(HandleCursorInput);
            pipe_look.UnregisterProcessor(HandleLookInput);
        }

        private void HandleCursorInput(ref ProcessedInputAction<Vector2> inputAction)
        {
            cursorPosition = inputAction.ProcessedValue;
        }

        private void HandleLookInput(ref ProcessedInputAction<Vector2> inputAction)
        {
            inputAction.Consume();
        }

    }
}