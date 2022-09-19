using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviourPunCallbacks
{
    Animator animator;
    AnimatorStateInfo info;

    [SerializeField] GameObject[] allPlayer;//所有玩家 

    [SerializeField] Dictionary<int, float> allPlayerDamage = new Dictionary<int, float>();//紀錄所有玩家傷害

    //碰撞框
    Vector3 boxCenter;
    Vector3 boxSize;

    [Header("攻擊")]
    float longAttackRadius;//攻擊半徑(遠距離)
    float closeAttackRadius;//攻擊半徑(近距離)
    [SerializeField] GameObject target;//攻擊目標
    float[] attackRandomTime;//攻擊亂數時間(最小,最大)
    float attackTime;//攻擊時間(計時器)
    int maxAttackNumber;//擁有攻擊招式
    int attackNumber;//使用攻擊招式

    [Header("攻擊待機")]
    bool isStart;//是否開始
    [SerializeField] float attackIdleTime;//攻擊待機時間(計時器)
    float maxAttackIdleTime;//最大攻擊待機時間    

    [Header("追擊")]
    float chaseSpeed;//追擊速度
    //尋找
    float fineTargetTime;//尋找目標時間
    float findTime;//尋找目標時間(計時器)

    [Header("攻擊3")]
    [SerializeField] float attack3Time;
    [SerializeField] bool isAttacked;
    public GameObject boomPos;

    [Header("移動")]
    bool isMoveUp;//是否向上移動
    float moveTime;
    bool isMove;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        transform.position = new Vector3(4.8f, 2.5f, -3f);//設定位置

        //連線 && 不是自己的
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            GameSceneManagement.Instance.OnSetMiniMapPoint(transform, GameSceneManagement.Instance.loadPath.miniMapMatirial_Enemy);//設定小地圖點點
            this.enabled = false;
            return;
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();

        //碰撞框
        boxCenter = GetComponent<BoxCollider>().center;
        boxSize = GetComponent<BoxCollider>().size;
        GetComponent<BoxCollider>().enabled = false;

        //攻擊
        longAttackRadius = 30;//攻擊半徑(遠距離)
        closeAttackRadius = 10;//攻擊半徑(近距離)
        attackRandomTime = new float[] { 0.5f, 1.3f };//攻擊亂數時間(最小,最大)
        maxAttackNumber = 4;//擁有攻擊招式

        //攻擊待機
        maxAttackIdleTime = 1.5f;//最大攻擊待機時間

        //追擊
        chaseSpeed = 5.3f;//追擊速度

        fineTargetTime = 3;//尋找玩家時間        

        state = State.待機狀態;
    }

    void Update()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        OnJudgeAnimation();//判斷動畫

        if (state == State.追擊狀態)
        {
            OnFindTargetTime();//尋找目標時間
            OnRotateToTarget();//轉向至目標
            if (isStart) OnChaseTarget();//追擊目標
        }

        if (state == State.攻擊狀態)
        {
            OnFindTargetTime();//尋找目標時間
            OnAttaclIdleTime();//攻擊待機時間
        }

        OnAttack3JudgeTime();
        OnMove();//移動控制
    }


    public enum State
    {
        待機狀態,
        追擊狀態,
        攻擊狀態
    }
    [Header("狀態")]
    public State state;

    /// <summary>
    /// 激活行動
    /// </summary>
    public void OnActive()
    {
        if (state != State.追擊狀態)
        {
            state = State.追擊狀態;
            allPlayer = GameObject.FindGameObjectsWithTag("Player");

            if (GameDataManagement.Instance.isConnect)
            {
                for (int i = 0; i < allPlayer.Length; i++)
                {
                    allPlayerDamage.Add(allPlayer[i].GetComponent<PhotonView>().ViewID, 0);
                }
            }

            OnChangeAnimation(animationName: "Roar", animationType: true);
        }
    }

    /// <summary>
    /// 攻擊3攻擊時間判斷
    /// </summary>
    void OnAttack3JudgeTime()
    {
        if (boomPos.transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying)
        {
            attack3Time += Time.deltaTime;

            //攻擊時間
            if (attack3Time >= 2f && !isAttacked)
            {
                GetComponent<Boss_Exclusive>().OnAttack3_Boss();
                isAttacked = true;
                StartCoroutine(OnWaitAttack3());
            }
        }
    }

    /// <summary>
    /// 等待攻擊3
    /// </summary>
    /// <returns></returns>
    IEnumerator OnWaitAttack3()
    {
        yield return new WaitForSeconds(0.67f);

        isAttacked = false;
        attack3Time = 0;
    }

    /// <summary>
    /// 移動控制
    /// </summary>
    private void OnMove()
    {
        if (!info.IsTag("Attack") && GetComponent<CharactersCollision>().Hp > 450)
        {
            float speed = 2.5f;

            if (transform.position.y >= 4.5f) isMoveUp = false;
            if (transform.position.y <= 2.5) isMoveUp = true;           

            if (isMove)
            {
                if (isMoveUp) transform.position = transform.position + Vector3.up * speed * Time.deltaTime;
                else
                {
                    transform.position = transform.position - Vector3.up * speed * Time.deltaTime;
                    if (transform.position.y <= 2.5) isMove = false;
                }
            }
            else
            {
                moveTime -= Time.deltaTime;
                if (moveTime <= 0)
                {
                    moveTime = 3;

                    isMove = true;
                }
            }
        }
    }

    /// <summary>
    /// 尋找目標時間
    /// </summary>
    void OnFindTargetTime()
    {
        findTime -= Time.deltaTime;

        if (findTime <= 0)
        {
            findTime = fineTargetTime;
            if (!GameDataManagement.Instance.isConnect) OnFindTarget();//尋找目標
            else OnFindTarget_Connect();//尋找目標_連線
        }
    }

    /// <summary>
    /// 紀錄傷害
    /// </summary>
    /// <param name="id">玩家ID</param>
    /// <param name="damage">傷害</param>
    public void OnSetRecordDamage(int id, float damage)
    {
        allPlayerDamage[id] += damage;
    }

    /// <summary>
    /// 尋找目標_連線
    /// </summary>
    void OnFindTarget_Connect()
    {
        float bestDamage = 0;
        int number = 0;
        int targetNumber = 0;
        foreach (var player in allPlayerDamage)
        {
            if (player.Value > bestDamage)
            {
                if (allPlayer[targetNumber].activeSelf)
                {
                    bestDamage = player.Value;
                    targetNumber = number;
                }

            }
            number++;
        }

        if (bestDamage != 0 && allPlayer[targetNumber].GetComponent<CharactersCollision>().Hp > 0)
        {
            target = allPlayer[targetNumber];
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendBossTarget(target.GetComponent<PhotonView>().ViewID);//發送目標物件
            else GameSceneManagement.Instance.BossTargetObject = target;
        }
        else OnFindTarget();
    }

    /// <summary>
    /// 尋找目標
    /// </summary>
    void OnFindTarget()
    {
        //最近為目標
        float closestPlayerDistance = 100000;//最近距離
        float distance;//其他玩家距離
        int chaseNumber = 0;//追擊編號
        for (int i = 0; i < allPlayer.Length; i++)
        {
            if (allPlayer[i].GetComponent<CharactersCollision>().Hp > 0)
            {
                distance = (allPlayer[i].transform.position - transform.position).magnitude;
                if (distance < closestPlayerDistance)
                {
                    closestPlayerDistance = distance;
                    chaseNumber = i;
                }
            }
        }

        if (allPlayer[chaseNumber].GetComponent<CharactersCollision>().Hp > 0)
        {
            target = allPlayer[chaseNumber];
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendBossTarget(target.GetComponent<PhotonView>().ViewID);//發送目標物件
            else GameSceneManagement.Instance.BossTargetObject = target;
        }
        else
        {
            for (int i = 0; i < allPlayer.Length; i++)
            {
                if (allPlayer[i].GetComponent<CharactersCollision>().Hp > 0)
                {
                    target = allPlayer[i];
                    if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendBossTarget(target.GetComponent<PhotonView>().ViewID);//發送目標物件
                    else GameSceneManagement.Instance.BossTargetObject = target;

                    return;
                }
            }
        }
    }

    /// <summary>
    /// 轉向至目標
    /// </summary>
    /// <param name="speed">轉向速度</param>
    public void OnRotateToTarget()
    {
        float speed = 2.0f;
        if (info.IsTag("Attack"))
        {
            speed = 0.85f;
            if (info.IsName("Attack.Attack4-3")) speed = 2.5f;            
        }

        if (target != null || target.activeSelf)
        {
            if (!info.IsTag("Die"))
            {
                //轉向目標
                transform.forward = Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, speed * Time.deltaTime, speed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
            }
        }
    }

    /// <summary>
    /// 追擊目標
    /// </summary>
    void OnChaseTarget()
    {
        //小大於攻擊範圍
        if ((transform.position - target.transform.position).magnitude > longAttackRadius)
        {
            if (info.IsName("Fly"))
            {
                transform.position = transform.position + transform.forward * chaseSpeed * Time.deltaTime;
            }
        }
        else
        {
            //追擊後第一次攻擊
            if (state != State.攻擊狀態)
            {
                state = State.攻擊狀態;

                OnAttackNumber();//攻擊招式判斷

                OnChangeAnimation(animationName: "AttackNumber", animationType: attackNumber);
                OnChangeAnimation(animationName: "Fly", animationType: false);
            }
        }
    }

    /// <summary>
    /// 攻擊招式判斷
    /// </summary>
    void OnAttackNumber()
    {
        if ((transform.position - target.transform.position).magnitude <= closeAttackRadius)//使用攻擊招式(近)
        {
            int random = UnityEngine.Random.Range(0, 3);
            if (random == 0) attackNumber = 4;
            else if (random == 1) attackNumber = 3;
            else attackNumber = 2;            
        }
        else attackNumber = UnityEngine.Random.Range(1, 3);//使用攻擊招式(遠)
    }

    /// <summary>
    /// 攻擊待機時間
    /// </summary>
    void OnAttaclIdleTime()
    {
        if (attackIdleTime > 0)
        {
            if (!info.IsTag("Attack"))
            {
                OnRotateToTarget();
                attackIdleTime -= Time.deltaTime;
            }

            if (attackIdleTime <= 0)
            {
                //大於攻擊範圍
                if ((transform.position - target.transform.position).magnitude > longAttackRadius)
                {
                    state = State.追擊狀態;
                    OnChangeAnimation(animationName: "Fly", animationType: true);
                    return;
                }
            }
        }

        if (attackIdleTime <= 0)
        {
            float dir = Vector3.Dot(transform.forward, Vector3.Cross(Vector3.up, target.transform.position - transform.position));
            //已轉向至目標
            if (Vector3.Dot(transform.forward, target.transform.position - transform.position) > 0 && dir > -1 && dir < 1)
            {
                if (!info.IsTag("Attack"))
                {
                    OnAttackNumber();//攻擊招式判斷

                    OnChangeAnimation(animationName: "AttackNumber", animationType: attackNumber);
                    OnChangeAnimation(animationName: "Fly", animationType: false);
                    OnChangeAnimation(animationName: "Pain", animationType: false);
                }
            }
            else
            {
                if (!info.IsTag("Attack")) OnRotateToTarget();//轉向至目標
            }

            attackIdleTime = UnityEngine.Random.Range(1, maxAttackIdleTime);//攻擊待機時間(計時器)
        }
    }

    /// <summary>
    /// 判斷動畫
    /// </summary>
    void OnJudgeAnimation()
    {
        //咆嘯完畢
        if (info.IsTag("Roar") && info.normalizedTime >= 1)
        {
            isStart = true;//是否開始
            OnChangeAnimation(animationName: "Roar", animationType: false);
            OnChangeAnimation(animationName: "Fly", animationType: true);
        }

        /* //攻擊1(飛 噴火)
         if(info.IsName("Attack1") && info.normalizedTime < 0.95f)
         {            
             transform.position = transform.position + Vector3.up * 11 * Time.deltaTime;
         } */

        if (GameDataManagement.Instance.isConnect)
        {
            //攻擊轉身
            if (info.IsTag("Attack") && info.normalizedTime < 0.9f)
            {
                OnRotateToTarget();
            }
        }

        //俯衝攻擊-1
        if(info.IsName("Attack.Attack4-1"))
        {
            transform.position = transform.position + Vector3.up * 5 * Time.deltaTime;
            transform.position = transform.position - transform.forward * 7 * Time.deltaTime;
        }

        //俯衝攻擊-2
        if (info.IsName("Attack.Attack4-2"))
        {
            if (GetComponent<CharactersCollision>().boxSize.y != 1.5f)
            {                
                GetComponent<CharactersCollision>().boxSize = new Vector3(boxSize.x, 1.5f, boxSize.z);
            }            
                        
            if (info.normalizedTime < 0.5f)
            {                
                if(transform.position.y > 0.3f) transform.position = transform.position - Vector3.up * 15 * Time.deltaTime;
                transform.position = transform.position + transform.forward * 20 * Time.deltaTime;
            }
            else transform.position = transform.position + transform.forward * 15 * Time.deltaTime;

            if (info.normalizedTime > 0.7f)
            {
                transform.position = transform.position + Vector3.up * 10 * Time.deltaTime;                
            }
        }

        //俯衝攻擊-3
        if (info.IsName("Attack.Attack4-3"))
        {
            OnRotateToTarget();

            /*if (GetComponent<CharactersCollision>().boxSize.y != 4.85f)
            {
                GetComponent<CharactersCollision>().boxSize = new Vector3(boxSize.x, 4.85f, boxSize.z);
            }*/

            if (transform.position.y > 2.5f)
            {                
                transform.position = transform.position - Vector3.up * 5 * Time.deltaTime;
            }
            else OnChangeAnimation(animationName: "AttackNumber", animationType: 0);
        }

        //攻擊完成
        if (info.IsTag("Attack") && info.normalizedTime >= 1)
        {
            if (!info.IsName("Attack.Attack4-1") && !info.IsName("Attack.Attack4-2") && !info.IsName("Attack.Attack4-3"))
            {
                OnChangeAnimation(animationName: "AttackNumber", animationType: 0);
            }
        }

        //待機狀態
        if (info.IsTag("Idle") && state != State.待機狀態)
        {
            if (target != null && target.activeSelf)
            {

                //OnRotateToTarget();//轉向至目標
            }
        }

        //死亡
        if(info.IsTag("Die"))
        {
            if(transform.position.y != 0.5f) transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
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

        switch (animationType.GetType().Name)
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
        /*Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 10);*/
    }

    public Transform GetTarget()
    {
        return target.transform;
    }
}
