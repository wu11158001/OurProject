using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviourPunCallbacks
{
    Animator animator;
    AnimatorStateInfo info;
    CharactersCollision charactersCollision;
    readonly AStart aStart = new AStart();

    LayerMask mask;//攻擊對象Layer

    [Header("腳色")]
    public Role role;
    public enum Role 
    {
        同盟士兵1,
        石頭人,
        弓箭手,
        斧頭人,
        小Boss
    }

    [Header("範圍")]
    [SerializeField] public float normalStateMoveRadius;//一般狀態移動範圍
    [SerializeField] public float alertRadius;//警戒範圍
    [SerializeField] public float chaseRadius;//追擊範圍
    [SerializeField] public float attackRadius;//攻擊範圍

    [Header("一般狀態")]
    float[] normalStateMoveTime = new float[2];//一般狀態移動亂數最小與最大值
    float normalStateMoveSpeed;//一般狀態移動速度
    bool isNormalMove;//是否一般狀態已經移動
    Vector3 originalPosition;//初始位置
    Vector3 forwardVector;//移動目標向量    
    float normalStateTime;//一般狀態移動時間(計時器)
    float normalRandomMoveTime;//一般狀態亂數移動時間
    float normalRandomAngle;//一般狀態亂數選轉角度

    [Header("警戒狀態")]
    float alertToChaseTime;//警戒到追擊時間
    float leaveAlertRadiusAlertTime;//離開警戒範圍警戒時間
    float leaveAlertTime;//離開警戒範圍警戒時間(計時器)
    float alertTime;//警戒到追擊時間(計時器)
    float CheckPlayerDistanceTime;//偵測玩家距離時間
    float CheckTargetTime;//偵測玩家時間(計時器)        

    [Header("咆嘯狀態")]
    bool isRotateToPlayer;//是否轉向至玩家
    [SerializeField]bool isHowling;//是否咆嘯

    [Header("追擊狀態")]
    [SerializeField] float chaseSpeed;//追擊速度
    float maxRadiansDelta;//轉向角度
    float[] readyChaseRandomTime;//離開戰鬥後亂數準備追擊時間(亂數最小值, 最大值)    
    float loseSpeed;//減少的速度比例
    float startChaseTime;//離開戰鬥後亂數開始追擊時間(計時器)
    int chaseNumber;//追擊對象編號
    bool isStartChase;//是否開始追擊
    bool isReadChase;//是否準備追擊
    float chaseTurnTime;//追擊轉向時間(計時器)
    int chaseDiretion;//追擊方向(0 = 前方, 1 = 右方, 2 = 左方)       
    float delayAttackRandomTime;//延遲攻擊亂數時間(計時器)
    float[] delayAttackTime;//延遲攻擊時間(亂數最小值, 最大值)    

    [Header("檢查同伴")]
    float changeDiretionTime_Forward;//更換方向時間_前方偵測(計時器)
    float changeDiretionTime_Near;//更換方向時間_左右方偵測(計時器)
    float changeDiretionTime;//更換方向時間 
    float chaseSlowDownSpeed;//追擊減速速度
    bool isPauseChase;//是否暫停追擊
    bool[] isCheckNearCompanion;//檢查附近同伴(0 = 右方有碰撞, 1 = 左方有碰撞)

    [Header("攻擊狀態")]
    [SerializeField] int maxAttackNumber;//可使用攻擊招式
    [SerializeField] float[] attackFrequency;//攻擊頻率(亂數最小值, 最大值)
    [SerializeField] float meleeAttackDistance;//近距離招式攻擊距離    
    int attackNumber;//攻擊招式編號(0 = 不攻擊)            
    float waitAttackTime;//等待攻擊時間(計時器)
    bool isAttackIdle;//是否攻擊待機
    bool isAttacking;//是否攻擊中    
    bool isGetHit;//是否被攻擊(判定"Pain"動畫是否觸發)

    [Header("攻擊待機")]
    [SerializeField] float attackIdleMoveSpeed;//攻擊待機移動速度
    [SerializeField] float backMoveDistance;//距離玩家多近向後走
    bool isAttackIdleMove;//是否攻擊待機移動
    float attackIdleMoveRandomTime;//攻擊待機移動亂數時間(計時器)     
    int attackIdleMoveDiretion;//攻擊待機移動方向(0 = 不移動, 1 = 右, 2 = 左)

    [Header("尋路")]
    [SerializeField] bool isExecuteAStart;//是否執行AStart
    List<Vector3> pathsList = new List<Vector3>();//移動路徑節點  
    int point = 0;//尋路節點編號
    int aStarCheckPointNumber;//AStar至少經過多少點

    [Header("所有玩家")]
    [SerializeField] GameObject[] allPlayers;//所有玩家    
    [SerializeField] GameObject[] allPlayerAlliance;//所有玩家同盟士兵
    [SerializeField] GameObject[] allEnemySoldier;//所有敵人士兵
    [SerializeField] GameObject chaseObject;//追擊對象

    [Header("是否為近戰")]
    [SerializeField] bool isMelee;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        //連線 && 不是自己的
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            //GameSceneManagement.Instance.OnSetMiniMapPoint(transform, GameSceneManagement.Instance.loadPath.miniMapMatirial_Enemy);//設定小地圖點點
            this.enabled = false;
            return;
        }
    }
    void Start()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Enemy")) mask = LayerMask.GetMask("Player", "Alliance");//攻擊對象Layer
        if (gameObject.layer == LayerMask.NameToLayer("Alliance")) mask = LayerMask.GetMask("Enemy");//攻擊對象Layer

        aStart.initial();
        charactersCollision = GetComponent<CharactersCollision>();

        //判斷角色
         switch(role)
         {
             case Role.同盟士兵1://同盟士兵
                 isMelee = true;//近戰腳色

                 //偵測範圍
                 normalStateMoveRadius = 2.5f;//一般狀態移動範圍
                 alertRadius = 12;//警戒範圍
                 chaseRadius = 8;//追擊範圍
                 attackRadius = 3.0f;//攻擊範圍

                 //追擊狀態
                 chaseSpeed = 5.3f;//追擊速度
                 readyChaseRandomTime = new float[] { 0.5f, 3.3f };//離開戰鬥後亂數準備追擊時間(亂數最小值, 最大值)

                 //攻擊狀態
                 attackFrequency = new float[2] { 0.5f, 3.5f };//攻擊頻率(亂數最小值, 最大值)  
                 maxAttackNumber = 3;//可使用攻擊招式
                 meleeAttackDistance = 2.0f;//近距離招式攻擊距離

                 //攻擊待機
                 attackIdleMoveSpeed = 1;//攻擊待機移動速度
                 backMoveDistance = 2.0f;//距離玩家多近向後走
                 break;
             case Role.石頭人://石頭人
                 isMelee = true;//近戰腳色

                 //偵測範圍
                 normalStateMoveRadius = 2.5f;//一般狀態移動範圍
                 alertRadius = 12;//警戒範圍
                 chaseRadius = 8;//追擊範圍
                 attackRadius = 3.0f;//攻擊範圍

                 //追擊狀態
                 chaseSpeed = 5.3f;//追擊速度
                 readyChaseRandomTime = new float[] { 0.5f, 3.3f };//離開戰鬥後亂數準備追擊時間(亂數最小值, 最大值)

                 //攻擊狀態
                 attackFrequency = new float[2] { 0.5f, 3.5f };//攻擊頻率(亂數最小值, 最大值)  
                 maxAttackNumber = 3;//可使用攻擊招式

                 //攻擊待機
                 attackIdleMoveSpeed = 1;//攻擊待機移動速度
                 backMoveDistance = 2.0f;//距離玩家多近向後走
                 meleeAttackDistance = 2.5f;//近距離招式攻擊距離
                 break;
             case Role.弓箭手://弓箭手
                 isMelee = false;//非近戰腳色

                 //偵測範圍
                 normalStateMoveRadius = 2.5f;//一般狀態移動範圍
                 alertRadius = 14.0f;//警戒範圍
                 chaseRadius = 13.0f;//追擊範圍
                 attackRadius = 11.0f;//攻擊範圍

                 //追擊狀態
                 chaseSpeed = 5.3f;//追擊速度
                 readyChaseRandomTime = new float[] { 1.0f, 4.0f };//離開戰鬥後亂數準備追擊時間(亂數最小值, 最大值)

                 //攻擊狀態
                 attackFrequency = new float[2] { 1.0f, 5.0f };//攻擊頻率(亂數最小值, 最大值)  
                 maxAttackNumber = 3;//可使用攻擊招式

                 //攻擊待機
                 attackIdleMoveSpeed = 2;//攻擊待機移動速度
                 backMoveDistance = 5.0f;//距離玩家多近向後走
                 meleeAttackDistance = 2.3f;//近距離招式攻擊距離
                 break;
             case Role.斧頭人://斧頭人
                 isMelee = true;//近戰腳色

                 //偵測範圍
                 normalStateMoveRadius = 2.5f;//一般狀態移動範圍
                 alertRadius = 12;//警戒範圍
                 chaseRadius = 8;//追擊範圍
                 attackRadius = 3.0f;//攻擊範圍

                 //追擊狀態
                 chaseSpeed = 5.3f;//追擊速度
                 readyChaseRandomTime = new float[] { 0.5f, 2.3f };//離開戰鬥後亂數準備追擊時間(亂數最小值, 最大值)

                 //攻擊狀態
                 attackFrequency = new float[2] { 0.5f, 2.75f };//攻擊頻率(亂數最小值, 最大值)  
                 maxAttackNumber = 3;//可使用攻擊招式

                 //攻擊待機
                 attackIdleMoveSpeed = 1;//攻擊待機移動速度
                 backMoveDistance = 2.0f;//距離玩家多近向後走
                 meleeAttackDistance = 2.5f;//近距離招式攻擊距離
                 break;
             case Role.小Boss:
                 isMelee = true;//近戰腳色
                 //偵測範圍
                 normalStateMoveRadius = 2.5f;//一般狀態移動範圍
                 alertRadius = 35;//警戒範圍
                 chaseRadius = 30;//追擊範圍
                 attackRadius = 3.0f;//攻擊範圍

                 //追擊狀態
                 chaseSpeed = 6.3f;//追擊速度
                 readyChaseRandomTime = new float[] { 0.5f, 1.0f };//離開戰鬥後亂數準備追擊時間(亂數最小值, 最大值)

                 //攻擊狀態
                 attackFrequency = new float[2] { 0.5f, 1.0f };//攻擊頻率(亂數最小值, 最大值)  
                 maxAttackNumber = 4;//可使用攻擊招式

                 //攻擊待機
                 attackIdleMoveSpeed = 2;//攻擊待機移動速度
                 backMoveDistance = 2.3f;//距離玩家多近向後走
                 meleeAttackDistance = 2.7f;//近距離招式攻擊距離
                 break;
         }    

        //一般狀態
        originalPosition = transform.position;//初始位置
        forwardVector = transform.forward;//前方向量
        normalStateMoveSpeed = 1;//一般狀態移動速度
        normalStateMoveTime = new float[2] { 1.5f, 3.5f };//一般狀態移動亂數最小與最大值

        //警戒狀態        
        if (GameDataManagement.Instance.isConnect) allPlayers = new GameObject[PhotonNetwork.CurrentRoom.PlayerCount];//所有玩家
        else allPlayers = new GameObject[1];
        CheckPlayerDistanceTime = 2;//偵測玩家距離時間
        alertToChaseTime = 1;//警戒到追擊時間
        leaveAlertRadiusAlertTime = 3;//離開警戒範圍警戒時間
        leaveAlertTime = leaveAlertRadiusAlertTime;//離開警戒範圍警戒時間(計時器)

        //檢查同伴
        isCheckNearCompanion = new bool[2] { false, false};//檢查附近同伴(0 = 右方有碰撞, 1 = 左方有碰撞)

        //追擊狀態
        maxRadiansDelta = 0.065f;//轉向角度        
        changeDiretionTime = 0.5f;//更換方向時間
        delayAttackTime = new float[] { 0.5f, 1};//延遲攻擊時間(亂數最小值, 最大值)
        loseSpeed = 0.45f;//減少的速度比例
        readyChaseRandomTime = new float[] { 0.5f, 2.3f };//離開戰鬥後亂數準備追擊時間(亂數最小值, 最大值)

        //尋路
        aStarCheckPointNumber = 2;//AStar至少經過多少點

        OnGetAllPlayers();//獲取所有玩家
        isHowling = true;
        OnChangeState(state: AIState.追擊狀態, openAnimationName: "Howling", closeAnimationName: "Alert", animationType: true);
    }

    void Update()
    {
        if (!charactersCollision.isDie)
        {
            //OnCollision();//碰撞框

            OnStateBehavior();//狀態行為               
            OnCheckLefrAndRightCompanion();//檢查左右同伴

            OnGetAllPlayers();//獲取所有玩家
            OnCheckClosestPlayer();//檢查最近玩家
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void OnInitial()
    {
        isHowling = true;
        OnChangeState(state: AIState.追擊狀態, openAnimationName: "Howling", closeAnimationName: "Alert", animationType: true);
        isExecuteAStart = false;//是否執行AStart
    }

    /// <summary>
    /// AI狀態
    /// </summary>
    enum AIState
    {
        一般狀態,
        警戒狀態,
        追擊狀態,
        攻擊狀態
    }
    [Header("AI狀態")]
    [SerializeField] AIState aiState = AIState.一般狀態;


   /* /// <summary>
    /// 碰撞框
    /// </summary>    
    /// <returns></returns>
    public void OnCollision()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        if (!info.IsTag("Attack") && !isExecuteAStart)//攻擊&&執行AStart不碰撞
        {
            LayerMask mask = LayerMask.GetMask("Enemy");
            RaycastHit hit;

            //射線方向
            Vector3[] rayDiration = new Vector3[] { transform.forward,
                                                transform.forward - transform.right,
                                                transform.right,
                                                transform.forward + transform.right,
                                                -transform.right };

            float boxSize = 0.4f;//碰撞框大小
                                 //腳色碰撞    
            for (int i = 0; i < rayDiration.Length; i++)
            {
                if (Physics.BoxCast(transform.position + charactersCollision.boxCenter, new Vector3(charactersCollision.boxCollisionDistance * boxSize, charactersCollision.boxSize.y / 2, charactersCollision.boxCollisionDistance * boxSize), rayDiration[i], out hit, Quaternion.Euler(transform.localEulerAngles), charactersCollision.boxCollisionDistance * boxSize, mask))
                {
                    //碰撞
                    transform.position = transform.position - rayDiration[i] * (Mathf.Abs((charactersCollision.boxCollisionDistance * boxSize) - hit.distance));
                }
            }
        }
    }*/
    
    /// <summary>
    /// 狀態行為
    /// </summary>
    void OnStateBehavior()
    {
        switch (aiState)
        {
            case AIState.一般狀態:
                OnNormalStateBehavior();
                OnRotateToTarget();//選轉至玩家方向
                break;
            case AIState.警戒狀態:
                OnAlertStateBehavior();
                OnRotateToTarget();//選轉至玩家方向
                break;
            case AIState.追擊狀態:
                OnChaseBehavior();
                break;
            case AIState.攻擊狀態:
                OnAttackBehavior();                
                break;
        }
    }

    /// <summary>
    /// 一般狀態行為
    /// </summary>
    void OnNormalStateBehavior()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        normalStateTime -= Time.deltaTime;//一般狀態移動時間

        if (normalStateTime <= 0 && !isRotateToPlayer)
        {
            if (!isNormalMove)//是否一般狀態已經移動
            {
                isNormalMove = true;

                normalRandomMoveTime = UnityEngine.Random.Range(2, 4.5f);//一般狀態亂數移動時間

                //選轉
                if ((transform.position - originalPosition).magnitude > normalStateMoveRadius)//離開一般狀態移動範圍
                {                    
                    forwardVector = originalPosition - transform.position;//移動目標向量
                }
                else//一般狀態移動範圍內
                {
                    normalRandomAngle = UnityEngine.Random.Range(0, 360);//一般狀態亂數選轉角度                    
                    forwardVector = Quaternion.AngleAxis(normalRandomAngle, Vector3.up) * forwardVector;//移動目標向量
                }

                //更換動畫
                OnChangeAnimation(animationName: "Walk", animationType: true);
            }

            if (normalRandomMoveTime > 0)//移動
            {
                normalRandomMoveTime -= Time.deltaTime;
                float maxRadiansDelta = 0.03f;//轉向角度

                //檢查前方同伴 && 非執行AStart
                if (OnCheckCompanionBox(diretion: transform.forward) && !isExecuteAStart)
                {                    
                    maxRadiansDelta = 0.065f;//轉向角度                    
                    forwardVector = -transform.forward - transform.right;//轉向方向                    
                }
        
                //移動 && 轉向                
                transform.forward = Vector3.RotateTowards(transform.forward, forwardVector, maxRadiansDelta, maxRadiansDelta);
                transform.position = transform.position + transform.forward * normalStateMoveSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
            }
            else//待機
            {                
                isNormalMove = false;
                normalStateTime = UnityEngine.Random.Range(normalStateMoveTime[0], normalStateMoveTime[1]);//一般狀態亂數移動時間

                //更換動畫
                OnChangeAnimation(animationName: "Walk", animationType: false);
            }
        }
                
        OnAlertRangeCheck();//警戒範圍偵測
    }    

    /// <summary>
    /// 警戒範圍偵測
    /// </summary>
    void OnAlertRangeCheck()
    {
        //更換狀態偵測
        if (OnDetectionRange(radius: alertRadius))
        {
            alertTime += Time.deltaTime;//警戒到追擊時間

            if (aiState != AIState.警戒狀態)
            {
                OnChangeState(state: AIState.警戒狀態, openAnimationName: "Alert", closeAnimationName: "Walk", animationType: true);
            }
            else//警戒狀態中
            {
                //警戒到追擊時間
                if (alertTime >= alertToChaseTime)
                {
                    OnHowlingBehavior();//咆嘯行為
                    OnChangeState(state: AIState.追擊狀態, openAnimationName: "Howling", closeAnimationName: "Alert", animationType: true);
                    OnChangeAnimation(animationName: "Idle", animationType: false);
                }
            }
        }
        else
        {
            if (aiState == AIState.警戒狀態) leaveAlertTime -= Time.deltaTime;//離開警戒範圍警戒時間(計時器)

            if (leaveAlertTime <= 0)
            {
                if (aiState != AIState.一般狀態)
                {
                    leaveAlertTime = leaveAlertRadiusAlertTime;//重製離開警戒範圍警戒時間
                    normalRandomMoveTime = 0;//一般狀態亂數移動時間
                    normalStateTime = UnityEngine.Random.Range(normalStateMoveTime[0], normalStateMoveTime[1]);//一般狀態亂數移動時間

                    OnChangeState(state: AIState.一般狀態, openAnimationName: "Idle", closeAnimationName: "Alert", animationType: true);
                }
            }
        }
    }

    /// <summary>
    /// 警戒狀態行為
    /// </summary>
    void OnAlertStateBehavior()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        OnAlertRangeCheck();//警戒範圍偵測        
        OnChaseRangeCheck();//追擊範圍偵測
        //OnCheckClosestPlayer();//檢查最近玩家        
    }

    /// <summary>
    /// 受到攻擊
    /// </summary>
    public void OnGetHit()
    {
        isGetHit = true;//被攻擊(判定"Pain"動畫是否觸發)

        if (aiState == AIState.追擊狀態 || aiState == AIState.攻擊狀態) return;
        
        //關閉動畫
        if (aiState == AIState.警戒狀態) OnChangeAnimation(animationName: "Alert", animationType: false);
        if (aiState == AIState.一般狀態)
        {
            OnChangeAnimation(animationName: "Walk", animationType: false);
            OnChangeAnimation(animationName: "Idle", animationType: false);
        }

        isHowling = true;//是否咆嘯

        //OnGetAllPlayers();//獲取所有玩家
        OnChangeState(state: AIState.追擊狀態, openAnimationName: "Howling", closeAnimationName: "Alert", animationType: true);

        OnHowlingBehavior();//咆嘯行為
    }

    /// <summary>
    /// 咆嘯行為
    /// </summary>
    void OnHowlingBehavior()
    {
        /*AI[] companions = GameObject.FindObjectsOfType<AI>();        
        foreach (var companion in companions)
        {
            float distance = (transform.position - companion.transform.position).magnitude;
            if(distance <= alertRadius)
            {
                //通知同伴
                companion.OnChangeStateToChase(allPlayers: allPlayers, chaseObject: chaseNumber);
            }
        }*/

        StartCoroutine(OnWaitChase());//等待追擊
    }

    /*/// <summary>
    /// 更換狀態到追擊狀態
    /// </summary>
    /// <param name="allPlayers">所有玩家</param>
    /// <param name="chaseObject">追擊的玩家</param>
    void OnChangeStateToChase(GameObject[] allPlayers, int chaseObject)
    {
        this.allPlayers = allPlayers;//所有玩家
        this.chaseNumber = chaseObject;//追擊的玩家
        StartCoroutine(OnWaitChase());//等待追擊
    }*/

    /// <summary>
    /// 選轉至目標方向
    /// </summary>
    void OnRotateToTarget()
    {
        //選轉至目標方向
        if (isRotateToPlayer)
        {            
            Vector3 targetDiration = chaseObject.transform.position - transform.position;//allPlayers[chaseNumber].transform.position - transform.position;
            transform.forward = Vector3.RotateTowards(transform.forward, targetDiration, maxRadiansDelta, maxRadiansDelta);
        }
    }

    /// <summary>
    /// 等待追擊
    /// </summary>
    /// <returns></returns>
    IEnumerator OnWaitChase()
    {
        isRotateToPlayer = true;//轉向至玩家                
        
        yield return new WaitForSeconds(0.55f);

        isRotateToPlayer = false;

        if (aiState != AIState.追擊狀態 || aiState != AIState.攻擊狀態)
        {
            isHowling = true;//是否咆嘯

            OnChangeState(state: AIState.追擊狀態, openAnimationName: "Howling", closeAnimationName: "Alert", animationType: true);
        }

        yield return 0;
    }

    /// <summary>
    /// 追擊範圍偵測
    /// </summary>
    void OnChaseRangeCheck()
    {
        //更換狀態偵測
        if (OnDetectionRange(radius: chaseRadius))
        {
            if (aiState != AIState.追擊狀態)
            {
                isHowling = true;//是否咆嘯
                OnChangeState(state: AIState.追擊狀態, openAnimationName: "Howling", closeAnimationName: "Alert", animationType: true);
            }
        }
    }   

    /// <summary>
    /// 追擊行為
    /// </summary>
    void OnChaseBehavior()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        if(isHowling && !info.IsName("Howling"))
        {
            OnChangeAnimation(animationName: "Howling", animationType: true);
        }

        //咆嘯完開始追擊
        if (info.IsName("Howling") && info.normalizedTime >= 1)
        {
            //回一般狀態事前設定
            isNormalMove = false;//是否一般狀態已經移動
            normalStateTime = UnityEngine.Random.Range(normalStateMoveTime[0], normalStateMoveTime[1]);//重製一般狀態亂數移動時間
            
            //追擊
            isStartChase = true;//開始追擊                        

            if (chaseObject != null)
            {
                Vector3 targetDiration = chaseObject.transform.position - transform.position; //allPlayers[chaseNumber].transform.position - transform.position;
                transform.forward = transform.forward = Vector3.RotateTowards(transform.forward, targetDiration, maxRadiansDelta, maxRadiansDelta);
            }

            isHowling = false;//是否咆嘯

            OnChangeAnimation(animationName: "Idle", animationType: false);
            OnChangeAnimation(animationName: "Howling", animationType: false);
            OnChangeAnimation(animationName: "Run", animationType: true);
        }

        //開始追擊
        if (isStartChase)
        {
            OnAttackRangeCheck();//攻擊範圍偵測        
            OnCheckExecuteAStart();//偵測是否執行AStart
            //OnCheckClosestPlayer();//檢查最近玩家            
            OnCheckForwardCompanion();//檢查前方同伴
            

            //朝玩家移動
            if (info.IsName("Run")) OnMoveBehavior();

            //檢查最近玩家
            //if (chaseTurnTime <= 0) OnCheckClosestPlayer();

            /*//追擊的玩家死亡 && 追擊範圍內沒有其他玩家
            if (chaseObject.activeSelf == false && OnDetectionRange(radius: chaseRadius) == false)
            {
                if (aiState != AIState.一般狀態)
                {
                    OnChangeState(state: AIState.一般狀態, openAnimationName: "Idle", closeAnimationName: "Run", animationType: true);
                    return;
                }
            }*/
        }
    }

    /// <summary>
    /// 檢查同伴碰撞框
    /// </summary>
    /// <param name="diretion">方向</param>
    bool OnCheckCompanionBox(Vector3 diretion)
    {
        float chechSize = 0.6f;//檢查框大小
        LayerMask mask = LayerMask.GetMask("Enemy", "StageObject");

        //碰到同伴
        if (Physics.CheckBox(transform.position + charactersCollision.boxCenter + diretion * (charactersCollision.boxSize.x + (chechSize / 2)), new Vector3(chechSize * 1.1f, 0.1f, chechSize), Quaternion.Euler(transform.localEulerAngles), mask))
        {            
            return true;            
        }

        return false;        
    }

    /// <summary>
    /// 檢查前方同伴
    /// </summary>
    void OnCheckForwardCompanion()
    {
        //檢查前方同伴 && 非執行AStart
        if (OnCheckCompanionBox(diretion: transform.forward) && !isExecuteAStart)
        {
            chaseSlowDownSpeed -= loseSpeed * Time.deltaTime;//追擊減速速度            
            if (chaseSlowDownSpeed <= 0f)
            {
                chaseSlowDownSpeed = 0f;
                
                if (!isPauseChase)
                {
                    isPauseChase = true;//暫停追擊

                    OnChangeAnimation(animationName: "Run", animationType: false);
                    OnChangeAnimation(animationName: "AttackIdle", animationType: true);
                }
            }

            changeDiretionTime_Forward -= Time.deltaTime;//更換方向時間
            if (changeDiretionTime_Forward <= 0)
            {
                changeDiretionTime_Forward = changeDiretionTime;
                if (!isCheckNearCompanion[1]) chaseDiretion = 2;//左沒有同伴
                if (!isCheckNearCompanion[0] || (!isCheckNearCompanion[0] && !isCheckNearCompanion[1])) chaseDiretion = 1;//右沒有同伴 || 左右都沒有同伴                
            }
        }
        else
        {
            //是否暫停追擊
            if (isPauseChase)
            {
                isPauseChase = false;//暫停追擊

                OnChangeAnimation(animationName: "Run", animationType: true);
                OnChangeAnimation(animationName: "AttackIdle", animationType: false);
            }

            if (chaseSlowDownSpeed < 1)//追擊速度
            {
                chaseSlowDownSpeed += Time.deltaTime;
                if (chaseSlowDownSpeed >= 1) chaseSlowDownSpeed = 1;
            }
        }
    }

    /// <summary>
    /// 檢查左右同伴
    /// </summary>
    void OnCheckLefrAndRightCompanion()
    {
        changeDiretionTime_Near -= Time.deltaTime;

        if (changeDiretionTime_Near <= 0)
        {
            changeDiretionTime_Near = changeDiretionTime;

            //檢查右方同伴 && 非執行AStart
            if (OnCheckCompanionBox(diretion: transform.right) && !isExecuteAStart)//檢查右方同伴
            {
                if (!isCheckNearCompanion[0])//檢查附近同伴(0 = 右方有碰撞, 1 = 左方有碰撞)
                {
                    isCheckNearCompanion[0] = true;
                    chaseDiretion = 2;//追擊方向(0 = 前方, 1 = 右方, 2 = 左方)
                }
            }
            else isCheckNearCompanion[0] = false;//檢查附近同伴(0 = 右方有碰撞, 1 = 左方有碰撞)

            //檢查左方同伴 && 非執行AStart
            if (OnCheckCompanionBox(diretion: -transform.right) && !isExecuteAStart)
            {
                if (!isCheckNearCompanion[1])//檢查附近同伴(0 = 右方有碰撞, 1 = 左方有碰撞)
                {
                    isCheckNearCompanion[1] = true;
                    chaseDiretion = 1;//追擊方向(0 = 前方, 1 = 右方, 2 = 左方)
                }
            }
            else isCheckNearCompanion[1] = false;//檢查附近同伴(0 = 右方有碰撞, 1 = 左方有碰撞)    

            //左右都沒有碰撞同伴
            if (!isCheckNearCompanion[0] && !isCheckNearCompanion[1]) chaseDiretion = 0;//追擊方向(0 = 前方, 1 = 右方, 2 = 左方)
        }
    }

    /// <summary>
    /// 移動行為
    /// </summary>
    void OnMoveBehavior()
    {        
        //追擊方向(0 = 前方, 1 = 右方, 2 = 左方)
        switch (chaseDiretion)
        {
            case 1://向右移動
                transform.position = transform.position + transform.right * (chaseSpeed / 2) * Time.deltaTime;
                break;
            case 2://向左移動
                transform.position = transform.position - transform.right * (chaseSpeed / 2) * Time.deltaTime;
                break;
        }

        //移動(判斷是否有轉彎)
        if (chaseDiretion == 0) transform.position = transform.position + transform.forward * chaseSpeed * chaseSlowDownSpeed * Time.deltaTime;
        else transform.position = transform.position + transform.forward * (chaseSpeed / 2) * chaseSlowDownSpeed * Time.deltaTime;
    }    

    /// <summary>
    /// 攻擊範圍偵測
    /// </summary>
    void OnAttackRangeCheck()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        //更換狀態偵測
        if (OnDetectionRange(radius: attackRadius))
        {
            delayAttackRandomTime -= Time.deltaTime;//延遲攻擊亂數時間(計時器)

            if (isReadChase) isReadChase = false;//是否準備追擊
            
            if (aiState != AIState.攻擊狀態 && delayAttackRandomTime <= 0)
            {
                delayAttackRandomTime = UnityEngine.Random.Range(delayAttackTime[0], delayAttackTime[1]);//延遲攻擊時間
                
                //等待轉向去攻擊
                StartCoroutine(OnWaitTurnToAttack());             
            }
        } 
        else
        {
            isReadChase = true;//準備追擊
            startChaseTime -= Time.deltaTime;//離開戰鬥後亂數開始追擊時間(計時器)

            if (chaseObject != null)
            {
                //追擊的玩家死亡 && 追擊範圍內沒有其他玩家
                if (chaseObject.activeSelf == false && OnDetectionRange(radius: chaseRadius) == false)
                {
                    if (aiState != AIState.追擊狀態)
                    {
                        OnChangeState(state: AIState.追擊狀態, openAnimationName: "Run", closeAnimationName: "AttackIdle", animationType: true);

                        if (isAttackIdleMove)
                        {
                            isAttackIdleMove = false;
                            OnChangeAnimation(animationName: "AttackIdleMove", animationType: false);
                        }
                        if (isAttacking)
                        {
                            isAttacking = false; ;
                            OnChangeAnimation(animationName: "AttackNumber", animationType: 0);
                        }
                        /* if(isStartChase)
                         {
                             isStartChase = false;
                             OnChangeAnimation(animationName: "Run", animationType: false);
                         }*/

                        return;
                    }
                }
            }

            if (startChaseTime <= 0)
            {
                //"待機中動畫"中沒關閉"攻擊中"狀態
                if (info.IsName("AttackIdle") && isAttacking) isAttacking = false;

                //在攻擊待機中 && 不再攻擊中
                if (isAttackIdle && !isAttacking)
                {
                    if (aiState != AIState.追擊狀態)
                    {                           
                        OnChangeAnimation(animationName: "AttackNumber", animationType: 0);
                        OnChangeState(state: AIState.追擊狀態, openAnimationName: "Run", closeAnimationName: "AttackIdle", animationType: true);
                    }
                }
            }
            else
            {
                if (isReadChase)//準備追擊
                {
                    if (info.IsTag("Attack") && info.normalizedTime >= 1)
                    {
                        if (isAttacking)//攻擊中
                        {
                            OnChangeAnimation(animationName: "AttackNumber", animationType: 0);                            
                            OnChangeAnimation(animationName: "AttackIdle", animationType: true);
                        }
                        
                    }
                }
            }

            //攻擊待機移動
            if (isAttackIdleMove)
            {
                isAttackIdleMove = false;
                OnChangeAnimation(animationName: "AttackIdleMove", animationType: false);
                OnChangeAnimation(animationName: "AttackIdle", animationType: true);
            }
        }
    }

    /// <summary>
    /// 等待轉向去攻擊
    /// </summary>
    /// <returns></returns>
    IEnumerator OnWaitTurnToAttack()
    {
        isRotateToPlayer = true;//轉向至玩家
        isAttacking = true;//攻擊中
        isAttackIdle = false;//非攻擊待機

        //咆嘯狀態未解除
        if (isHowling)
        {
            isHowling = false;
            OnChangeAnimation(animationName: "Howling", animationType: false);
        }

        /* //攻擊招式
         if ((transform.position - chaseObject.transform.position).magnitude < meleeAttackDistance)//近身攻擊
         {
             attackNumber = maxAttackNumber;
         }
         else if ((transform.position - chaseObject.transform.position).magnitude >= meleeAttackDistance)//衝刺攻擊
         {
             if(gameObject.tag == "GuardBoss") attackNumber = UnityEngine.Random.Range(1, 3);
             else attackNumber = 1;
         }
         else//一般攻擊
         {
             if (gameObject.tag == "EnemySoldier_2") attackNumber = UnityEngine.Random.Range(1, 3);
             else attackNumber = UnityEngine.Random.Range(1, maxAttackNumber + 1);
         }*/
        attackNumber = UnityEngine.Random.Range(1, maxAttackNumber + 1);
        yield return new WaitForSeconds(0.3f);               

        OnChangeState(state: AIState.攻擊狀態, openAnimationName: "AttackNumber", closeAnimationName: "Run", animationType: attackNumber);

        //離開戰鬥後亂數開始追擊時間(計時器)
        startChaseTime = UnityEngine.Random.Range(readyChaseRandomTime[0], readyChaseRandomTime[1]);
    }

    /// <summary>
    /// 攻擊狀態行為
    /// </summary>
    void OnAttackBehavior()
    {        
        info = animator.GetCurrentAnimatorStateInfo(0);

        OnAttackRangeCheck();//攻擊範圍偵測
                             //if (!isAttacking) OnCheckClosestPlayer();//檢查最近玩家       

        if (!isHowling && info.IsName("Howling"))
        {
            OnChangeAnimation(animationName: "AttackIdle", animationType: true);
        }

        //移除尋路
        if (pathsList.Count > 0)
        {            
            isExecuteAStart = false;//非執行AStart
            point = 0;//尋路節點編號
            pathsList.Clear();
        }

        //攻擊結束
        if (info.IsTag("Attack") && info.normalizedTime >= 1)
        {
            if (!isAttackIdle)//是否攻擊待機
            {                
                isAttackIdle = true;
                isAttacking = false;
             
                OnChangeAnimation(animationName: "AttackNumber", animationType: 0);

                if (OnDetectionRange(radius: attackRadius))//攻擊範圍內
                {
                    OnChangeAnimation(animationName: "AttackIdle", animationType: true);
                }               
                
                attackIdleMoveDiretion = UnityEngine.Random.Range(1, 3);//攻擊待機移動方向(0 = 不移動, 1 = 右, 2 = 左)
                waitAttackTime = UnityEngine.Random.Range(attackFrequency[0], attackFrequency[1]);//亂數攻擊待機時間
                attackIdleMoveRandomTime = UnityEngine.Random.Range(1, waitAttackTime); ;//攻擊待機移動亂數時間(計時器)

                waitAttackTime = waitAttackTime + attackIdleMoveRandomTime;//攻擊待機時間 + 攻擊待機移動時間
            } 
        }

        //攻擊中曾被攻擊過關閉"受擊"動畫
        if (info.IsTag("Attack") && isGetHit)
        {            
            OnChangeAnimation(animationName: "Pain", animationType: false);
            isGetHit = false;
        }

        if (isHowling)//是否咆嘯
        {
            isHowling = false;                
            OnChangeAnimation(animationName: "Howling", animationType: false);
        }

        if (!isReadChase) OnWaitAttackBehavior();//等待攻擊行為
    }    

    /// <summary>
    /// 等待攻擊行為
    /// </summary>
    void OnWaitAttackBehavior()
    {
        //等待攻擊時間 && 非攻擊中
        if (waitAttackTime > 0 && !isAttacking)
        {
            //非執行AStart
            if (!isExecuteAStart)
            {
                waitAttackTime -= Time.deltaTime;//亂數攻擊待機時間
                attackIdleMoveRandomTime -= Time.deltaTime;//攻擊待機移動亂數時間(計時器)
            }
            
            OnAttackIdleMove();//攻擊待機移動            

            if (waitAttackTime <= 0)//攻擊待機時間
            {
                isAttackIdle = false;//攻擊待機
                isAttacking = true;//攻擊中

                //攻擊待機移動
                if (isAttackIdleMove)
                {
                    isAttackIdleMove = false;//是否攻擊待機移動
                                  
                    if(!isMelee) OnChangeAnimation(animationName: "Backflip", animationType: false);
                    OnChangeAnimation(animationName: "AttackIdleMove", animationType: false);
                }

                OnChangeAnimation(animationName: "AttackIdle", animationType: false);

                if ((transform.position - chaseObject.transform.position).magnitude < meleeAttackDistance)//近身攻擊
                {
                    attackNumber = maxAttackNumber;
                }
                else attackNumber = UnityEngine.Random.Range(1, maxAttackNumber);
                OnChangeAnimation(animationName: "AttackNumber", animationType: attackNumber);                
            }
        }  
    }   

    /// <summary>
    /// 攻擊待機移動
    /// </summary>
    void OnAttackIdleMove()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        //攻擊待機移動時間 && 非攻擊待機移動 && 攻擊待機移動方向!=前方
        if (attackIdleMoveRandomTime <= 0 && !isAttackIdleMove && attackIdleMoveDiretion != 0)
        {            
            OnAttackIdleMoveExecution();//執行攻擊移動動畫            
        }
        
        //攻擊待機移動
        if (isAttackIdleMove)
        {
            if (info.IsName("AttackIdleMove"))
            {
                int dir = 1;//攻擊待機移動方向
                if (attackIdleMoveDiretion == 2) dir = -1;

                changeDiretionTime_Forward -= Time.deltaTime;//更換方向時間
                if (changeDiretionTime_Forward <= 0)
                {
                    changeDiretionTime_Forward = changeDiretionTime;//更換方向時間

                    //右方有同伴
                    if (isCheckNearCompanion[0] && dir == 1)
                    {
                        attackIdleMoveDiretion = 2;
                        dir = -1;
                        OnChangeAnimation(animationName: "IsAttackIdleMoveMirror", animationType: true);
                    }

                    //左方有同伴
                    if (isCheckNearCompanion[1] && dir == -1)
                    {
                        attackIdleMoveDiretion = 1;
                        dir = 1;
                        OnChangeAnimation(animationName: "IsAttackIdleMoveMirror", animationType: false);
                    }
                }

                if (isCheckNearCompanion[0] && isCheckNearCompanion[1])
                {
                    attackIdleMoveDiretion = 0;
                    /*OnChangeAnimation(animationName: "AttackIdleMove", animationType: false);
                    OnChangeAnimation(animationName: "AttackIdle", animationType: true);*/
                }

                OnRotateToTarget();//選轉至玩家方向

                //左右移動
                transform.position = transform.position + (transform.right * dir) * attackIdleMoveSpeed * Time.deltaTime;

                //判斷與玩家距離 || 左右都有同伴
                if ((transform.position - chaseObject.transform.position).magnitude < backMoveDistance
                    || attackIdleMoveDiretion == 0)
                {
                    //向後走
                    transform.position = transform.position - transform.forward * attackIdleMoveSpeed * Time.deltaTime;
                }
            }            
        }

        if (info.IsName("Backflip"))//遠距離逃跑
        {
            //向後跳
            if (info.normalizedTime > 0.25f && info.normalizedTime < 0.6f) transform.position = transform.position - transform.forward * chaseSpeed * Time.deltaTime;

            if (info.normalizedTime >= 1)
            {
                OnChangeAnimation(animationName: "Backflip", animationType: false);
                OnChangeAnimation(animationName: "AttackIdleMove", animationType: true);
            }
        }
    }

    /// <summary>
    /// 執行攻擊移動動畫
    /// </summary>
    void OnAttackIdleMoveExecution()
    {        
        isAttackIdleMove = true;//是否攻擊待機移動

        //鏡像動畫Boolen
        if (attackIdleMoveDiretion == 2) OnChangeAnimation(animationName: "IsAttackIdleMoveMirror", animationType: true);
        else OnChangeAnimation(animationName: "IsAttackIdleMoveMirror", animationType: false);

        //非近戰角色 && 與玩家距離 < backMoveDistance
        if (!isMelee && (transform.position - chaseObject.transform.position).magnitude < backMoveDistance)
        {
            OnChangeAnimation(animationName: "AttackIdle", animationType: false);
            OnChangeAnimation(animationName: "Backflip", animationType: true);                     

            return;
        }

        OnChangeAnimation(animationName: "AttackIdle", animationType: false);
        OnChangeAnimation(animationName: "AttackIdleMove", animationType: true);
    }
  
    /// <summary>
    /// 偵測範圍
    /// </summary>
    /// <param name="radius">偵測半徑</param>  
    bool OnDetectionRange(float radius)
    {
        if (Physics.CheckSphere(transform.position, radius, mask))
        {
            //OnGetAllPlayers();//獲取所有玩家

           /* if (allPlayerAlliance != null)
            {
                for (int i = 0; i < allPlayerAlliance.Length; i++)
                {
                    if(allPlayerAlliance[i])
                }
            }*/

            return true;
        }

        return false;
    }

    /// <summary>
    /// 獲取所有玩家
    /// </summary>
    void OnGetAllPlayers()
    {
        //尋找所有玩家
        if (allPlayers[allPlayers.Length - 1] == null) allPlayers = GameObject.FindGameObjectsWithTag("Player");        
    }

    /// <summary>
    /// 偵測是否執行AStart
    /// </summary>
    void OnCheckExecuteAStart()
    {
        //碰撞偵測_牆面
        if (charactersCollision.OnCollision_Wall())
        {
            if(isExecuteAStart)//正在執行AStar
            {
                isExecuteAStart = false;
            }
        }

        //碰撞偵測_目標
        if (OnCollision_Target())
        {
            //尋找節點
            if (!isExecuteAStart)
            {
                if (pathsList.Count > 0) pathsList.Clear();

                isExecuteAStart = true;
                Vector3 startPosition = transform.position;//起始位置(AI位置)
                Vector3 targetPosition = chaseObject.transform.position;
                pathsList = aStart.OnGetBestPoint(startPosition, targetPosition);//尋找路徑
                point = 0;//尋路節點編號
            }
        }      

        if(isExecuteAStart && pathsList.Count > 0) OnWayPoint();//尋路方向
    }

    /// <summary>
    /// 碰撞偵測_目標
    /// </summary>
    /// <returns></returns>
    bool OnCollision_Target()
    {
        //玩家物件存在 && 非執行AStar
        if (chaseObject != null && chaseObject.activeSelf != false)
        {
            CharactersCollision charactersCollision = chaseObject.GetComponent<CharactersCollision>();

            if (charactersCollision != null)
            {
                //偵測障礙物
                LayerMask mask = LayerMask.GetMask("StageObject");
                if (Physics.Linecast(transform.position + (this.charactersCollision.boxCenter / 2), chaseObject.transform.position + (charactersCollision.boxCenter / 2), mask))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 尋路方向
    /// </summary>
    void OnWayPoint()
    {
        //AI位置
        Vector3 AIPosistion = transform.position;
        AIPosistion.y = 0;

        //玩家位置
        Vector3 playerPosition = pathsList[point];
        playerPosition.y = 0;

        //與節點距離
        float distance = (AIPosistion - playerPosition).magnitude;
        
        if (distance < 1f)
        {
            point++;//目前尋路節點編號

            //到達目標
            if (point >= pathsList.Count)
            {                
                point = pathsList.Count;//尋路節點編號
                isExecuteAStart = false;//非執行AStart
                pathsList.Clear();                
            }

            //到達搜索節點數量
            if (point >= aStarCheckPointNumber)
            {                
                //與目標之間沒有障礙物
                if (!OnCollision_Target())
                {
                    point = pathsList.Count - 1;//尋路節點編號
                    isExecuteAStart = false;//非執行AStart
                    pathsList.Clear();
                }
            }
        }
    }

    /// <summary>
    /// 檢查最近玩家
    /// </summary>
    void OnCheckClosestPlayer()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        CheckTargetTime -= Time.deltaTime;//偵測目標時間

        if (CheckTargetTime < 0)
        {
            //敵人士兵AI
            if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                allPlayerAlliance = GameObject.FindGameObjectsWithTag("Alliance");//所有玩家同盟士兵                              

                if (allPlayerAlliance != null)
                {
                    chaseNumber = 0;//追擊對象編號
                    float closestPlayerDistance = 100000;//最近距離
                    float distance;//其他玩家距離

                    for (int i = 0; i < allPlayers.Length; i++)
                    {
                        if (allPlayers[i].activeSelf != false)
                        {
                            distance = (allPlayers[i].transform.position - transform.position).magnitude;
                            if (distance < closestPlayerDistance)
                            {
                                closestPlayerDistance = distance;
                                chaseNumber = i;
                            }
                        }
                    }
                    
                    //檢查玩家同盟士兵
                    for (int i = 0; i < allPlayerAlliance.Length; i++)
                    {
                        if (allPlayerAlliance[i].activeSelf != false)
                        {
                            if ((allPlayerAlliance[i].transform.position - transform.position).magnitude < (allPlayers[chaseNumber].transform.position - transform.position).magnitude &&
                                allPlayerAlliance[i].GetComponent<CharactersCollision>() != null)
                            {
                                CheckTargetTime = CheckPlayerDistanceTime;
                                chaseObject = allPlayerAlliance[i];//追擊目標
                                return;
                            }
                        }
                    }
                }

                chaseObject = allPlayers[chaseNumber];//追擊目標(玩家)
            }

            //我方同盟士兵AI
            if(gameObject.layer == LayerMask.NameToLayer("Alliance"))
            {
                allEnemySoldier = GameObject.FindGameObjectsWithTag("Enemy");//所有敵人士兵

                if (allEnemySoldier != null)
                {
                    //檢查玩家同盟士兵
                    chaseNumber = 0;//追擊對象編號
                    float closestPlayerDistance = 100000;//最近距離
                    float distance;//其他玩家距離
                    for (int i = 0; i < allEnemySoldier.Length; i++)
                    {
                        if (allEnemySoldier[i].activeSelf != false &&
                            allEnemySoldier[i].GetComponent<CharactersCollision>() != null)
                        {
                            distance = (allEnemySoldier[i].transform.position - transform.position).magnitude;
                            if (distance < closestPlayerDistance)
                            {
                                closestPlayerDistance = distance;
                                chaseNumber = i;
                            }
                        }
                    }

                    if (allEnemySoldier != null)
                    {
                        chaseObject = allEnemySoldier[chaseNumber];//追擊目標
                    }
                    else chaseObject = null;
                }                
            }

            CheckTargetTime = CheckPlayerDistanceTime;
        }
        
        //觀看最近玩家 
        Vector3 targetDiration = Vector3.one;//目標向量        
        if (pathsList.Count > 0)//執行AStart
        {
            targetDiration = pathsList[point] - transform.position;       
        }

        else//一般情況
        {
            if(chaseObject != null) targetDiration = chaseObject.transform.position - transform.position;
        }
        
        //判斷目標在左/右方               
        transform.forward = Vector3.RotateTowards(transform.forward, targetDiration, maxRadiansDelta, maxRadiansDelta);        
        transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);  
    }

    /// <summary>
    /// 更換狀態
    /// </summary>
    /// <param name="state">更換狀態</param>
    /// <param name="openAnimationName">開啟動畫名稱</param>
    /// <param name="closeAnimationName">開關閉名稱</param>
    /// <param name="animationType">執行動畫Type</param>
    void OnChangeState<T>(AIState state, string openAnimationName, string closeAnimationName, T animationType)
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        //更換狀態
        if (aiState != state)
        {
            aiState = state;//更換狀態
            OnChangeAnimation(animationName: openAnimationName, animationType: animationType);
            OnChangeAnimation(animationName: closeAnimationName, animationType: false);            
        }
    }

    /// <summary>
    /// 更換動畫
    /// </summary>
    /// <param name="animationName">執行動畫名稱</param>
    /// <param name="animationType">動畫Type</param>
    void OnChangeAnimation<T>(string animationName, T animationType)
    {
        info = animator.GetCurrentAnimatorStateInfo(0);
        
        switch(animationType.GetType().Name)
        {
            case "Boolean":
                animator.SetBool(animationName, Convert.ToBoolean(animationType));
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, animationName, Convert.ToBoolean(animationType));
                break;
            case "Single":                
            break;
            case "Int32":
                animator.SetInteger(animationName, Convert.ToInt32(animationType));
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, animationName, Convert.ToInt32(animationType));
                break;
            case "String":                
            break;
        }
    }    

    private void OnDrawGizmos()
    {
        //一般狀態移動範圍
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(originalPosition, normalStateMoveRadius);

        //警戒範圍
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, alertRadius);

        //追擊範圍
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);        

        //攻擊範圍
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        //尋路
        for (int i = 0; i < pathsList.Count - 1; i++)
        {
            Vector3 s = pathsList[i];
            //s.y = 3;
            Vector3 n = pathsList[i + 1];
            //n.y = 3;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(s, n);
        }

        //前方同伴偵測範圍
       /* float chechSize = 1;
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(transform.position + charactersCollision.boxCenter + transform.forward * (charactersCollision.boxSize.x + chechSize / 2), new Vector3(chechSize, 1, chechSize));*/

        /*//攻擊範圍測試
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position + transform.forward * 1.4f, 1.3f);*/
    }
}
