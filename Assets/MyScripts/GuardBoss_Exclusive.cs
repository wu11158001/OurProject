using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 守衛Boss專用
/// </summary>
public class GuardBoss_Exclusive : MonoBehaviourPunCallbacks
{
    GameData_NumericalValue NumericalValue;

    void Start()
    {
        NumericalValue = GameDataManagement.Instance.numericalValue;
    }

    /// <summary>
    /// 攻擊1_守衛Boss
    /// </summary>
    void OnAttack1_GuardBoss()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率
        float getDamage = NumericalValue.guardBoss_Attack_1_Damge * rate;//造成傷害

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//執行攻擊腳色
        attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetHitSphereFunction);//設定執行函式
        attack.damage = getDamage;//造成傷害 
        attack.direction = NumericalValue.guardBoss_Attack_1_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.guardBoss_Attack_1_RepelDistance;//擊退距離
        attack.animationName = NumericalValue.guardBoss_Attack_1_Effect;//攻擊效果(播放動畫名稱)
        attack.forwardDistance = NumericalValue.guardBoss_Attack_1_ForwardDistance;//攻擊範圍中心點距離物件前方
        attack.attackRadius = NumericalValue.guardBoss_Attack_1_attackRadius;//攻擊半徑
        attack.isAttackBehind = NumericalValue.guardBoss_Attack_1_IsAttackBehind;//是否攻擊背後敵人

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)                  
    }

    /// <summary>
    /// 攻擊2_守衛Boss
    /// </summary>
    void OnAttack2_GuardBoss()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率
        float getDamage = NumericalValue.guardBoss_Attack_2_Damge * rate;//造成傷害

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//執行攻擊腳色
        attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetHitSphereFunction);//設定執行函式
        attack.damage = getDamage;//造成傷害 
        attack.direction = NumericalValue.guardBoss_Attack_2_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.guardBoss_Attack_2_RepelDistance;//擊退距離
        attack.animationName = NumericalValue.guardBoss_Attack_2_Effect;//攻擊效果(播放動畫名稱)
        attack.forwardDistance = NumericalValue.guardBoss_Attack_2_ForwardDistance;//攻擊範圍中心點距離物件前方
        attack.attackRadius = NumericalValue.guardBoss_Attack_2_attackRadius;//攻擊半徑
        attack.isAttackBehind = NumericalValue.guardBoss_Attack_2_IsAttackBehind;//是否攻擊背後敵人

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)                  
    }

    /// <summary>
    /// 攻擊3_守衛Boss
    /// </summary>
    void OnAttack3_GuardBoss()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率
        float getDamage = NumericalValue.guardBoss_Attack_3_Damge * rate;//造成傷害

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//執行攻擊腳色
        attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetHitSphereFunction);//設定執行函式
        attack.damage = getDamage;//造成傷害 
        attack.direction = NumericalValue.guardBoss_Attack_3_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.guardBoss_Attack_3_RepelDistance;//擊退距離
        attack.animationName = NumericalValue.guardBoss_Attack_3_Effect;//攻擊效果(播放動畫名稱)
        attack.forwardDistance = NumericalValue.guardBoss_Attack_3_ForwardDistance;//攻擊範圍中心點距離物件前方
        attack.attackRadius = NumericalValue.guardBoss_Attack_3_attackRadius;//攻擊半徑
        attack.isAttackBehind = NumericalValue.guardBoss_Attack_3_IsAttackBehind;//是否攻擊背後敵人

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)                  
    }
}
