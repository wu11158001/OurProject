using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar_Characters : MonoBehaviour
{
    Canvas canvas_World;
    float hpProportion;//生命比例
    Transform target;//目標物件
    [SerializeField]Image lifeBarFront_Image;//生命條(前)
    Image lifeBarMid_Image;//生命條(中)
    Image lifeBarBack_Image;//生命條(後)
    float targetHight;//物件高度

    void Start()
    {
        hpProportion = 1;
        
        lifeBarFront_Image.fillAmount = hpProportion;
        lifeBarMid_Image = ExtensionMethods.FindAnyChild<Image>(transform, "LifeBarMid_Image");//生命條(中)
        lifeBarMid_Image.fillAmount = hpProportion;
        lifeBarBack_Image = ExtensionMethods.FindAnyChild<Image>(transform, "LifeBarBack_Image");//生命條(後)
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
            targetHight = target.GetComponent<BoxCollider>().size.y / 5f;
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
        if (target == null)
        {
            if (gameObject.activeSelf) gameObject.SetActive(false);
            return;
        }
               
        //跟隨目標
        Camera cnavasCamera = canvas_World.worldCamera;
        transform.position = new Vector3(target.position.x, target.position.y + targetHight, target.position.z);
        transform.rotation = cnavasCamera.transform.rotation;
        
        //生命條行為
        if (hpProportion <= 0) hpProportion = 0;//生命比例                
        lifeBarFront_Image.fillAmount = hpProportion;//生命條(前)        
        if (lifeBarFront_Image.fillAmount < lifeBarMid_Image.fillAmount)//生命條(中)
        {
            lifeBarMid_Image.fillAmount -= 0.5f * Time.deltaTime;
        }

        //關閉物件
        if (lifeBarMid_Image.fillAmount <= 0)
        {
            lifeBarFront_Image.enabled = false;//生命條(前)
            lifeBarMid_Image.enabled = false;//生命條(中)
            lifeBarBack_Image.enabled = false;//生命條(後)
        }

        //開啟物件
        if(lifeBarFront_Image.fillAmount > 0 && !lifeBarFront_Image.enabled)
        {
            lifeBarFront_Image.enabled = true;//生命條(前)
            lifeBarMid_Image.enabled = true;//生命條(中)
            lifeBarBack_Image.enabled = true;//生命條(後)
        }
    }
}
