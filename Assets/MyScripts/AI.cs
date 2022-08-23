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
    AStart aStart = new AStart();

    LayerMask mask;//攻擊對象Layer

    [Header("範圍")]
    [Tooltip("一般狀態移動範圍")] public float normalStateMoveRadius;
    [Tooltip("警戒範圍")] public float alertRadius;
    [Tooltip("追擊範圍")] public float chaseRadius;
    [Tooltip("攻擊範圍")] public float attackRadius;

    [Header("一般狀態")]
    [SerializeField] float[] normalStateMoveTime = new float[2];//一般狀態移動亂數最小與最大值
    [SerializeField] float normalStateMoveSpeed;//一般狀態移動速度
    bool isNormalMove;//是否一般狀態已經移動
    Vector3 originalPosition;//初始位置
    Vector3 forwardVector;//移動目標向量    
    float normalStateTime;//一般狀態移動時間(計時器)
    float normalRandomMoveTime;//一般狀態亂數移動時間
    float normalRandomAngle;//一般狀態亂數選轉角度

    [Header("警戒狀態")]
    [Tooltip("警戒到追擊時間")] public float alertToChaseTime;//警戒到追擊時間
    float alertTime;//警戒到追擊時間(計時器)
    float CheckPlayerDistanceTime;//偵測玩家距離時間
    float CheckPlayerTime;//偵測玩家時間(計時器)    

    [Header("追擊狀態")]
    [SerializeField] float chaseSpeed;//追擊速度
    [SerializeField] float maxRadiansDelta;//轉向角度
    [SerializeField] float[] readyChaseRandomTime;//離開戰鬥後亂數準備追擊時間(亂數最小值, 最大值)
    float startChaseTime;//離開戰鬥後亂數開始追擊時間(計時器)
    int chaseObject;//追擊對象編號
    bool isStartChase;//是否開始追擊
    bool isReadChase;//是否準備追擊
    float chaseTurnTime;//追擊轉向時間(計時器)
    float chaseTurningMoveTime;//追擊轉向移動時間
    [SerializeField] float[] chaseTurningMoveRandomTime;//追擊轉向移動亂數時間
    int chaseDiretion;//追擊方向(0 = 前方, 1 = 右方, 2 = 左方)

    [Header("攻擊狀態")]
    [SerializeField] float[] attackFrequency;//攻擊頻率(亂數最小值, 最大值)
    float attackTime;//攻擊時間(計時器)
    bool isAttackIdle;//是否攻擊待機
    bool isAttacking;//是否攻擊中    
    bool isGetHit;//是否被攻擊(判定"Pain"動畫是否觸發)

    [Header("尋路")]
    [SerializeField] bool isExecuteAStart;//是否執行AStart
    List<Vector3> pathsList = new List<Vector3>();//移動路徑節點  
    int point = 0;//尋路節點編號        

    [Header("所有玩家")]
    [SerializeField] GameObject[] players;//所有玩家

    private void Awake()
    {
        animator = GetComponent<Animator>();

        gameObject.layer = LayerMask.NameToLayer("Enemy");//設定Layer        

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
        mask = LayerMask.GetMask("Player");//攻擊對象Layer

        aStart.initial();
        charactersCollision = GetComponent<CharactersCollision>();

        //偵測範圍
        normalStateMoveRadius = 2.5f;//一般狀態移動範圍
        alertRadius = 12;//警戒範圍
        chaseRadius = 8;//追擊範圍
        attackRadius = 2.0f;//攻擊範圍

        //一般狀態
        originalPosition = transform.position;//初始位置
        forwardVector = transform.forward;//前方向量
        normalStateMoveSpeed = 1;//一般狀態移動速度
        normalStateMoveTime = new float[2] { 1.5f, 3.5f };//一般狀態移動亂數最小與最大值

        //警戒狀態        
        if (GameDataManagement.Instance.isConnect) players = new GameObject[PhotonNetwork.CurrentRoom.PlayerCount];//所有玩家
        else players = new GameObject[1];
        CheckPlayerDistanceTime = 3;//偵測玩家距離時間
        alertToChaseTime = 2;//警戒到追擊時間

        //追擊狀態
        chaseSpeed = 5.3f;//追擊速度
        maxRadiansDelta = 0.1405f;//轉向角度
        readyChaseRandomTime = new float[] { 1.5f, 3.5f};//離開戰鬥後亂數準備追擊時間(亂數最小值, 最大值)
        chaseTurningMoveRandomTime = new float[] { 1.5f, 3.3f};//追擊轉向移動亂數時間

        //攻擊狀態
        attackFrequency = new float[2] { 0.5f, 1.75f};//攻擊頻率(亂數最小值, 最大值)  
    }

    void Update()
    {
        OnStateBehavior();        
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

    /// <summary>
    /// 狀態行為
    /// </summary>
    void OnStateBehavior()
    {
        switch (aiState)
        {
            case AIState.一般狀態:
                OnNormalStateBehavior();
                break;
            case AIState.警戒狀態:
                OnAlertStateBehavior();
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

        if (normalStateTime <= 0)
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

                //偵測腳色碰撞
              /*  RaycastHit hit;
                if (charactersCollision.OnCollision_Characters(out hit)) forwardVector = transform.position - hit.transform.position;//轉向*/
        
                //移動 && 轉向
                float maxRadiansDelta = 0.03f;//轉向角度
                transform.forward = Vector3.RotateTowards(transform.forward, forwardVector, maxRadiansDelta, maxRadiansDelta);
                transform.position = transform.position + transform.forward * normalStateMoveSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);

                //charactersCollision.OnCollision_Character();//碰撞框_腳色
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
            if (aiState != AIState.警戒狀態)
            {
                OnChangeState(state: AIState.警戒狀態, openAnimationName: "Alert", closeAnimationName: "Walk");
            }
            else//警戒狀態中
            {
                alertTime += Time.deltaTime;//警戒到追擊時間

                if (alertTime >= alertToChaseTime)
                {
                    OnHowlingBehavior();//咆嘯行為
                    OnChangeState(state: AIState.追擊狀態, openAnimationName: "Howling", closeAnimationName: "Alert");
                    OnChangeAnimation(animationName: "Idle", animationType: false);
                }
            }
        }
        else
        {
            if (aiState != AIState.一般狀態)
            {
                normalRandomMoveTime = 0;//一般狀態亂數移動時間
                alertTime = 0;
                normalStateTime = UnityEngine.Random.Range(normalStateMoveTime[0], normalStateMoveTime[1]);//一般狀態亂數移動時間
                                                                                               
                OnChangeState(state: AIState.一般狀態, openAnimationName: "Idle", closeAnimationName: "Alert");
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
        OnCheckClosestPlayer();//檢查最近玩家             
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

        OnGetAllPlayers();//獲取所有玩家
        OnChangeState(state: AIState.追擊狀態, openAnimationName: "Howling", closeAnimationName: "Alert");
    }

    /// <summary>
    /// 咆嘯行為
    /// </summary>
    void OnHowlingBehavior()
    {
        AI[] companions = GameObject.FindObjectsOfType<AI>();

        foreach (var companion in companions)
        {
            float distance = (transform.position - companion.transform.position).magnitude;
            if(distance <= alertRadius)
            {
                //通知同伴
                companion.OnChangeStateToChase(players);
            }

        }

       /* RaycastHit hit;
        if(Physics.SphereCast(transform.position, alertRadius, transform.up, out hit, alertRadius, 1 << LayerMask.NameToLayer("Enemy")))
        {
            AI ai =  hit.transform.GetComponent<AI>();

            //通知同伴
            if (ai != null) ai.OnChangeStateToChase();
        }*/
    }

    /// <summary>
    /// 更換狀態到追擊狀態
    /// </summary>
    /// <param name="chasePlaters">追擊的玩家</param>
    void OnChangeStateToChase(GameObject[] chasePlaters)
    {
        if (aiState != AIState.追擊狀態 || aiState != AIState.攻擊狀態)
        {
            players = chasePlaters;//追擊的玩家
            OnChangeState(state: AIState.追擊狀態, openAnimationName: "Howling", closeAnimationName: "Alert");
        }
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
                OnChangeState(state: AIState.追擊狀態, openAnimationName: "Howling", closeAnimationName: "Alert");
            }
        }
    }   

    /// <summary>
    /// 追擊行為
    /// </summary>
    void OnChaseBehavior()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        //咆嘯完開始追擊
        if (info.IsName("Howling") && info.normalizedTime >= 1)
        {
            isStartChase = true;//開始追擊

            OnChangeAnimation(animationName: "Howling", animationType: false);
            OnChangeAnimation(animationName: "Run", animationType: true);
        }

        //開始追擊
        if (isStartChase)
        {
            OnAttackRangeCheck();//攻擊範圍偵測        
            OnCheckExecuteAStart();//偵測碰撞
            OnCheckClosestPlayer();//檢查最近玩家

            //朝玩家移動
            if (info.IsName("Run")) OnMoveBehavior();            

            /*//前方有自己人
            if (charactersCollision.OnCollision_Enemy())
            {
                chaseTurnTime = 1;

                //轉向                
                transform.forward = Vector3.RotateTowards(transform.forward, transform.right, maxRadiansDelta, maxRadiansDelta);
                
            }
            else
            {
                if (chaseTurnTime <= 0) OnCheckClosestPlayer();//檢查最近玩家
            }*/

            if (chaseTurnTime <= 0) OnCheckClosestPlayer();//檢查最近玩家
        }

        /*//碰牆重新計算尋路
        if (pathsList.Count > 0)
        {
            for (int i = 0; i < charactersCollision.collisionObject.Length; i++)
            {
                aStart.OnGetBestPoint(transform.position, players[chaseObject].transform.position);
            }
        }*/
    }

    /// <summary>
    /// 移動行為
    /// </summary>
    void OnMoveBehavior()
    {
        if(chaseDiretion == 0) transform.position = transform.position + transform.forward * chaseSpeed * Time.deltaTime;
        else transform.position = transform.position + transform.forward * (chaseSpeed / 2) * Time.deltaTime;

        if (!isExecuteAStart)//非執行AStart
        {
            chaseTurnTime -= Time.deltaTime;//追擊轉向時間

            //追擊轉向時間(計時器) <= 0 && 追擊轉向移動時間 <= 0
            if (chaseTurnTime <= 0 && chaseTurningMoveTime <= 0)
            {
                chaseDiretion = UnityEngine.Random.Range(0, 5);//追擊方向(0 = 前方, 1.2 = 右方, 3.4 = 左方)         
                chaseTurningMoveTime = UnityEngine.Random.Range(chaseTurningMoveRandomTime[0], chaseTurningMoveRandomTime[0]);//追擊轉向移動時間
                
            }

            //追擊轉向
            if (chaseTurningMoveTime > 0)
            {
                chaseTurningMoveTime -= Time.deltaTime;//追擊轉向移動時間

                //攻擊範圍內沒有障礙物
                if (!Physics.CheckBox(transform.position + charactersCollision.boxCenter + transform.forward * attackRadius, new Vector3(attackRadius * 2, 0.1f, attackRadius), Quaternion.Euler(transform.localEulerAngles), 1 << LayerMask.NameToLayer("StageObject")))
                {
                    //追擊方向(0 = 前方, 1 = 右方, 2 = 左方)
                    switch (chaseDiretion)
                    {
                        case 1://向右移動(較偏)
                            transform.position = transform.position + transform.right * (chaseSpeed / 2) * Time.deltaTime;
                            break;
                        case 2://向右移動
                            transform.position = transform.position + (transform.right / 2) * (chaseSpeed / 2) * Time.deltaTime;
                            break;
                        case 3://向左移動(較偏)
                            transform.position = transform.position - transform.right * (chaseSpeed / 2) * Time.deltaTime;
                            break;
                        case 4://向右移動
                            transform.position = transform.position - (transform.right / 2) * (chaseSpeed / 2) * Time.deltaTime;
                            break;
                    }
                }

                if (chaseTurningMoveTime <= 0)
                {
                    chaseTurnTime = UnityEngine.Random.Range(chaseTurningMoveRandomTime[0], chaseTurningMoveRandomTime[0]);//追擊轉向時間(計時器)
                }
            }
            else chaseDiretion = 0;//追擊方向(0 = 前方, 1 = 右方, 2 = 左方)
        }
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
            if (isReadChase) isReadChase = false;//是否準備追擊

            if (aiState != AIState.攻擊狀態)
            {
                isAttacking = true;//攻擊中
                isAttackIdle = false;//非攻擊待機

                OnChangeState(state: AIState.攻擊狀態, openAnimationName: "NormalAttack", closeAnimationName: "Run");
                startChaseTime = UnityEngine.Random.Range(readyChaseRandomTime[0], readyChaseRandomTime[1]);//離開戰鬥後亂數開始追擊時間(計時器)
            }
        } 
        else
        {
            isReadChase = true;//準備追擊
            startChaseTime -= Time.deltaTime;//離開戰鬥後亂數開始追擊時間(計時器)

            if (startChaseTime <= 0)
            {
                //"待機中動畫"中沒關閉"攻擊中"狀態
                if (info.IsName("AttackIdle") && isAttacking) isAttacking = false;

                //在攻擊待機中 && 不再攻擊中
                if (isAttackIdle && !isAttacking)
                {
                    if (aiState != AIState.追擊狀態)
                    {
                        OnChangeAnimation("NormalAttack", false);
                        OnChangeState(state: AIState.追擊狀態, openAnimationName: "Run", closeAnimationName: "AttackIdle");
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
                            OnChangeAnimation("NormalAttack", false);
                            OnChangeAnimation("AttackIdle", true);
                        }
                        
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// 攻擊狀態行為
    /// </summary>
    void OnAttackBehavior()
    {        
        info = animator.GetCurrentAnimatorStateInfo(0);

        OnAttackRangeCheck();
        if (!isAttacking) OnCheckClosestPlayer();//檢查最近玩家       

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
               
                OnChangeAnimation("NormalAttack", false);

                if (OnDetectionRange(radius: attackRadius))//攻擊範圍內
                {
                    OnChangeAnimation("AttackIdle", true);
                }                

                //亂數攻擊待機時間
                attackTime = UnityEngine.Random.Range(attackFrequency[0], attackFrequency[1]);
            }
        }

        //攻擊中曾被攻擊過關閉"受擊"動畫
        if (info.IsTag("Attack") && isGetHit)
        {            
            OnChangeAnimation(animationName: "Pain", animationType: false);
            isGetHit = false;
        }
        
        if(!isReadChase) OnWaitAttack();//等待攻擊
    }    

    /// <summary>
    /// 等待攻擊
    /// </summary>
    void OnWaitAttack()
    {
        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;

            if(attackTime <= 0)
            {
                isAttackIdle = false;
                isAttacking = true;
                
                OnChangeAnimation(animationName: "AttackIdle", animationType: false);
                OnChangeAnimation(animationName: "NormalAttack", animationType: true);
            }
        }
    }
  
    /// <summary>
    /// 偵測範圍
    /// </summary>
    /// <param name="radius">偵測半徑</param>  
    bool OnDetectionRange(float radius)
    {
        if (Physics.CheckSphere(transform.position, radius, mask))
        {
            OnGetAllPlayers();//獲取所有玩家
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
        if (players[players.Length - 1] == null) players = GameObject.FindGameObjectsWithTag("Player");
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

        //碰撞偵測_玩家
        if (OnCollision_Player())
        {
            //尋找節點
            if (!isExecuteAStart)
            {
                if (pathsList.Count > 0) pathsList.Clear();

                isExecuteAStart = true;
                Vector3 startPosition = transform.position;//起始位置(AI位置)
                Vector3 targetPosition = players[chaseObject].transform.position;
                pathsList = aStart.OnGetBestPoint(startPosition, targetPosition);//尋找路徑
                point = 0;//尋路節點編號
            }
        }      

        if(isExecuteAStart && pathsList.Count > 0) OnWayPoint();//尋路方向
    }

    /// <summary>
    /// 碰撞偵測_玩家
    /// </summary>
    /// <returns></returns>
    bool OnCollision_Player()
    {
        //玩家物件存在 && 非執行AStar
        if (players[chaseObject] != null)
        {
            CharactersCollision playerCharactersCollision = players[chaseObject].GetComponent<CharactersCollision>();

            //偵測障礙物
            LayerMask mask = LayerMask.GetMask("StageObject");
            if (Physics.Linecast(transform.position + (charactersCollision.boxCenter / 2), players[chaseObject].transform.position + (playerCharactersCollision.boxCenter / 2), mask))
            {                
                return true;
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

            //到達目標 || 到達搜索節點數量
            if (point >= pathsList.Count || !OnCollision_Player())
            {                
                point = pathsList.Count;//尋路節點編號
                isExecuteAStart = false;//非執行AStart
                pathsList.Clear();
            }            
        }
    }
    
    /// <summary>
    /// 檢查最近玩家
    /// </summary>
    void OnCheckClosestPlayer()
    {
        CheckPlayerTime -= Time.deltaTime;//偵測玩家時間

        if (CheckPlayerTime < 0)
        {
            chaseObject = 0;//追擊對象編號
            float closestPlayerDistance = 100000;//最近距離
            float distance;//其他玩家距離

            for (int i = 0; i < players.Length; i++)
            {
                distance = (players[i].transform.position - transform.position).magnitude;
                if (distance < closestPlayerDistance)
                {
                    closestPlayerDistance = distance;
                    chaseObject = i;
                }
            }

            CheckPlayerTime = CheckPlayerDistanceTime;
        }

        //觀看最近玩家 
        Vector3 targetDiration;//目標向量        
        if (pathsList.Count > 0)//執行AStart
        {
            targetDiration = pathsList[point] - transform.position;       
        }

        else//一般情況
        {
            targetDiration = players[chaseObject].transform.position - transform.position;
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
    void OnChangeState(AIState state, string openAnimationName, string closeAnimationName)
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        //更換狀態
        if (aiState != state)
        {
            aiState = state;
            OnChangeAnimation(animationName: openAnimationName, animationType: true);
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
    }
}
