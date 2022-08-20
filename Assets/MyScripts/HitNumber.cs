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

    [SerializeField]Transform target;//受傷目標   
    Vector3 startPosition;//初始位置
    float lifeTime;//生存時間
    float speed;//速度
    float colorAlpha;//透明度

    void Start()
    {
        canvas_Overlay = GameObject.Find("Canvas_Overlay").GetComponent<Canvas>();     
        transform.SetParent(canvas_Overlay.transform);

        lifeTime = 1.5f;//生存時間
    }

    
    void Update()
    {
        OnHitNumberBehavior();
    }

    /// <summary>
    /// 設定數值
    /// </summary>
    /// <param name="target">受傷目標</param>
    /// <param name="damage">受到傷害</param>
    /// <param name="color">文字顏色</param>
    /// <param name="isCritical">是否爆擊</param>
    public void OnSetValue(Transform target, float damage, Color color, bool isCritical)
    {
        if (thisText == null) thisText = GetComponent<Text>();

        //爆擊字放大
        if (isCritical) thisText.fontSize = 105;
        else thisText.fontSize = 70;

        this.target = target;//受傷目標
        thisText.text = Mathf.Round(damage).ToString();//受到傷害(四捨五入)        
        thisText.color = color;//文字顏色
        colorAlpha = color.a;
    }
    
    /// <summary>
    /// 擊中文字行為
    /// </summary>
    void OnHitNumberBehavior()
    {
        if (target == null) return;

        speed += 2 * Time.deltaTime; 

        //文字移動
        startPosition = target.position + target.transform.up * (1 + speed);
        //文字透明度
        thisText.color = new Color(thisText.color.r, thisText.color.g, thisText.color.b, lifeTime);

        Camera camera = canvas_Overlay.worldCamera;
        Vector3 position = Camera.main.WorldToScreenPoint(startPosition);        

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

        //生存時間
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0 || position.z < 0)
        {
            Destroy(gameObject);
        }        
    }
}
