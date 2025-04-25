using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Container for a scene transition payload.
/// In essence, adding an object to this protects it 
/// from being unloaded by the SceneLoader on the same gameObject
/// </summary>
public class SceneLoaderPayload : MonoBehaviour
{

    [SerializeField]
    private List<Object> payloadObjects;

    public IReadOnlyList<Object> PayloadObjects => payloadObjects.AsReadOnly();

    private void Awake()
    {
        payloadObjects = new List<Object>();
    }

    /// <summary>
    /// Adds objects to the payload
    /// </summary>
    public void AddToPayload(params Object[] objects)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            payloadObjects.Add(objects[i]);
        }
    }

    public void RemoveFromPayload(params Object[] objects)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            payloadObjects.Remove(objects[i]);
        }
    }

}
