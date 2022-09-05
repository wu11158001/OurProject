using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Exclusive : MonoBehaviourPunCallbacks
{
    Animator animator;
    GameData_NumericalValue NumericalValue;

    void Start()
    {
        animator = GetComponent<Animator>();
        NumericalValue = GameDataManagement.Instance.numericalValue; 
    }

    /// <summary>
    /// 攻擊1_Boss(飛行攻擊)
    /// </summary>
    void OnAttack1_Boss()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//爆擊攻擊提升倍率

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//執行攻擊腳色
        attack.performObject = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber("bossAttack1"), GameSceneManagement.Instance.loadPath.bossAttack1);//執行攻擊的物件(自身/射出物件)
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetShootFunction_Group);//設定執行函式       
        attack.damage = NumericalValue.bossAttack1_Damge * rate;//造成傷害 
        attack.direction = NumericalValue.bossAttack1_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.bossAttack1_RepelDistance;//擊退/擊飛距離
        attack.animationName = NumericalValue.bossAttack1_Effect;//攻擊效果(播放動畫名稱)        

        attack.flightSpeed = NumericalValue.bossAttack1_FloatSpeed;//飛行速度
        attack.lifeTime = NumericalValue.bossAttack1_LifeTime;//生存時間
        attack.flightDiration = transform.forward;//飛行方向        

        attack.performObject.transform.SetParent(gameObject.transform);
        attack.performObject.transform.localPosition = new Vector3(0, 5.5f, -1);

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//加入List(執行)       
    }

    /// <summary>
    /// 攻擊2_Boss(頭攻擊)
    /// </summary>
    void OnAttack2_Boss()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//爆擊攻擊提升倍率
        float getDamage = NumericalValue.bossAttack2_Damge * rate;//造成傷害

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//執行攻擊腳色
        attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetHitSphereFunction);//設定執行函式
        attack.damage = getDamage;//造成傷害 
        attack.direction = NumericalValue.bossAttack2_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.bossAttack2_RepelDistance;//擊退距離
        attack.animationName = NumericalValue.bossAttack2_Effect;//攻擊效果(播放動畫名稱)
        attack.forwardDistance = NumericalValue.bossAttack2_ForwardDistance;//攻擊範圍中心點距離物件前方
        attack.attackRadius = NumericalValue.bossAttack2_attackRadius;//攻擊半徑
        attack.isAttackBehind = NumericalValue.bossAttack2_IsAttackBehind;//是否攻擊背後敵人

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//加入List(執行)           
    }
}
