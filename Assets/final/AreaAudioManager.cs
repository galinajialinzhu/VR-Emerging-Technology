using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAudioManager : MonoBehaviour
{
    [Header("Area Colliders (Trigger)")]
    public List<Collider> areaColliders; // 4 area Trigger Collider

    [Header("Objects to Detect")]
    public List<GameObject> targetObjects; // object

    [Header("Matching AudioSources (Same order as targetObjects)")]
    public List<AudioSource> audioSources; // audio

   
    private Dictionary<int, GameObject> areaCurrentObject = new Dictionary<int, GameObject>();
    private Dictionary<int, float> areaEnterTime = new Dictionary<int, float>();
    private bool hasPlayed = false;

    void Start()
    {
        for (int i = 0; i < areaColliders.Count; i++)
        {
            areaCurrentObject[i] = null;
            areaEnterTime[i] = 0f;
        }
    }

    void Update()
    {
        for (int i = 0; i < areaColliders.Count; i++)
        {
            GameObject obj = GetObjectInArea(areaColliders[i]);
            if (obj != null)
            {
                if (areaCurrentObject[i] != obj)
                {
                    areaCurrentObject[i] = obj;
                    areaEnterTime[i] = Time.time;
                }
            }
            else
            {
                areaCurrentObject[i] = null;
                areaEnterTime[i] = 0f;
            }
        }

        if (!hasPlayed && AllAreasReady())
        {
            PlayAllSounds();
            hasPlayed = true;
        }

        if (!AllAreasOccupied())
        {
            hasPlayed = false;
        }
    }

    GameObject GetObjectInArea(Collider area)
    {
        foreach (GameObject obj in targetObjects)
        {
            if (area.bounds.Contains(obj.transform.position))
            {
                return obj;
            }
        }
        return null;
    }

    bool AllAreasOccupied()
    {
        foreach (var kvp in areaCurrentObject)
        {
            if (kvp.Value == null)
                return false;
        }
        return true;
    }

    bool AllAreasReady()
    {
        foreach (var kvp in areaCurrentObject)
        {
            if (kvp.Value == null || Time.time - areaEnterTime[kvp.Key] < 1f)
                return false;
        }
        return true;
    }

    void PlayAllSounds()
    {
        Debug.Log("All areas occupied for >1s. Playing sounds!");

        foreach (var kvp in areaCurrentObject)
        {
            GameObject obj = kvp.Value;
            int index = targetObjects.IndexOf(obj);
            if (index >= 0 && index < audioSources.Count)
            {
                audioSources[index].Play();
            }
        }
    }
}
