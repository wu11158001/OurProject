using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵人士兵2專用
/// </summary>
public class EnemySoldier2_Exclusive : MonoBehaviourPunCallbacks
{
    Animator animator;
    GameData_NumericalValue NumericalValue;

    MeshRenderer arrowMeshRenderer;//弓箭物件皮膚

    void Start()
    {
        animator = GetComponent<Animator>();
        NumericalValue = GameDataManagement.Instance.numericalValue;

        //弓箭物件皮膚
        arrowMeshRenderer = ExtensionMethods.FindAnyChild<MeshRenderer>(transform, "Arrow");
        arrowMeshRenderer.enabled = false;
    }

    void Update()
    {
        OnArrowEnabledControl();
    }

    /// <summary>
    /// 攻擊1_敵人士兵2
    /// </summary>
    void OnAttack1_EnemySoldier2()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//爆擊攻擊提升倍率

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//執行攻擊腳色
        attack.performObject = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber("enemySoldier2Attack_Arrow"), GameSceneManagement.Instance.loadPath.enemySoldier2Attack_Arrow);//執行攻擊的物件(自身/射出物件)
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetShootFunction_Single);//設定執行函式       
        attack.damage = NumericalValue.enemySoldier2_Attack1_Damge * rate;//造成傷害 
        attack.direction = NumericalValue.enemySoldier2_Attack1_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.enemySoldier2_Attack1_RepelDistance;//擊退/擊飛距離
        attack.animationName = NumericalValue.enemySoldier2_Attack1_Effect;//攻擊效果(播放動畫名稱)        

        attack.flightSpeed = NumericalValue.enemySoldier2_Attack1_FloatSpeed;//飛行速度
        attack.lifeTime = NumericalValue.enemySoldier2_Attack1_LifeTime;//生存時間
        attack.flightDiration = transform.forward;//飛行方向        
        attack.performObject.transform.position = arrowMeshRenderer.transform.position;//射出位置

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//加入List(執行)             
    }

    /// <summary>
    /// 攻擊2_敵人士兵2
    /// </summary>
    void OnAttack2_EnemySoldier2()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//爆擊攻擊提升倍率

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//執行攻擊腳色
        attack.performObject = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber("enemySoldier2Attack_Arrow"), GameSceneManagement.Instance.loadPath.enemySoldier2Attack_Arrow);//執行攻擊的物件(自身/射出物件)
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetShootFunction_Single);//設定執行函式       
        attack.damage = NumericalValue.enemySoldier2_Attack2_Damge * rate;//造成傷害 
        attack.direction = NumericalValue.enemySoldier2_Attack2_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.enemySoldier2_Attack2_RepelDistance;//擊退/擊飛距離
        attack.animationName = NumericalValue.enemySoldier2_Attack2_Effect;//攻擊效果(播放動畫名稱)        

        attack.flightSpeed = NumericalValue.enemySoldier2_Attack2_FloatSpeed;//飛行速度
        attack.lifeTime = NumericalValue.enemySoldier2_Attack2_LifeTime;//生存時間
        attack.flightDiration = transform.forward;//飛行方向        
        attack.performObject.transform.position = arrowMeshRenderer.transform.position;//射出位置

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//加入List(執行)                
    }

    /// <summary>
    /// 攻擊3_敵人士兵2
    /// </summary>
    void OnAttack3_EnemySoldier2()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//爆擊攻擊提升倍率
        float getDamage = NumericalValue.enemySoldier2_Attack_3_Damge * rate;//造成傷害

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//執行攻擊腳色
        attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetHitSphereFunction);//設定執行函式
        attack.damage = getDamage;//造成傷害 
        attack.direction = NumericalValue.enemySoldier2_Attack_3_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.enemySoldier2_Attack_3_RepelDistance;//擊退距離
        attack.animationName = NumericalValue.enemySoldier2_Attack_3_Effect;//攻擊效果(播放動畫名稱)
        attack.forwardDistance = NumericalValue.enemySoldier2_Attack_3_ForwardDistance;//攻擊範圍中心點距離物件前方
        attack.attackRadius = NumericalValue.enemySoldier2_Attack_3_attackRadius;//攻擊半徑
        attack.isAttackBehind = NumericalValue.enemySoldier2_Attack_3_IsAttackBehind;//是否攻擊背後敵人

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//加入List(執行)                  
    }

    /// <summary>
    /// 弓箭顯示控制
    /// </summary>
    void OnArrowEnabledControl()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName("Attack.Attack1") && info.normalizedTime > 0.2f && info.normalizedTime < 0.63f)
        {
            if (!arrowMeshRenderer.enabled)
            {                
                //位置
                arrowMeshRenderer.transform.localPosition = new Vector3(1.99999995e-05f, 0.00486999983f, 0.00153999997f);
                arrowMeshRenderer.transform.localRotation = Quaternion.Euler(286.910248f, 1.72138309f, 257.902863f);

                arrowMeshRenderer.enabled = true;
            }
        }
        else if (info.IsName("Attack.Attack2") && info.normalizedTime > 0.2f && info.normalizedTime < 0.63f)
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
