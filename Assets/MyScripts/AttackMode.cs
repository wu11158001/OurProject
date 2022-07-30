using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻擊模式
/// </summary>
public class AttackMode
{
    public Action function;    

    //通用
    public GameObject performObject;//執行攻擊的物件(自身/射出物件)    
    public string layer;//攻擊者layer
    public float damage;//造成傷害
    public string animationName;//攻擊效果(受擊者播放的動畫名稱)
    public float repel;//擊退距離
    public int direction;//擊退方向((0:擊退 1:擊飛))
    public bool isCritical;//是否爆擊

    //近身    
    public float forwardDistance;//攻擊範圍中心點距離物件前方
    public float attackRadius;//攻擊半徑
    public bool isAttackBehind;//是否攻擊背後敵人

    //遠程
    List<Transform> record = new List<Transform>();//紀錄已擊中的物件
    public float flightSpeed;//飛行速度
    public Vector3 flightDiration;//飛行方向
    public float lifeTime;//生存時間    

    /// <summary>
    /// 實例化
    /// </summary>
    public static AttackMode Instance => new AttackMode();

    /// <summary>
    /// 設定打擊事件
    /// </summary>
    public void OnSetHitFunction()
    {        
        function = OnHit;
    }

    /// <summary>
    /// 設定射擊事件_群體攻擊
    /// </summary>
    public void OnSetShootFunction_Group()
    {
        function = OnShoot;
        function += OnShootionCollision_Group;
    }

    /// <summary>
    /// 設定射擊事件_單體攻擊
    /// </summary>
    public void OnSetShootFunction_Single()
    {        
        function = OnShoot;
        function += OnShootionCollision_Single;        
    }

    /// <summary>
    /// 打擊攻擊(近身攻擊)
    /// </summary>
    void OnHit()
    {
        BoxCollider box = performObject.GetComponent<BoxCollider>();
        
        Collider[] hits = Physics.OverlapSphere(performObject.transform.position + box.center + performObject.transform.forward * forwardDistance, attackRadius);
        foreach (var hit in hits)
        {
            CharactersCollision collision = hit.GetComponent<CharactersCollision>();
            if (collision != null)
            {
                //是否攻擊背後敵人
                if (!isAttackBehind && Vector3.Dot(performObject.transform.forward, hit.transform.position - performObject.transform.position) <= 0) continue;
                OnSetAttackNumbericalValue(collision);
            }
        }   

       GameSceneManagement.Instance.AttackBehavior_List.Remove(this);
    }   

    /// <summary>
    /// 射出物件(遠程攻擊)
    /// </summary>
    void OnShoot()
    {
        //生存時間
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendObjectActive(performObject, false);
            performObject.SetActive(false);
            GameSceneManagement.Instance.AttackBehavior_List.Remove(this);
        }

        //設定前方
        if(performObject.transform.forward != flightDiration) performObject.transform.forward = flightDiration;
        
        //物件飛行
        performObject.transform.position = performObject.transform.position + performObject.transform.forward * flightSpeed * Time.deltaTime;
    }

    /// <summary>
    /// 碰撞偵測_群體攻擊(射擊物件)
    /// </summary>
    void OnShootionCollision_Group()
    {
        SphereCollider sphere = performObject.GetComponent<SphereCollider>();
        Collider[] hits = Physics.OverlapSphere(performObject.transform.position, sphere.radius * sphere.transform.localScale.x);     
        foreach (var hit in hits)
        {
            for (int i = 0; i < record.Count; i++)
            {
                //不重複擊中
                if (record[i] == hit.transform) return;
            }

            CharactersCollision collision = hit.GetComponent<CharactersCollision>();
            if (collision != null)
            {
                OnSetAttackNumbericalValue(collision);               
                record.Add(hit.transform);//紀錄以擊中物件                
            }            
        }
    }

    /// <summary>
    /// 碰撞偵測_單體攻擊(射擊物件)
    /// </summary>
    void OnShootionCollision_Single()
    {
        SphereCollider sphere = performObject.GetComponent<SphereCollider>();
        Collider[] hits = Physics.OverlapSphere(performObject.transform.position, sphere.radius * sphere.transform.localScale.x);
        foreach (var hit in hits)
        {
            CharactersCollision collision = hit.GetComponent<CharactersCollision>();
            if (collision != null)
            {
                OnSetAttackNumbericalValue(collision);
                
                //物件關閉
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendObjectActive(performObject, false);
                performObject.SetActive(false);
                GameSceneManagement.Instance.AttackBehavior_List.Remove(this);
                return;
            }
        }
    }

    /// <summary>
    /// 設定攻擊數值
    /// </summary>
    /// <param name="charactersCollision">受攻擊物件的碰撞腳本</param>
    void OnSetAttackNumbericalValue(CharactersCollision charactersCollision)
    {
        charactersCollision.OnGetHit(attacker: performObject,//攻擊者物件
                                     layer: layer,//攻擊者layer
                                     damage: damage,//造成傷害
                                     animationName: animationName,//攻擊效果(受擊者播放的動畫名稱)
                                     knockDirection: direction,//擊退方向((0:擊退 1:擊飛))
                                     repel: repel,//擊退距離
                                     isCritical: isCritical);//是否爆擊          
    }       
}
