using MEEP.Inventories;
using UnityEngine;

public class HideBrokenSample : MonoBehaviour
{

    public void HideSample()
    {
        if (GetComponent<Inventory>().GetItem(0) == null)
            return;

        GameObject.Destroy(GetComponent<Inventory>().GetItem(0).gameObject);
    }

}
