using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTriggerManager : MonoBehaviour
{
    public DetectArea[] detectAreas; // 拖入4个DetectArea
    private bool hasPlayed = false;

    void Update()
    {
        // 检查是否每个区域都有至少一个物体
        bool allAreasOccupied = true;
        foreach (DetectArea area in detectAreas)
        {
            if (area.GetObjectsInArea().Count == 0)
            {
                allAreasOccupied = false;
                break;
            }
        }

        // 如果满足条件并且之前没播放过，触发播放
        if (allAreasOccupied && !hasPlayed)
        {
            PlayAllAudios();
            hasPlayed = true;
        }

        // 如果有任何区域空了，就重置播放标志
        if (!allAreasOccupied)
        {
            hasPlayed = false;
        }
    }

    void PlayAllAudios()
    {
        foreach (DetectArea area in detectAreas)
        {
            foreach (GameObject obj in area.GetObjectsInArea())
            {
                Transform audioTransform = obj.transform.Find("Audio");
                if (audioTransform != null)
                {
                    AudioSource audio = audioTransform.GetComponent<AudioSource>();
                    if (audio != null && !audio.isPlaying)
                    {
                        audio.Play(); // 同步播放
                    }
                }
            }
        }
    }

}
