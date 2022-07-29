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

    string[] normalAttackArrows;//弓箭物件
    MeshRenderer arrowMeshRenderer;//弓箭物件皮膚
    int normalAttackNumber;//普通攻擊編號
    float[] shoopPosition;//射出位置(0:向上距離 1:向前距離)

    void Start()
    {
        animator = GetComponent<Animator>();
        NumericalValue = GameDataManagement.Instance.numericalValue;
        playerControl = GetComponent<PlayerControl>();
 
        arrowMeshRenderer = ExtensionMethods.FindAnyChild<MeshRenderer>(transform, "Arrow");
        arrowMeshRenderer.enabled = false;

        
        normalAttackArrows = new string[] { "archerNormalAttack_1", "archerNormalAttack_2", "archerNormalAttack_3" };//弓箭物件(尋找物件編號用)                
        shoopPosition = new float[] { 1.35f, 0.65f };//射出位置(0:向上距離 1:向前距離)
    }
   
    void Update()
    {
        normalAttackNumber = playerControl.GetNormalAttackNumber;//普通攻擊編號
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
            attack.performObject.transform.position = transform.position + GetComponent<BoxCollider>().center + transform.forward * 1;//射出位置

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

        attack.function = new Action(attack.OnSetHitFunction);//設定執行函式
        attack.damage = NumericalValue.archerSkillAttack_2_Damge * rate;//造成傷害 
        attack.direction = NumericalValue.archerSkillAttack_2_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.archerSkillAttack_2_RepelDistance;//擊退距離
        attack.animationName = NumericalValue.archerSkillAttack_2_Effect;//攻擊效果(播放動畫名稱)
        attack.forwardDistance = NumericalValue.archerSkillAttack_2_ForwardDistance;//攻擊範圍中心點距離物件前方
        attack.attackRadius = NumericalValue.archerSkillAttack_2_attackRadius;//攻擊半徑
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

        attack.function = new Action(attack.OnSetHitFunction);//設定執行函式
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

        attack.function = new Action(attack.OnSetHitFunction);//設定執行函式
        attack.damage = NumericalValue.archerJumpAttack_Damage * rate;//造成傷害 
        attack.direction = NumericalValue.archerJumpAttack_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.archerJumpAttack_RepelDistance;//擊退距離
        attack.animationName = NumericalValue.archerJumpAttack_Effect;//攻擊效果(播放動畫名稱)
        attack.forwardDistance = NumericalValue.archerJumpAttack_ForwardDistance;//攻擊範圍中心點距離物件前方
        attack.attackRadius = NumericalValue.archerJumpAttack_attackRadius;//攻擊半徑
        attack.isAttackBehind = NumericalValue.archerJumpAttack_IsAttackBehind;//是否攻擊背後敵人

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)   
    }

    /// <summary>
    /// 普通攻擊_弓箭手
    /// </summary>
    void OnNormalAttacks_Archer()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率

        AttackMode attack = AttackMode.Instance;        
        attack.performObject = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber(normalAttackArrows[normalAttackNumber - 1]), GameSceneManagement.Instance.loadPath.archerAllNormalAttack[normalAttackNumber - 1]);//執行攻擊的物件(自身/射出物件)
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetShootFunction_Single);//設定執行函式
        attack.damage = NumericalValue.archerNormalAttack_Damge[normalAttackNumber - 1] * rate;//造成傷害 
        attack.direction = NumericalValue.archerNormalAttack_RepelDirection[normalAttackNumber - 1];//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.archerNormalAttack_RepelDistance[normalAttackNumber - 1];//擊退/擊飛距離
        attack.animationName = NumericalValue.archerNormalAttack_Effect[normalAttackNumber - 1];//攻擊效果(播放動畫名稱)
        

        attack.flightSpeed = NumericalValue.archerNormalAttack_FloatSpeed[normalAttackNumber - 1];//飛行速度
        attack.lifeTime = NumericalValue.archerNormalAttack_LifeTime[normalAttackNumber - 1];//生存時間
        attack.flightDiration = transform.forward;//飛行方向        
        attack.performObject.transform.position = transform.position + Vector3.up * shoopPosition[0] + transform.forward * shoopPosition[1];//射出位置

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)           
    }

    /// <summary>
    /// 弓箭顯示控制
    /// </summary>
    void OnArrowEnabledControl()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName("Attack.NormalAttack_1") && info.normalizedTime < 0.45f && !arrowMeshRenderer.enabled) arrowMeshRenderer.enabled = true;
        else if (info.IsName("Attack.NormalAttack_2") && info.normalizedTime < 0.45f && !arrowMeshRenderer.enabled) arrowMeshRenderer.enabled = true;
        else if (info.IsName("Attack.NormalAttack_3") && info.normalizedTime > 0.23f && info.normalizedTime < 0.7f && !arrowMeshRenderer.enabled) arrowMeshRenderer.enabled = true;
        else
        {
            if(arrowMeshRenderer.enabled) arrowMeshRenderer.enabled = false;
        }
    }
}
