using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 腳色碰撞
/// </summary>
public class CharactersCollision : MonoBehaviourPunCallbacks
{
    Animator animator;
    GameData_NumericalValue NumericalValue;

    //碰撞框
    Vector3 boxCenter;
    Vector3 boxSize;

    //生命條
    LifeBar_Characters lifeBar;//生命條

    //數值
    [SerializeField]float Hp;//生命值
    float MaxHp;//最大生命值

    public bool isDie;//是否死亡

    public List<CharactersFloating> floating_List = new List<CharactersFloating>();//浮空/跳躍List

    private void Awake()
    {
        animator = GetComponent<Animator>();        
    }

    void Start()
    {
        NumericalValue = GameDataManagement.Instance.numericalValue;     

        //數值
        switch(gameObject.tag)
        {
            case "Player":
                MaxHp = NumericalValue.playerHp;
                break;
            case "EnemySoldier_1":
                MaxHp = NumericalValue.enemySoldier1_Hp;
                break;
        }
        
        //OnSetLifeBar_Character(transform);//設定生命條
        OnInitial();//初始化
    }

    void Update()
    {
        if(lifeBar != null) lifeBar.gameObject.SetActive(gameObject.activeSelf);
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;//連線模式      

        OnCollisionControl();
        OnAnimationOver();
        OnFloation();

        //測試用
        if (Input.GetKeyDown(KeyCode.K)) OnGetHit(gameObject, "Enemy", 100, "Pain", 0, 1, false);
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void OnInitial()
    {
        Hp = MaxHp;
        
        //生命條(頭頂)
        if (lifeBar != null)
        {
            lifeBar.SetValue = Hp / MaxHp;
        }
    }

    /// <summary>
    /// 設定生命條_遊戲腳色
    /// </summary>
    /// <param name="target">掛上的物件</param>
    void OnSetLifeBar_Character(Transform target)
    {
        lifeBar = Instantiate(Resources.Load<GameObject>(GameDataManagement.Instance.loadPath.lifeBar).GetComponent<LifeBar_Characters>());
        lifeBar.SetTarget = target;      
    }

    /// <summary>
    /// 物件身體隱藏
    /// </summary>
    /// <param name="body">隱藏身體物件</param>
    /// <param name="active">是否顯示(1:顯示 0:不顯示)</param>
    public void OnBodySetActive(int active)
    {
        Transform body = ExtensionMethods.FindAnyChild<Transform>(transform, "Mesh");//法師身體
        if (body != null)
        {
            bool act = active == 1 ? act = true : act = false;
            body.gameObject.SetActive(act);
        }
    }

    /// <summary>
    /// 受到治療
    /// </summary>
    /// <param name="attacker">治療者者物件</param>
    /// <param name="layer">治療者layer</param>
    /// <param name="heal">回復量(%)</param>
    /// <param name="isCritical">是否爆擊</param>
    public void OnGetHeal(GameObject attacker, string layer, float heal, bool isCritical)
    {
        //判斷受擊對象
        if (gameObject.layer == LayerMask.NameToLayer("Player") && layer == "Player" ||
            gameObject.layer == LayerMask.NameToLayer("Enemy") && layer == "Enemy")
        {
            Hp += MaxHp * (heal / 100);//回復生命值
            if (Hp >= MaxHp) Hp = MaxHp;

            if (lifeBar != null) lifeBar.SetValue = Hp / MaxHp;//設定生命條比例(頭頂)
            if (gameObject.layer == LayerMask.NameToLayer("Player")) GameSceneUI.Instance.SetPlayerHpProportion = Hp / MaxHp;//設定玩家生命條比例(玩家的)

            //產生文字            
            HitNumber hitNumber = Instantiate(Resources.Load<GameObject>(GameDataManagement.Instance.loadPath.hitNumber)).GetComponent<HitNumber>();
            hitNumber.OnSetValue(target: transform,//治療目標
                                 damage: MaxHp * (heal / 100),//受到治療
                                 color: isCritical ? Color.yellow : Color.green,//文字顏色
                                 isCritical: isCritical);//是否爆擊

            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendGetHeal(photonView.ViewID, heal, isCritical);
        }
    }

    /// <summary>
    /// 連線他人物件治療
    /// </summary>
    /// <param name="position">位置</param>
    /// <param name="rotation">選轉</param>
    /// <param name="damage">受到傷害</param>
    /// <param name="isCritical">是否爆擊</param>
    public void OnConnectOtherGetHeal(float heal, bool isCritical)
    {
        Hp += MaxHp * (heal / 100);//回復生命值
        if (Hp >= MaxHp) Hp = MaxHp;
 
        if (lifeBar != null) lifeBar.SetValue = Hp / MaxHp;//設定生命條比例(頭頂)

        if (gameObject.layer == LayerMask.NameToLayer("Player") && photonView.IsMine) GameSceneUI.Instance.SetPlayerHpProportion = Hp / MaxHp;//設定玩家生命條比例(玩家的)

        //產生文字            
        HitNumber hitNumber = Instantiate(Resources.Load<GameObject>(GameDataManagement.Instance.loadPath.hitNumber)).GetComponent<HitNumber>();
        hitNumber.OnSetValue(target: transform,//治療目標
                             damage: MaxHp * (heal / 100),//受到治療
                             color: isCritical ? Color.yellow : Color.green,//文字顏色
                             isCritical: isCritical);//是否爆擊
     
    }

    /// <summary>
    /// 受到攻擊
    /// </summary>
    /// <param name="attacker">攻擊者物件</param>
    /// <param name="layer">攻擊者layer</param>
    /// <param name="damage">造成傷害</param>
    /// <param name="animationName">播放動畫名稱</param>
    /// <param name="knockDirection">擊中效果(0:擊退, 1:擊飛)</param>
    /// <param name="repel">擊退距離</param>
    /// <param name="isCritical">是否爆擊</param>
    public void OnGetHit(GameObject attacker, string layer, float damage, string animationName, int knockDirection, float repel, bool isCritical)
    {       
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        
        //閃躲
        if (info.IsName("Dodge") || info.IsName("Die")) return;

        //判斷受擊對象
        if (gameObject.layer == LayerMask.NameToLayer("Player") && layer == "Enemy" ||
            gameObject.layer == LayerMask.NameToLayer("Enemy") && layer == "Player")
        {
            Hp -= damage;//生命值減少
            if (Hp <= 0) Hp = 0;

            if(lifeBar != null) lifeBar.SetValue = Hp / MaxHp;//設定生命條比例(頭頂)            
            if (gameObject.layer == LayerMask.NameToLayer("Player")) GameSceneUI.Instance.SetPlayerHpProportion = Hp / MaxHp;//設定玩家生命條比例(玩家的)

            //面向攻擊者          
            Vector3 attackerPosition = attacker.transform.position;//攻擊者向量
            attackerPosition.y = 0;
            Vector3 targetPosition = transform.position;//受擊者向量
            targetPosition.y = 0;
            transform.forward = attackerPosition - targetPosition;

            //產生文字            
            HitNumber hitNumber = Instantiate(Resources.Load<GameObject>(GameDataManagement.Instance.loadPath.hitNumber)).GetComponent<HitNumber>();
            hitNumber.OnSetValue(target: transform,//受傷目標
                                 damage: damage,//受到傷害
                                 color: isCritical ? Color.yellow : Color.red,//文字顏色
                                 isCritical: isCritical);//是否爆擊

            //命中特效
            if (gameObject.layer == LayerMask.NameToLayer( "Enemy") && attacker.GetComponent<Effects>().effects.transform.GetChild(0).name.Equals("1_Warrior-NA_1"))
            {
                attacker.GetComponent<Effects>().HitEffect(attacker, gameObject.GetComponent<Collider>());
            }

            //判斷擊中效果
            switch (knockDirection)
            {
                case 0://擊退
                    transform.position = transform.position + attacker.transform.forward * repel * Time.deltaTime;//擊退
                    break;
                case 1://擊飛
                    floating_List.Add(new CharactersFloating { target = transform, force = repel, gravity = NumericalValue.gravity });//浮空List
                    break;
            }

            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendGetHit(photonView.ViewID, transform.position, transform.rotation, damage, isCritical);

            //死亡
            if (Hp <= 0)
            {                
                isDie = true;
                animator.SetTrigger("Die");
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Die", "Die");
                return;
            }

            //重複觸發動畫
            if (info.IsTag(animationName))
            {
                StartCoroutine(OnAniamtionRepeatTrigger(animationName));
                return;
            }

            //狀態改變(關閉前一個動畫)
            if (info.IsTag("KnockBack") && animationName == "Pain")
            {
                animator.SetBool("KnockBack", false);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "KnockBack", false);
            }
            if (info.IsTag("Pain") && animationName == "KnockBack")
            {
                animator.SetBool("Pain", false);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Pain", false);
            }

            //待機 & 奔跑 才執行受擊動畫
            if (info.IsName("Idle") || info.IsName("Run"))
            {
                animator.SetBool(animationName, true);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, animationName, true);
            }            
        }        
    }

    /// <summary>
    /// 連線他人物件受擊
    /// </summary>
    /// <param name="position">位置</param>
    /// <param name="rotation">選轉</param>
    /// <param name="damage">受到傷害</param>
    /// <param name="isCritical">是否爆擊</param>
    public void OnConnectOtherGetHit(Vector3 position, Quaternion rotation, float damage, bool isCritical)
    {
        transform.position = position;
        transform.rotation = rotation;

        Hp -= damage;//生命值減少
        if (lifeBar != null) lifeBar.SetValue = Hp / MaxHp;//設定生命條比例(頭頂)

        //產生文字
        HitNumber hitNumber = Instantiate(Resources.Load<GameObject>(GameDataManagement.Instance.loadPath.hitNumber)).GetComponent<HitNumber>();
        hitNumber.OnSetValue(target: transform,//受傷目標
                             damage: damage,//受到傷害
                             color: isCritical ? Color.yellow : Color.red,//文字顏色
                             isCritical: isCritical);//是否爆擊
        
    }

    /// <summary>
    /// 浮空
    /// </summary>
    void OnFloation()
    {
        //浮空/跳躍
        for (int i = 0; i < floating_List.Count; i++)
        {
            floating_List[i].OnFloating();
        }

        //碰撞偵測
        LayerMask mask = LayerMask.GetMask("StageObject");
        if (Physics.CheckBox(transform.position + boxCenter, new Vector3(boxSize.x / 4, boxSize.y / 2, boxSize.z / 4), Quaternion.identity, mask))
        {
            floating_List.Clear();//清除List
        }
    }

    /// <summary>
    /// 碰撞控制
    /// </summary>
    void OnCollisionControl()
    {
        //碰撞框
        if (GetComponent<BoxCollider>() != null)
        {
            boxCenter = GetComponent<BoxCollider>().center;
            boxSize = GetComponent<BoxCollider>().size;            
        }
        float boxCollisionDistance = boxSize.x > boxSize.z ? boxSize.x : boxSize.z;

        //射線方向
        Vector3[] rayDiration = new Vector3[] { transform.forward,
                                                transform.forward - transform.right,
                                                transform.right,
                                                transform.right + transform.forward,
                                               -transform.forward,
                                               -transform.forward + transform.right,
                                               -transform.right,
                                               -transform.right -transform.forward };

        //牆壁碰撞
        LayerMask mask = LayerMask.GetMask("StageObject");
        RaycastHit hit;
        for (int i = 0; i < rayDiration.Length; i++)
        {          
            if(Physics.Raycast(transform.position + boxCenter, rayDiration[i], out hit, boxCollisionDistance, mask))
            {
                transform.position = transform.position - rayDiration[i] * (boxCollisionDistance - 0.01f - hit.distance);
            }
        }       

        //地板碰撞
        if (Physics.CheckBox(transform.position + boxCenter, new Vector3(boxSize.x / 2 + 0.05f, boxSize.y / 2, boxSize.z / 2 + 0.05f), transform.rotation, mask))
        {      
            if(Physics.BoxCast(transform.position + Vector3.up * boxSize.y, new Vector3(boxSize.x / 2, 0.1f, boxSize.z / 2), -transform.up, out hit, transform.rotation, boxSize.y, mask))
            {                
                if(hit.distance < boxSize.y) transform.position = transform.position + Vector3.up * (boxSize.y - 0.1f - hit.distance);
            }              
        }       
        else
        {                        
            transform.position = transform.position - Vector3.up * NumericalValue.gravity * Time.deltaTime;//重力
        }      
    }

    /// <summary>
    /// 動畫結束
    /// </summary>
    void OnAnimationOver()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsTag("Pain") && info.normalizedTime >= 1)
        {
            animator.SetBool("Pain", false);
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Pain", false);
        }

        if (info.IsTag("KnockBack") && info.normalizedTime >= 1)
        {
            animator.SetBool("KnockBack", false);
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "KnockBack", false);
        }
        if (info.IsTag("Die") && info.normalizedTime >= 1)
        {            
            //連線模式
            if (GameDataManagement.Instance.isConnect && photonView.IsMine)
            {                
                PhotonConnect.Instance.OnSendObjectActive(gameObject, false);
            }

            //關閉物件
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 動畫重複觸發
    /// </summary>
    /// <param name="aniamtionName">動畫名稱</param>
    /// <returns></returns>
    IEnumerator OnAniamtionRepeatTrigger(string aniamtionName)
    {
        animator.SetBool(aniamtionName, false);
        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, aniamtionName, false);

        yield return new WaitForSeconds(0.03f);

        animator.SetBool(aniamtionName, true);
        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, aniamtionName, true);
    }       
}