using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// 敵人控制控制
/// </summary>
public class EnemyControl : MonoBehaviourPunCallbacks
{
    private GameObject playerUnit;          //獲取玩家單位
    private Animator thisAnimator;          //自身動畫組件
    private Vector3 initialPosition;            //初始位置

    public float wanderRadius;          //遊走半徑，移動狀態下，如果超出遊走半徑會返回出生位置
    public float alertRadius;         //警戒半徑，玩家進入後怪物會發出警告，並一直面朝玩家
    public float defendRadius;          //自衛半徑，玩家進入後怪物會追擊玩家，當距離<攻擊距離則會發動攻擊（或者觸發戰鬥）
    public float chaseRadius;            //追擊半徑，當怪物超出追擊半徑後會放棄追擊，返回追擊起始位置

    public float attackRange;            //攻擊距離
    public float walkSpeed;          //移動速度
    public float runSpeed;          //跑動速度
    public float turnSpeed;         //轉身速度，建議0.1

    private enum MonsterState
    {
        STAND,      //原地呼吸
        CHECK,       //原地觀察
        WALK,       //移動
        WARN,       //盯着玩家
        CHASE,      //追擊玩家
        RETURN      //超出追擊範圍後返回
    }
    private MonsterState currentState = MonsterState.STAND;          //默認狀態爲原地呼吸

    public float[] actionWeight = { 3000, 3000, 4000 };         //設置待機時各種動作的權重，順序依次爲呼吸、觀察、移動
    public float actRestTme;            //更換待機指令的間隔時間
    private float lastActTime;          //最近一次指令時間

    private float diatanceToPlayer;         //怪物與玩家的距離
    private float diatanceToInitial;         //怪物與初始位置的距離
    private Quaternion targetRotation;         //怪物的目標朝向

    private bool is_Warned = false;
    private bool is_Running = false;

    void Start()
    {
        playerUnit = GameObject.FindGameObjectWithTag("Player");
        thisAnimator = GetComponent<Animator>();

        //保存初始位置信息
        initialPosition = gameObject.GetComponent<Transform>().position;

        //檢查並修正怪物設置
        //1. 自衛半徑不大於警戒半徑，否則就無法觸發警戒狀態，直接開始追擊了
        defendRadius = Mathf.Min(alertRadius, defendRadius);
        //2. 攻擊距離不大於自衛半徑，否則就無法觸發追擊狀態，直接開始戰鬥了
        attackRange = Mathf.Min(defendRadius, attackRange);
        //3. 遊走半徑不大於追擊半徑，否則怪物可能剛剛開始追擊就返回出生點
        wanderRadius = Mathf.Min(chaseRadius, wanderRadius);

        //隨機一個待機動作
        RandomAction();
    }

    /// <summary>
    /// 根據權重隨機待機指令
    /// </summary>
    void RandomAction()
    {
        //更新行動時間
        lastActTime = Time.time;
        //根據權重隨機
        float number = Random.Range(0, actionWeight[0] + actionWeight[1] + actionWeight[2]);
        if (number <= actionWeight[0])
        {
            currentState = MonsterState.STAND;
            thisAnimator.SetTrigger("Stand");
        }
        else if (actionWeight[0] < number && number <= actionWeight[0] + actionWeight[1])
        {
            currentState = MonsterState.CHECK;
            thisAnimator.SetTrigger("Check");
        }
        if (actionWeight[0] + actionWeight[1] < number && number <= actionWeight[0] + actionWeight[1] + actionWeight[2])
        {
            currentState = MonsterState.WALK;
            //隨機一個朝向
            targetRotation = Quaternion.Euler(0, Random.Range(1, 5) * 90, 0);
            thisAnimator.SetTrigger("Walk");
        }
    }

    void Update()
    {
        switch (currentState)
        {
            //待機狀態，等待actRestTme後重新隨機指令
            case MonsterState.STAND:
                if (Time.time - lastActTime > actRestTme)
                {
                    RandomAction();         //隨機切換指令
                }
                //該狀態下的檢測指令
                EnemyDistanceCheck();
                break;

            //待機狀態，由於觀察動畫時間較長，並希望動畫完整播放，故等待時間是根據一個完整動畫的播放長度，而不是指令間隔時間
            case MonsterState.CHECK:
                if (Time.time - lastActTime > thisAnimator.GetCurrentAnimatorStateInfo(0).length)
                {
                    RandomAction();         //隨機切換指令
                }
                //該狀態下的檢測指令
                EnemyDistanceCheck();
                break;

            //遊走，根據狀態隨機時生成的目標位置修改朝向，並向前移動
            case MonsterState.WALK:
                transform.Translate(Vector3.forward * Time.deltaTime * walkSpeed);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed);

                if (Time.time - lastActTime > actRestTme)
                {
                    RandomAction();         //隨機切換指令
                }
                //該狀態下的檢測指令
                WanderRadiusCheck();
                break;

            //警戒狀態，播放一次警告動畫和聲音，並持續朝向玩家位置
            case MonsterState.WARN:
                if (!is_Warned)
                {
                    thisAnimator.SetTrigger("Warn");
                    //               gameObject.GetComponent<AudioSource>().Play();
                    is_Warned = true;
                }
                //持續朝向玩家位置
                targetRotation = Quaternion.LookRotation(playerUnit.transform.position - transform.position, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed);
                //該狀態下的檢測指令
                WarningCheck();
                break;

            //追擊狀態，朝着玩家跑去
            case MonsterState.CHASE:
                if (!is_Running)
                {
                    thisAnimator.SetTrigger("Run");
                    is_Running = true;
                }
                transform.Translate(Vector3.forward * Time.deltaTime * runSpeed);
                //朝向玩家位置
                targetRotation = Quaternion.LookRotation(playerUnit.transform.position - transform.position, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed);
                //該狀態下的檢測指令
                ChaseRadiusCheck();
                break;

            //返回狀態，超出追擊範圍後返回出生位置
            case MonsterState.RETURN:
                //朝向初始位置移動
                targetRotation = Quaternion.LookRotation(initialPosition - transform.position, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed);
                transform.Translate(Vector3.forward * Time.deltaTime * runSpeed);
                //該狀態下的檢測指令
                ReturnCheck();
                break;
        }
    }

    /// <summary>
    /// 原地呼吸、觀察狀態的檢測
    /// </summary>
    void EnemyDistanceCheck()
    {
        diatanceToPlayer = Vector3.Distance(playerUnit.transform.position, transform.position);
        //      if (diatanceToPlayer < attackRange)
        //      {
        //          SceneManager.LoadScene("Battle");
        //      }
        if (diatanceToPlayer < defendRadius)
        {
            currentState = MonsterState.CHASE;
        }
        else if (diatanceToPlayer < alertRadius)
        {
            currentState = MonsterState.WARN;
        }
    }

    /// <summary>
    /// 警告狀態下的檢測，用於啓動追擊及取消警戒狀態
    /// </summary>
    void WarningCheck()
    {
        diatanceToPlayer = Vector3.Distance(playerUnit.transform.position, transform.position);
        if (diatanceToPlayer < defendRadius)
        {
            is_Warned = false;
            currentState = MonsterState.CHASE;
        }

        if (diatanceToPlayer > alertRadius)
        {
            is_Warned = false;
            RandomAction();
        }
    }

    /// <summary>
    /// 遊走狀態檢測，檢測敵人距離及遊走是否越界
    /// </summary>
    void WanderRadiusCheck()
    {
        diatanceToPlayer = Vector3.Distance(playerUnit.transform.position, transform.position);
        diatanceToInitial = Vector3.Distance(transform.position, initialPosition);

        if (diatanceToPlayer < attackRange)
        {
            SceneManager.LoadScene("Battle");
        }
        else if (diatanceToPlayer < defendRadius)
        {
            currentState = MonsterState.CHASE;
        }
        else if (diatanceToPlayer < alertRadius)
        {
            currentState = MonsterState.WARN;
        }

        if (diatanceToInitial > wanderRadius)
        {
            //朝向調整爲初始方向
            targetRotation = Quaternion.LookRotation(initialPosition - transform.position, Vector3.up);
        }
    }

    /// <summary>
    /// 追擊狀態檢測，檢測敵人是否進入攻擊範圍以及是否離開警戒範圍
    /// </summary>
    void ChaseRadiusCheck()
    {
        diatanceToPlayer = Vector3.Distance(playerUnit.transform.position, transform.position);
        diatanceToInitial = Vector3.Distance(transform.position, initialPosition);

        //       if (diatanceToPlayer < attackRange)
        //       {
        //           SceneManager.LoadScene("Battle");
        //       }
        //如果超出追擊範圍或者敵人的距離超出警戒距離就返回
        if (diatanceToInitial > chaseRadius || diatanceToPlayer > alertRadius)
        {
            currentState = MonsterState.RETURN;
        }
    }

    /// <summary>
    /// 超出追擊半徑，返回狀態的檢測，不再檢測敵人距離
    /// </summary>
    void ReturnCheck()
    {
        diatanceToInitial = Vector3.Distance(transform.position, initialPosition);
        //如果已經接近初始位置，則隨機一個待機狀態
        if (diatanceToInitial < 0.5f)
        {
            is_Running = false;
            RandomAction();
        }
    }



    Animator animator;

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");//設定Layer        
        gameObject.tag = "EnemySoldier_1";//設定Tag
        animator = GetComponent<Animator>();

        //if (GetComponent<CharactersCollision>() == null) gameObject.AddComponent<CharactersCollision>();

        //連線 && 不是自己的
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            GameSceneManagement.Instance.OnSetMiniMapPoint(transform, GameSceneManagement.Instance.loadPath.miniMapMatirial_Enemy);//設定小地圖點點
            this.enabled = false;
            return;
        }
    }
}
