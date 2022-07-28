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

    int normalAttackNumber;//普通攻擊編號

    void Start()
    {
        animator = GetComponent<Animator>();
        NumericalValue = GameDataManagement.Instance.numericalValue;
        playerControl = GetComponent<PlayerControl>();
    }

    void Update()
    {
        normalAttackNumber = playerControl.GetNormalAttackNumber;//普通攻擊編號
        OnAttackMove_Warrior();
    }

    /// <summary>
    /// 技能攻擊行為_戰士
    /// </summary>
    void OnSkillAttackBehavior_Warrior()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        AttackMode attack = AttackMode.Instance;
        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率

        attack.function = new Action(attack.OnSetHitFunction);//設定執行函式
        attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.damage = NumericalValue.warriorSkillAttackDamage[normalAttackNumber - 1] * rate;//造成傷害 
        attack.animationName = NumericalValue.warriorSkillAttackEffect[normalAttackNumber - 1];//攻擊效果(播放動畫名稱)
        attack.direction = NumericalValue.warriorSkillAttackRepelDirection[normalAttackNumber - 1];//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.warriorSkillAttackRepel[normalAttackNumber - 1];//擊退距離
        attack.boxSize = NumericalValue.warriorSkillAttackBoxSize[normalAttackNumber - 1] * transform.lossyScale.x;//近身攻擊框Size
        attack.isCritical = isCritical;//是否爆擊
        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)              
    }

    /// <summary>
    /// 跳躍攻擊行為_戰士
    /// </summary>
    void OnJumpAttackBehavior_Warrior()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率

        //設定AttackBehavior Class數值
        AttackMode attack = AttackMode.Instance;
        attack.function = new Action(attack.OnSetHitFunction);//設定執行函式
        attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.damage = NumericalValue.warriorJumpAttackDamage * rate;//造成傷害 
        attack.animationName = NumericalValue.warriorJumpAttackEffect;//攻擊效果(播放動畫名稱)
        attack.direction = NumericalValue.warriorJumpAttackRepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.warriorJumpAttackRepelDistance;//擊退距離
        attack.boxSize = NumericalValue.warriorJumpAttackBoxSize * transform.lossyScale.x;//近身攻擊框Size
        attack.isCritical = isCritical;//是否爆擊
        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)   
    }

    /// <summary>
    /// 普通攻擊行為_戰士
    /// </summary>
    void OnNormalAttackBehavior_Warrior()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率

        //設定AttackBehavior Class數值
        AttackMode attack = AttackMode.Instance;
        attack.function = new Action(attack.OnSetHitFunction);//設定執行函式
        attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.damage = NumericalValue.warriorNormalAttackDamge[normalAttackNumber - 1] * rate;//造成傷害 
        attack.animationName = NumericalValue.warriorNormalAttackEffect[normalAttackNumber - 1];//攻擊效果(播放動畫名稱)
        attack.direction = NumericalValue.warriorNormalAttackRepelDirection[normalAttackNumber - 1];//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.warriorNormalAttackRepelDistance[normalAttackNumber - 1];//擊退距離
        attack.boxSize = NumericalValue.warriorNormalAttackBoxSize[normalAttackNumber - 1] * transform.lossyScale.x;//近身攻擊框Size
        attack.isCritical = isCritical;//是否爆擊
        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)           
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
            if (animationInfo.IsName("SkillAttack_1") && animationInfo.normalizedTime > 0.55f && animationInfo.normalizedTime < 0.65f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
            if (animationInfo.IsName("SkillAttack_2") && animationInfo.normalizedTime > 0.35f && animationInfo.normalizedTime < 0.45f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
            if (animationInfo.IsName("SkillAttack_2") && animationInfo.normalizedTime > 0.6f && animationInfo.normalizedTime < 0.7f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
            if (animationInfo.IsName("SkillAttack_3") && animationInfo.normalizedTime > 0.2f && animationInfo.normalizedTime < 0.3f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
            if (animationInfo.IsName("SkillAttack_3") && animationInfo.normalizedTime > 0.35f && animationInfo.normalizedTime < 0.45f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
            if (animationInfo.IsName("SkillAttack_3") && animationInfo.normalizedTime > 0.58f && animationInfo.normalizedTime < 0.68f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
        }
    }
}
