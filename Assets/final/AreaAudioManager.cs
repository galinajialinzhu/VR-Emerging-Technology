using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAudioManager : MonoBehaviour
{
    [Header("Area Colliders (Trigger)")]
    public List<Collider> areaColliders; // 4个区域 Trigger Collider

    [Header("Objects to Detect")]
    public List<GameObject> targetObjects; // 16个物体

    [Header("Matching AudioSources (Same order as targetObjects)")]
    public List<AudioSource> audioSources; // 与 targetObjects 一一对应

    private Dictionary<GameObject, float> enterTimeDict = new Dictionary<GameObject, float>();
    private Dictionary<GameObject, bool> hasPlayedDict = new Dictionary<GameObject, bool>();

    void Start()
    {
        foreach (GameObject obj in targetObjects)
        {
            enterTimeDict[obj] = -1f;     // -1表示未进入
            hasPlayedDict[obj] = false;   // 尚未播放
        }
    }

    void Update()
    {
        for (int i = 0; i < targetObjects.Count; i++)
        {
            GameObject obj = targetObjects[i];
            bool isInAnyArea = false;

            foreach (Collider area in areaColliders)
            {
                if (area.bounds.Contains(obj.transform.position))
                {
                    isInAnyArea = true;

                    // 首次进入时记录时间
                    if (enterTimeDict[obj] < 0)
                    {
                        enterTimeDict[obj] = Time.time;
                    }

                    // 如果停留满1秒 且尚未播放声音
                    if (!hasPlayedDict[obj] && Time.time - enterTimeDict[obj] >= 1f)
                    {
                        PlayObjectSound(i);
                        hasPlayedDict[obj] = true;
                    }

                    break; // 如果已经在某个区域内，就不用再检查其他区域
                }
            }

            if (!isInAnyArea)
            {
                // 离开区域时重置计时和播放状态
                enterTimeDict[obj] = -1f;
                hasPlayedDict[obj] = false;
            }
        }
    }

    void PlayObjectSound(int index)
    {
        if (index >= 0 && index < audioSources.Count)
        {
            AudioSource audio = audioSources[index];
            if (audio != null && !audio.isPlaying)
            {
                audio.Play();
                Debug.Log($"[Audio] Played: {targetObjects[index].name}");
            }
        }
    }
}