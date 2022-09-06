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

    //碰撞框
    Vector3 boxCenter;
    Vector3 boxSize;

    [Header("移動")]
    float walkSpeed;//行走速度
    float flyAttackSpeed;//飛行攻擊速度
    float flyAttackUpSpeed;//飛行攻擊上升速度

    [Header("攻擊")]
    GameObject attackTarget;//攻擊對象
    int maxAttackNumber;//擁有的攻擊招式
    [SerializeField]int activeAttackNumber;//使用的攻擊招式
    float[] attackDelayTime;//攻擊延遲時間(最小值,最大值)
    [SerializeField] float attackTime;//攻擊時間

    void Start()
    {
        animator = GetComponent<Animator>();

        allPlayer = GameObject.FindGameObjectsWithTag("Player");

        //尋找最近玩家
        OnFineClosestplayer();

        //碰撞框
        boxCenter = GetComponent<BoxCollider>().center;
        boxSize = GetComponent<BoxCollider>().size;

        //移動
        walkSpeed = 2;//行走速度
        flyAttackSpeed = 30;//飛行攻擊速度
        flyAttackUpSpeed = 20;//飛行攻擊上升速度
        //攻擊
        maxAttackNumber = 2;//擁有的攻擊招式
        attackDelayTime = new float[] { 0.5f, 5f};//攻擊延遲時間(最小值,最大值)
        attackTime = 3;//攻擊時間
    }
     
    void Update()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        OnJudgeAnimation();//判斷動畫
    }

    /// <summary>
    /// 尋找最近玩家
    /// </summary>
    void OnFineClosestplayer()
    {
        float closest = 1000;
        int target = 0;
        for (int i = 0; i < allPlayer.Length; i++)
        {
            float dir = (transform.position - allPlayer[i].transform.position).magnitude;
            if(dir < closest)
            {
                closest = dir;
                target = i;
            }
        }

        attackTarget = allPlayer[target];
    }

    /// <summary>
    /// 轉向至目標
    /// </summary>
    void OnRotateToTarget()
    {
        //轉向目標
        transform.forward = Vector3.RotateTowards(transform.forward, attackTarget.transform.position - transform.position, 0.065f, 0.065f);
        transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
    }

    /// <summary>
    /// 攻擊時間
    /// </summary>
    void OnAttackTime()
    {
        attackTime -= Time.deltaTime;//攻擊時間
        if (attackTime <= 0)
        {
            attackTime = UnityEngine.Random.Range(attackDelayTime[0], attackDelayTime[1]);//攻擊時間
            activeAttackNumber = UnityEngine.Random.Range(1, maxAttackNumber + 1);//使用的攻擊招式

            //行走機率
            if (UnityEngine.Random.Range(0, 100) < 50) OnChangeAnimation(animationName: "Walk", animationType: true);
            else OnChangeAnimation(animationName: "AttackNumber", animationType: activeAttackNumber);
        }
    }

    /// <summary>
    /// 判斷動畫
    /// </summary>
    void OnJudgeAnimation()
    {
        //待機狀態
        if(info.IsTag("Idle"))
        {
            OnAttackTime();//攻擊時間 
            OnRotateToTarget();//轉向至目標
        }

        //咆嘯狀態
        if (info.IsName("Roar"))
        {
            //transform.position = transform.position + transform.forward * walkSpeed * Time.deltaTime;

            //進行攻擊
            if (info.normalizedTime >= 1)
            {
                OnChangeAnimation(animationName: "Walk", animationType: false);
                OnChangeAnimation(animationName: "AttackNumber", animationType: activeAttackNumber);
            }
        }

        //飛行攻擊
        if(info.IsName("FlyAttack"))
        {            
            transform.position = transform.position + transform.forward * flyAttackSpeed / 2 * Time.deltaTime;
        }

        //攻擊結束
        if (info.IsTag("Attack") && info.normalizedTime >= 1) OnChangeAnimation(animationName: "AttackNumber", animationType: 0);
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
        Gizmos.DrawWireSphere(transform.position + boxCenter + transform.forward * 5, 3);
    }
}
