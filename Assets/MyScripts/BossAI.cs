using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    GameObject[] allPlayer;//所有玩家    

    [Header("攻擊")]
    GameObject AttackTarget;//攻擊對象
    int maxAttackNumber;//擁有的攻擊招式
    int activeAttackNumber;//使用的攻擊招式
    float[] attackDelayTime;//攻擊延遲時間(最小值,最大值)
    float attackTime;//攻擊時間

    void Start()
    {
        allPlayer = GameObject.FindGameObjectsWithTag("Player");
        AttackTarget = allPlayer[0];

        //攻擊
        maxAttackNumber = 1;//擁有的攻擊招式
        attackDelayTime = new float[] { 0.5f, 2.5f};//攻擊延遲時間(最小值,最大值)
        attackTime = 3;//攻擊時間
    }
     
    void Update()
    {
        
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
        }
    }
}
