using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectName : MonoBehaviour
{
    Canvas canvas_Overlay;
    Text thisText;

    PlayerControl playerControl;

    Transform theTarget;//目標   
    Vector3 startPosition;//初始位置
    float postitionHight;//高度

    void Start()
    {
        canvas_Overlay = GameObject.Find("Canvas_Overlay").GetComponent<Canvas>();

        //thisText = GetComponent<Text>();
        transform.SetParent(canvas_Overlay.transform);
        //target = transform.parent;

    }

    void Update()
    {
        OnBehavior();
    }

    /// <summary>
    /// 設定名稱
    /// </summary>
    /// <param name="target">目標物件</param>
    /// <param name="thisName">名稱</param>
    /// <param name="thisColor">顏色</param>
    /// <param name="hight">高度</param>
    public void OnSetName(Transform target, string thisName, Color thisColor, float hight )
    {
        if (thisText == null) thisText = GetComponent<Text>();

        theTarget = target;
        thisText.text = thisName;
        thisText.color = thisColor;
        postitionHight = hight;//高度

        PlayerControl[] PC = GameObject.FindObjectsOfType<PlayerControl>();

        for (int i = 0; i < PC.Length; i++)
        {
            if (PC[i].enabled)
            {
                playerControl = PC[i];
                break;
            }
        }
    }

    /// <summary>
    /// 行為
    /// </summary>
    void OnBehavior()
    {
        if (theTarget == null) return;
        if (!theTarget.gameObject.activeSelf) Destroy(gameObject);

        Camera camera = canvas_Overlay.worldCamera;
        Vector3 position = Camera.main.WorldToScreenPoint(startPosition);
        int maxSize = 33;

        startPosition = theTarget.position + theTarget.transform.up * postitionHight;
        int size = (int)(400 / (theTarget.position - playerControl.transform.position).magnitude);
        if (size <= 0) size = maxSize;
        if (size >= maxSize) size = maxSize;
        thisText.fontSize = size;

        //判斷Canvas的RenderMode
        if (canvas_Overlay.renderMode == RenderMode.ScreenSpaceOverlay || camera == null)
        {
            transform.position = position;
        }
        else
        {
            Vector2 localPosition = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), position, camera, out localPosition);
        }


        //與玩家之間有障礙物
        if (Physics.Linecast(theTarget.position + Vector3.up * 0.5f, playerControl.transform.position + Vector3.up * 0.5f, 1 << LayerMask.NameToLayer("StageObject"))
            || GameSceneUI.Instance.isOptions
            || GameSceneUI.Instance.isGameOver
            || Vector3.Dot(Camera.main.transform.forward, theTarget.transform.position - Camera.main.transform.position) < 0)
        {
            if (thisText.enabled) thisText.enabled = false;
        }
        else
        {
            if (!thisText.enabled) thisText.enabled = true;
        }


    }
}
