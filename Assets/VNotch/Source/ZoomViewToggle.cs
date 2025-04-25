using MEEP.InteractionSystem;
using MEEP.PlayerController;
using UnityEngine;

public class ZoomViewToggle : MonoBehaviour
{

    public void EnterZoomView(InteractionHandle handle)
    {
        var instigator = handle.Instigator.GetComponent<PlayerBrain>();
        instigator.EnterZoomView();
    }

    public void ExitZoomView(InteractionHandle handle)
    {
        var instigator = handle.Instigator.GetComponent<PlayerBrain>();
        instigator.ExitZoomView();
    }

}
