using MEEP.Experiment;
using MEEP.Inventories;
using TMPro;
using UnityEngine;

public class TongsFocusUI : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI sampleNamePanel;

    [SerializeField]
    private TextMeshProUGUI sampleTempPanel;

    private void OnEnable()
    {
        var sample = GetComponentInParent<Inventory>()?.GetItem(0)?.GetComponent<MaterialSampleInstance>();

        if (sample != null)
        {
            sampleNamePanel.text = sample.GetLabel();
            sampleTempPanel.text = string.Format("{0} °C", sample.GetTemperature());
        }
        else
        {
            sampleNamePanel.text = "No sample";
            sampleTempPanel.text = "";
        }
    }
}
