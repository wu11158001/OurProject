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
    
    [Header("弓箭物件")]
    string[] normalAttackArrows;//所有普通攻擊弓箭

    [Header("其他")]
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

        //弓箭物件
        normalAttackArrows = new string[] { "archerNormalAttack_1_Arrow", "archerNormalAttack_2_Arrow", "archerNormalAttack_3_Arrow" };

        //其他
        shoopPosition = new float[] { 1.35f, 0.65f };//射出位置(0:向上距離 1:向前距離)
    }
   
    void Update()
    {
        normalAttackNumber = playerControl.GetNormalAttackNumber;//普通攻擊編號
        OnArrowEnabledControl();
    }

    /// <summary>
    /// 技能1攻擊行為_弓箭手
    /// </summary>
    void OnSkillAttackBehavior_1_Archer()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        Vector3[] diration = new Vector3[] { transform.forward - transform.right / 2, transform.forward - transform.right, transform.forward, transform.forward + transform.right / 2, transform.forward + transform.right };

        for (int i = 0; i < diration.Length; i++)
        {
            AttackMode attack = AttackMode.Instance;
            bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
            float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率

            attack.function = new Action(attack.OnSetShootFunction_Single);//設定執行函式
            attack.performObject = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber("archerSkilllAttack_1_Arrow"), GameSceneManagement.Instance.loadPath.archerSkilllAttack_1_Arrow);//執行攻擊的物件(自身/射出物件)
            attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
            attack.damage = NumericalValue.archerSkillAttackDamage[normalAttackNumber - 1] * rate;//造成傷害 
            attack.animationName = NumericalValue.archerSkillAttackEffect[normalAttackNumber - 1];//攻擊效果(播放動畫名稱)
            attack.direction = NumericalValue.archerSkillAttackRepelDirection[normalAttackNumber - 1];//擊退方向(0:擊退, 1:擊飛)
            attack.repel = NumericalValue.archerSkillAttackRepel[normalAttackNumber - 1];//擊退/擊飛距離
            attack.isCritical = isCritical;//是否爆擊
            attack.speed = NumericalValue.arrowFloatSpeed;//飛行速度
            attack.lifeTime = NumericalValue.arrowLifeTime;//生存時間
            attack.diration = diration[i];//飛行方向        
            attack.performObject.transform.position = transform.position + Vector3.up * shoopPosition[0] + transform.forward * shoopPosition[1];//射出位置
            GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)           
        }        
    }

    /// <summary>
    /// 技能2攻擊行為_弓箭手
    /// </summary>
    void OnSkillAttackBehavior_2_Archer()
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
        attack.damage = NumericalValue.archerSkillAttackDamage[normalAttackNumber - 1] * rate;//造成傷害 
        attack.animationName = NumericalValue.archerSkillAttackEffect[normalAttackNumber - 1];//攻擊效果(播放動畫名稱)
        attack.direction = NumericalValue.archerSkillAttackRepelDirection[normalAttackNumber - 1];//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.archerSkillAttackRepel[normalAttackNumber - 1];//擊退/擊飛距離
        attack.boxSize = NumericalValue.archerSkillAttackBoxSize[normalAttackNumber - 1] * transform.lossyScale.x;//近身攻擊框Size
        attack.isCritical = isCritical;//是否爆擊
        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)   
    }

    /// <summary>
    /// 技能2攻擊行為_弓箭手
    /// </summary>
    void OnSkillAttackBehavior_3_Archer()
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
        attack.damage = NumericalValue.archerSkillAttackDamage[normalAttackNumber - 1] * rate;//造成傷害 
        attack.animationName = NumericalValue.archerSkillAttackEffect[normalAttackNumber - 1];//攻擊效果(播放動畫名稱)
        attack.direction = NumericalValue.archerSkillAttackRepelDirection[normalAttackNumber - 1];//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.archerSkillAttackRepel[normalAttackNumber - 1];//擊退/擊飛距離
        attack.boxSize = NumericalValue.archerSkillAttackBoxSize[normalAttackNumber - 1] * transform.lossyScale.x;//近身攻擊框Size
        attack.isCritical = isCritical;//是否爆擊
        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)   
    }

    /// <summary>
    /// 跳躍攻擊行為_弓箭手
    /// </summary>
    void OnJumpAttackBehavior_Archer()
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
        attack.damage = NumericalValue.archerJumpAttackDamage * rate;//造成傷害 
        attack.animationName = NumericalValue.archerJumpAttackEffect;//攻擊效果(播放動畫名稱)
        attack.direction = NumericalValue.archerJumpAttackRepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.archerJumpAttackRepelDistance;//擊退距離
        attack.boxSize = NumericalValue.archerJumpAttackBoxSize * transform.lossyScale.x;//近身攻擊框Size
        attack.isCritical = isCritical;//是否爆擊
        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)   
    }

    /// <summary>
    /// 普通攻擊行為_弓箭手
    /// </summary>
    void OnNormalAttackBehavior_Archer()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率
        
        //設定AttackBehavior Class數值
        AttackMode attack = AttackMode.Instance;
        attack.function = new Action(attack.OnSetShootFunction_Single);//設定執行函式
        attack.performObject = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber(normalAttackArrows[normalAttackNumber - 1]), GameSceneManagement.Instance.loadPath.archerNormalAttackArrows[normalAttackNumber - 1]);//執行攻擊的物件(自身/射出物件)
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.damage = NumericalValue.archerNormalAttackDamge[normalAttackNumber - 1] * rate;//造成傷害 
        attack.animationName = NumericalValue.archerNormalAttackEffect[normalAttackNumber - 1];//攻擊效果(播放動畫名稱)
        attack.direction = NumericalValue.archerNormalAttackRepelDirection[normalAttackNumber - 1];//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.archerNormalAttackRepelDistance[normalAttackNumber - 1];//擊退/擊飛距離
        attack.isCritical = isCritical;//是否爆擊
        attack.speed = NumericalValue.arrowFloatSpeed;//飛行速度
        attack.lifeTime = NumericalValue.arrowLifeTime;//生存時間
        attack.diration = transform.forward;//飛行方向        
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

    private void OnDrawGizmos()
    {
        
    }
}
