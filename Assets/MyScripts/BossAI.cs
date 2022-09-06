using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviourPunCallbacks
{
    Animator animator;
    AnimatorStateInfo info;

    GameObject[] allPlayer;//所有玩家 

    [SerializeField] Dictionary<GameObject, float> playersDamage = new Dictionary<GameObject, float>();//記錄所有玩家傷害

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
    [SerializeField] float attackIdleTime;//攻擊待機時間(計時器)
    float maxAttackIdleTime;//最大攻擊待機時間    

    [Header("追擊")]
    float chaseSpeed;//追擊速度

    //尋找
    float fineTargetTime;//尋找目標時間
    float findTime;//尋找目標時間(計時器)

    void Start()
    {
        animator = GetComponent<Animator>();

        //碰撞框
        boxCenter = GetComponent<BoxCollider>().center;
        boxSize = GetComponent<BoxCollider>().size;

        //攻擊
        longAttackRadius = 10;//攻擊半徑(遠距離)
        attackRandomTime = new float[] { 0.5f, 3.0f};//攻擊亂數時間(最小,最大)
        maxAttackNumber = 1;//擁有攻擊招式

        //攻擊待機
        maxAttackIdleTime = 3;//最大攻擊待機時間

        //追擊
        chaseSpeed = 6.3f;//追擊速度

        fineTargetTime = 5;//尋找玩家時間

        state = State.待機狀態;
    }
     
    void Update()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);
        
        OnJudgeAnimation();//判斷動畫

        if(state == State.追擊狀態)
        {
            OnFindTargetTime();//尋找目標時間
            OnRotateToTarget(0.03f);//轉向至目標
            OnChaseTarget();//追擊目標
        }

        if(state == State.攻擊狀態)
        {
            OnAttaclIdleTime();//攻擊待機時間
        }
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
        state = State.追擊狀態;
        allPlayer = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < allPlayer.Length; i++)
        {
            playersDamage.Add(allPlayer[i], 0);
        }

        OnChangeAnimation(animationName: "Roar", animationType: true);
    } 

    /// <summary>
    /// 尋找目標時間
    /// </summary>
    void OnFindTargetTime()
    {
        findTime -= Time.deltaTime;

        if(findTime <= 0)
        {
            findTime = fineTargetTime;
            OnFindTarget();//尋找目標
        }
    }

    /// <summary>
    /// 尋找目標
    /// </summary>
    void OnFindTarget()
    {
        //傷害最高為目標
        float number = -1;
        int i = 0;
        int bestDamage = 0;
        foreach (var player in playersDamage)
        {
            if (player.Value > number)
            {
                number = player.Value;
                bestDamage = i;
            }

            i++;
        }

        target = allPlayer[bestDamage];
    }

    /// <summary>
    /// 轉向至目標
    /// </summary>
    /// <param name="speed">轉向速度</param>
    void OnRotateToTarget(float speed)
    {
        if (target != null || target.activeSelf)
        {            
            //轉向目標
            transform.forward = Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, speed, speed);
            transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
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
            if (info.IsTag("Run"))
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

                attackNumber = 1;//UnityEngine.Random.Range(1, maxAttackNumber + 1);//使用攻擊招式
                OnChangeAnimation(animationName: "AttackNumber", animationType: attackNumber);
                OnChangeAnimation(animationName: "Run", animationType: false);

                OnChangeAnimation(animationName: "TurnLeft", animationType: false);
                OnChangeAnimation(animationName: "TurnRight", animationType: false);
            }
        }
    }

    /// <summary>
    /// 攻擊待機時間
    /// </summary>
    void OnAttaclIdleTime()
    {
        if(attackIdleTime > 0 && !info.IsTag("Attack"))
        {
            attackIdleTime -= Time.deltaTime;

          
            if (attackIdleTime <= 0)
            {
                float dir = Vector3.Dot(transform.forward, Vector3.Cross(Vector3.up, target.transform.position - transform.position));
                if (dir < 0)
                {
                    if (!info.IsTag("Turn_Right")) OnChangeAnimation(animationName: "TurnRight", animationType: true);
                }
                else
                {                    
                    if (!info.IsTag("Turn_Lift")) OnChangeAnimation(animationName: "TurnLeft", animationType: true);
                }                     
            }
        }

        if(attackIdleTime <= 0)
        {
            if(info.IsTag("Turn_Lift") || info.IsTag("Turn_Right")) OnRotateToTarget(0.02f);//轉向至目標
            
            float dir = Vector3.Dot(transform.forward, Vector3.Cross(Vector3.up, target.transform.position - transform.position));
            
            //已轉向至目標
            if (dir > -1 && dir < 1)
            {
                //大於攻擊範圍
                if ((transform.position - target.transform.position).magnitude > longAttackRadius)
                {
                    if (!info.IsTag("Run"))
                    {                        
                        state = State.追擊狀態;
                        OnChangeAnimation(animationName: "Run", animationType: true);
                    }
                }
                else
                {
                    if (state != State.攻擊狀態) state = State.攻擊狀態;

                    if (!info.IsTag("Attack"))
                    {
                        attackNumber = UnityEngine.Random.Range(1, maxAttackNumber + 1);//使用攻擊招式
                        OnChangeAnimation(animationName: "AttackNumber", animationType: attackNumber);
                        OnChangeAnimation(animationName: "Run", animationType: false);
                        OnChangeAnimation(animationName: "TurnLeft", animationType: false);
                        OnChangeAnimation(animationName: "TurnRight", animationType: false);
                        OnChangeAnimation(animationName: "Pain", animationType: false);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 判斷動畫
    /// </summary>
    void OnJudgeAnimation()
    {
        //咆嘯完畢
        if(info.IsTag("Roar") && info.normalizedTime >= 1)
        {
            OnChangeAnimation(animationName: "Roar", animationType: false);
            OnChangeAnimation(animationName: "Run", animationType: true);
        }     
        
       /* //攻擊1(飛 噴火)
        if(info.IsName("Attack1") && info.normalizedTime < 0.95f)
        {            
            transform.position = transform.position + Vector3.up * 11 * Time.deltaTime;
        } */              

        //攻擊完成
        if(info.IsTag("Attack") && info.normalizedTime >= 1)
        {
            OnChangeAnimation(animationName: "AttackNumber", animationType: 0);

            attackIdleTime = UnityEngine.Random.Range(1, maxAttackIdleTime);//攻擊待機時間(計時器)
        }

        //待機狀態
        if(info.IsTag("Idle") && state != State.待機狀態)
        {
            if (target != null && target.activeSelf)
            {
                
                //OnRotateToTarget();//轉向至目標
            }
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
       /* Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 18);*/
    }
}
