using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lets you open a file at runtime
/// </summary>
public class FileLink : MonoBehaviour
{

    [Tooltip("URL to open file")]
    [SerializeField]
    private string urlToOpen;

    public void OpenURL()
    {
        var path = string.Format("{0}/{1}", Application.dataPath, urlToOpen);
        Debug.Log(path);
        Application.OpenURL(path);
    }

}
