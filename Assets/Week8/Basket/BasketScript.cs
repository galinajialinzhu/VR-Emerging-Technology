using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction; // 确保 Oculus SDK 交互命名空间

public class BasketScript : MonoBehaviour
{
    public List<GameObject> flowersInBasket = new List<GameObject>(); // 存储放入花篮的花
    public int maxFlowers = 4; // 限制最多 4 朵花

    private void OnTriggerEnter(Collider other)
    {
        // 确保对象是花（beat1, beat2, beat3, beat4）
        if ((other.gameObject.name.StartsWith("beat") || other.gameObject.CompareTag("Flower"))
            && !flowersInBasket.Contains(other.gameObject))
        {
            if (flowersInBasket.Count < maxFlowers)
            {
                AddFlowerToBasket(other.gameObject);
            }
            else
            {
                Debug.Log("花篮已满，无法放入更多花朵");
            }
        }
    }

    private void AddFlowerToBasket(GameObject flower)
    {
        flowersInBasket.Add(flower); // 记录放入花篮的花

        // 获取花的 AudioSource
        AudioSource flowerAudio = flower.GetComponentInChildren<AudioSource>();
        if (flowerAudio != null)
        {
            flowerAudio.loop = true; // 进入花篮后启用 Loop
            if (!flowerAudio.isPlaying)
            {
                flowerAudio.Play(); // 确保音乐播放
            }
        }

        // 禁用 Grabbable（防止再次抓取）
        var grabComponent = flower.GetComponent<Grabbable>();
        if (grabComponent != null)
        {
            grabComponent.enabled = false;
        }

        //  禁用 TouchHandGrabInteractable（防止手部交互）
        var touchGrab = flower.GetComponent<TouchHandGrabInteractable>();
        if (touchGrab != null)
        {
            touchGrab.enabled = false;
        }

        //  让 InteractableUnityEventWrapper忽略 Unhover 事件
        var eventWrapper = flower.GetComponent<InteractableUnityEventWrapper>();
        if (eventWrapper != null)
        {
            eventWrapper.WhenUnhover.RemoveAllListeners(); // 移除 Unhover 触发 `Stop()`
        }

        //  让花固定在花篮里
        flower.transform.SetParent(transform);
        flower.transform.localPosition = new Vector3(0, flowersInBasket.Count * 0.1f, 0);
        flower.transform.localRotation = Quaternion.identity;
    }
}