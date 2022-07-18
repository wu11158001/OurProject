using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar_Characters : MonoBehaviour
{
    Canvas canvas_World;
    float hpProportion;//生命比例
    Transform target;//目標物件
    Image lifeBarFront_Image;//生命條(前)
    Image lifeBarMid_Image;//生命條(中)

    void Start()
    {
        hpProportion = 1;
        lifeBarFront_Image = ExtensionMethods.FindAnyChild<Image>(transform, "LifeBarFront_Image");
        lifeBarFront_Image.fillAmount = hpProportion;
        lifeBarMid_Image = ExtensionMethods.FindAnyChild<Image>(transform, "LifeBarMid_Image");
        lifeBarMid_Image.fillAmount = hpProportion;        
    }
    
    void Update()
    {
        OnLifeBarBehavior();
    }

    /// <summary>
    /// 設定目標物件
    /// </summary>
    public Transform SetTarget 
    { 
        set
        {
            target = value;
            canvas_World = GameObject.Find("Canvas_World").GetComponent<Canvas>();
            transform.SetParent(canvas_World.transform); 
        }
    }

    /// <summary>
    /// 設定數值
    /// </summary>
    public float SetValue { set { hpProportion = value; } }
    
    /// <summary>
    /// 生命條行為
    /// </summary>
    void OnLifeBarBehavior()
    {
        if (target == null) return;

        //跟隨目標
        Camera cnavasCamera = canvas_World.worldCamera;
        transform.position = new Vector3(target.position.x, target.position.y, target.position.z);
        transform.rotation = cnavasCamera.transform.rotation;

        //生命條行為
        if (hpProportion <= 0) hpProportion = 0;//生命比例                
        lifeBarFront_Image.fillAmount = hpProportion;//生命條(前)        
        if (lifeBarFront_Image.fillAmount < lifeBarMid_Image.fillAmount)//生命條(中)
        {
            lifeBarMid_Image.fillAmount -= 0.5f * Time.deltaTime;
        }
    }
}
