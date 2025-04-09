using System.Collections.Generic;
using UnityEngine;

public class DetectArea : MonoBehaviour
{
    public string areaTag; // 区域名称或ID，例如 "Area1", "Area2"...
    public List<GameObject> objectsInArea = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (!objectsInArea.Contains(other.gameObject))
        {
            objectsInArea.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsInArea.Contains(other.gameObject))
        {
            objectsInArea.Remove(other.gameObject);
        }
    }

    public List<GameObject> GetObjectsInArea()
    {
        return objectsInArea;
    }
}
