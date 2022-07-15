using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Buff物件拖拉
/// </summary>
public class BuffButtonDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform originalParent;//初始父物件

    void Start()
    {
        originalParent = transform.parent;
    }

    /// <summary>
    /// 開始拖拉
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject buff = eventData.pointerDrag;
        if (buff == null) return;

        buff.GetComponent<Image>().raycastTarget = false;
        buff.transform.SetParent(StartSceneUI.Instance.transform);
        buff.transform.position = eventData.position;
    }

    /// <summary>
    /// 拖拉
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        GameObject buff = eventData.pointerDrag;
        buff.transform.position = eventData.position;
    }

    /// <summary>
    /// 結束拖拉
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject buff = eventData.pointerDrag;
        if (buff == null) return;

        buff.GetComponent<Image>().raycastTarget = true;

        //沒拉進Buff框
        if (buff.transform.parent == StartSceneUI.Instance.transform)
        {
            buff.GetComponent<RectTransform>().sizeDelta = new Vector2(originalParent.GetComponent<RectTransform>().sizeDelta.x - 10, originalParent.GetComponent<RectTransform>().sizeDelta.y - 10);
            buff.transform.SetParent(originalParent);//回原位
            buff.transform.localPosition = Vector3.zero;
        }
    }
}
