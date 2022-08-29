using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 戰士專用
/// </summary>
public class WarriorExclusive : MonoBehaviourPunCallbacks
{
    Animator animator;
    GameData_NumericalValue NumericalValue;
    PlayerControl playerControl;

    //Buff
    float addDamage;//增加傷害值      

    void Start()
    {
        animator = GetComponent<Animator>();
        NumericalValue = GameDataManagement.Instance.numericalValue;
        playerControl = GetComponent<PlayerControl>();

        //Buff
        for (int i = 0; i < GameDataManagement.Instance.equipBuff.Length; i++)
        {
            //增加傷害值
            if (GameDataManagement.Instance.equipBuff[i] == 1 && GetComponent<PlayerControl>()) addDamage = GameDataManagement.Instance.numericalValue.buffAbleValue[1] / 100;                      
        }        
    }

    void Update()
    {
        OnAttackMove_Warrior();//攻擊移動
    }

    /// <summary>
    /// 技能攻擊1_戰士
    /// </summary>
    void OnSkillAttack1_Warrior()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率
        float getDamage = (NumericalValue.warriorSkillAttack_1_Damge + (NumericalValue.warriorSkillAttack_1_Damge * addDamage)) * rate;//造成傷害

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//執行攻擊腳色
        attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetHitSphereFunction);//設定執行函式
        attack.damage = getDamage;//造成傷害 
        attack.direction = NumericalValue.warriorSkillAttack_1_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.warriorSkillAttack_1_RepelDistance;//擊退距離
        attack.animationName = NumericalValue.warriorSkillAttack_1_Effect;//攻擊效果(播放動畫名稱)
        attack.forwardDistance = NumericalValue.warriorSkillAttack_1_ForwardDistance;//攻擊範圍中心點距離物件前方
        attack.attackRadius = NumericalValue.warriorSkillAttack_1_attackRadius;//攻擊半徑
        attack.isAttackBehind = NumericalValue.warriorSkillAttack_1_IsAttackBehind;//是否攻擊背後敵人

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//加入List(執行)                  
    }

    /// <summary>
    /// 技能攻擊2_戰士
    /// </summary>
    void OnSkillAttack2_Warrior()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率
        float getDamage = (NumericalValue.warriorSkillAttack_2_Damge + (NumericalValue.warriorSkillAttack_2_Damge * addDamage)) * rate;//造成傷害

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//執行攻擊腳色
        attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetHitSphereFunction);//設定執行函式
        attack.damage = getDamage;//造成傷害 
        attack.direction = NumericalValue.warriorSkillAttack_2_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.warriorSkillAttack_2_RepelDistance;//擊退距離
        attack.animationName = NumericalValue.warriorSkillAttack_2_Effect;//攻擊效果(播放動畫名稱)
        attack.forwardDistance = NumericalValue.warriorSkillAttack_2_ForwardDistance;//攻擊範圍中心點距離物件前方
        attack.attackRadius = NumericalValue.warriorSkillAttack_2_attackRadius;//攻擊半徑
        attack.isAttackBehind = NumericalValue.warriorSkillAttack_2_IsAttackBehind;//是否攻擊背後敵人

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//加入List(執行)                  
    }

    /// <summary>
    /// 技能攻擊3_戰士
    /// </summary>
    /// <param name="count">第幾段攻擊</param>
    void OnSkillAttack3_Warrior(int count)
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率
        float getDamage = (NumericalValue.warriorSkillAttack_3_Damge[count] + (NumericalValue.warriorSkillAttack_3_Damge[count] * addDamage)) * rate;//造成傷害

        AttackMode attack = AttackMode.Instance;        
        attack.performCharacters = gameObject;//執行攻擊腳色
        attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetHitSphereFunction);//設定執行函式
        attack.damage = getDamage;//造成傷害 
        attack.direction = NumericalValue.warriorSkillAttack_3_RepelDirection[count];//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.warriorSkillAttack_3_RepelDistance[count];//擊退距離
        attack.animationName = NumericalValue.warriorSkillAttack_3_Effect[count];//攻擊效果(播放動畫名稱)
        attack.forwardDistance = NumericalValue.warriorSkillAttack_3_ForwardDistance[count];//攻擊範圍中心點距離物件前方
        attack.attackRadius = NumericalValue.warriorSkillAttack_3_attackRadius[count];//攻擊半徑
        attack.isAttackBehind = NumericalValue.warriorSkillAttack_3_IsAttackBehind[count];//是否攻擊背後敵人
        
        GameSceneManagement.Instance.AttackMode_List.Add(attack);//加入List(執行)                                 
    }

    /// <summary>
    /// 跳躍攻擊_戰士
    /// </summary>
    void OnJumpAttack_Warrior()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率
        float getDamage = (NumericalValue.warriorJumpAttack_Damage + (NumericalValue.warriorJumpAttack_Damage * addDamage)) * rate;//造成傷害

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//執行攻擊腳色
        attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetHitSphereFunction);//設定執行函式
        attack.damage = getDamage;//造成傷害 
        attack.direction = NumericalValue.warriorJumpAttack_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.warriorJumpAttac_kRepelDistance;//擊退距離
        attack.animationName = NumericalValue.warriorJumpAttack_Effect;//攻擊效果(播放動畫名稱)
        attack.forwardDistance = NumericalValue.warriorJumpAttack_ForwardDistance;//攻擊範圍中心點距離物件前方
        attack.attackRadius = NumericalValue.warriorJumpAttack_attackRadius;//攻擊半徑
        attack.isAttackBehind = NumericalValue.warriorJumpAttack_IsAttackBehind;//是否攻擊背後敵人

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//加入List(執行)

        playerControl.isJumpAttackMove = true;//跳躍攻擊下降
    }

    /// <summary>
    /// 普通攻擊1_戰士
    /// </summary>
    void OnNormalAttack1_Warrior()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率
        float getDamage = (NumericalValue.warriorNormalAttack_1_Damge + (NumericalValue.warriorNormalAttack_1_Damge * addDamage)) * rate;//造成傷害

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//執行攻擊腳色
        attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetHitSphereFunction);//設定執行函式
        attack.damage = getDamage;//造成傷害 
        attack.direction = NumericalValue.warriorNormalAttack_1_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.warriorNormalAttack_1_RepelDistance;//擊退距離
        attack.animationName = NumericalValue.warriorNormalAttack_1_Effect;//攻擊效果(播放動畫名稱)
        attack.forwardDistance = NumericalValue.warriorNormalAttack_1_ForwardDistance;//攻擊範圍中心點距離物件前方
        attack.attackRadius = NumericalValue.warriorNormalAttack_1_attackRadius;//攻擊半徑
        attack.isAttackBehind = NumericalValue.warriorNormalAttack_1_IsAttackBehind;//是否攻擊背後敵人

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//加入List(執行)           
    }

    /// <summary>
    /// 普通攻擊2_戰士
    /// </summary>
    void OnNormalAttack2_Warrior()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率
        float getDamage = (NumericalValue.warriorNormalAttack_2_Damge + (NumericalValue.warriorNormalAttack_2_Damge * addDamage)) * rate;//造成傷害

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//執行攻擊腳色
        attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetHitSphereFunction);//設定執行函式
        attack.damage = getDamage;//造成傷害 
        attack.direction = NumericalValue.warriorNormalAttack_2_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.warriorNormalAttack_2_RepelDistance;//擊退距離
        attack.animationName = NumericalValue.warriorNormalAttack_2_Effect;//攻擊效果(播放動畫名稱)
        attack.forwardDistance = NumericalValue.warriorNormalAttack_2_ForwardDistance;//攻擊範圍中心點距離物件前方
        attack.attackRadius = NumericalValue.warriorNormalAttack_2_attackRadius;//攻擊半徑
        attack.isAttackBehind = NumericalValue.warriorNormalAttack_2_IsAttackBehind;//是否攻擊背後敵人

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//加入List(執行)           
    }

    /// <summary>
    /// 普通攻擊3_戰士
    /// </summary>
    void OnNormalAttack3_Warrior()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;
        
        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率
        float getDamage = (NumericalValue.warriorNormalAttack_3_Damge + (NumericalValue.warriorNormalAttack_3_Damge * addDamage)) * rate;//造成傷害

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//執行攻擊腳色
        attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetHitSphereFunction);//設定執行函式
        attack.damage = getDamage;//造成傷害 
        attack.direction = NumericalValue.warriorNormalAttack_3_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.warriorNormalAttack_3_RepelDistance;//擊退距離
        attack.animationName = NumericalValue.warriorNormalAttack_3_Effect;//攻擊效果(播放動畫名稱)
        attack.forwardDistance = NumericalValue.warriorNormalAttack_3_ForwardDistance;//攻擊範圍中心點距離物件前方
        attack.attackRadius = NumericalValue.warriorNormalAttack_3_attackRadius;//攻擊半徑
        attack.isAttackBehind = NumericalValue.warriorNormalAttack_3_IsAttackBehind;//是否攻擊背後敵人

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//加入List(執行)           
    }

    /// <summary>
    /// 攻擊移動_戰士
    /// </summary>
    void OnAttackMove_Warrior()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        AnimatorStateInfo animationInfo = animator.GetCurrentAnimatorStateInfo(0);
        float move = 3.5f;

        if (GameDataManagement.Instance.selectRoleNumber == 0)
        {
            if (animationInfo.IsName("NormalAttack_1") && animationInfo.normalizedTime > 0.35f && animationInfo.normalizedTime < 0.6f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
            if (animationInfo.IsName("NormalAttack_2") && animationInfo.normalizedTime > 0.4f && animationInfo.normalizedTime < 0.5f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
            if (animationInfo.IsName("NormalAttack_3") && animationInfo.normalizedTime > 0.35f && animationInfo.normalizedTime < 0.45f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
            if (animationInfo.IsName("SkillAttack_1") && animationInfo.normalizedTime > 0.55f && animationInfo.normalizedTime < 0.65f) transform.position = transform.position + transform.forward * (-move - Time.deltaTime) * Time.deltaTime;
            if (animationInfo.IsName("SkillAttack_2") && animationInfo.normalizedTime > 0.35f && animationInfo.normalizedTime < 0.45f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
            if (animationInfo.IsName("SkillAttack_2") && animationInfo.normalizedTime > 0.6f && animationInfo.normalizedTime < 0.7f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
            if (animationInfo.IsName("SkillAttack_3") && animationInfo.normalizedTime > 0.2f && animationInfo.normalizedTime < 0.3f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
            if (animationInfo.IsName("SkillAttack_3") && animationInfo.normalizedTime > 0.35f && animationInfo.normalizedTime < 0.45f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
            if (animationInfo.IsName("SkillAttack_3") && animationInfo.normalizedTime > 0.58f && animationInfo.normalizedTime < 0.68f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
        }
    }
}
