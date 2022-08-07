using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弓箭手專用
/// </summary>
public class ArcherExclusive : MonoBehaviourPunCallbacks
{
    Animator animator;
    GameData_NumericalValue NumericalValue;
    PlayerControl playerControl;

    MeshRenderer arrowMeshRenderer;//弓箭物件皮膚    
    string[] normalAttackArrowsPath;//普通攻擊弓箭物件

    void Start()
    {
        animator = GetComponent<Animator>();
        NumericalValue = GameDataManagement.Instance.numericalValue;
        playerControl = GetComponent<PlayerControl>();

        //弓箭物件皮膚
        arrowMeshRenderer = ExtensionMethods.FindAnyChild<MeshRenderer>(transform, "Arrow");
        //arrowMeshRenderer.enabled = false;

        normalAttackArrowsPath = new string[] { "archerNormalAttack_1", "archerNormalAttack_2", "archerNormalAttack_3" };//普通攻擊弓箭物件
    }
   
    void Update()
    {        
        OnArrowEnabledControl();
    }

    /// <summary>
    /// 技能攻擊1_弓箭手
    /// </summary>
    void OnSkillAttack1_Archer()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        //射擊方向
        Vector3[] diration = new Vector3[] { transform.forward - transform.right / 2,
                                             transform.forward - transform.right / 4,
                                             transform.forward,
                                             transform.forward + transform.right / 4,
                                             transform.forward + transform.right / 2};

        for (int i = 0; i < diration.Length; i++)
        {
            bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
            float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率

            AttackMode attack = AttackMode.Instance;
            attack.performObject = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber("archerSkilllAttack_1"), GameSceneManagement.Instance.loadPath.archerSkilllAttack_1);//執行攻擊的物件(自身/射出物件)
            attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
            attack.isCritical = isCritical;//是否爆擊

            attack.function = new Action(attack.OnSetShootFunction_Single);//設定執行函式       
            attack.damage = NumericalValue.archerSkillAttack_1_Damage * rate;//造成傷害 
            attack.direction = NumericalValue.archerSkillAttack_1_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
            attack.repel = NumericalValue.archerSkillAttack_1_Repel;//擊退/擊飛距離
            attack.animationName = NumericalValue.archerSkillAttack_1_Effect;//攻擊效果(播放動畫名稱)        

            attack.flightSpeed = NumericalValue.archerSkillAttack_1_FlightSpeed;//飛行速度
            attack.lifeTime = NumericalValue.archerSkillAttack_1_LifeTime;//生存時間
            attack.flightDiration = diration[i];//飛行方向        
            attack.performObject.transform.position = arrowMeshRenderer.transform.position;//射出位置

            GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)           
        }        
    }

    /// <summary>
    /// 技能攻擊2_弓箭手
    /// </summary>
    void OnSkillAttack2_Archer()
    {        
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率

        AttackMode attack = AttackMode.Instance;
        attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetHitSphereFunction);//設定執行函式
        attack.damage = NumericalValue.archerSkillAttack_2_Damge * rate;//造成傷害 
        attack.direction = NumericalValue.archerSkillAttack_2_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.archerSkillAttack_2_RepelDistance;//擊退距離
        attack.animationName = NumericalValue.archerSkillAttack_2_Effect;//攻擊效果(播放動畫名稱)
        attack.forwardDistance = NumericalValue.archerSkillAttack_2_ForwardDistance;//攻擊範圍中心點距離物件前方
        attack.attackRadius = NumericalValue.archerSkillAttack_2_attackRadius;//攻擊範圍
        attack.isAttackBehind = NumericalValue.archerSkillAttack_2_IsAttackBehind;//是否攻擊背後敵人
        
        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)           
    }

    /// <summary>
    /// 技能攻擊3_弓箭手
    /// </summary>
    void OnSkillAttack3_Archer()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率

        AttackMode attack = AttackMode.Instance;
        attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetHitSphereFunction);//設定執行函式
        attack.damage = NumericalValue.archerSkillAttack_3_Damge * rate;//造成傷害 
        attack.direction = NumericalValue.archerSkillAttack_3_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.archerSkillAttack_3_RepelDistance;//擊退距離
        attack.animationName = NumericalValue.archerSkillAttack_3_Effect;//攻擊效果(播放動畫名稱)
        attack.forwardDistance = NumericalValue.archerSkillAttack_3_ForwardDistance;//攻擊範圍中心點距離物件前方
        attack.attackRadius = NumericalValue.archerSkillAttack_3_attackRadius;//攻擊半徑
        attack.isAttackBehind = NumericalValue.archerSkillAttack_3_IsAttackBehind;//是否攻擊背後敵人

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)           
    }

    /// <summary>
    /// 跳躍攻擊_弓箭手
    /// </summary>
    void OnJumpAttack_Archer()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率

        AttackMode attack = AttackMode.Instance;
        attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetHitSphereFunction);//設定執行函式
        attack.damage = NumericalValue.archerJumpAttack_Damage * rate;//造成傷害 
        attack.direction = NumericalValue.archerJumpAttack_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.archerJumpAttack_RepelDistance;//擊退距離
        attack.animationName = NumericalValue.archerJumpAttack_Effect;//攻擊效果(播放動畫名稱)
        attack.forwardDistance = NumericalValue.archerJumpAttack_ForwardDistance;//攻擊範圍中心點距離物件前方
        attack.attackRadius = NumericalValue.archerJumpAttack_attackRadius;//攻擊半徑
        attack.isAttackBehind = NumericalValue.archerJumpAttack_IsAttackBehind;//是否攻擊背後敵人

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)   

        playerControl.isLockJumpHight = false;//是否鎖住跳躍高度
    }

    /// <summary>
    /// 普通攻擊_弓箭手
    /// </summary>
    /// <param name="number"></param>
    void OnNormalAttacks_Archer(int number)
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率

        AttackMode attack = AttackMode.Instance;
        attack.performObject = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber(normalAttackArrowsPath[number]), GameSceneManagement.Instance.loadPath.archerAllNormalAttack[number]);//執行攻擊的物件(自身/射出物件)
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetShootFunction_Group);//設定執行函式       
        attack.damage = NumericalValue.archerNormalAttack_Damge[number] * rate;//造成傷害 
        attack.direction = NumericalValue.archerNormalAttack_RepelDirection[number];//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.archerNormalAttack_RepelDistance[number];//擊退/擊飛距離
        attack.animationName = NumericalValue.archerNormalAttack_Effect[number];//攻擊效果(播放動畫名稱)        

        attack.flightSpeed = NumericalValue.archerNormalAttack_FloatSpeed[number];//飛行速度
        attack.lifeTime = NumericalValue.archerNormalAttack_LifeTime[number];//生存時間
        attack.flightDiration = transform.forward;//飛行方向        
        attack.performObject.transform.position = arrowMeshRenderer.transform.position;//射出位置

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)           
    }

    /// <summary>
    /// 弓箭顯示控制
    /// </summary>
    void OnArrowEnabledControl()
    {
       /* AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName("Attack.NormalAttack_1") && info.normalizedTime < 0.4f)
        {
            if (!arrowMeshRenderer.enabled) arrowMeshRenderer.enabled = true;
        }
        else if (info.IsName("Attack.NormalAttack_2") && info.normalizedTime < 0.4f)
        {
            if (!arrowMeshRenderer.enabled) arrowMeshRenderer.enabled = true;
        }
        else if (info.IsName("Attack.NormalAttack_3") && info.normalizedTime > 0.2f && info.normalizedTime < 0.68f)
        {
            if (!arrowMeshRenderer.enabled) arrowMeshRenderer.enabled = true;
        }
        else if (info.IsName("Attack.SkillAttack_1") && info.normalizedTime > 0.2f && info.normalizedTime < 0.68f)
        {
            if (!arrowMeshRenderer.enabled) arrowMeshRenderer.enabled = true;
        }
        else
        {            
            if (arrowMeshRenderer.enabled) arrowMeshRenderer.enabled = false;            
        }*/
    }
}
