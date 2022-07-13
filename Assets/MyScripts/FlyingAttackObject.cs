using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 飛行攻擊物件
/// </summary>
public class FlyingAttackObject
{
    public GameObject flyingObject;//飛行物件
    public float speed;//飛行速度
    public Vector3 diration;//飛行方向
    public float lifeTime;//生存時間
    public LayerMask layer;//攻擊者layer
    public float damage;//造成傷害
    public string animationName;//攻擊效果(受擊者播放的動畫名稱)
    public float repel;//擊退距離
   
    List<Transform> record = new List<Transform>();//紀錄已擊中的物件

    //飛行
    public void OnFlying()
    {
        //生存時間
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {         
            flyingObject.SetActive(false);
            GameManagement.Instance.flyingAttackObject_List.Remove(this);
        }
        
        //物件飛行
        flyingObject.transform.position = flyingObject.transform.position + diration * speed * Time.deltaTime;

        SphereCollider box = flyingObject.GetComponent<SphereCollider>();
        Collider[] hits = Physics.OverlapSphere(flyingObject.transform.position, box.radius, layer);

        foreach (var hit in hits)
        {
            for (int i = 0; i < record.Count; i++)
            {
                //不重複擊中
                if (record[i] == hit.transform) return;
            }

            CharactersCollision charactersCollision = hit.GetComponent<CharactersCollision>();
            if (charactersCollision != null) charactersCollision.OnGetHit(attacker: flyingObject,//攻擊者物件
                                                                          layer: layer,
                                                                          damage: damage,
                                                                          animationName: animationName,
                                                                          effect: 0,
                                                                          repel: repel);

            record.Add(hit.transform);//紀錄以擊中物件
        }
    }

    /// <summary>
    /// 碰撞偵測
    /// </summary>
   /* public void OnCollision()
    {
        SphereCollider box = flyingObject.GetComponent<SphereCollider>();
        Collider[] hits = Physics.OverlapSphere(flyingObject.transform.position, box.radius, layer);
        
        foreach (var hit in hits)
        {
            for (int i = 0; i < record.Count; i++)
            {
                //不重複擊中
                if (record[i] == hit.transform) return;                               
            }

            CharactersCollision charactersCollision = hit.GetComponent<CharactersCollision>();
            if (charactersCollision != null) charactersCollision.OnGetHit(attacker: flyingObject,//攻擊者物件
                                                                          layer: layer,
                                                                          damage: damage,
                                                                          animationName: animationName,
                                                                          effect: 0,
                                                                          repel: repel);

            record.Add(hit.transform);//紀錄以擊中物件
        }
    }*/
}
