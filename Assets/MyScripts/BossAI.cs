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
    [SerializeField] float attackRadius;//攻擊半徑
    [SerializeField] GameObject target;//攻擊目標

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
        attackRadius = 5;//攻擊半徑

        //追擊
        chaseSpeed = 6.3f;//追擊速度

        fineTargetTime = 5;//尋找玩家時間
    }
     
    void Update()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);
        Debug.LogError((transform.position - target.transform.position).magnitude);
        //OnJudgeAnimation();//判斷動畫

        if(state == State.待機狀態)
        {
            OnStartDistance();
        }
        if(state == State.追擊狀態)
        {
            OnFindTargetTime();//尋找目標時間
            OnRotateToTarget();//轉向至目標
            OnChaseTarget();//追擊目標
        }
    }

    public enum State
    {
        待機狀態,
        追擊狀態,
        攻擊狀態
    }
    State state;

    /// <summary>
    /// 開始戰鬥範圍
    /// </summary>
    void OnStartDistance()
    {
        if (Physics.CheckSphere(transform.position, 19, 1 << LayerMask.NameToLayer("Player")))
        {
            state = State.追擊狀態;
            allPlayer = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < allPlayer.Length; i++)
            {
                playersDamage.Add(allPlayer[i], 0);
            }
            
            OnChangeAnimation(animationName: "Roar", animationType: true);
        }
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
    void OnRotateToTarget()
    {
        if (target != null || target.activeSelf)
        {
            //轉向目標
            transform.forward = Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, 0.03f, 0.03f);
            transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
        }
    }

    /// <summary>
    /// 追擊目標
    /// </summary>
    void OnChaseTarget()
    {
        //小大於攻擊範圍
        if((transform.position - target.transform.position).magnitude > attackRadius && info.IsTag("Run"))
        {
            transform.position = transform.position + transform.forward * chaseSpeed * Time.deltaTime;
        }
    }

    /// <summary>
    /// 判斷動畫
    /// </summary>
    void OnJudgeAnimation()
    {
        if(info.IsTag("Roar") && info.normalizedTime >= 1)
        {

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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5);
    }
}
