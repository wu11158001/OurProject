using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 小地圖物件小點點
/// </summary>
public class MiniMapPoint : MonoBehaviour
{
    public Material pointMaterial;

    void Start()
    {
        //抓取物件碰撞框大小
        float sizeX = transform.parent.GetComponent<BoxCollider>().size.x;
        float sizeY = transform.parent.GetComponent<BoxCollider>().size.y;

        //設定大小/位置/選轉
        transform.localScale = new Vector3(sizeX, sizeY, 1);
        transform.position = transform.parent.position;

        //設定材質
        GetComponent<Renderer>().material = pointMaterial;
    }   
}
