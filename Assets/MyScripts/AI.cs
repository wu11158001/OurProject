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

    [Header("範圍")]
    [SerializeField] public float normalStateMoveRadius;//一般狀態移動範圍
    [SerializeField] public float alertRadius;//警戒範圍
    [SerializeField] public float chaseRadius;//追擊範圍
    [SerializeField] public float attackRadius;//攻擊範圍

    [Header("一般狀態")]
    [SerializeField] float[] normalStateMoveTime = new float[2];//一般狀態移動亂數最小與最大值
    [SerializeField] float normalStateMoveSpeed;//一般狀態移動速度
    bool isNormalMove;//是否一般狀態已經移動
    Vector3 originalPosition;//初始位置
    Vector3 forwardVector;//移動目標向量    
    [SerializeField]float normalStateTime;//一般狀態移動時間(計時器)
    float normalRandomMoveTime;//一般狀態亂數移動時間
    float normalRandomAngle;//一般狀態亂數選轉角度

    [Header("警戒狀態")]
    [SerializeField] float alertToChaseTime;//警戒到追擊時間
    [SerializeField] float leaveAlertRadiusAlertTime;//離開警戒範圍警戒時間
    float leaveAlertTime;//離開警戒範圍警戒時間(計時器)
    float alertTime;//警戒到追擊時間(計時器)
    float CheckPlayerDistanceTime;//偵測玩家距離時間
    float CheckPlayerTime;//偵測玩家時間(計時器)        

    [Header("咆嘯狀態")]
    bool isRotateToPlayer;//是否轉向至玩家

    [Header("追擊狀態")]
    [SerializeField] float chaseSpeed;//追擊速度
    [SerializeField] float maxRadiansDelta;//轉向角度
    [SerializeField] float[] readyChaseRandomTime;//離開戰鬥後亂數準備追擊時間(亂數最小值, 最大值)
    float startChaseTime;//離開戰鬥後亂數開始追擊時間(計時器)
    int chaseObject;//追擊對象編號
    bool isStartChase;//是否開始追擊
    bool isReadChase;//是否準備追擊
    float chaseTurnTime;//追擊轉向時間(計時器)
    [SerializeField]int chaseDiretion;//追擊方向(0 = 前方, 1 = 右方, 2 = 左方)

    [Header("檢查同伴")]
    float chaseSlowDownSpeed;//追擊減速速度
    bool isPauseChase;//是否暫停追擊
    [SerializeField]bool[] isCheckNearCompanion;//檢查附近同伴(0 = 右方有碰撞, 1 = 左方有碰撞)

    [Header("攻擊狀態")]
    [SerializeField] float[] attackFrequency;//攻擊頻率(亂數最小值, 最大值)
    [SerializeField] int attackNumber;//攻擊招式編號(0 = 不攻擊)    
    int maxAttackNumber;//可使用攻擊招式
    float waitAttackTime;//等待攻擊時間(計時器)
    bool isAttackIdle;//是否攻擊待機
    bool isAttacking;//是否攻擊中    
    bool isGetHit;//是否被攻擊(判定"Pain"動畫是否觸發)

    [Header("攻擊待機")]
    [SerializeField] float attackIdleMoveSpeed;//攻擊待機移動速度
    [SerializeField] float backMoveDistance;//距離玩家多近向後走
    bool isAttackIdleMove;//是否攻擊待機移動
    float attackIdleMoveRandomTime;//攻擊待機移動亂數時間(計時器)     
    int attackIdleMoveDiretion;//攻擊待機移動方向

    [Header("尋路")]
    bool isExecuteAStart;//是否執行AStart
    List<Vector3> pathsList = new List<Vector3>();//移動路徑節點  
    int point = 0;//尋路節點編號        

    [Header("所有玩家")]
    [SerializeField] GameObject[] allPlayers;//所有玩家

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
        attackRadius = 2.3f;//攻擊範圍

        //一般狀態
        originalPosition = transform.position;//初始位置
        forwardVector = transform.forward;//前方向量
        normalStateMoveSpeed = 1;//一般狀態移動速度
        normalStateMoveTime = new float[2] { 1.5f, 3.5f };//一般狀態移動亂數最小與最大值

        //警戒狀態        
        if (GameDataManagement.Instance.isConnect) allPlayers = new GameObject[PhotonNetwork.CurrentRoom.PlayerCount];//所有玩家
        else allPlayers = new GameObject[1];
        CheckPlayerDistanceTime = 3;//偵測玩家距離時間
        alertToChaseTime = 2;//警戒到追擊時間
        leaveAlertRadiusAlertTime = 3;//離開警戒範圍警戒時間
        leaveAlertTime = leaveAlertRadiusAlertTime;//離開警戒範圍警戒時間(計時器)

        //檢查同伴
        isCheckNearCompanion = new bool[2] { false, false};//檢查附近同伴(0 = 右方有碰撞, 1 = 左方有碰撞)

        //追擊狀態
        chaseSpeed = 5.3f;//追擊速度
        maxRadiansDelta = 0.1405f;//轉向角度
        readyChaseRandomTime = new float[] { 1.5f, 3.5f};//離開戰鬥後亂數準備追擊時間(亂數最小值, 最大值)

        //攻擊狀態
        attackFrequency = new float[2] { 0.5f, 3.75f};//攻擊頻率(亂數最小值, 最大值)  
        maxAttackNumber = 3;//可使用攻擊招式

        //攻擊待機
        attackIdleMoveSpeed = 1;//攻擊待機移動速度
        backMoveDistance = 1.5f;//距離玩家多近向後走
    }

    void Update()
    {
        OnStateBehavior();//狀態行為
        OnRotateToPlayer();//選轉至玩家方向
        OnCollision();//碰撞框
    }

    /// <summary>
    /// 碰撞框
    /// </summary>    
    /// <returns></returns>
    public void OnCollision()
    {
        LayerMask mask = LayerMask.GetMask("Enemy");
        RaycastHit hit;

        //射線方向
        Vector3[] rayDiration = new Vector3[] { transform.forward,
                                                transform.forward - transform.right,
                                                transform.right,
                                                transform.forward + transform.right,
                                                -transform.right };

        float boxSize = 0.5f;//碰撞框大小
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
        OnChangeState(state: AIState.追擊狀態, openAnimationName: "Howling", closeAnimationName: "Alert", animationType: true);

        OnHowlingBehavior();//咆嘯行為
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
                companion.OnChangeStateToChase(allPlayers: allPlayers, chaseObject: chaseObject);
            }
        }   
    }

    /// <summary>
    /// 更換狀態到追擊狀態
    /// </summary>
    /// <param name="allPlayers">所有玩家</param>
    /// <param name="chaseObject">追擊的玩家</param>
    void OnChangeStateToChase(GameObject[] allPlayers, int chaseObject)
    {
        this.allPlayers = allPlayers;//所有玩家
        this.chaseObject = chaseObject;//追擊的玩家
        StartCoroutine(OnWautChase());//等待追擊
    }

    /// <summary>
    /// 選轉至玩家方向
    /// </summary>
    void OnRotateToPlayer()
    {
        //轉向至玩家
        if (isRotateToPlayer)
        {
            Vector3 targetDiration = allPlayers[chaseObject].transform.position - transform.position;
            transform.forward = transform.forward = Vector3.RotateTowards(transform.forward, targetDiration, maxRadiansDelta, maxRadiansDelta);
        }
    }

    /// <summary>
    /// 等待追擊
    /// </summary>
    /// <returns></returns>
    IEnumerator OnWautChase()
    {
        isRotateToPlayer = true;//轉向至玩家

        yield return new WaitForSeconds(1);

        isRotateToPlayer = false;

        if (aiState != AIState.追擊狀態 || aiState != AIState.攻擊狀態)
        {
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

        //咆嘯完開始追擊
        if (info.IsName("Howling") && info.normalizedTime >= 1)
        {
            //回一般狀態事前設定
            isNormalMove = false;//是否一般狀態已經移動
            normalStateTime = UnityEngine.Random.Range(normalStateMoveTime[0], normalStateMoveTime[1]);//重製一般狀態亂數移動時間

            //追擊
            isStartChase = true;//開始追擊                        

            Vector3 targetDiration = allPlayers[chaseObject].transform.position - transform.position;
            transform.forward = transform.forward = Vector3.RotateTowards(transform.forward, targetDiration, 1, 1);

            OnChangeAnimation(animationName: "Howling", animationType: false);
            OnChangeAnimation(animationName: "Run", animationType: true);
        }

        //開始追擊
        if (isStartChase)
        {
            OnAttackRangeCheck();//攻擊範圍偵測        
            OnCheckExecuteAStart();//偵測是否執行AStart
            OnCheckClosestPlayer();//檢查最近玩家            
            OnCheckForwardCompanion();//檢查前方同伴
            OnCheckLefrAndRightCompanion();//檢查左右同伴

            //朝玩家移動
            if (info.IsName("Run")) OnMoveBehavior();

            //檢查最近玩家
            if (chaseTurnTime <= 0) OnCheckClosestPlayer();

            //追擊的玩家死亡 && 追擊範圍內沒有其他玩家
            if (allPlayers[chaseObject].activeSelf == false && OnDetectionRange(radius: chaseRadius) == false)
            {
                if (aiState != AIState.一般狀態)
                {
                    OnChangeState(state: AIState.一般狀態, openAnimationName: "Idle", closeAnimationName: "Run", animationType: true);
                    return;
                }
            }
        }
    }

    /// <summary>
    /// 檢查 同伴/障礙物 碰撞框
    /// </summary>
    /// <param name="diretion">方向</param>
    bool OnCheckCompanionBox(Vector3 diretion)
    {
        float chechSize = 0.5f;//檢查框大小
        LayerMask mask = LayerMask.GetMask("Enemy");

        //碰到同伴
        if (Physics.CheckBox(transform.position + charactersCollision.boxCenter + diretion * (charactersCollision.boxSize.x + (chechSize / 2)), new Vector3(chechSize, 0.1f, chechSize), Quaternion.Euler(transform.localEulerAngles), mask))
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
        //檢查前方同伴
        if (OnCheckCompanionBox(diretion: transform.forward))
        {
            chaseSlowDownSpeed -= 0.4f * Time.deltaTime;//追擊減速速度            
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

            chaseDiretion = UnityEngine.Random.Range(1, 3);//更改方向
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
        //檢查右方同伴
        if (OnCheckCompanionBox(diretion: transform.right))//檢查右方同伴
        {
            if (!isCheckNearCompanion[0])//檢查附近同伴(0 = 右方有碰撞, 1 = 左方有碰撞)
            {
                isCheckNearCompanion[0] = true;
                chaseDiretion = 2;//追擊方向(0 = 前方, 1 = 右方, 2 = 左方)
            }
        }
        else isCheckNearCompanion[0] = false;//檢查附近同伴(0 = 右方有碰撞, 1 = 左方有碰撞)
       
        //檢查左方同伴
        if (OnCheckCompanionBox(diretion: -transform.right))
        {
            if (!isCheckNearCompanion[1])//檢查附近同伴(0 = 右方有碰撞, 1 = 左方有碰撞)
            {
                isCheckNearCompanion[1] = true;
                chaseDiretion = 1;//追擊方向(0 = 前方, 1 = 右方, 2 = 左方)
            }
        }
        else isCheckNearCompanion[1] = false;//檢查附近同伴(0 = 右方有碰撞, 1 = 左方有碰撞)    
        
        //左右都沒有碰撞同伴
        if(!isCheckNearCompanion[0] && !isCheckNearCompanion[1]) chaseDiretion = 0;//追擊方向(0 = 前方, 1 = 右方, 2 = 左方)
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
            if (isReadChase) isReadChase = false;//是否準備追擊

            if (aiState != AIState.攻擊狀態)
            {
                isAttacking = true;//攻擊中
                isAttackIdle = false;//非攻擊待機               
                                                
                attackNumber = UnityEngine.Random.Range(1, maxAttackNumber + 1);//攻擊招式
                OnChangeState(state: AIState.攻擊狀態, openAnimationName: "AttackNumber", closeAnimationName: "Run", animationType: attackNumber);

                startChaseTime = UnityEngine.Random.Range(readyChaseRandomTime[0], readyChaseRandomTime[1]);//離開戰鬥後亂數開始追擊時間(計時器)
            }
        } 
        else
        {
            isReadChase = true;//準備追擊
            startChaseTime -= Time.deltaTime;//離開戰鬥後亂數開始追擊時間(計時器)

            //追擊的玩家死亡 && 追擊範圍內沒有其他玩家
            if (allPlayers[chaseObject].activeSelf == false && OnDetectionRange(radius: chaseRadius) == false)
            {
                if (aiState != AIState.一般狀態)
                {
                    OnChangeState(state: AIState.一般狀態, openAnimationName: "Idle", closeAnimationName: "AttackIdle", animationType: true);
                    if(isAttackIdleMove) OnChangeAnimation(animationName: "AttackIdleMove", animationType: false);

                    return;
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
    /// 攻擊狀態行為
    /// </summary>
    void OnAttackBehavior()
    {        
        info = animator.GetCurrentAnimatorStateInfo(0);

        OnAttackRangeCheck();//攻擊範圍偵測
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
             
                OnChangeAnimation(animationName: "AttackNumber", animationType: 0);

                if (OnDetectionRange(radius: attackRadius))//攻擊範圍內
                {
                    OnChangeAnimation(animationName: "AttackIdle", animationType: true);
                }

                attackIdleMoveDiretion = chaseDiretion;//攻擊待機移動方向
                waitAttackTime = UnityEngine.Random.Range(attackFrequency[0], attackFrequency[1]);//亂數攻擊待機時間
                attackIdleMoveRandomTime = UnityEngine.Random.Range(0.5f, waitAttackTime); ;//攻擊待機移動亂數時間(計時器)

                waitAttackTime = waitAttackTime + attackIdleMoveRandomTime;//攻擊待機時間 + 攻擊待機移動時間
            } 
        }

        //攻擊中曾被攻擊過關閉"受擊"動畫
        if (info.IsTag("Attack") && isGetHit)
        {            
            OnChangeAnimation(animationName: "Pain", animationType: false);
            isGetHit = false;
        }
        
        if(!isReadChase) OnWaitAttackBehavior();//等待攻擊行為
    }    

    /// <summary>
    /// 等待攻擊行為
    /// </summary>
    void OnWaitAttackBehavior()
    {
        //等待攻擊時間 && 非攻擊中
        if (waitAttackTime > 0 && !isAttacking)
        {
            waitAttackTime -= Time.deltaTime;//亂數攻擊待機時間
            attackIdleMoveRandomTime -= Time.deltaTime;//攻擊待機移動亂數時間(計時器)

            OnCheckLefrAndRightCompanion();//檢查左右同伴
            OnAttackIdleMove();//攻擊待機移動            

            if (waitAttackTime <= 0)//攻擊待機時間
            {
                isAttackIdle = false;//攻擊待機
                isAttacking = true;//攻擊中

                //攻擊待機移動
                if (isAttackIdleMove)
                {
                    isAttackIdleMove = false;//是否攻擊待機移動
                    OnChangeAnimation(animationName: "AttackIdleMove", animationType: false);
                }

                OnChangeAnimation(animationName: "AttackIdle", animationType: false);

                attackNumber = UnityEngine.Random.Range(1, maxAttackNumber + 1);
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

        //攻擊待機移動時間 && 非攻擊待機移動 && 非攻擊中 && 追擊方向不=前方
        if (attackIdleMoveRandomTime <= 0 && !isAttackIdleMove && chaseDiretion != 0)
        {
            OnCheckLefrAndRightCompanion();//檢查左右同伴
            OnAttackIdleMoveExecution();//執行攻擊移動動畫            
        }
        
        //攻擊待機移動
        if (isAttackIdleMove && info.IsName("AttackIdleMove"))
        {            
            //攻擊待機移動方向!=前方
            if (attackIdleMoveDiretion != 0)
            {
                int dir = 1;//攻擊待機移動方向
                if (attackIdleMoveDiretion == 2) dir = -1;
                else dir = 1;

                //左右移動
                transform.position = transform.position + (transform.right * dir) * attackIdleMoveSpeed * Time.deltaTime;
            }
            else
            {
                if(isAttackIdleMove)//是否攻擊待機移動
                {                    
                    isAttackIdleMove = false;
                    OnChangeAnimation(animationName: "AttackIdleMove", animationType: false);
                    OnChangeAnimation(animationName: "AttackIdle", animationType: true);
                }                
            }

            //判斷與玩家距離
            if ((transform.position - allPlayers[chaseObject].transform.position).magnitude < backMoveDistance)
            {
                //向後走
                transform.position = transform.position - transform.forward * attackIdleMoveSpeed * Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// 執行攻擊移動動畫
    /// </summary>
    void OnAttackIdleMoveExecution()
    {
        attackIdleMoveDiretion = chaseDiretion;//攻擊待機移動方向

        isAttackIdleMove = true;//是否攻擊待機移動
        OnChangeAnimation(animationName: "AttackIdle", animationType: false);
        OnChangeAnimation(animationName: "AttackIdleMove", animationType: true);

        //鏡像動畫Boolen
        if (attackIdleMoveDiretion == 2) OnChangeAnimation(animationName: "IsAttackIdleMoveMirror", animationType: true);
        else OnChangeAnimation(animationName: "IsAttackIdleMoveMirror", animationType: false);
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

        //碰撞偵測_玩家
        if (OnCollision_Player())
        {
            //尋找節點
            if (!isExecuteAStart)
            {
                if (pathsList.Count > 0) pathsList.Clear();

                isExecuteAStart = true;
                Vector3 startPosition = transform.position;//起始位置(AI位置)
                Vector3 targetPosition = allPlayers[chaseObject].transform.position;
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
        if (allPlayers[chaseObject] != null)
        {
            CharactersCollision playerCharactersCollision = allPlayers[chaseObject].GetComponent<CharactersCollision>();

            //偵測障礙物
            LayerMask mask = LayerMask.GetMask("StageObject");
            if (Physics.Linecast(transform.position + (charactersCollision.boxCenter / 2), allPlayers[chaseObject].transform.position + (playerCharactersCollision.boxCenter / 2), mask))
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

            for (int i = 0; i < allPlayers.Length; i++)
            {
                if (allPlayers[i].activeSelf != false)
                {
                    distance = (allPlayers[i].transform.position - transform.position).magnitude;
                    if (distance < closestPlayerDistance)
                    {
                        closestPlayerDistance = distance;
                        chaseObject = i;
                    }
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
            targetDiration = allPlayers[chaseObject].transform.position - transform.position;
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
        float chechSize = 1;
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(transform.position + charactersCollision.boxCenter + transform.forward * (charactersCollision.boxSize.x + chechSize / 2), new Vector3(chechSize, 1, chechSize));

        /*//攻擊範圍測試
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position + transform.forward * 1.4f, 1.3f);*/
    }
}
