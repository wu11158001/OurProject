using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviourPunCallbacks
{
    Animator animator;
    AnimatorStateInfo info;

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
    float alertCheckDistanceTime;//警戒狀態偵測範圍時間
    float alertCheckTime;//警戒狀態偵測偵測時間(計時器)
    int alertObject;//警戒對象編號

    [Header("追擊狀態")]
    [SerializeField] float chaseSpeed;//追擊速度

    [Header("攻擊狀態")]
    [SerializeField] float[] attackFrequency;//攻擊頻率(亂數最小值, 最大值)

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

        //偵測範圍
        normalStateMoveRadius = 3;//一般狀態移動範圍
        alertRadius = 30;//警戒範圍
        chaseRadius = 18;//追擊範圍
        attackRadius = 7.5f;//攻擊範圍

        //一般狀態
        originalPosition = transform.position;//初始位置
        forwardVector = transform.forward;//前方向量
        normalStateMoveSpeed = 1;//一般狀態移動速度
        normalStateMoveTime = new float[2] { 1.5f, 3.5f };//一般狀態移動亂數最小與最大值

        //警戒狀態        
        if (GameDataManagement.Instance.isConnect) players = new GameObject[PhotonNetwork.CurrentRoom.PlayerCount];//所有玩家
        else players = new GameObject[1];
        alertCheckDistanceTime = 3;//警戒狀態偵測範圍時間

        //追擊狀態
        chaseSpeed = 5;//追擊速度

        //攻擊狀態
        attackFrequency = new float[2] { 0.5f, 3.0f};//攻擊頻率(亂數最小值, 最大值)
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

                normalRandomMoveTime = Random.Range(2, 4.5f);//一般狀態亂數移動時間

                //選轉
                if ((transform.position - originalPosition).magnitude > normalStateMoveRadius)//離開一般狀態移動範圍
                {
                    forwardVector = originalPosition - transform.position;//移動目標向量
                }
                else//一般狀態移動範圍內
                {
                    normalRandomAngle = Random.Range(0, 360);//一般狀態亂數選轉角度
                    forwardVector = Quaternion.AngleAxis(normalRandomAngle, Vector3.up) * forwardVector;//移動目標向量
                }

                //更換動畫
                OnChangeAnimation(animationName: "Walk", isAnimationActive: true);
            }

            if (normalRandomMoveTime > 0)//移動
            {
                normalRandomMoveTime -= Time.deltaTime;

                float maxRadiansDelta = 0.03f;//轉向角度
                transform.forward = Vector3.RotateTowards(transform.forward, forwardVector, maxRadiansDelta, maxRadiansDelta);
                transform.position = transform.position + transform.forward * normalStateMoveSpeed * Time.deltaTime;                              
            }
            else//待機
            {
                isNormalMove = false;
                normalStateTime = Random.Range(normalStateMoveTime[0], normalStateMoveTime[1]);//一般狀態亂數移動時間                

                //更換動畫
                OnChangeAnimation(animationName: "Walk", isAnimationActive: false);
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
            OnChangeState(state: AIState.警戒狀態, openAnimationName: "Alert", closeAnimationName: "Walk");
        }
        else
        {
            OnChangeState(state: AIState.一般狀態, openAnimationName: "Walk", closeAnimationName: "Alert");
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
    /// 追擊範圍偵測
    /// </summary>
    void OnChaseRangeCheck()
    {
        //更換狀態偵測
        if (OnDetectionRange(radius: chaseRadius))
        {            
            OnChangeState(state: AIState.追擊狀態, openAnimationName: "Run", closeAnimationName: "Alert");
        }
    }

    /// <summary>
    /// 追擊行為
    /// </summary>
    void OnChaseBehavior()
    {
        OnCheckClosestPlayer();//檢查最近玩家
        OnAttackRangeCheck();//攻擊範圍偵測        

        //朝玩家移動
        transform.position = transform.position + transform.forward * chaseSpeed * Time.deltaTime;
    }

    /// <summary>
    /// 攻擊範圍偵測
    /// </summary>
    void OnAttackRangeCheck()
    {
        //更換狀態偵測
        if (OnDetectionRange(radius: attackRadius))
        {            
            OnChangeState(state: AIState.攻擊狀態, openAnimationName: "NormalAttack", closeAnimationName: "Run");
        } 
        else
        {
            if (!info.IsName("NormalAttack"))
            {                
                OnChangeState(state: AIState.追擊狀態, openAnimationName: "Run", closeAnimationName: "AttackIdle");
            }
        }
    }
    bool isAttackIdle;//是否攻擊待機
    /// <summary>
    /// 攻擊狀態行為
    /// </summary>
    void OnAttackBehavior()
    {        
        info = animator.GetCurrentAnimatorStateInfo(0);

        OnAttackRangeCheck();
        if (!info.IsName("NormalAttack")) OnCheckClosestPlayer();//檢查最近玩家


        if (info.IsName("NormalAttack") && info.normalizedTime >= 1)
        {
            if (!isAttackIdle)//是否攻擊待機
            {
                isAttackIdle = true;

                OnChangeAnimation("AttackIdle", true);
                OnChangeAnimation("NormalAttack", false);
                
                StartCoroutine(OnWaitAttack());
            }
        }
    }

    /// <summary>
    /// 等待攻擊
    /// </summary>
    /// <returns></returns>
    IEnumerator OnWaitAttack()
    {
        //攻擊時間
        float attackTime = Random.Range(attackFrequency[0], attackFrequency[1]);
        yield return new WaitForSeconds(attackTime);

        isAttackIdle = false;

        OnChangeAnimation(animationName: "AttackIdle", isAnimationActive: false);
        OnChangeAnimation(animationName: "NormalAttack", isAnimationActive: true);
    }

    /// <summary>
    /// 偵測範圍
    /// </summary>
    /// <param name="radius">偵測半徑</param>  
    bool OnDetectionRange(float radius)
    {
        if (Physics.CheckSphere(transform.position, radius, mask))
        {
            //尋找所有玩家
            if (players[players.Length - 1] == null) players = GameObject.FindGameObjectsWithTag("Player");

            return true;
        }

        return false;
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
            OnChangeAnimation(animationName: openAnimationName, isAnimationActive: true);
            OnChangeAnimation(animationName: closeAnimationName, isAnimationActive: false);            
        }
    }

    /// <summary>
    /// 更換動畫
    /// </summary>
    /// <param name="animationName">執行動畫名稱</param>
    /// <param name="isAnimationActive">動畫是否執行</param>
    void OnChangeAnimation(string animationName, bool isAnimationActive)
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        animator.SetBool(animationName, isAnimationActive);
        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, animationName, isAnimationActive);
    }

    /// <summary>
    /// 檢查最近玩家
    /// </summary>
    void OnCheckClosestPlayer()
    {
        alertCheckTime -= Time.deltaTime;//警戒狀態偵測偵測時間

        if(alertCheckTime < 0)
        {
            alertObject = 0;//警戒對象編號
            float closestPlayerDistance = 100000;//最近距離
            float distance;//其他玩家距離

            for (int i = 0; i < players.Length; i++)
            {
                distance = (players[i].transform.position - transform.position).magnitude;
                if (distance < closestPlayerDistance)
                {
                    closestPlayerDistance = distance;
                    alertObject = i;
                }
            }

            alertCheckTime = alertCheckDistanceTime;
        }

        //觀看最近玩家
        float maxRadiansDelta = 0.03f;//轉向角度
        Vector3 forward = players[alertObject].transform.position - transform.position;//面相玩家向量
        transform.forward = Vector3.RotateTowards(transform.forward, forward, maxRadiansDelta, maxRadiansDelta);
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
    }
}
