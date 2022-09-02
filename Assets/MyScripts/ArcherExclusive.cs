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

    //Buff
    [SerializeField]float addDamage;//增加傷害值

    MeshRenderer arrowMeshRenderer;//弓箭物件皮膚    
    string[] normalAttackArrowsPath;//普通攻擊弓箭物件

    void Start()
    {
        animator = GetComponent<Animator>();
        NumericalValue = GameDataManagement.Instance.numericalValue;
        playerControl = GetComponent<PlayerControl>();

        //Buff
        for (int i = 0; i < GameDataManagement.Instance.equipBuff.Length; i++)
        {
            if (GameDataManagement.Instance.equipBuff[i] == 1)
            {
                addDamage = GameDataManagement.Instance.numericalValue.buffAbleValue[1] / 100;//增加傷害值
            }
        }

        //弓箭物件皮膚
        arrowMeshRenderer = ExtensionMethods.FindAnyChild<MeshRenderer>(transform, "Arrow");
        arrowMeshRenderer.enabled = false;

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
            float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//爆擊攻擊提升倍率

            AttackMode attack = AttackMode.Instance;
            attack.performCharacters = gameObject;//執行攻擊腳色
            attack.performObject = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber("archerSkilllAttack_1"), GameSceneManagement.Instance.loadPath.archerSkilllAttack_1);//執行攻擊的物件(自身/射出物件)
            attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
            attack.isCritical = isCritical;//是否爆擊

            attack.function = new Action(attack.OnSetShootFunction_Single);//設定執行函式       
            attack.damage = (NumericalValue.archerSkillAttack_1_Damage + (NumericalValue.archerSkillAttack_1_Damage * addDamage)) * rate;//造成傷害 
            attack.direction = NumericalValue.archerSkillAttack_1_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
            attack.repel = NumericalValue.archerSkillAttack_1_Repel;//擊退/擊飛距離
            attack.animationName = NumericalValue.archerSkillAttack_1_Effect;//攻擊效果(播放動畫名稱)        

            attack.flightSpeed = NumericalValue.archerSkillAttack_1_FlightSpeed;//飛行速度
            attack.lifeTime = NumericalValue.archerSkillAttack_1_LifeTime;//生存時間
            attack.flightDiration = diration[i];//飛行方向        
            attack.performObject.transform.position = arrowMeshRenderer.transform.position;//射出位置

            GameSceneManagement.Instance.AttackMode_List.Add(attack);//加入List(執行)           
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
        float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//爆擊攻擊提升倍率

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//執行攻擊腳色
        attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetHitSphereFunction);//設定執行函式
        attack.damage = (NumericalValue.archerSkillAttack_2_Damge + (NumericalValue.archerSkillAttack_2_Damge * addDamage)) * rate;//造成傷害 
        attack.direction = NumericalValue.archerSkillAttack_2_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.archerSkillAttack_2_RepelDistance;//擊退距離
        attack.animationName = NumericalValue.archerSkillAttack_2_Effect;//攻擊效果(播放動畫名稱)
        attack.forwardDistance = NumericalValue.archerSkillAttack_2_ForwardDistance;//攻擊範圍中心點距離物件前方
        attack.attackRadius = NumericalValue.archerSkillAttack_2_attackRadius;//攻擊範圍
        attack.isAttackBehind = NumericalValue.archerSkillAttack_2_IsAttackBehind;//是否攻擊背後敵人
        
        GameSceneManagement.Instance.AttackMode_List.Add(attack);//加入List(執行)           
    }

    /// <summary>
    /// 技能攻擊3_弓箭手
    /// </summary>
    void OnSkillAttack3_Archer()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//爆擊攻擊提升倍率

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//執行攻擊腳色
        attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetHitSphereFunction);//設定執行函式
        attack.damage = (NumericalValue.archerSkillAttack_3_Damge + (NumericalValue.archerSkillAttack_3_Damge * addDamage)) * rate;//造成傷害 
        attack.direction = NumericalValue.archerSkillAttack_3_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.archerSkillAttack_3_RepelDistance;//擊退距離
        attack.animationName = NumericalValue.archerSkillAttack_3_Effect;//攻擊效果(播放動畫名稱)
        attack.forwardDistance = NumericalValue.archerSkillAttack_3_ForwardDistance;//攻擊範圍中心點距離物件前方
        attack.attackRadius = NumericalValue.archerSkillAttack_3_attackRadius;//攻擊半徑
        attack.isAttackBehind = NumericalValue.archerSkillAttack_3_IsAttackBehind;//是否攻擊背後敵人

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//加入List(執行)           
    }

    /// <summary>
    /// 跳躍攻擊_弓箭手
    /// </summary>
    void OnJumpAttack_Archer()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//爆擊攻擊提升倍率

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//執行攻擊腳色
        attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetHitSphereFunction);//設定執行函式
        attack.damage = (NumericalValue.archerJumpAttack_Damage + (NumericalValue.archerJumpAttack_Damage * addDamage)) * rate;//造成傷害 
        attack.direction = NumericalValue.archerJumpAttack_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.archerJumpAttack_RepelDistance;//擊退距離
        attack.animationName = NumericalValue.archerJumpAttack_Effect;//攻擊效果(播放動畫名稱)
        attack.forwardDistance = NumericalValue.archerJumpAttack_ForwardDistance;//攻擊範圍中心點距離物件前方
        attack.attackRadius = NumericalValue.archerJumpAttack_attackRadius;//攻擊半徑
        attack.isAttackBehind = NumericalValue.archerJumpAttack_IsAttackBehind;//是否攻擊背後敵人

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//加入List(執行)   

        playerControl.isJumpAttackMove = true;//跳躍攻擊下降
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
        float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//爆擊攻擊提升倍率

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//執行攻擊腳色
        attack.performObject = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber(normalAttackArrowsPath[number]), GameSceneManagement.Instance.loadPath.archerAllNormalAttack[number]);//執行攻擊的物件(自身/射出物件)
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetShootFunction_Single);//設定執行函式       
        attack.damage = (NumericalValue.archerNormalAttack_Damge[number] + (NumericalValue.archerNormalAttack_Damge[number] * addDamage)) * rate;//造成傷害 
        attack.direction = NumericalValue.archerNormalAttack_RepelDirection[number];//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.archerNormalAttack_RepelDistance[number];//擊退/擊飛距離
        attack.animationName = NumericalValue.archerNormalAttack_Effect[number];//攻擊效果(播放動畫名稱)        

        attack.flightSpeed = NumericalValue.archerNormalAttack_FloatSpeed[number];//飛行速度
        attack.lifeTime = NumericalValue.archerNormalAttack_LifeTime[number];//生存時間
        attack.flightDiration = transform.forward;//飛行方向        
        attack.performObject.transform.position = arrowMeshRenderer.transform.position;//射出位置

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//加入List(執行)           
    }

    /// <summary>
    /// 弓箭顯示控制
    /// </summary>
    void OnArrowEnabledControl()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName("Attack.NormalAttack_1") && info.normalizedTime < 0.2f)
        {
            if (!arrowMeshRenderer.enabled)
            {
                //位置
                arrowMeshRenderer.transform.localPosition = new Vector3(-0.000209999998f, 0.00526000001f, 0.000190000006f);
                arrowMeshRenderer.transform.localRotation = Quaternion.Euler(272.496521f, 261.24234f, 8.01684952f);

                arrowMeshRenderer.enabled = true;
            }
        }
        else if (info.IsName("Attack.NormalAttack_2") && info.normalizedTime < 0.2f)
        {
            //位置
            arrowMeshRenderer.transform.localPosition = new Vector3(-0.000209999998f, 0.00526000001f, 0.000190000006f);
            arrowMeshRenderer.transform.localRotation = Quaternion.Euler(272.496521f, 261.24234f, 8.01684952f);

            if (!arrowMeshRenderer.enabled) arrowMeshRenderer.enabled = true;
        }
        else if (info.IsName("Attack.NormalAttack_3") && info.normalizedTime > 0.2f && info.normalizedTime < 0.63f)
        {
            //位置
            arrowMeshRenderer.transform.localPosition = new Vector3(1.99999995e-05f, 0.00486999983f, 0.00153999997f);
            arrowMeshRenderer.transform.localRotation = Quaternion.Euler(286.910248f, 1.72138309f, 257.902863f);

            if (!arrowMeshRenderer.enabled) arrowMeshRenderer.enabled = true;
        }
        else if (info.IsName("Attack.SkillAttack_1") && info.normalizedTime > 0.2f && info.normalizedTime < 0.63f)
        {
            //位置
            arrowMeshRenderer.transform.localPosition = new Vector3(1.99999995e-05f, 0.00486999983f, 0.00153999997f);
            arrowMeshRenderer.transform.localRotation = Quaternion.Euler(286.910248f, 1.72138309f, 257.902863f);

            if (!arrowMeshRenderer.enabled) arrowMeshRenderer.enabled = true;
        }
        else
        {            
            if (arrowMeshRenderer.enabled) arrowMeshRenderer.enabled = false;            
        }
    }
}
