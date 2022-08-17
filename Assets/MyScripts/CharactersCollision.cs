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
    AnimatorStateInfo info;
    GameData_NumericalValue NumericalValue;

    //碰撞框
    public Vector3 boxCenter;
    public Vector3 boxSize;
    [Header("距離地面高度")] public float heightFromGround;
    [Header("牆面碰撞距離")] public float wallCollisionDistance;
    [Header("牆面碰撞高度")] public float wallCollisionHight;
    float boxCollisionDistance;//碰撞距離
    Transform[] collisionObject = new Transform[9];//碰撞物件(判定是否有碰撞)    
    public Transform[] GetCollisionObject => collisionObject;
    float jumpRayDistance;//跳躍射線距離

    //生命條
    LifeBar_Characters lifeBar;//生命條

    //數值
    public float Hp;//生命值
    public float MaxHp;//最大生命值
    public float addDefence;//增加防禦值
    public bool isSuckBlood;//是否有吸血效果
    public bool isSelfHeal;//是否有回復效果
    float selfTime;//自我回復時間    

    //判斷
    public bool isDie;//是否死亡
    bool isFall;//是否落下

    public List<CharactersFloating> floating_List = new List<CharactersFloating>();//浮空/跳躍List

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        NumericalValue = GameDataManagement.Instance.numericalValue;

        //碰撞框
        if (GetComponent<BoxCollider>() != null)
        {
            boxCenter = GetComponent<BoxCollider>().center;
            boxSize = GetComponent<BoxCollider>().size;
        }
        boxCollisionDistance = boxSize.x < boxSize.z ? boxSize.x / 2 : boxSize.z / 2;//碰撞距離
//        heightFromGround = -0.063f;//距離地面高度        
        wallCollisionDistance = 0.25f;//牆面碰撞距離
        wallCollisionHight = 0.5f;//牆面碰撞高度

        //數值
        switch (gameObject.tag)
        {
            case "Player":
                MaxHp = NumericalValue.playerHp;
                break;
            case "EnemySoldier_1":
                MaxHp = NumericalValue.enemySoldier1_Hp;
                break;
            case "EnemySoldier_2":
                MaxHp = NumericalValue.enemySoldier1_Hp;
                break;
        }

        //Buff
        for (int i = 0; i < GameDataManagement.Instance.equipBuff.Length; i++)
        {
            //增加最大生命值
            if (GameDataManagement.Instance.equipBuff[i] == 0 && GetComponent<PlayerControl>()) MaxHp += MaxHp * (GameDataManagement.Instance.numericalValue.buffAbleValue[0] / 100);
        }

        //OnSetLifeBar_Character(transform);//設定生命條
        OnInitial();//初始化
    }

    void Update()
    {
        if (lifeBar != null) lifeBar.gameObject.SetActive(gameObject.activeSelf);

        if (!GameDataManagement.Instance.isConnect || photonView.IsMine)
        {                        
            OnSelfHeal();
            OnCollisionControl();
            OnFloation();
            OnAnimationOver();
        }        

        //測試用
        if (Input.GetKeyDown(KeyCode.K)) OnGetHit(gameObject,gameObject, "Enemy", 100, "Pain", 0, 1, false);
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
        Transform body = ExtensionMethods.FindAnyChild<Transform>(transform, "Mesh");//身體物件
        if (body != null)
        {
            bool act = active == 1 ? act = true : act = false;
            body.gameObject.SetActive(act);
        }
    }

    /// <summary>
    /// 自身回復
    /// </summary>
    void OnSelfHeal()
    {
        if (Hp <= 0) return;//死了

        if(isSelfHeal)
        {
            selfTime -= Time.deltaTime;
            if(selfTime <= 0 && Hp < MaxHp)
            {
                selfTime = NumericalValue.playerSelfHealTime;//重製時間

                float heal = MaxHp * (NumericalValue.buffAbleValue[5] / 100);//回復生命值
                Hp += heal;//回復生命值
                if (Hp >= MaxHp) Hp = MaxHp;

                //產生文字            
                HitNumber hitNumber = Instantiate(Resources.Load<GameObject>(GameDataManagement.Instance.loadPath.hitNumber)).GetComponent<HitNumber>();
                hitNumber.OnSetValue(target: transform,//治療目標
                                     damage: heal,//受到治療
                                     color: Color.green,//文字顏色
                                     isCritical: false);//是否爆擊

                //連線
                if (GameDataManagement.Instance.isConnect)
                {
                    PhotonConnect.Instance.OnSendGetHeal(photonView.ViewID, heal, false);

                    //自己
                    if (photonView.IsMine)
                    {
                        if (lifeBar != null) lifeBar.SetValue = Hp / MaxHp;//設定生命條比例(頭頂)
                        if (gameObject.layer == LayerMask.NameToLayer("Player")) GameSceneUI.Instance.SetPlayerHpProportion = Hp / MaxHp;//設定玩家生命條比例(玩家的)
                    }
                }
                else
                {
                    if (lifeBar != null) lifeBar.SetValue = Hp / MaxHp;//設定生命條比例(頭頂)
                    if (gameObject.layer == LayerMask.NameToLayer("Player")) GameSceneUI.Instance.SetPlayerHpProportion = Hp / MaxHp;//設定玩家生命條比例(玩家的)
                }
            }
        }
    }

    /// <summary>
    /// 吸血效果
    /// </summary>
    /// <param name="heal">回復量</param>
    /// <param name="isCritical">是否爆擊</param>
    public void OnSuckBlood(float heal, bool isCritical)
    {
        if (Hp <= 0) return;//死了

        float suckBlood = heal * (NumericalValue.buffAbleValue[4] / 100);//吸血值

        Hp += suckBlood;//回復生命值
        if (Hp >= MaxHp) Hp = MaxHp;

        //產生文字            
        HitNumber hitNumber = Instantiate(Resources.Load<GameObject>(GameDataManagement.Instance.loadPath.hitNumber)).GetComponent<HitNumber>();
        hitNumber.OnSetValue(target: transform,//治療目標
                             damage: suckBlood,//受到治療
                             color: isCritical ? Color.blue : Color.green,//文字顏色
                             isCritical: isCritical);//是否爆擊

        //連線
        if (GameDataManagement.Instance.isConnect)
        {
            PhotonConnect.Instance.OnSendGetHeal(photonView.ViewID, heal, isCritical);

            //自己
            if (photonView.IsMine)
            {
                if (lifeBar != null) lifeBar.SetValue = Hp / MaxHp;//設定生命條比例(頭頂)
                if (gameObject.layer == LayerMask.NameToLayer("Player")) GameSceneUI.Instance.SetPlayerHpProportion = Hp / MaxHp;//設定玩家生命條比例(玩家的)
            }
        }
        else
        {
            if (lifeBar != null) lifeBar.SetValue = Hp / MaxHp;//設定生命條比例(頭頂)
            if (gameObject.layer == LayerMask.NameToLayer("Player")) GameSceneUI.Instance.SetPlayerHpProportion = Hp / MaxHp;//設定玩家生命條比例(玩家的)
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

            //產生文字            
            HitNumber hitNumber = Instantiate(Resources.Load<GameObject>(GameDataManagement.Instance.loadPath.hitNumber)).GetComponent<HitNumber>();
            hitNumber.OnSetValue(target: transform,//治療目標
                                 damage: MaxHp * (heal / 100),//受到治療
                                 color: isCritical ? Color.blue : Color.green,//文字顏色
                                 isCritical: isCritical);//是否爆擊

            //連線
            if (GameDataManagement.Instance.isConnect)
            {
                PhotonConnect.Instance.OnSendGetHeal(photonView.ViewID, heal, isCritical);

                //自己
                if (photonView.IsMine)
                {
                    if (lifeBar != null) lifeBar.SetValue = Hp / MaxHp;//設定生命條比例(頭頂)
                    if (gameObject.layer == LayerMask.NameToLayer("Player")) GameSceneUI.Instance.SetPlayerHpProportion = Hp / MaxHp;//設定玩家生命條比例(玩家的)
                }
            }
            else
            {
                if (lifeBar != null) lifeBar.SetValue = Hp / MaxHp;//設定生命條比例(頭頂)
                if (gameObject.layer == LayerMask.NameToLayer("Player")) GameSceneUI.Instance.SetPlayerHpProportion = Hp / MaxHp;//設定玩家生命條比例(玩家的)
            }
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

        //產生文字            
        HitNumber hitNumber = Instantiate(Resources.Load<GameObject>(GameDataManagement.Instance.loadPath.hitNumber)).GetComponent<HitNumber>();
        hitNumber.OnSetValue(target: transform,//治療目標
                             damage: MaxHp * (heal / 100),//受到治療
                             color: isCritical ? Color.yellow : Color.green,//文字顏色
                             isCritical: isCritical);//是否爆擊
        Debug.LogError(Hp + ":" + transform.name);
        if (lifeBar != null) lifeBar.SetValue = Hp / MaxHp;//設定生命條比例(頭頂)
        if (gameObject.layer == LayerMask.NameToLayer("Player") && photonView.IsMine) GameSceneUI.Instance.SetPlayerHpProportion = Hp / MaxHp;//設定玩家生命條比例(玩家的)
    }

    /// <summary>
    /// 受到攻擊
    /// </summary>
    /// <param name="attacker">攻擊者</param>
    /// <param name="attackerObject">攻擊者物件</param>
    /// <param name="layer">攻擊者layer</param>
    /// <param name="damage">造成傷害</param>
    /// <param name="animationName">播放動畫名稱</param>
    /// <param name="knockDirection">擊中效果(0:擊退, 1:擊飛)</param>
    /// <param name="repel">擊退距離</param>
    /// <param name="isCritical">是否爆擊</param>
    public void OnGetHit(GameObject attacker, GameObject attackerObject, string layer, float damage, string animationName, int knockDirection, float repel, bool isCritical)
    {        
        info = animator.GetCurrentAnimatorStateInfo(0);

        //閃躲
        if (info.IsName("Dodge") || info.IsName("Die")) return;
        
        //判斷受擊對象
        if (gameObject.layer == LayerMask.NameToLayer("Player") && layer == "Enemy" ||
            gameObject.layer == LayerMask.NameToLayer("Enemy") && layer == "Player")
        {
            float getDamge = (damage - (damage * (addDefence / 100))) < 0 ? 0: damage - addDefence;//受到的方害

            //判斷攻擊者是否有吸血效果
            if (attacker.TryGetComponent(out CharactersCollision attackerCollision))
            {
                if (attackerCollision.isSuckBlood) attackerCollision.OnSuckBlood(getDamge, isCritical);               
            }

            Hp -= getDamge;//生命值減少
            if (Hp <= 0) Hp = 0;

            if (lifeBar != null) lifeBar.SetValue = Hp / MaxHp;//設定生命條比例(頭頂)            
            if (gameObject.layer == LayerMask.NameToLayer("Player")) GameSceneUI.Instance.SetPlayerHpProportion = Hp / MaxHp;//設定玩家生命條比例(玩家的)

            //面向攻擊者          
            Vector3 attackerPosition = attackerObject.transform.position;//攻擊者向量
            attackerPosition.y = 0;
            Vector3 targetPosition = transform.position;//受擊者向量
            targetPosition.y = 0;
            transform.forward = attackerPosition - targetPosition;

            //產生文字            
            HitNumber hitNumber = Instantiate(Resources.Load<GameObject>(GameDataManagement.Instance.loadPath.hitNumber)).GetComponent<HitNumber>();
            hitNumber.OnSetValue(target: transform,//受傷目標
                                 damage: getDamge,//受到傷害
                                 color: isCritical ? Color.yellow : Color.red,//文字顏色
                                 isCritical: isCritical);//是否爆擊

            //命中特效
            if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (attackerObject.GetComponent<Effects>()!=null &&attackerObject.GetComponent<Effects>().effects.transform.GetChild(0).name.Equals("1_Warrior-NA_1"))
                {
                     attackerObject.GetComponent<Effects>().HitEffect(attackerObject, gameObject.GetComponent<Collider>());
                }
            }

            //不是連線 || 房主
            if (!GameDataManagement.Instance.isConnect || photonView.IsMine)
            {
                //判斷擊中效果
                switch (knockDirection)
                {
                    case 0://擊退
                        LayerMask mask = LayerMask.GetMask("StageObject");
                        if (!Physics.Raycast(transform.position + boxCenter, -transform.forward, 1f, mask))//碰牆不再擊退
                        {
                            transform.position = transform.position + attackerObject.transform.forward * repel * Time.deltaTime;//擊退
                        }                        
                        break;
                    case 1://擊飛                    
                        floating_List.Add(new CharactersFloating { target = transform, force = repel, gravity = NumericalValue.gravity });//浮空List                    
                        break;
                }
            }

            //連線
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendGetHit(targetID: photonView.ViewID,
                                                                                           position: transform.position,
                                                                                           rotation: transform.rotation,
                                                                                           damage: getDamge,
                                                                                           isCritical: isCritical,
                                                                                           knockDirection: knockDirection,
                                                                                           repel: repel,
                                                                                           attackerObjectID: attackerObject.GetPhotonView().ViewID);

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
            if (info.IsTag("KnockBack") && animationName == "Pain"||
                info.IsTag("Pain") && animationName == "KnockBack")
            {
                animator.SetBool(animationName, false);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, animationName, false);
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
    /// <param name="knockDirection">擊退方向</param>
    /// <param name="attackObj">攻擊者物件</param>
    public void OnConnectOtherGetHit(Vector3 position, Quaternion rotation, float damage, bool isCritical, int knockDirection, float repel, GameObject attackObj)
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

        //判斷擊中效果
        switch (knockDirection)
        {
            case 0://擊退
                LayerMask mask = LayerMask.GetMask("StageObject");
                if (!Physics.Raycast(transform.position + boxCenter, -transform.forward, 1f, mask))//碰牆不再擊退
                {
                    transform.position = transform.position + attackObj.transform.forward * repel * Time.deltaTime;//擊退
                }
                break;
            case 1://擊飛                    
                floating_List.Add(new CharactersFloating { target = transform, force = repel, gravity = NumericalValue.gravity });//浮空List                    
                break;
        }
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

            LayerMask mask = LayerMask.GetMask("StageObject");
            RaycastHit hit;
            //地板碰撞
            if(OnJumpCollision_Floor(mask, out hit, jumpRayDistance))
            {
                if (floating_List[i].force < NumericalValue.playerJumpForce / 1.35f)
                {
                    floating_List.Clear();//清除浮空效果                  
                }
            }   
        }    
    }

    /// <summary>
    /// 重力
    /// </summary>
    void OnGravity()
    {        
        transform.position = transform.position + NumericalValue.gravity * Time.deltaTime * -Vector3.up;//重力    
    }

    /// <summary>
    /// 碰撞控制
    /// </summary>
    void OnCollisionControl()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);        
        LayerMask mask = LayerMask.GetMask("StageObject");
        RaycastHit hit;

        //射線方向
        Vector3[] rayDiration = new Vector3[] { transform.forward,
                                                transform.forward - transform.right,
                                                transform.right,
                                                transform.right + transform.forward,
                                               -transform.forward,
                                               -transform.forward + transform.right,
                                               -transform.right,
                                               -transform.right -transform.forward };

        float wallHight = boxCenter.y + wallCollisionHight;//牆壁高度多少碰撞
        //牆壁碰撞    
        for (int i = 0; i < rayDiration.Length; i++)
        {
            if (Physics.BoxCast(transform.position + Vector3.up * wallHight, new Vector3(boxCollisionDistance, (boxSize.y / 2) - wallCollisionHight, boxCollisionDistance), rayDiration[i], out hit, Quaternion.Euler(transform.localEulerAngles), boxCollisionDistance + (wallCollisionDistance / 2), mask))
            {
                transform.position = transform.position - rayDiration[i] * (Mathf.Abs(boxCollisionDistance + (wallCollisionDistance / 2) - hit.distance));

                collisionObject[i] = hit.transform;//紀錄碰撞物件
            }
            else
            {
                collisionObject[i] = null;//紀錄碰撞物件

                //避免失誤塞進物件
                /* if (Physics.CheckBox(transform.position + boxCenter + Vector3.up * wallHight, new Vector3(boxCollisionDistance - 0.06f, boxSize.y - (boxCenter.y + wallHight), boxCollisionDistance - 0.06f), Quaternion.Euler(transform.localEulerAngles), mask))
                 {
                     //transform.position = transform.position - rayDiration[i] * 5 * Time.deltaTime;
                 }*/
            }
        }
        
        //跳躍||擊飛||落下 地面碰撞
        if (info.IsName("Jump") || info.IsName("Pain") || info.IsName("Fall"))
        {          
            jumpRayDistance = info.normalizedTime < 0.3f ? jumpRayDistance = -0.1f : jumpRayDistance = 0;//射線長度            

            if (OnJumpCollision_Floor(mask, out hit, jumpRayDistance))
            {
                floating_List.Clear();//清除浮空效果
                transform.position = transform.position + ((boxSize.y / 2) - 0.1f - hit.distance) * Vector3.up;
            }
            else
            {
                OnGravity();//重力
                OnFallJudge();//落下判斷
            }
        }

        //一般地面碰撞
        if (floating_List.Count == 0)
        {
            if (OnCollision_Floor(mask, out hit))
            {
                transform.position = transform.position + ((boxSize.y / 2) - 0.1f - hit.distance) * Vector3.up;
                
                if (isFall)
                {
                    isFall = false;
                    animator.SetBool("Fall", isFall);
                    if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Fall", isFall);
                }
            }
            else//沒碰到地板
            {
                OnGravity();//重力
                OnFallJudge();//落下判斷
                
            }
        }
    }

    /// <summary>
    /// 碰撞框_地面
    /// </summary>
    /// <param name="mask">LayerMask</param>
    /// <param name="hit">RaycastHit</param>
    /// <returns></returns>
    public bool OnCollision_Floor(LayerMask mask, out RaycastHit hit)
    {
        if (Physics.BoxCast(transform.position + boxCenter + Vector3.up * heightFromGround, new Vector3(boxCollisionDistance - 0.1f, 0.01f, boxCollisionDistance - 0.1f), -transform.up, out hit, Quaternion.Euler(transform.localEulerAngles), boxSize.y / 2, mask))
        {            
            return true;
        }

        return false;
    }

    /// <summary>
    /// 跳躍碰撞框_地面
    /// </summary>
    /// <param name="mask">LayerMask</param>
    /// <param name="hit">RaycastHit</param>
    /// /// <param name="distance">射線長度</param>
    /// <returns></returns>
    public bool OnJumpCollision_Floor(LayerMask mask, out RaycastHit hit, float distance)
    {
        if (Physics.BoxCast(transform.position + boxCenter + Vector3.up * heightFromGround, new Vector3(boxCollisionDistance - 0.1f, 0.01f, boxCollisionDistance - 0.1f), -transform.up, out hit, Quaternion.Euler(transform.localEulerAngles), boxSize.y / 2, mask))
        {            
            return true;
        }

        return false;
    }

    /// <summary>
    /// 落下判斷
    /// </summary>
    void OnFallJudge()
    {        
        if (!isFall)
        {
            if (info.IsName("Jump") || info.IsName("JumpAttack") || info.IsName("Dodge") || info.IsName("Pain"))
            {
                if (info.normalizedTime >= 0.9f)
                {
                    //距離地面距離
                    if (!OnFallDistance())
                    {
                        if (!isFall)
                        {
                            isFall = true;
                            animator.SetBool("Fall", isFall);

                            if (info.IsName("Jump")) animator.SetBool("Jump", false);
                            if (info.IsName("JumpAttack")) animator.SetBool("JumpAttack", false);
                            if (info.IsName("Dodge")) animator.SetBool("Dodge", false);

                            //連線
                            if (GameDataManagement.Instance.isConnect)
                            {
                                PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Fall", isFall);
                                if (info.IsName("Jump")) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Jump", false);
                                if (info.IsName("JumpAttack")) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "JumpAttack", false);
                                if (info.IsName("Dodge")) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Dodge", false);
                            }
                        }
                    }
                }
            }
            else
            {
                //距離地面距離
                if (!OnFallDistance())
                {
                    isFall = true;
                    animator.SetBool("Fall", isFall);
                    if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Fall", isFall);
                }
            }
        }
    }

    /// <summary>
    /// 落下距離
    /// </summary>
    bool OnFallDistance()
    {
        float distance = boxSize.y * 1f;//落下距離
        LayerMask mask = LayerMask.GetMask("StageObject");
        if (Physics.Raycast(transform.position, -transform.up, distance, mask))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 動畫結束
    /// </summary>
    void OnAnimationOver()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

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
            if (GameDataManagement.Instance.isConnect && photonView.IsMine) PhotonConnect.Instance.OnSendObjectActive(gameObject, false);
          
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