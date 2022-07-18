using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 擊中文字
/// </summary>
public class HitNumber : MonoBehaviour
{
    Canvas canvas_Overlay;
    Text thisText;

    Transform target;//受傷目標   
    Vector3 startPosition;//初始位置
    float lifeTime;//生存時間

    void Start()
    {
        canvas_Overlay = GameObject.Find("Canvas_Overlay").GetComponent<Canvas>();     
        transform.SetParent(canvas_Overlay.transform);

        OnInitail();
    }

    
    void Update()
    {
        OnHitNumberBehavior();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    void OnInitail()
    {
        lifeTime = 1.5f;//生存時間
    }

    /// <summary>
    /// 設定數值
    /// </summary>
    /// <param name="target">受傷目標</param>
    /// <param name="damage">受到傷害</param>
    /// <param name="color">文字顏色</param>
    public void OnSetValue(Transform target, float damage, Color color)
    {
        if (thisText == null) thisText = GetComponent<Text>();


        this.target = target;//受傷目標
        thisText.text = damage.ToString();//受到傷害
        startPosition = target.position + Vector3.up * 1;//初始位置
        thisText.color = color;//文字顏色        
    }

    /// <summary>
    /// 擊中文字行為
    /// </summary>
    void OnHitNumberBehavior()
    {
        if (target == null) return;

        //生存時間
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            OnInitail();
            gameObject.SetActive(false);
        }

        //向上飄
        startPosition += Vector3.up * 1 * Time.deltaTime;

        Camera camera = canvas_Overlay.worldCamera;
        Vector3 position = Camera.main.WorldToScreenPoint(startPosition);

        //物件在後方
        if(position.z < 0)
        {
            OnInitail();
            gameObject.SetActive(false);
        }

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
    }
}
