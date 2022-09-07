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

    [Header("任務設定")]
    [SerializeField] bool isTaskObject;//是否為任務物件
    [SerializeField] string enemyName;//敵人名稱

    //碰撞框
    public Vector3 boxCenter;
    public Vector3 boxSize;
    [Header("距離地面高度")] public float heightFromGround;
    [Header("牆面碰撞距離")] public float wallCollisionDistance;
    [Header("牆面碰撞高度")] public float wallCollisionHight;
    [Header("碰撞框大小_腳色")] public float collisionSize_Character;//碰撞框大小_腳色    
    [Header("碰撞框最小距離")] public float boxCollisionDistance;//碰撞距離
    public Transform[] collisionObject = new Transform[9];//碰撞物件(判定是否有碰撞)    
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
    public float acceleration;//加速度

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
        //heightFromGround = -0.1f;//距離地面高度        
        wallCollisionDistance = 0.25f;//牆面碰撞距離
        wallCollisionHight = 0.5f;//牆面碰撞高度
        acceleration = 1;//加速度
        collisionSize_Character = 1.5f;//碰撞框大小_腳色
        //collisionPushForce_Character = 0.77f;//碰撞框推力_腳色

        //腳色HP
        switch (gameObject.tag)
        {
            case "Player":
                MaxHp = NumericalValue.playerHp;
                break;
                /*case "Alliance":
                    MaxHp = NumericalValue.allianceSoldier1_Hp;
                    break;
                case "EnemySoldier_1":
                    MaxHp = NumericalValue.enemySoldier1_Hp;
                    break;
                case "EnemySoldier_2":
                    MaxHp = NumericalValue.enemySoldier2_Hp;
                    break;
                case "EnemySoldier_3":
                    MaxHp = NumericalValue.enemySoldier3_Hp;
                    break;
                case "GuardBoss":
                    MaxHp = NumericalValue.guardBoss_Hp;
                    break;*/
        }

        AI ai = GetComponent<AI>();
        if (ai)
        {
            //判斷角色
            switch (ai.role)
            {
                case AI.Role.同盟士兵1://同盟士兵
                    MaxHp = NumericalValue.allianceSoldier1_Hp;
                    break;
                case AI.Role.石頭人://石頭人
                    MaxHp = NumericalValue.enemySoldier1_Hp;
                    break;
                case AI.Role.弓箭手://弓箭手
                    MaxHp = NumericalValue.enemySoldier2_Hp;
                    break;
                case AI.Role.斧頭人://斧頭人
                    MaxHp = NumericalValue.enemySoldier3_Hp;
                    break;
                case AI.Role.小Boss:
                    MaxHp = NumericalValue.guardBoss_Hp;
                    break;
            }
        }

        BossAI bossAI = GetComponent<BossAI>();
        if(bossAI) MaxHp = NumericalValue.boss_Hp;

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
        if (Input.GetKeyDown(KeyCode.K)) OnGetHit(gameObject, gameObject, "Player", 1000, "Pain", 0, 1, false);
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void OnInitial()
    {
        Hp = MaxHp;
        GetComponent<BoxCollider>().enabled = true;//開啟碰撞框

        if (isDie)
        {
            isDie = false;
            animator.SetBool("Pain", false);
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Pain", false);
        }

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

        if (isSelfHeal)
        {
            selfTime -= Time.deltaTime;
            if (selfTime <= 0 && Hp < MaxHp)
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
                             color: isCritical ? Color.green : Color.green,//文字顏色
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
                                 color: isCritical ? Color.green : Color.green,//文字顏色
                                 isCritical: isCritical);//是否爆擊

            
            //連線
            if (GameDataManagement.Instance.isConnect)
            {
                if (gameObject.layer == LayerMask.NameToLayer("Player") && gameObject.GetComponent<PlayerControl>().enabled)
                {
                    PhotonConnect.Instance.OnSendOtherPlayerLifeBar(PhotonNetwork.NickName, Hp / MaxHp);
                }

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
                             color: isCritical ? Color.green : Color.green,//文字顏色
                             isCritical: isCritical);//是否爆擊

        if (gameObject.layer == LayerMask.NameToLayer("Player") && gameObject.GetComponent<PlayerControl>().enabled)
        {                     
            PhotonConnect.Instance.OnSendOtherPlayerLifeBar(PhotonNetwork.NickName, Hp / MaxHp);
        }

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
        if ((((gameObject.layer == LayerMask.NameToLayer("Player") || gameObject.layer == LayerMask.NameToLayer("Alliance")) && (layer == "Enemy" || layer == "Boss"))) ||
            (((gameObject.layer == LayerMask.NameToLayer("Enemy") || gameObject.layer == LayerMask.NameToLayer("Boss")) && (layer == "Player" || layer == "Alliance"))))
        {
            float getDamge = (damage - (damage * (addDefence / 100))) < 0 ? 0 : (damage - (damage * (addDefence / 100)));//受到的方害
            
            //判斷攻擊者是否有吸血效果
            if (attacker.TryGetComponent(out CharactersCollision attackerCollision))
            {
                if (attackerCollision.isSuckBlood) attackerCollision.OnSuckBlood(getDamge, isCritical);
            }

            Hp -= getDamge;//生命值減少
            if (Hp <= 0) Hp = 0;

            if (lifeBar != null) lifeBar.SetValue = Hp / MaxHp;//設定生命條比例(頭頂)            
            if (gameObject.layer == LayerMask.NameToLayer("Player") && gameObject.GetComponent<PlayerControl>().enabled)
            {                
                GameSceneUI.Instance.SetPlayerHpProportion = Hp / MaxHp;//設定玩家生命條比例(玩家的)
                if(GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendOtherPlayerLifeBar(PhotonNetwork.NickName, Hp / MaxHp);
            }

            //累積傷害
            if(layer == "Player")
            {
                GameSceneUI.Instance.accumulationDamage += getDamge;
            }
            

            /*//面向攻擊者(Enemy執行)
            if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Vector3 attackerPosition = attackerObject.transform.position;//攻擊者向量
                attackerPosition.y = 0;
                Vector3 targetPosition = transform.position;//受擊者向量
                targetPosition.y = 0;
                transform.forward = attackerPosition - targetPosition;
            }*/

            //產生文字            
            HitNumber hitNumber = Instantiate(Resources.Load<GameObject>(GameDataManagement.Instance.loadPath.hitNumber)).GetComponent<HitNumber>();
            hitNumber.OnSetValue(target: transform,//受傷目標
                                 damage: getDamge,//受到傷害
                                 color: isCritical ? Color.yellow : Color.red,//文字顏色
                                 isCritical: isCritical);//是否爆擊

            //命中特效
            if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (attackerObject.GetComponent<Effects>() != null)
                {
                    attackerObject.GetComponent<Effects>().HitEffect(attackerObject, gameObject.GetComponent<Collider>());
                }
            }
            if (gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (attackerObject.GetComponent<EffectsEnemyHit>() != null)
                {
                    attackerObject.GetComponent<EffectsEnemyHit>().HitEffect(attackerObject, gameObject.GetComponent<Collider>());
                }
            }

            //設定連擊數
            if ((gameObject.layer == LayerMask.NameToLayer("Enemy")|| gameObject.layer == LayerMask.NameToLayer("Boss")) && attacker.GetComponent<PlayerControl>()) GameSceneUI.Instance.OnSetComboNumber();


            //不是連線 || 房主
            if (!GameDataManagement.Instance.isConnect || photonView.IsMine)
            {
                //Boss不擊退
                if (gameObject.layer != LayerMask.NameToLayer("Boss"))
                {                    
                    //判斷擊中效果
                    switch (knockDirection)
                    {
                        case 0://擊退
                            LayerMask mask = LayerMask.GetMask("StageObject");
                            if (!Physics.Raycast(transform.position + boxCenter, -transform.forward, 1f, mask))//碰牆不再擊退
                            {
                                int dir = 1;//擊退方向
                                            //在攻擊者後方
                                if (Vector3.Dot(attackerObject.transform.forward, transform.position - attackerObject.transform.position) <= 0)
                                {
                                    dir = -1;
                                }
                                else//在攻擊者前方
                                {
                                    dir = 1;
                                }

                                transform.position = transform.position + dir * attackerObject.transform.forward * repel * Time.deltaTime;//擊退
                            }
                            break;
                        case 1://擊飛                        
                            floating_List.Add(new CharactersFloating { target = transform, force = repel, gravity = NumericalValue.gravity });//浮空List                    
                            break;
                    }
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


           /* //Boss
            if (gameObject.layer == LayerMask.NameToLayer("Boss"))
            {
                gameObject.GetComponent<BossAI>().OnSetPlayDamage(attacker, getDamge);//紀錄玩家累積傷害
            }*/

            //任務物件
            if (isTaskObject)
            {
                //設定生命條
                GameSceneUI.Instance.OnSetEnemyLifeBarValue(enemyName, Hp / MaxHp);
                GameSceneUI.Instance.SetEnemyLifeBarActive = true;
            }

            //死亡
            if (Hp <= 0)
            {
                //設定擊殺數
                if ((gameObject.layer == LayerMask.NameToLayer("Enemy") || gameObject.layer == LayerMask.NameToLayer("Boss")) && layer == "Player") GameSceneUI.Instance.OnSetKillNumber();

                isDie = true;
                animator.SetTrigger("Die");
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Die", "Die");

                GetComponent<BoxCollider>().enabled = false;//關閉碰撞框               

                //任務物件
                if (isTaskObject)
                {
                    if (GameSceneManagement.Instance.taskStage == 1)//第2階段
                    {
                        GameSceneUI.Instance.OnSetTip($"{enemyName}已擊倒", 5);//設定提示文字
                    }

                    GameSceneManagement.Instance.OnTaskText();//任務文字

                    //連線
                    if (GameDataManagement.Instance.isConnect)
                    {
                        PhotonConnect.Instance.OnSendRenewTask(enemyName);//更新任務
                    }
                    
                    GameSceneUI.Instance.SetEnemyLifeBarActive = false;//關閉生命條        
                }

                //非連線 && 玩家死亡
                if(!GameDataManagement.Instance.isConnect && gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    //遊戲結果文字
                    GameSceneUI.Instance.OnSetGameResult(true, "失 敗");
                    //設定遊戲結束
                    StartCoroutine(GameSceneManagement.Instance.OnSetGameOver(false));
                }
                return;
            }            
            
            //判斷動畫是否mirror
            int isMirror = UnityEngine.Random.Range(0, 2);
            if (isMirror == 0)
            {
                animator.SetBool("IsPainMirror", true);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "IsPainMirror", true);
            }
            else
            {
                animator.SetBool("IsPainMirror", false);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "IsPainMirror", false);
            }

            //重複觸發動畫
            if (info.IsTag(animationName))
            {
                StartCoroutine(OnAniamtionRepeatTrigger(animationName));
                return;
            }

            //狀態改變(關閉前一個動畫)
            if (info.IsTag("KnockBack") && animationName == "Pain" ||
                info.IsTag("Pain") && animationName == "KnockBack")
            {
                animator.SetBool(animationName, false);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, animationName, false);
            }

            //待機 & 奔跑 才執行受擊動畫
            if (info.IsTag("Idle") || info.IsTag("Run"))
            {
                //玩家跳躍狀態
                if (gameObject.layer == LayerMask.NameToLayer("Player") && GetComponent<PlayerControl>().isJump)
                {

                }
                else
                {
                    animator.SetBool(animationName, true);
                    if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, animationName, true);
                }
            }

            //不是連線 || 是房主
            if (!GameDataManagement.Instance.isConnect || PhotonNetwork.IsMasterClient)
            {
                //敵人觸發
                if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    AI ai = GetComponent<AI>();
                    if (ai != null) ai.OnGetHit();
                }
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

        //任務物件
        if (isTaskObject)
        {
            //設定生命條
            GameSceneUI.Instance.OnSetEnemyLifeBarValue(enemyName, Hp / MaxHp);
            GameSceneUI.Instance.SetEnemyLifeBarActive = true;
        }

        if (Hp <= 0)
        {
            GetComponent<BoxCollider>().enabled = false;//關閉碰撞框
        }

        if (gameObject.layer == LayerMask.NameToLayer("Player") && gameObject.GetComponent<PlayerControl>().enabled)
        {                       
            GameSceneUI.Instance.SetPlayerHpProportion = Hp / MaxHp;//設定玩家生命條比例(玩家的)                       
            PhotonConnect.Instance.OnSendOtherPlayerLifeBar(PhotonNetwork.NickName, Hp / MaxHp);
        }

        if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //產生文字
            HitNumber hitNumber = Instantiate(Resources.Load<GameObject>(GameDataManagement.Instance.loadPath.hitNumber)).GetComponent<HitNumber>();
            hitNumber.OnSetValue(target: transform,//受傷目標
                                 damage: damage,//受到傷害
                                 color: isCritical ? Color.yellow : Color.red,//文字顏色
                                 isCritical: isCritical);//是否爆擊
        }

        if (gameObject.layer != LayerMask.NameToLayer("Boss"))
        {
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

        //不是連線 || 是房主
        if (!GameDataManagement.Instance.isConnect || PhotonNetwork.IsMasterClient)
        {
            //敵人觸發
            if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                AI ai = GetComponent<AI>();
                if (ai != null) ai.OnGetHit();
            }
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

            RaycastHit hit;
            //地板碰撞
            if (OnJumpCollision_Floor(out hit, jumpRayDistance))
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
        acceleration += 0.9f * Time.deltaTime;//加速度        
        transform.position = transform.position + NumericalValue.gravity * acceleration * Time.deltaTime * -Vector3.up;//重力加速度    
    }

    /// <summary>
    /// 碰撞控制
    /// </summary>
    void OnCollisionControl()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);
        RaycastHit hit;

        OnCollision_Wall();//碰撞框_牆面        

        //跳躍||落下 地面碰撞
        if (info.IsName("Jump") || info.IsName("Fall"))
        {
            jumpRayDistance = info.normalizedTime < 0.3f ? jumpRayDistance = -0.1f : jumpRayDistance = 0;//射線長度            

            if (OnJumpCollision_Floor(out hit, jumpRayDistance))
            {
                transform.position = transform.position + ((boxSize.y / 2) - 0.1f - hit.distance) * Vector3.up;
            }
        }

        //一般地面碰撞
        if (floating_List.Count == 0)
        {
            if (OnCollision_Floor(out hit))
            {
                transform.position = transform.position + ((boxSize.y / 2) - 0.1f - hit.distance) * Vector3.up;
                acceleration = 1;//加速度

                if (isFall)
                {
                    isFall = false;
                    animator.SetBool("Fall", isFall);
                    if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Fall", isFall);
                }
            }
            else//沒碰到地板
            {
                OnGravity();//重力
                OnFallJudge();//落下判斷

            }
        }
        else//浮空狀態
        {
            OnGravity();//重力
            OnFallJudge();//落下判斷
        }
    }

    /// <summary>
    /// 碰撞框_Boss
    /// </summary>
    public void OnCollision_Boss()
    {
        LayerMask mask = LayerMask.GetMask("Boss");
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
            if (Physics.BoxCast(transform.position + Vector3.up * wallHight, new Vector3(boxCollisionDistance, (boxSize.y / 2), boxCollisionDistance), rayDiration[i], out hit, Quaternion.Euler(transform.localEulerAngles), boxCollisionDistance, mask))
            {
                transform.position = transform.position - rayDiration[i] * (Mathf.Abs(boxCollisionDistance - hit.distance));
            }
        }
    }

    /// <summary>
    /// 碰撞框_腳色
    /// </summary>    
    /// <returns></returns>
    public bool OnCollision_Characters(out RaycastHit hit)
    {
        LayerMask mask = LayerMask.GetMask("Enemy");
        hit = default;

        //射線方向
        Vector3[] rayDiration = new Vector3[] { transform.forward,
                                                transform.forward - transform.right,
                                                transform.right,
                                                transform.forward + transform.right,
                                                -transform.right };

        //腳色碰撞    
        for (int i = 0; i < rayDiration.Length; i++)
        {
            if (Physics.BoxCast(transform.position + boxCenter, new Vector3(boxCollisionDistance * collisionSize_Character, boxSize.y, boxCollisionDistance * collisionSize_Character), rayDiration[i], out hit, Quaternion.Euler(transform.localEulerAngles), boxCollisionDistance * collisionSize_Character, mask))
            {
                //推擠方向
                Vector3 dir = Vector3.Dot(transform.forward, Vector3.Cross(hit.transform.position - transform.position, Vector3.up)) > 0 ? transform.right : -transform.right;
                hit.transform.position = hit.transform.position + dir * (Mathf.Abs(boxCollisionDistance * collisionSize_Character - hit.distance));
                //hit.transform.position = hit.transform.position + dir * 4.3f * Time.deltaTime;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 碰撞框_牆面
    /// </summary>    
    /// <returns></returns>
    public bool OnCollision_Wall()
    {
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

                return true;
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

        return false;
    }

    /// <summary>
    /// 碰撞框_地面
    /// </summary>
    /// <param name="hit">RaycastHit</param>
    /// <returns></returns>
    public bool OnCollision_Floor(out RaycastHit hit)
    {
        LayerMask mask = LayerMask.GetMask("StageObject");      
        if (Physics.BoxCast(transform.position + boxCenter + Vector3.up * heightFromGround, new Vector3(boxCollisionDistance - 0.1f, 0.01f, boxCollisionDistance - 0.1f), -transform.up, out hit, Quaternion.Euler(transform.localEulerAngles), boxSize.y / 2, mask))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 跳躍碰撞框_地面
    /// </summary>
    /// <param name="hit">RaycastHit</param>
    /// /// <param name="distance">射線長度</param>
    /// <returns></returns>
    public bool OnJumpCollision_Floor(out RaycastHit hit, float distance)
    {
        LayerMask mask = LayerMask.GetMask("StageObject");
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
            if (floating_List.Count == 0)//沒有浮空
            {
                if (info.IsName("Dodge"))
                {
                    if (info.normalizedTime > 0.75f)
                    {
                        if (OnFallDistance()) OnFallBehavior();
                    }
                }
                else
                {
                    if (OnFallDistance()) OnFallBehavior();
                }
            }
            else//浮空狀態
            {
                if (info.IsName("Jump") || info.IsName("JumpAttack") || info.IsName("Pain"))
                {
                    if (info.normalizedTime > 1)
                    {
                        if (OnFallDistance()) OnFallBehavior();
                    }
                }
            }
        }
    }

    /// <summary>
    /// 落下距離
    /// </summary>
    bool OnFallDistance()
    {
        float distance = boxSize.y * 1.2f;//落下距離
        LayerMask mask = LayerMask.GetMask("StageObject");
        if (Physics.Raycast(transform.position + boxCenter, -transform.up, distance, mask))
        {
            return false;
        }
                
        return true;
    }

    /// <summary>
    /// 落下行為
    /// </summary>
    void OnFallBehavior()
    {
        //Boss不播放動畫
        if (gameObject.layer == LayerMask.NameToLayer("Boss")) return;

        isFall = true;
        animator.SetBool("Fall", isFall);
        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Fall", isFall);

        //玩家執行
        if (gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (info.IsName("Jump")) animator.SetBool("Jump", false);
            if (info.IsName("JumpAttack")) animator.SetBool("JumpAttack", false);
            if (info.IsName("Dodge")) animator.SetBool("Dodge", false);
            if (info.IsName("Pain")) animator.SetBool("Pain", false);
            if (GameDataManagement.Instance.isConnect)
            {
                PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Jump", isFall);
                PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "JumpAttack", isFall);
                PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Dodge", isFall);
                PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Pain", isFall);
            }
        }
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
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Pain", false);
        }

        if (info.IsTag("KnockBack") && info.normalizedTime >= 1)
        {
            animator.SetBool("KnockBack", false);
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "KnockBack", false);
        }
        if (info.IsTag("Die") && info.normalizedTime >= 1 && gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            //OnJudgeGameResult();//判定遊戲結果

            //連線模式
            if (GameDataManagement.Instance.isConnect && photonView.IsMine) PhotonConnect.Instance.OnSendObjectActive(gameObject, false);

            //關閉物件
            gameObject.SetActive(false);
        }
    }

    /*/// <summary>
    /// 判定遊戲結果
    /// </summary>
    void OnJudgeGameResult()
    {
        //玩家執行
        if(gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //連線
            if(GameDataManagement.Instance.isConnect)
            {

            }
            else//單人模式
            {
                //設定遊戲結束UI
                GameSceneUI.Instance.OnSetGameOverUI(clearance: false);
            }
        }
    }*/

    /// <summary>
    /// 動畫重複觸發
    /// </summary>
    /// <param name="aniamtionName">動畫名稱</param>
    /// <returns></returns>
    IEnumerator OnAniamtionRepeatTrigger(string aniamtionName)
    {
        animator.SetBool(aniamtionName, false);
        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, aniamtionName, false);

        yield return new WaitForSeconds(0.05f);

        animator.SetBool(aniamtionName, true);
        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, aniamtionName, true);
    }

   /* public float X;
    public float Y;
    public float Z;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + boxCenter, new Vector3(boxSize.x * X, boxSize.y * Y, boxSize.z * Z));
    }*/
}