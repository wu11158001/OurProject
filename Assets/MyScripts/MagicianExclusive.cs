using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianExclusive : MonoBehaviourPunCallbacks
{
    Animator animator;
    GameData_NumericalValue NumericalValue;

    //碰撞框
    Vector3 boxCenter;
    Vector3 boxSize;

    void Start()
    {
        animator = GetComponent<Animator>();
        NumericalValue = GameDataManagement.Instance.numericalValue;

        //碰撞框
        boxCenter = GetComponent<BoxCollider>().center;
        boxSize = GetComponent<BoxCollider>().size;
    }

    void Update()
    {
        OnSkillAttack2_Magician();
    }    

    /// <summary>
    /// 技能攻擊1_法師
    /// </summary>
    void OnSkillAttack1_Magician()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率

        AttackMode attack = AttackMode.Instance;
        attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetHealFunction);//設定執行函式
        attack.damage = NumericalValue.magicianSkillAttack_1_HealValue * rate;//治療量(%)
        attack.forwardDistance = NumericalValue.magicianSkillAttack_1_ForwardDistance;//治療範圍中心點距離物件前方
        attack.attackRadius = NumericalValue.magicianSkillAttack_1_attackRange;//治療半徑
        attack.isAttackBehind = NumericalValue.magicianSkillAttack_1_IsAttackBehind;//法師普通攻擊1_是否治療背後盟友

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)   
    }

    /// <summary>
    /// 技能攻擊2_法師
    /// </summary>
    void OnSkillAttack2_Magician()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        LayerMask mask = LayerMask.GetMask("Enemy");

        if (info.IsName("SkillAttack_2") && info.normalizedTime < 1)
        {
            //移動
            transform.position = transform.position + transform.forward * 10 * Time.deltaTime;

            //碰撞敵人
            if (Physics.CheckBox(transform.position + boxCenter, new Vector3(boxSize.x / 1.3f, boxSize.y, boxSize.z / 1.3f), transform.rotation, mask))
            {
                //觸發技能之2
                animator.SetBool("SkillAttack-2", true);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "SkillAttack-2", true);

                GetComponent<CharactersCollision>().OnBodySetActive(active: 1);//(1:顯示 0:不顯示)
            }
        }
    }

    /// <summary>
    /// 技能攻擊2第二段_法師
    /// </summary>
    /// <param name="number">攻擊段數</param>
    void OnOnSkillAttack2Second_Magician(int number)
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
        attack.damage = NumericalValue.magicianSkillAttack_2_Damge[number] * rate;//造成傷害 
        attack.direction = NumericalValue.magicianSkillAttack_2_RepelDirection[number];//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.magicianSkillAttack_2_RepelDistance[number];//擊退距離
        attack.animationName = NumericalValue.magicianSkillAttack_2_Effect[number];//攻擊效果(播放動畫名稱)
        attack.forwardDistance = NumericalValue.magicianSkillAttack_2_ForwardDistance[number];//攻擊範圍中心點距離物件前方
        attack.attackRadius = NumericalValue.magicianSkillAttack_2_attackRadius[number];//攻擊半徑
        attack.isAttackBehind = NumericalValue.magicianSkillAttack_2_IsAttackBehind[number];//是否攻擊背後敵人

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)   
    }

    /// <summary>
    /// 技能攻擊3_法師
    /// </summary>
    void OnOnSkillAttack3_Magician()
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
        attack.damage = NumericalValue.magicianSkillAttack_3_Damge * rate;//造成傷害 
        attack.direction = NumericalValue.magicianSkillAttack_3_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.magicianSkillAttack_3_RepelDistance;//擊退距離
        attack.animationName = NumericalValue.magicianSkillAttack_32_Effect;//攻擊效果(播放動畫名稱)
        attack.forwardDistance = NumericalValue.magicianSkillAttack_3_ForwardDistance;//攻擊範圍中心點距離物件前方
        attack.attackRadius = NumericalValue.magicianSkillAttack_3_attackRadius;//攻擊半徑
        attack.isAttackBehind = NumericalValue.magicianSkillAttack_3_IsAttackBehind;//是否攻擊背後敵人

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)   
    }

    /// <summary>
    /// 跳躍攻擊_法師
    /// </summary>
    void OnJumpAttack_Magician()
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
        attack.damage = NumericalValue.magicianJumpAttack_Damage * rate;//造成傷害 
        attack.direction = NumericalValue.magicianJumpAttack_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.magicianJumpAttack_RepelDistance;//擊退距離
        attack.animationName = NumericalValue.magicianJumpAttack_Effect;//攻擊效果(播放動畫名稱)
        attack.forwardDistance = NumericalValue.magicianJumpAttack_ForwardDistance;//攻擊範圍中心點距離物件前方
        attack.attackRadius = NumericalValue.magicianJumpAttack_attackRadius;//攻擊半徑
        attack.isAttackBehind = NumericalValue.magicianJumpAttack_IsAttackBehind;//是否攻擊背後敵人

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)   
    }

    /// <summary>
    /// 普通攻擊1_法師
    /// </summary>
    void OnNormalAttack1_Magician()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;
  
        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率

        AttackMode attack = AttackMode.Instance;
        attack.performObject = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber("magicianNormalAttack_1"), GameSceneManagement.Instance.loadPath.magicianNormalAttack_1);//執行攻擊的物件(自身/射出物件)
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetShootFunction_Group);//設定執行函式       
        attack.damage = NumericalValue.magicianNormalAttack_1_Damage * rate;//造成傷害 
        attack.direction = NumericalValue.magicianNormalAttack_1_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.magicianNormalAttack_1_Repel;//擊退/擊飛距離
        attack.animationName = NumericalValue.magicianNormalAttack_1_Effect;//攻擊效果(播放動畫名稱)        

        attack.flightSpeed = NumericalValue.magicianNormalAttack_1_FlightSpeed;//飛行速度
        attack.lifeTime = NumericalValue.magicianNormalAttack_1_LifeTime;//生存時間
        attack.flightDiration = transform.forward;//飛行方向        
        attack.performObject.transform.position = transform.position + GetComponent<BoxCollider>().center + transform.forward * 1;//射出位置

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)                 
    }

    /// <summary>
    /// 普通攻擊2_法師
    /// </summary>
    void OnNormalAttack2_Magician()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率

        AttackMode attack = AttackMode.Instance;
        attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.isCritical = isCritical;//是否爆擊

        attack.function = new Action(attack.OnSetHitBoxFunction);//設定執行函式
        attack.damage = NumericalValue.magicianNormalAttack_2_Damge * rate;//造成傷害 
        attack.direction = NumericalValue.magicianNormalAttack_2_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.magicianNormalAttack_2_RepelDistance;//擊退距離
        attack.animationName = NumericalValue.magicianNormalAttack_2_Effect;//攻擊效果(播放動畫名稱)
        attack.forwardDistance = NumericalValue.magicianNormalAttack_2_ForwardDistance;//攻擊範圍中心點距離物件前方
        attack.attackRange = NumericalValue.magicianNormalAttack_2_attackRange;//攻擊範圍
        attack.isAttackBehind = NumericalValue.magicianNormalAttack_2_IsAttackBehind;//是否攻擊背後敵人

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)           
    }

    /// <summary>
    /// 普通攻擊3_法師
    /// </summary>
    void OnNormalAttack3_Magician()
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
        attack.damage = NumericalValue.magicianNormalAttack_3_Damge * rate;//造成傷害 
        attack.direction = NumericalValue.magicianNormalAttack_3_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.magicianNormalAttack_3_RepelDistance;//擊退距離
        attack.animationName = NumericalValue.magicianNormalAttack_3_Effect;//攻擊效果(播放動畫名稱)
        attack.forwardDistance = NumericalValue.magicianNormalAttack_3_ForwardDistance;//攻擊範圍中心點距離物件前方
        attack.attackRadius = NumericalValue.magicianNormalAttack_3_attackRadius;//攻擊範圍
        attack.isAttackBehind = NumericalValue.magicianNormalAttack_3_IsAttackBehind;//是否攻擊背後敵人

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)           
    }
}
