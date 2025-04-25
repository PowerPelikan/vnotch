using System.Collections;
using UnityEngine;


/// <summary>
/// A small workaround to fix layout errors occuring when nesting
/// LayoutGroups in conjunction with ContentSizeFitters.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class ForceRelayout : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(RebuildLayoutDelayed());
    }

    IEnumerator RebuildLayoutDelayed()
    {
        yield return new WaitForEndOfFrame();

        RebuildLayout();
    }

    [ContextMenu("Rebuild Layout")]
    public void RebuildLayout()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}
