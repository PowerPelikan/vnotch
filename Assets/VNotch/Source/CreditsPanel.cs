using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreditsPanel : MonoBehaviour
{

    [SerializeField]
    private TextAsset text;

    [ContextMenu("Update Text")]
    private void Start()
    {
        GetComponent<TextMeshProUGUI>().text = text.text;
    }

}
