using UnityEngine;

/// <summary>
/// 遊戲數值
/// </summary>
[System.Serializable]
public class GameData_NumericalValue
{
    [Header("共通")]
    public float gravity;//重力
    public float criticalBonus;//報擊傷害加成
    public string[] levelNames;//關卡名稱

    [Header("Buff增加數值")]
    public string[] buffAbleString;//Buff增益文字
    public float[] buffAbleValue;//Buff增益數值

    [Header("攝影機")]
    public float distance;//與玩家距離
    public float limitUpAngle;//限制向上角度
    public float limitDownAngle;//限制向下角度
    //public float cameraAngle;//攝影機角度

    [Header("玩家")]
    public float playerHp;//玩家生命值
    public float playerMoveSpeed;//玩家移動速度
    public float playerJumpForce;//玩家跳躍力
    public float playerCriticalRate;//玩家暴擊率
    public float playerDodgeSeppd;//玩家閃躲速度
    public float playerSelfHealTime;//玩家自身回復時間

    [Header("據點")]
    public float strongholdHp;//據點HP

    [Header("同盟士兵HP")]
    public float allianceSoldier1_Hp;//同盟士兵1_生命值

    [Header("敵人HP")]
    public float boss_Hp;//Boss_生命值
    public float enemySoldier1_Hp;//敵人士兵1_生命值
    public float enemySoldier2_Hp;//敵人士兵2_生命值
    public float enemySoldier3_Hp;//敵人士兵3_生命值
    public float guardBoss_Hp;//城門守衛Boss_生命值

    #region 戰士
    [Header("戰士 普通攻擊1")]
    public float warriorNormalAttack_1_Damge;//戰士普通攻擊1_傷害
    public int warriorNormalAttack_1_RepelDirection;//戰士普通攻擊1_擊退方向(0:擊退 1:擊飛)
    public float warriorNormalAttack_1_RepelDistance;//戰士普通攻擊1_擊退/擊飛距離    
    public string warriorNormalAttack_1_Effect;//戰士普通攻擊1_效果(受擊者播放的動畫名稱)        
    public float warriorNormalAttack_1_ForwardDistance;//戰士普通攻擊1_攻擊範圍中心點距離物件前方
    public float warriorNormalAttack_1_attackRadius;//戰士普通攻擊1_攻擊半徑
    public bool warriorNormalAttack_1_IsAttackBehind;//戰士普通攻擊1_是否攻擊背後敵人

    [Header("戰士 普通攻擊2")]
    public float warriorNormalAttack_2_Damge;//戰士普通攻擊2_傷害
    public int warriorNormalAttack_2_RepelDirection;//戰士普通攻擊2_擊退方向(0:擊退 1:擊飛)
    public float warriorNormalAttack_2_RepelDistance;//戰士普通攻擊2_擊退/擊飛距離    
    public string warriorNormalAttack_2_Effect;//戰士普通攻擊2_效果(受擊者播放的動畫名稱)        
    public float warriorNormalAttack_2_ForwardDistance;//戰士普通攻擊2_攻擊範圍中心點距離物件前方
    public float warriorNormalAttack_2_attackRadius;//戰士普通攻擊2_攻擊半徑
    public bool warriorNormalAttack_2_IsAttackBehind;//戰士普通攻擊2_是否攻擊背後敵人

    [Header("戰士 普通攻擊3")]
    public float warriorNormalAttack_3_Damge;//戰士普通攻擊3_傷害
    public int warriorNormalAttack_3_RepelDirection;//戰士普通攻擊3_擊退方向(0:擊退 1:擊飛)
    public float warriorNormalAttack_3_RepelDistance;//戰士普通攻擊3_擊退/擊飛距離    
    public string warriorNormalAttack_3_Effect;//戰士普通攻擊3_效果(受擊者播放的動畫名稱)        
    public float warriorNormalAttack_3_ForwardDistance;//戰士普通攻擊3_攻擊範圍中心點距離物件前方
    public float warriorNormalAttack_3_attackRadius;//戰士普通攻擊3_攻擊半徑
    public bool warriorNormalAttack_3_IsAttackBehind;//戰士普通攻擊3_是否攻擊背後敵人

    [Header("戰士 跳躍攻擊")]
    public float warriorJumpAttack_Damage;//戰士跳躍攻擊_傷害
    public int warriorJumpAttack_RepelDirection;//戰士跳躍攻擊_擊退方向(0:擊退 1:擊飛)
    public float warriorJumpAttac_kRepelDistance;//戰士跳躍攻擊_擊退距離
    public string warriorJumpAttack_Effect;//戰士跳躍攻擊效果(受擊者播放的動畫名稱)
    public float warriorJumpAttack_ForwardDistance;//戰士普通攻擊3_攻擊範圍中心點距離物件前方
    public float warriorJumpAttack_attackRadius;//戰士普通攻擊3_攻擊半徑
    public bool warriorJumpAttack_IsAttackBehind;//戰士普通攻擊3_是否攻擊背後敵人

    [Header("戰士 技能攻擊1")]
    public float warriorSkillAttack_1_Damge;//戰士技能攻擊1_傷害
    public int warriorSkillAttack_1_RepelDirection;//戰士技能攻擊1_擊退方向(0:擊退 1:擊飛)
    public float warriorSkillAttack_1_RepelDistance;//戰士技能攻擊1_擊退/擊飛距離    
    public string warriorSkillAttack_1_Effect;//戰士普技能攻擊1_效果(受擊者播放的動畫名稱)        
    public float warriorSkillAttack_1_ForwardDistance;//戰士技能攻擊1_攻擊範圍中心點距離物件前方
    public float warriorSkillAttack_1_attackRadius;//戰士技能攻擊1_攻擊半徑
    public bool warriorSkillAttack_1_IsAttackBehind;//戰士技能攻擊1_是否攻擊背後敵人

    [Header("戰士 技能攻擊2")]
    public float warriorSkillAttack_2_Damge;//戰士技能攻擊2_傷害
    public int warriorSkillAttack_2_RepelDirection;//戰士技能攻擊2_擊退方向(0:擊退 1:擊飛)
    public float warriorSkillAttack_2_RepelDistance;//戰士技能攻擊2_擊退/擊飛距離    
    public string warriorSkillAttack_2_Effect;//戰士普技能攻擊2_效果(受擊者播放的動畫名稱)        
    public float warriorSkillAttack_2_ForwardDistance;//戰士技能攻擊2_攻擊範圍中心點距離物件前方
    public float warriorSkillAttack_2_attackRadius;//戰士技能攻擊2_攻擊半徑
    public bool warriorSkillAttack_2_IsAttackBehind;//戰士技能攻擊2_是否攻擊背後敵人

    [Header("戰士 技能攻擊3")]
    public float[] warriorSkillAttack_3_Damge;//戰士技能攻擊2_傷害
    public int[] warriorSkillAttack_3_RepelDirection;//戰士技能攻擊3_擊退方向(0:擊退 1:擊飛)
    public float[] warriorSkillAttack_3_RepelDistance;//戰士技能攻擊3_擊退/擊飛距離    
    public string[] warriorSkillAttack_3_Effect;//戰士普技能攻擊3_效果(受擊者播放的動畫名稱)        
    public float[] warriorSkillAttack_3_ForwardDistance;//戰士技能攻擊3_攻擊範圍中心點距離物件前方
    public float[] warriorSkillAttack_3_attackRadius;//戰士技能攻擊3_攻擊半徑
    public bool[] warriorSkillAttack_3_IsAttackBehind;//戰士技能攻擊3_是否攻擊背後敵人
    #endregion

    #region 弓箭手   
    [Header("弓箭手 普通攻擊")]
    public float[] archerNormalAttack_Damge;//弓箭手普通攻擊_傷害
    public int[] archerNormalAttack_RepelDirection;//弓箭手普通攻擊_擊退方向(0:擊退 1:擊飛)
    public float[] archerNormalAttack_RepelDistance;//弓箭手普通攻擊_擊退/擊飛距離    
    public string[] archerNormalAttack_Effect;//弓箭手普通攻擊_效果(受擊者播放的動畫名稱)
    public float[] archerNormalAttack_FloatSpeed;//弓箭手普通攻擊飛行速度
    public float[] archerNormalAttack_LifeTime;//弓箭手普通攻擊生存時間

    [Header("弓箭手 跳躍攻擊")]
    public float archerJumpAttack_Damage;//弓箭手跳躍攻擊_傷害
    public int archerJumpAttack_RepelDirection;//弓箭手跳躍攻擊_擊退方向(0:擊退 1:擊飛)  
    public float archerJumpAttack_RepelDistance;//弓箭手跳躍攻擊_擊退距離
    public string archerJumpAttack_Effect;//弓箭手跳躍攻擊_效果(受擊者播放的動畫名稱)
    public float archerJumpAttack_ForwardDistance;//弓箭手跳躍攻擊_攻擊範圍中心點距離物件前方
    public float archerJumpAttack_attackRadius;//弓箭手跳躍攻擊_攻擊半徑
    public bool archerJumpAttack_IsAttackBehind;//弓箭手跳躍攻擊_是否攻擊背後敵人

    [Header("弓箭手 技能攻擊1")]
    public float archerSkillAttack_1_Damage;//弓箭手技能攻擊1_傷害
    public int archerSkillAttack_1_RepelDirection;//弓箭手技能攻擊1_擊退方向(0:擊退 1:擊飛)
    public float archerSkillAttack_1_Repel;//弓箭手技能攻擊1_擊退距離        
    public string archerSkillAttack_1_Effect;//弓箭手技能攻擊1_效果(受擊者播放的動畫名稱)
    public float archerSkillAttack_1_FlightSpeed;//弓箭手技能攻擊1_飛行速度
    public float archerSkillAttack_1_LifeTime;//弓箭手技能攻擊1_生存時間

    [Header("弓箭手 技能攻擊2")]
    public float archerSkillAttack_2_Damge;//弓箭手技能攻擊2_傷害
    public int archerSkillAttack_2_RepelDirection;//弓箭手技能攻擊2_擊退方向(0:擊退 1:擊飛)
    public float archerSkillAttack_2_RepelDistance;//弓箭手技能攻擊2_擊退/擊飛距離    
    public string archerSkillAttack_2_Effect;//弓箭手技能攻擊2_效果(受擊者播放的動畫名稱)        
    public float archerSkillAttack_2_ForwardDistance;//弓箭手技能攻擊2_攻擊範圍中心點距離物件前方
    public float archerSkillAttack_2_attackRadius;//弓箭手技能攻擊2_攻擊半徑
    public bool archerSkillAttack_2_IsAttackBehind;//弓箭手技能攻擊2_是否攻擊背後敵人

    [Header("弓箭手 技能攻擊3")]
    public float archerSkillAttack_3_Damge;//弓箭手技能攻擊3_傷害
    public int archerSkillAttack_3_RepelDirection;//弓箭手技能攻擊3_擊退方向(0:擊退 1:擊飛)
    public float archerSkillAttack_3_RepelDistance;//弓箭手技能攻擊3_擊退/擊飛距離    
    public string archerSkillAttack_3_Effect;//弓箭手技能攻擊3_效果(受擊者播放的動畫名稱)        
    public float archerSkillAttack_3_ForwardDistance;//弓箭手技能攻擊3_攻擊範圍中心點距離物件前方
    public float archerSkillAttack_3_attackRadius;//弓箭手技能攻擊3_攻擊半徑
    public bool archerSkillAttack_3_IsAttackBehind;//弓箭手技能攻擊3_是否攻擊背後敵人
    #endregion

    #region 法師
    [Header("法師 普通攻擊1")]
    public float magicianNormalAttack_1_Damage;//法師普通攻擊1_傷害
    public int magicianNormalAttack_1_RepelDirection;//法師普通攻擊1_擊退方向(0:擊退 1:擊飛)
    public float magicianNormalAttack_1_Repel;//法師普通攻擊1_擊退距離        
    public string magicianNormalAttack_1_Effect;//法師普通攻擊1_效果(受擊者播放的動畫名稱)
    public float magicianNormalAttack_1_FlightSpeed;//法師普通攻擊1_飛行速度
    public float magicianNormalAttack_1_LifeTime;//法師普通攻擊1_生存時間

    [Header("法師 普通攻擊2")]
    public float magicianNormalAttack_2_Damge;//法師普通攻擊2_傷害
    public int magicianNormalAttack_2_RepelDirection;//法師普通攻擊2_擊退方向(0:擊退 1:擊飛)
    public float magicianNormalAttack_2_RepelDistance;//法師普通攻擊2_擊退/擊飛距離    
    public string magicianNormalAttack_2_Effect;//法師普通攻擊2_效果(受擊者播放的動畫名稱)        
    public float magicianNormalAttack_2_ForwardDistance;//法師普通攻擊2_攻擊範圍中心點距離物件前方
    public Vector3 magicianNormalAttack_2_attackRange;//法師普通攻擊2_攻擊範圍
    public bool magicianNormalAttack_2_IsAttackBehind;//法師普通攻擊2_是否攻擊背後敵人

    [Header("法師 普通攻擊3")]
    public float magicianNormalAttack_3_Damge;//法師普通攻擊3_傷害
    public int magicianNormalAttack_3_RepelDirection;//法師普通攻擊3_擊退方向(0:擊退 1:擊飛)
    public float magicianNormalAttack_3_RepelDistance;//法師普通攻擊3_擊退/擊飛距離    
    public string magicianNormalAttack_3_Effect;//法師普通攻擊3_效果(受擊者播放的動畫名稱)        
    public float magicianNormalAttack_3_ForwardDistance;//法師普通攻擊3_攻擊範圍中心點距離物件前方
    public float magicianNormalAttack_3_attackRadius;//法師普通攻擊3_攻擊半徑
    public bool magicianNormalAttack_3_IsAttackBehind;//法師普通攻擊3_是否攻擊背後敵人

    [Header("法師 跳躍攻擊")]
    public float magicianJumpAttack_Damage;//法師跳躍攻擊_傷害
    public int magicianJumpAttack_RepelDirection;//法師跳躍攻擊_擊退方向(0:擊退 1:擊飛)  
    public float magicianJumpAttack_RepelDistance;//法師跳躍攻擊_擊退距離
    public string magicianJumpAttack_Effect;//法師跳躍攻擊_效果(受擊者播放的動畫名稱)
    public float magicianJumpAttack_ForwardDistance;//法師跳躍攻擊_攻擊範圍中心點距離物件前方
    public float magicianJumpAttack_attackRadius;//法師跳躍攻擊_攻擊半徑
    public bool magicianJumpAttack_IsAttackBehind;//法師跳躍攻擊_是否攻擊背後敵人

    [Header("法師 技能攻擊1")]
    public float magicianSkillAttack_1_HealValue;//法師普通攻擊1_治療量    
    public float magicianSkillAttack_1_ForwardDistance;//法師普通攻擊1_攻擊範圍中心點距離物件前方
    public float magicianSkillAttack_1_attackRange;//法師普通攻擊1_治療範圍
    public bool magicianSkillAttack_1_IsAttackBehind;//法師普通攻擊1_是否治療背後盟友

    /*[Header("法師 技能攻擊2")]
    public float[] magicianSkillAttack_2_Damge;//法師技能攻擊2_傷害
    public int[] magicianSkillAttack_2_RepelDirection;//法師技能攻擊2_擊退方向(0:擊退 1:擊飛)
    public float[] magicianSkillAttack_2_RepelDistance;//法師技能攻擊2_擊退/擊飛距離    
    public string[] magicianSkillAttack_2_Effect;//法師技能攻擊2_效果(受擊者播放的動畫名稱)        
    public float[] magicianSkillAttack_2_ForwardDistance;//法師技能攻擊2_攻擊範圍中心點距離物件前方
    public float[] magicianSkillAttack_2_attackRadius;//法師技能攻擊2_攻擊半徑
    public bool[] magicianSkillAttack_2_IsAttackBehind;//法師技能攻擊2_是否攻擊背後敵人*/

    [Header("法師 技能攻23")]
    public float magicianSkillAttack_2_Damge;//法師技能攻擊2_傷害
    public int magicianSkillAttack_2_RepelDirection;//法師技能攻擊2_擊退方向(0:擊退 1:擊飛)
    public float magicianSkillAttack_2_RepelDistance;//法師技能攻擊2_擊退/擊飛距離    
    public string magicianSkillAttack_2_Effect;//法師技能攻擊2_效果(受擊者播放的動畫名稱)        
    public float magicianSkillAttack_2_ForwardDistance;//法師技能攻擊2_攻擊範圍中心點距離物件前方
    public float magicianSkillAttack_2_attackRadius;//法師技能攻擊2_攻擊半徑
    public bool magicianSkillAttack_2_IsAttackBehind;//法師技能攻擊2_是否攻擊背後敵人

    [Header("法師 技能攻擊3")]
    public float magicianSkillAttack_3_Damge;//法師技能攻擊3_傷害
    public int magicianSkillAttack_3_RepelDirection;//法師技能攻擊3_擊退方向(0:擊退 1:擊飛)
    public float magicianSkillAttack_3_RepelDistance;//法師技能攻擊3_擊退/擊飛距離    
    public string magicianSkillAttack_3_Effect;//法師技能攻擊3_效果(受擊者播放的動畫名稱)        
    public float magicianSkillAttack_3_ForwardDistance;//法師技能攻擊3_攻擊範圍中心點距離物件前方
    public float magicianSkillAttack_3_attackRadius;//法師技能攻擊3_攻擊半徑
    public bool magicianSkillAttack_3_IsAttackBehind;//法師技能攻擊3_是否攻擊背後敵人
    #endregion

    #region 敵人士兵1(石頭人)
    [Header("敵人士兵1 攻擊1")]
    public float enemySoldier1_Attack_1_Damge;//敵人士兵1_攻擊1_傷害
    public int enemySoldier1_Attack_1_RepelDirection;//敵人士兵1_攻擊1_擊退方向(0:擊退 1:擊飛)
    public float enemySoldier1_Attack_1_RepelDistance;//敵人士兵1_攻擊1_擊退/擊飛距離    
    public string enemySoldier1_Attack_1_Effect;//敵人士兵1_攻擊1_效果(受擊者播放的動畫名稱)        
    public float enemySoldier1_Attack_1_ForwardDistance;//敵人士兵1_攻擊1_攻擊範圍中心點距離物件前方
    public float enemySoldier1_Attack_1_attackRadius;//敵人士兵1_攻擊1_攻擊半徑
    public bool enemySoldier1_Attack_1_IsAttackBehind;//敵人士兵1_攻擊1_是否攻擊背後敵人

    [Header("敵人士兵1 攻擊2")]
    public float enemySoldier1_Attack_2_Damge;//敵人士兵1_攻擊2_傷害
    public int enemySoldier1_Attack_2_RepelDirection;//敵人士兵1_攻擊2_擊退方向(0:擊退 1:擊飛)
    public float enemySoldier1_Attack_2_RepelDistance;//敵人士兵1_攻擊2_擊退/擊飛距離    
    public string enemySoldier1_Attack_2_Effect;//敵人士兵1_攻擊2_效果(受擊者播放的動畫名稱)        
    public float enemySoldier1_Attack_2_ForwardDistance;//敵人士兵1_攻擊2_攻擊範圍中心點距離物件前方
    public float enemySoldier1_Attack_2_attackRadius;//敵人士兵1_攻擊2_攻擊半徑
    public bool enemySoldier1_Attack_2_IsAttackBehind;//敵人士兵1_攻擊2_是否攻擊背後敵人

    [Header("敵人士兵1 攻擊3")]
    public float enemySoldier1_Attack_3_Damge;//敵人士兵1_攻擊3_傷害
    public int enemySoldier1_Attack_3_RepelDirection;//敵人士兵1_攻擊3_擊退方向(0:擊退 1:擊飛)
    public float enemySoldier1_Attack_3_RepelDistance;//敵人士兵1_攻擊3_擊退/擊飛距離    
    public string enemySoldier1_Attack_3_Effect;//敵人士兵1_攻擊3_效果(受擊者播放的動畫名稱)        
    public float enemySoldier1_Attack_3_ForwardDistance;//敵人士兵1_攻擊3_攻擊範圍中心點距離物件前方
    public float enemySoldier1_Attack_3_attackRadius;//敵人士兵1_攻擊3_攻擊半徑
    public bool enemySoldier1_Attack_3_IsAttackBehind;//敵人士兵1_攻擊3_是否攻擊背後敵人
    #endregion

    #region 敵人士兵2(弓箭手)
    [Header("敵人士兵2 攻擊1")]
    public float enemySoldier2_Attack1_Damge;//敵人士兵2_攻擊1_傷害
    public int enemySoldier2_Attack1_RepelDirection;//敵人士兵2_攻擊1_擊退方向(0:擊退 1:擊飛)
    public float enemySoldier2_Attack1_RepelDistance;//敵人士兵2_攻擊1_擊退/擊飛距離    
    public string enemySoldier2_Attack1_Effect;//敵人士兵2_攻擊1_效果(受擊者播放的動畫名稱)
    public float enemySoldier2_Attack1_FloatSpeed;//敵人士兵2_攻擊1_飛行速度
    public float enemySoldier2_Attack1_LifeTime;//敵人士兵2_攻擊1_生存時間

    [Header("敵人士兵2 攻擊2")]
    public float enemySoldier2_Attack2_Damge;//敵人士兵2_攻擊2_傷害
    public int enemySoldier2_Attack2_RepelDirection;//敵人士兵2_攻擊2_擊退方向(0:擊退 1:擊飛)
    public float enemySoldier2_Attack2_RepelDistance;//敵人士兵2_攻擊2_擊退/擊飛距離    
    public string enemySoldier2_Attack2_Effect;//敵人士兵2_攻擊2_效果(受擊者播放的動畫名稱)
    public float enemySoldier2_Attack2_FloatSpeed;//敵人士兵2_攻擊2_飛行速度
    public float enemySoldier2_Attack2_LifeTime;//敵人士兵2_攻擊2_生存時間

    [Header("敵人士兵2 攻擊3")]
    public float enemySoldier2_Attack_3_Damge;//敵人士兵2_攻擊3_傷害
    public int enemySoldier2_Attack_3_RepelDirection;//敵人士兵2_攻擊3_擊退方向(0:擊退 1:擊飛)
    public float enemySoldier2_Attack_3_RepelDistance;//敵人士兵2_攻擊3_擊退/擊飛距離    
    public string enemySoldier2_Attack_3_Effect;//敵人士兵2_攻擊3_效果(受擊者播放的動畫名稱)        
    public float enemySoldier2_Attack_3_ForwardDistance;//敵人士兵2_攻擊3_攻擊範圍中心點距離物件前方
    public float enemySoldier2_Attack_3_attackRadius;//敵人士兵2_攻擊3_攻擊半徑
    public bool enemySoldier2_Attack_3_IsAttackBehind;//敵人士兵2_攻擊3_是否攻擊背後敵人
    #endregion

    #region 敵人士兵3(斧頭人)
    [Header("敵人士兵3 攻擊1")]
    public float enemySoldier3_Attack_1_Damge;//敵人士兵3_攻擊1_傷害
    public int enemySoldier3_Attack_1_RepelDirection;//敵人士兵3_攻擊1_擊退方向(0:擊退 1:擊飛)
    public float enemySoldier3_Attack_1_RepelDistance;//敵人士兵3_攻擊1_擊退/擊飛距離    
    public string enemySoldier3_Attack_1_Effect;//敵人士兵3_攻擊1_效果(受擊者播放的動畫名稱)        
    public float enemySoldier3_Attack_1_ForwardDistance;//敵人士兵3_攻擊1_攻擊範圍中心點距離物件前方
    public float enemySoldier3_Attack_1_attackRadius;//敵人士兵3_攻擊1_攻擊半徑
    public bool enemySoldier3_Attack_1_IsAttackBehind;//敵人士兵3_攻擊1_是否攻擊背後敵人

    [Header("敵人士兵3 攻擊2")]
    public float enemySoldier3_Attack_2_Damge;//敵人士兵3_攻擊2_傷害
    public int enemySoldier3_Attack_2_RepelDirection;//敵人士兵3_攻擊2_擊退方向(0:擊退 1:擊飛)
    public float enemySoldier3_Attack_2_RepelDistance;//敵人士兵3_攻擊2_擊退/擊飛距離    
    public string enemySoldier3_Attack_2_Effect;//敵人士兵1_攻擊3_效果(受擊者播放的動畫名稱)        
    public float enemySoldier3_Attack_2_ForwardDistance;//敵人士兵3_攻擊2_攻擊範圍中心點距離物件前方
    public float enemySoldier3_Attack_2_attackRadius;//敵人士兵3_攻擊2_攻擊半徑
    public bool enemySoldier3_Attack_2_IsAttackBehind;//敵人士兵3_攻擊2_是否攻擊背後敵人

    [Header("敵人士兵13攻擊3")]
    public float enemySoldier3_Attack_3_Damge;//敵人士兵3_攻擊3_傷害
    public int enemySoldier3_Attack_3_RepelDirection;//敵人士兵3_攻擊3_擊退方向(0:擊退 1:擊飛)
    public float enemySoldier3_Attack_3_RepelDistance;//敵人士兵3_攻擊3_擊退/擊飛距離    
    public string enemySoldier3_Attack_3_Effect;//敵人士兵3_攻擊3_效果(受擊者播放的動畫名稱)        
    public float enemySoldier3_Attack_3_ForwardDistance;//敵人士兵3_攻擊3_攻擊範圍中心點距離物件前方
    public float enemySoldier3_Attack_3_attackRadius;//敵人士兵3_攻擊3_攻擊半徑
    public bool enemySoldier3_Attack_3_IsAttackBehind;//敵人士兵3_攻擊3_是否攻擊背後敵人
    #endregion

    #region 守衛Boss
    [Header("守衛Boss 攻擊1")]
    public float guardBoss_Attack_1_Damge;//守衛Boss_攻擊1_傷害
    public int guardBoss_Attack_1_RepelDirection;//守衛Boss_攻擊1_擊退方向(0:擊退 1:擊飛)
    public float guardBoss_Attack_1_RepelDistance;//守衛Boss_攻擊1_擊退/擊飛距離    
    public string guardBoss_Attack_1_Effect;//守衛Boss_攻擊1_效果(受擊者播放的動畫名稱)        
    public float guardBoss_Attack_1_ForwardDistance;//守衛Boss_攻擊1_攻擊範圍中心點距離物件前方
    public float guardBoss_Attack_1_attackRadius;//守衛Boss_攻擊1_攻擊半徑
    public bool guardBoss_Attack_1_IsAttackBehind;//守衛Boss1_攻擊1_是否攻擊背後敵人

    [Header("守衛Boss 攻擊2")]
    public float guardBoss_Attack_2_Damge;//守衛Boss_攻擊2_傷害
    public int guardBoss_Attack_2_RepelDirection;//守衛Boss_攻擊2_擊退方向(0:擊退 1:擊飛)
    public float guardBoss_Attack_2_RepelDistance;//守衛Boss_攻擊2_擊退/擊飛距離    
    public string guardBoss_Attack_2_Effect;//守衛Boss_攻擊2_效果(受擊者播放的動畫名稱)        
    public float guardBoss_Attack_2_ForwardDistance;//守衛Boss_攻擊2_攻擊範圍中心點距離物件前方
    public float guardBoss_Attack_2_attackRadius;//守衛Boss_攻擊2_攻擊半徑
    public bool guardBoss_Attack_2_IsAttackBehind;//守衛Boss1_攻擊2_是否攻擊背後敵人

    [Header("守衛Boss 攻擊3")]
    public float guardBoss_Attack_3_Damge;//守衛Boss_攻擊3_傷害
    public int guardBoss_Attack_3_RepelDirection;//守衛Boss_攻擊3_擊退方向(0:擊退 1:擊飛)
    public float guardBoss_Attack_3_RepelDistance;//守衛Boss_攻擊3_擊退/擊飛距離    
    public string guardBoss_Attack_3_Effect;//守衛Boss_攻擊3_效果(受擊者播放的動畫名稱)        
    public float guardBoss_Attack_3_ForwardDistance;//守衛Boss_攻擊3_攻擊範圍中心點距離物件前方
    public float guardBoss_Attack_3_attackRadius;//守衛Boss_攻擊3_攻擊半徑
    public bool guardBoss_Attack_3_IsAttackBehind;//守衛Boss_攻擊3_是否攻擊背後敵人

    [Header("守衛Boss 攻擊4")]
    public float guardBoss_Attack_4_Damge;//守衛Boss_攻擊4_傷害
    public int guardBoss_Attack_4_RepelDirection;//守衛Boss_攻擊4_擊退方向(0:擊退 1:擊飛)
    public float guardBoss_Attack_4_RepelDistance;//守衛Boss_攻擊4_擊退/擊飛距離    
    public string guardBoss_Attack_4_Effect;//守衛Boss_攻擊4_效果(受擊者播放的動畫名稱)        
    public float guardBoss_Attack_4_ForwardDistance;//守衛Boss_攻擊4_攻擊範圍中心點距離物件前方
    public float guardBoss_Attack_4_attackRadius;//守衛Boss_攻擊4_攻擊半徑
    public bool guardBoss_Attack_4_IsAttackBehind;//守衛Boss_攻擊4_是否攻擊背後敵人
    #endregion

    #region Boss
    [Header("Boss 攻擊1")]
    public float bossAttack1_Damge;//Boss攻擊1_傷害
    public int bossAttack1_RepelDirection;//Boss攻擊1_擊退方向(0:擊退 1:擊飛)
    public float bossAttack1_RepelDistance;//Boss攻擊1_擊退/擊飛距離    
    public string bossAttack1_Effect;//Boss攻擊1_效果(受擊者播放的動畫名稱)
    public float bossAttack1_FloatSpeed;//Boss攻擊1_飛行速度
    public float bossAttack1_LifeTime;//Boss攻擊1_生存時間

    [Header("Boss 攻擊2")]
    public float bossAttack2_Damge;//Boss攻擊2_傷害
    public int bossAttack2_RepelDirection;//Boss攻擊2_擊退方向(0:擊退 1:擊飛)
    public float bossAttack2_RepelDistance;//Boss攻擊2_擊退/擊飛距離    
    public string bossAttack2_Effect;//Boss攻擊2_效果(受擊者播放的動畫名稱)
    public float bossAttack2_FloatSpeed;//Boss攻擊2_飛行速度
    public float bossAttack2_LifeTime;//Boss攻擊2_生存時間

    [Header("Boss 攻擊3")]
    public float bossAttack3_Damge;//Boss攻擊3_傷害
    public int bossAttack3_RepelDirection;//Boss攻擊3_擊退方向(0:擊退 1:擊飛)
    public float bossAttack3_RepelDistance;//Boss攻擊3_擊退/擊飛距離    
    public string bossAttack3_Effect;//Boss攻擊3_效果(受擊者播放的動畫名稱)        
    public float bossAttack3_ForwardDistance;//Boss攻擊3_攻擊範圍中心點距離物件前方
    public float bossAttack3_attackRadius;//Boss攻擊3_攻擊半徑
    public bool bossAttack3_IsAttackBehind;//Boss攻擊3_是否攻擊背後敵人
    #endregion

    /// <summary>
    /// 建構子
    /// </summary>
    private GameData_NumericalValue()
    {
        //共通
        gravity = 6.8f;//重力
        criticalBonus = 1.3f;//報擊傷害加成
        levelNames = new string[] { "第一章 : 橫掃千軍", "最終章 : 屠龍者" };//關卡名稱

        //Buff增加數值
        buffAbleString = new string[] { "生命", "傷害", "防禦", "吸血", "移動", "回血" };//Buff增益文字
        buffAbleValue = new float[] { 30, 25, 25, 20, 25, 1 };//Buff增益數值(%)

        //攝影機
        distance = 2.6f;//與玩家距離        
        limitUpAngle = 35;//限制向上角度
        limitDownAngle = 13;//限制向下角度
        //cameraAngle = 20;//攝影機角度

        //玩家
        playerHp = 850;//玩家生命值850
        playerMoveSpeed = 6.5f;//玩家移動速度  6.5
        playerJumpForce = 11.05f;//玩家跳躍力
        playerCriticalRate = 15;//玩家暴擊率
        playerDodgeSeppd = 6.3f;//玩家閃躲速度
        playerSelfHealTime = 5;//玩家自身回復時間(秒)

        //據點
        strongholdHp = 350;//據點HP350

        //同盟士兵HP
        allianceSoldier1_Hp = 40;//同盟士兵1_生命值

        //敵人HP
        boss_Hp = 10;//Boss_生命值
        enemySoldier1_Hp = 80;//石頭人_生命值
        enemySoldier2_Hp = 60;//弓箭手_生命值
        enemySoldier3_Hp = 40;//敵人士兵3_生命值
        guardBoss_Hp = 600;//城門守衛Boss_生命值600

        #region 戰士
        //戰士 普通攻擊1
        warriorNormalAttack_1_Damge = 30;//戰士普通攻擊1_傷害
        warriorNormalAttack_1_RepelDirection = 0;//戰士普通攻擊1_擊退方向(0:擊退 1:擊飛)
        warriorNormalAttack_1_RepelDistance = 30;//戰士普通攻擊1_擊退/擊飛距離    
        warriorNormalAttack_1_Effect = "Pain";//戰士普通攻擊1_效果(受擊者播放的動畫名稱)            
        warriorNormalAttack_1_ForwardDistance = 1.3f;//戰士普通攻擊1_攻擊範圍中心點距離物件前方
        warriorNormalAttack_1_attackRadius = 1.5f;//戰士普通攻擊1_攻擊半徑    
        warriorNormalAttack_1_IsAttackBehind = false;//戰士普通攻擊1_是否攻擊背後敵人

        //戰士 普通攻擊2
        warriorNormalAttack_2_Damge = 33;//戰士普通攻擊1_傷害
        warriorNormalAttack_2_RepelDirection = 0;//戰士普通攻擊1_擊退方向(0:擊退 1:擊飛)
        warriorNormalAttack_2_RepelDistance = 30;//戰士普通攻擊1_擊退/擊飛距離    
        warriorNormalAttack_2_Effect = "Pain";//戰士普通攻擊1_效果(受擊者播放的動畫名稱)            
        warriorNormalAttack_2_ForwardDistance = 1.3f;//戰士普通攻擊1_攻擊範圍中心點距離物件前方
        warriorNormalAttack_2_attackRadius = 1.5f;//戰士普通攻擊1_攻擊半徑    
        warriorNormalAttack_2_IsAttackBehind = false;//戰士普通攻擊1_是否攻擊背後敵人

        //戰士 普通攻擊3
        warriorNormalAttack_3_Damge = 36;//戰士普通攻擊1_傷害
        warriorNormalAttack_3_RepelDirection = 0;//戰士普通攻擊1_擊退方向(0:擊退 1:擊飛)
        warriorNormalAttack_3_RepelDistance = 45;//戰士普通攻擊1_擊退/擊飛距離    
        warriorNormalAttack_3_Effect = "Pain";//戰士普通攻擊1_效果(受擊者播放的動畫名稱)            
        warriorNormalAttack_3_ForwardDistance = 0.0f;//戰士普通攻擊1_攻擊範圍中心點距離物件前方
        warriorNormalAttack_3_attackRadius = 3.5f;//戰士普通攻擊1_攻擊半徑    
        warriorNormalAttack_3_IsAttackBehind = true;//戰士普通攻擊1_是否攻擊背後敵人

        //戰士 跳躍攻擊
        warriorJumpAttack_Damage = 37;//戰士跳躍攻擊_傷害
        warriorJumpAttack_RepelDirection = 0;//戰士跳躍攻擊_擊退方向(0:擊退 1:擊飛)
        warriorJumpAttac_kRepelDistance = 50;//戰士跳躍攻擊_擊退距離
        warriorJumpAttack_Effect = "Pain";//戰士跳躍攻擊效果(受擊者播放的動畫名稱)
        warriorJumpAttack_ForwardDistance = 0.77f;//戰士普通攻擊3_攻擊範圍中心點距離物件前方
        warriorJumpAttack_attackRadius = 1.3f;//戰士普通攻擊3_攻擊半徑
        warriorJumpAttack_IsAttackBehind = true;//戰士普通攻擊3_是否攻擊背後敵人

        //戰士 技能攻擊1
        warriorSkillAttack_1_Damge = 50;//戰士技能攻擊2_傷害
        warriorSkillAttack_1_RepelDirection = 0;//戰士技能攻擊2_擊退方向(0:擊退 1:擊飛)
        warriorSkillAttack_1_RepelDistance = 40;//戰士技能攻擊2_擊退/擊飛距離    
        warriorSkillAttack_1_Effect = "Pain";//戰士普技能攻擊2_效果(受擊者播放的動畫名稱)        
        warriorSkillAttack_1_ForwardDistance = 1.6f;//戰士技能攻擊2_攻擊範圍中心點距離物件前方
        warriorSkillAttack_1_attackRadius = 1.65f;//戰士技能攻擊2_攻擊半徑
        warriorSkillAttack_1_IsAttackBehind = false;//戰士技能攻擊2_是否攻擊背後敵人

        //戰士 技能攻擊2
        warriorSkillAttack_2_Damge = 28;//戰士技能攻擊2_傷害
        warriorSkillAttack_2_RepelDirection = 0;//戰士技能攻擊2_擊退方向(0:擊退 1:擊飛)
        warriorSkillAttack_2_RepelDistance = 25;//戰士技能攻擊2_擊退/擊飛距離    
        warriorSkillAttack_2_Effect = "Pain";//戰士普技能攻擊2_效果(受擊者播放的動畫名稱)        
        warriorSkillAttack_2_ForwardDistance = 1.3f;//戰士技能攻擊2_攻擊範圍中心點距離物件前方
        warriorSkillAttack_2_attackRadius = 1.5f;//戰士技能攻擊2_攻擊半徑
        warriorSkillAttack_2_IsAttackBehind = false;//戰士技能攻擊2_是否攻擊背後敵人

        //戰士 技能攻擊3
        warriorSkillAttack_3_Damge = new float[] { 16, 16, 35 };//戰士技能攻擊3_傷害
        warriorSkillAttack_3_RepelDirection = new int[] { 0, 0, 0 };//戰士技能攻擊3_擊退方向(0:擊退 1:擊飛)
        warriorSkillAttack_3_RepelDistance = new float[] { 25, 25, 30 };//戰士技能攻擊3_擊退/擊飛距離    
        warriorSkillAttack_3_Effect = new string[] { "Pain", "Pain", "Pain" };//戰士普技能攻擊3_效果(受擊者播放的動畫名稱)        
        warriorSkillAttack_3_ForwardDistance = new float[] { 1.3f, 1.3f, 0 };//戰士技能攻擊3_攻擊範圍中心點距離物件前方
        warriorSkillAttack_3_attackRadius = new float[] { 1.45f, 1.45f, 5f };//戰士技能攻擊3_攻擊半徑
        warriorSkillAttack_3_IsAttackBehind = new bool[] { false, false, true };//戰士技能攻擊3_是否攻擊背後敵人
        #endregion

        #region 弓箭手
        //弓箭手 普通攻擊
        archerNormalAttack_Damge = new float[] { 11, 11, 16 };//弓箭手普通攻擊_傷害
        archerNormalAttack_RepelDirection = new int[] { 0, 0, 0 };//弓箭手普通攻擊_擊退方向(0:擊退 1:擊飛)
        archerNormalAttack_RepelDistance = new float[] { 10, 10, 10 };//弓箭手普通攻擊_擊退/擊飛距離        
        archerNormalAttack_Effect = new string[] { "Pain", "Pain", "Pain" };//弓箭手普通攻擊_效果(受擊者播放的動畫名稱)     
        archerNormalAttack_FloatSpeed = new float[] { 30, 30, 30 };//弓箭手普通攻擊飛行速度
        archerNormalAttack_LifeTime = new float[] { 0.6f, 0.6f, 0.6f };//弓箭手普通攻擊生存時間

        //弓箭手 跳躍攻擊
        archerJumpAttack_Damage = 36;//弓箭手跳躍攻擊傷害
        archerJumpAttack_RepelDirection = 0;//弓箭手跳躍攻擊擊退方向(0:擊退 1:擊飛)
        archerJumpAttack_RepelDistance = 25;//弓箭手跳躍攻擊 擊退距離
        archerJumpAttack_Effect = "Pain";//弓箭手跳躍攻擊效果(受擊者播放的動畫名稱)
        archerJumpAttack_ForwardDistance = 0;//弓箭手跳躍攻擊_攻擊範圍中心點距離物件前方
        archerJumpAttack_attackRadius = 1.4f;//弓箭手跳躍攻擊_攻擊半徑
        archerJumpAttack_IsAttackBehind = false;//弓箭手跳躍攻擊_是否攻擊背後敵人

        //弓箭手 技能攻擊1
        archerSkillAttack_1_Damage = 11;//弓箭手技能攻擊1_傷害
        archerSkillAttack_1_RepelDirection = 12;//弓箭手技能攻擊1_擊退方向(0:擊退 1:擊飛)
        archerSkillAttack_1_Repel = 13;//弓箭手技能攻擊1_擊退距離        
        archerSkillAttack_1_Effect = "Pain";//弓箭手技能攻擊1_效果(受擊者播放的動畫名稱)
        archerSkillAttack_1_FlightSpeed = 30;//弓箭手技能攻擊1_飛行速度
        archerSkillAttack_1_LifeTime = 0.4f;//弓箭手技能攻擊1_生存時間

        //弓箭手 技能攻擊2
        archerSkillAttack_2_Damge = 35;//弓箭手技能攻擊2_傷害
        archerSkillAttack_2_RepelDirection = 0;//弓箭手技能攻擊2_擊退方向(0:擊退 1:擊飛)
        archerSkillAttack_2_RepelDistance = 30;//弓箭手技能攻擊2_擊退/擊飛距離    
        archerSkillAttack_2_Effect = "Pain";//弓箭手技能攻擊2_效果(受擊者播放的動畫名稱)        
        archerSkillAttack_2_ForwardDistance = 1.3f;//弓箭手技能攻擊2_攻擊範圍中心點距離物件前方
        archerSkillAttack_2_attackRadius = 1.4f;//弓箭手技能攻擊2_攻擊半徑
        archerSkillAttack_2_IsAttackBehind = false;//弓箭手技能攻擊2_是否攻擊背後敵人

        //弓箭手 技能攻擊3
        archerSkillAttack_3_Damge = 13;//弓箭手技能攻擊3_傷害
        archerSkillAttack_3_RepelDirection = 1;//弓箭手技能攻擊3_擊退方向(0:擊退 1:擊飛)
        archerSkillAttack_3_RepelDistance = 0;//弓箭手技能攻擊3_擊退/擊飛距離    
        archerSkillAttack_3_Effect = "Pain";//弓箭手技能攻擊3_效果(受擊者播放的動畫名稱)        
        archerSkillAttack_3_ForwardDistance = 0;//弓箭手技能攻擊3_攻擊範圍中心點距離物件前方
        archerSkillAttack_3_attackRadius = 5.0f;//弓箭手技能攻擊3_攻擊半徑
        archerSkillAttack_3_IsAttackBehind = true;//弓箭手技能攻擊3_是否攻擊背後敵人
        #endregion

        #region 法師
        //法師普通攻擊1
        magicianNormalAttack_1_Damage = 9;//法師普通攻擊1_傷害
        magicianNormalAttack_1_RepelDirection = 0;//法師普通攻擊1_擊退方向(0:擊退 1:擊飛)
        magicianNormalAttack_1_Repel = 5;//法師普通攻擊1_擊退距離        
        magicianNormalAttack_1_Effect = "Pain";//法師普通攻擊1_效果(受擊者播放的動畫名稱)
        magicianNormalAttack_1_FlightSpeed = 40;//法師普通攻擊1_飛行速度
        magicianNormalAttack_1_LifeTime = 1f;//法師普通攻擊1_生存時間

        //法師普通攻擊2
        magicianNormalAttack_2_Damge = 7;//法師普通攻擊2_傷害
        magicianNormalAttack_2_RepelDirection = 0;//法師普通攻擊2_擊退方向(0:擊退 1:擊飛)
        magicianNormalAttack_2_RepelDistance = 7;//法師普通攻擊2_擊退/擊飛距離    
        magicianNormalAttack_2_Effect = "Pain";//法師普通攻擊2_效果(受擊者播放的動畫名稱)        
        magicianNormalAttack_2_ForwardDistance = 8.0f;//法師普通攻擊2_攻擊範圍中心點距離物件前方
        magicianNormalAttack_2_attackRange = new Vector3(1, 1f, 16);//法師普通攻擊2_攻擊範圍
        magicianNormalAttack_2_IsAttackBehind = false;//法師普通攻擊2_是否攻擊背後敵人

        //法師普通攻擊3
        magicianNormalAttack_3_Damge = 14;//法師普通攻擊3_傷害
        magicianNormalAttack_3_RepelDirection = 0;//法師普通攻擊3_擊退方向(0:擊退 1:擊飛)
        magicianNormalAttack_3_RepelDistance = 5f;//法師普通攻擊3_擊退/擊飛距離    
        magicianNormalAttack_3_Effect = "Pain";//法師普通攻擊3_效果(受擊者播放的動畫名稱)        
        magicianNormalAttack_3_ForwardDistance = 0;//法師普通攻擊3_攻擊範圍中心點距離物件前方
        magicianNormalAttack_3_attackRadius = 8.0f;//法師普通攻擊3_攻擊半徑
        magicianNormalAttack_3_IsAttackBehind = true;//法師普通攻擊3_是否攻擊背後敵人

        //法師跳躍攻擊
        magicianJumpAttack_Damage = 37;//法師跳躍攻擊_傷害
        magicianJumpAttack_RepelDirection = 0;//法師跳躍攻擊_擊退方向(0:擊退 1:擊飛)  
        magicianJumpAttack_RepelDistance = 40;//法師跳躍攻擊_擊退距離
        magicianJumpAttack_Effect = "Pain";//法師跳躍攻擊_效果(受擊者播放的動畫名稱)
        magicianJumpAttack_ForwardDistance = 0;//法師跳躍攻擊_攻擊範圍中心點距離物件前方
        magicianJumpAttack_attackRadius = 1.3f;//法師跳躍攻擊_攻擊半徑
        magicianJumpAttack_IsAttackBehind = true;//法師跳躍攻擊_是否攻擊背後敵人

        //法師技能攻擊1
        magicianSkillAttack_1_HealValue = 8;//法師普通攻擊1_治療量(%)    
        magicianSkillAttack_1_ForwardDistance = 0;//法師普通攻擊1_治療範圍中心點距離物件前方
        magicianSkillAttack_1_attackRange = 8;//法師普通攻擊1_治療半徑
        magicianSkillAttack_1_IsAttackBehind = true;//法師普通攻擊1_是否治療背後盟友

        /*//法師技能攻擊2
        magicianSkillAttack_2_Damge = new float[] { 4, 4, 5, 4, 5, 4, 4, 7, 9, 12};//法師技能攻擊2_傷害
        magicianSkillAttack_2_RepelDirection = new int[] { 0, 0, 0, 0 ,0, 0, 0, 0, 0, 0};//法師技能攻擊2_擊退方向(0:擊退 1:擊飛)
        magicianSkillAttack_2_RepelDistance = new float[] { 25, 5, 5, 5, 5, 5, 5, 15, 10, 65};//法師技能攻擊2_擊退/擊飛距離    
        magicianSkillAttack_2_Effect = new string[] { "Pain", "Pain", "Pain", "Pain", "Pain", "Pain", "Pain", "Pain", "Pain", "Pain" };//法師技能攻擊2_效果(受擊者播放的動畫名稱)        
        magicianSkillAttack_2_ForwardDistance = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};//法師技能攻擊2_攻擊範圍中心點距離物件前方
        magicianSkillAttack_2_attackRadius = new float[] { 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f};//法師技能攻擊2_攻擊半徑
        magicianSkillAttack_2_IsAttackBehind = new bool[] { false, false, false, false, false, false, false, false, false, false };//法師技能攻擊2_是否攻擊背後敵人*/

        //法師技能攻擊2
        magicianSkillAttack_2_Damge = 32;//法師技能攻擊2_傷害
        magicianSkillAttack_2_RepelDirection = 0;//法師技能攻擊2_擊退方向(0:擊退 1:擊飛)
        magicianSkillAttack_2_RepelDistance = 10;//法師技能攻擊2_擊退/擊飛距離    
        magicianSkillAttack_2_Effect = "Pain";//法師技能攻擊2_效果(受擊者播放的動畫名稱)        
        magicianSkillAttack_2_ForwardDistance = 5;//法師技能攻擊2_攻擊範圍中心點距離物件前方
        magicianSkillAttack_2_attackRadius = 10.0f;//法師技能攻擊2_攻擊半徑
        magicianSkillAttack_2_IsAttackBehind = false;//法師技能攻擊2_是否攻擊背後敵人

        //法師技能攻擊3
        magicianSkillAttack_3_Damge = 35;//法師技能攻擊3_傷害
        magicianSkillAttack_3_RepelDirection = 0;//法師技能攻擊3_擊退方向(0:擊退 1:擊飛)
        magicianSkillAttack_3_RepelDistance = 15;//法師技能攻擊3_擊退/擊飛距離    
        magicianSkillAttack_3_Effect = "Pain";//法師技能攻擊3_效果(受擊者播放的動畫名稱)        
        magicianSkillAttack_3_ForwardDistance = 5;//法師技能攻擊3_攻擊範圍中心點距離物件前方
        magicianSkillAttack_3_attackRadius = 10.0f;//法師技能攻擊3_攻擊半徑
        magicianSkillAttack_3_IsAttackBehind = true;//法師技能攻擊3_是否攻擊背後敵人
        #endregion

        #region 敵人士兵1(石頭人)
        //敵人士兵1 攻擊1
        enemySoldier1_Attack_1_Damge = 13;//敵人士兵1_攻擊1_傷害
        enemySoldier1_Attack_1_RepelDirection = 0;//敵人士兵1_攻擊1_擊退方向(0:擊退 1:擊飛)
        enemySoldier1_Attack_1_RepelDistance = 0;//敵人士兵1_攻擊1_擊退/擊飛距離    
        enemySoldier1_Attack_1_Effect = "Pain";//敵人士兵1_攻擊1_效果(受擊者播放的動畫名稱)        
        enemySoldier1_Attack_1_ForwardDistance = 0;//敵人士兵1_攻擊1_攻擊範圍中心點距離物件前方
        enemySoldier1_Attack_1_attackRadius = 1.3f;//敵人士兵1_攻擊1_攻擊半徑
        enemySoldier1_Attack_1_IsAttackBehind = true;//敵人士兵1_攻擊1_是否攻擊背後敵人

        //敵人士兵1 攻擊2
        enemySoldier1_Attack_2_Damge = 9;//敵人士兵1_攻擊2_傷害
        enemySoldier1_Attack_2_RepelDirection = 0;//敵人士兵1_攻擊2_擊退方向(0:擊退 1:擊飛)
        enemySoldier1_Attack_2_RepelDistance = 0;//敵人士兵1_攻擊2_擊退/擊飛距離    
        enemySoldier1_Attack_2_Effect = "Pain";//敵人士兵1_攻擊2_效果(受擊者播放的動畫名稱)        
        enemySoldier1_Attack_2_ForwardDistance = 1.4f;//敵人士兵1_攻擊2_攻擊範圍中心點距離物件前方
        enemySoldier1_Attack_2_attackRadius = 1.3f;//敵人士兵1_攻擊2_攻擊半徑
        enemySoldier1_Attack_2_IsAttackBehind = false;//敵人士兵1_攻擊2_是否攻擊背後敵人

        //敵人士兵1 攻擊3
        enemySoldier1_Attack_3_Damge = 6;//敵人士兵1_攻擊3_傷害
        enemySoldier1_Attack_3_RepelDirection = 0;//敵人士兵1_攻擊3_擊退方向(0:擊退 1:擊飛)
        enemySoldier1_Attack_3_RepelDistance = 0;//敵人士兵1_攻擊3_擊退/擊飛距離    
        enemySoldier1_Attack_3_Effect = "Pain";//敵人士兵1_攻擊3_效果(受擊者播放的動畫名稱)        
        enemySoldier1_Attack_3_ForwardDistance = 1.4f;//敵人士兵1_攻擊3_攻擊範圍中心點距離物件前方
        enemySoldier1_Attack_3_attackRadius = 1.3f;//敵人士兵1_攻擊3_攻擊半徑
        enemySoldier1_Attack_3_IsAttackBehind = false;//敵人士兵1_攻擊3_是否攻擊背後敵人
        #endregion

        #region 敵人士兵2(弓箭手)
        //敵人士兵2 攻擊1
        enemySoldier2_Attack1_Damge = 5;//敵人士兵2_攻擊1_傷害
        enemySoldier2_Attack1_RepelDirection = 0;//敵人士兵2_攻擊1_擊退方向(0:擊退 1:擊飛)
        enemySoldier2_Attack1_RepelDistance = 0;//敵人士兵2_攻擊1_擊退/擊飛距離    
        enemySoldier2_Attack1_Effect = "Pain";//敵人士兵2_攻擊1_效果(受擊者播放的動畫名稱)
        enemySoldier2_Attack1_FloatSpeed = 30;//敵人士兵2_攻擊1_飛行速度
        enemySoldier2_Attack1_LifeTime = 0.45f;//敵人士兵2_攻擊1_生存時間

        //敵人士兵2 攻擊2
        enemySoldier2_Attack2_Damge = 5;//敵人士兵2_攻擊2_傷害
        enemySoldier2_Attack2_RepelDirection = 0;//敵人士兵2_攻擊2_擊退方向(0:擊退 1:擊飛)
        enemySoldier2_Attack2_RepelDistance = 0;//敵人士兵2_攻擊2_擊退/擊飛距離    
        enemySoldier2_Attack2_Effect = "Pain";//敵人士兵2_攻擊2_效果(受擊者播放的動畫名稱)
        enemySoldier2_Attack2_FloatSpeed = 30;//敵人士兵2_攻擊2_飛行速度
        enemySoldier2_Attack2_LifeTime = 0.45f;//敵人士兵2_攻擊2_生存時間

        //敵人士兵2 攻擊3
        enemySoldier2_Attack_3_Damge = 9;//敵人士兵2_攻擊3_傷害
        enemySoldier2_Attack_3_RepelDirection = 0;//敵人士兵2_攻擊3_擊退方向(0:擊退 1:擊飛)
        enemySoldier2_Attack_3_RepelDistance = 0;//敵人士兵2_攻擊3_擊退/擊飛距離    
        enemySoldier2_Attack_3_Effect = "Pain";//敵人士兵2_攻擊3_效果(受擊者播放的動畫名稱)        
        enemySoldier2_Attack_3_ForwardDistance = 1.4f;//敵人士兵2_攻擊3_攻擊範圍中心點距離物件前方
        enemySoldier2_Attack_3_attackRadius = 1.3f;//敵人士兵2_攻擊3_攻擊半徑
        enemySoldier2_Attack_3_IsAttackBehind = false;//敵人士兵2_攻擊3_是否攻擊背後敵人
        #endregion

        #region 敵人士兵3(斧頭人)
        //敵人士兵3 攻擊1
        enemySoldier3_Attack_1_Damge = 15;//敵人士兵3_攻擊1_傷害
        enemySoldier3_Attack_1_RepelDirection = 0;//敵人士兵3_攻擊1_擊退方向(0:擊退 1:擊飛)
        enemySoldier3_Attack_1_RepelDistance = 0;//敵人士兵3_攻擊1_擊退/擊飛距離    
        enemySoldier3_Attack_1_Effect = "Pain";//敵人士兵3_攻擊1_效果(受擊者播放的動畫名稱)        
        enemySoldier3_Attack_1_ForwardDistance = 0;//敵人士兵3_攻擊1_攻擊範圍中心點距離物件前方
        enemySoldier3_Attack_1_attackRadius = 1.5f;//敵人士兵3_攻擊1_攻擊半徑
        enemySoldier3_Attack_1_IsAttackBehind = true;//敵人士兵3_攻擊1_是否攻擊背後敵人

        //敵人士兵3 攻擊2
        enemySoldier3_Attack_2_Damge = 11;//敵人士兵3_攻擊2_傷害
        enemySoldier3_Attack_2_RepelDirection = 0;//敵人士兵3_攻擊2_擊退方向(0:擊退 1:擊飛)
        enemySoldier3_Attack_2_RepelDistance = 0;//敵人士兵3_攻擊2_擊退/擊飛距離    
        enemySoldier3_Attack_2_Effect = "Pain";//敵人士兵3_攻擊2_效果(受擊者播放的動畫名稱)        
        enemySoldier3_Attack_2_ForwardDistance = 1.4f;//敵人士兵3_攻擊2_攻擊範圍中心點距離物件前方
        enemySoldier3_Attack_2_attackRadius = 1.3f;//敵人士兵3_攻擊2_攻擊半徑
        enemySoldier3_Attack_2_IsAttackBehind = true;//敵人士兵3_攻擊2_是否攻擊背後敵人

        //敵人士兵3 攻擊3
        enemySoldier3_Attack_3_Damge = 10;//敵人士兵3_攻擊3_傷害
        enemySoldier3_Attack_3_RepelDirection = 0;//敵人士兵3_攻擊3_擊退方向(0:擊退 1:擊飛)
        enemySoldier3_Attack_3_RepelDistance = 0;//敵人士兵3_攻擊3_擊退/擊飛距離    
        enemySoldier3_Attack_3_Effect = "Pain";//敵人士兵3_攻擊3_效果(受擊者播放的動畫名稱)        
        enemySoldier3_Attack_3_ForwardDistance = 1.4f;//敵人士兵3_攻擊3_攻擊範圍中心點距離物件前方
        enemySoldier3_Attack_3_attackRadius = 1.3f;//敵人士兵3_攻擊3_攻擊半徑
        enemySoldier3_Attack_3_IsAttackBehind = false;//敵人士兵3_攻擊3_是否攻擊背後敵人
        #endregion

        #region 守衛Boss
        //守衛Boss 攻擊1
        guardBoss_Attack_1_Damge = 17;//守衛Boss_攻擊1_傷害
        guardBoss_Attack_1_RepelDirection = 0;//守衛Boss_攻擊1_擊退方向(0:擊退 1:擊飛)
        guardBoss_Attack_1_RepelDistance = 0;//守衛Boss_攻擊1_擊退/擊飛距離    
        guardBoss_Attack_1_Effect = "Pain";//守衛Boss_攻擊1_效果(受擊者播放的動畫名稱)        
        guardBoss_Attack_1_ForwardDistance = 1.4f;//守衛Boss_攻擊1_攻擊範圍中心點距離物件前方
        guardBoss_Attack_1_attackRadius = 1.3f;//守衛Boss_攻擊1_攻擊半徑
        guardBoss_Attack_1_IsAttackBehind = false;//守衛Boss_攻擊1_是否攻擊背後敵人

        //守衛Boss 攻擊2
        guardBoss_Attack_2_Damge = 17;//守衛Boss_攻擊2_傷害
        guardBoss_Attack_2_RepelDirection = 0;//守衛Boss_攻擊2_擊退方向(0:擊退 1:擊飛)
        guardBoss_Attack_2_RepelDistance = 0;//守衛Boss_攻擊2_擊退/擊飛距離    
        guardBoss_Attack_2_Effect = "Pain";//守衛Boss_攻擊2_效果(受擊者播放的動畫名稱)        
        guardBoss_Attack_2_ForwardDistance = 1.4f;//守衛Boss_攻擊2_攻擊範圍中心點距離物件前方
        guardBoss_Attack_2_attackRadius = 1.3f;//守衛Boss_攻擊2_攻擊半徑
        guardBoss_Attack_2_IsAttackBehind = false;//守衛Boss_攻擊2_是否攻擊背後敵人

        //守衛Boss 攻擊3
        guardBoss_Attack_3_Damge = 35;//守衛Boss_攻擊3_傷害
        guardBoss_Attack_3_RepelDirection = 0;//守衛Boss_攻擊3_擊退方向(0:擊退 1:擊飛)
        guardBoss_Attack_3_RepelDistance = 0;//守衛Boss_攻擊3_擊退/擊飛距離    
        guardBoss_Attack_3_Effect = "Pain";//守衛Boss_攻擊3_效果(受擊者播放的動畫名稱)        
        guardBoss_Attack_3_ForwardDistance = 1.4f;//守衛Boss_攻擊3_攻擊範圍中心點距離物件前方
        guardBoss_Attack_3_attackRadius = 1.3f;//守衛Boss_攻擊3_攻擊半徑
        guardBoss_Attack_3_IsAttackBehind = false;//守衛Boss_攻擊3_是否攻擊背後敵人

        //守衛Boss 攻擊4
        guardBoss_Attack_4_Damge = 33;//守衛Boss_攻擊4_傷害
        guardBoss_Attack_4_RepelDirection = 0;//守衛Boss_攻擊4_擊退方向(0:擊退 1:擊飛)
        guardBoss_Attack_4_RepelDistance = 0;//守衛Boss_攻擊4_擊退/擊飛距離    
        guardBoss_Attack_4_Effect = "Pain";//守衛Boss_攻擊4_效果(受擊者播放的動畫名稱)        
        guardBoss_Attack_4_ForwardDistance = 1.4f;//守衛Boss1_攻擊4_攻擊範圍中心點距離物件前方
        guardBoss_Attack_4_attackRadius = 1.3f;//守衛Boss_攻擊4_攻擊半徑
        guardBoss_Attack_4_IsAttackBehind = false;//守衛Boss_攻擊4_是否攻擊背後敵人
        #endregion

        #region Boss
        //Boss 攻擊1
        bossAttack1_Damge = 55;//Boss攻擊1_傷害
        bossAttack1_RepelDirection = 0;//Boss攻擊1_擊退方向(0:擊退 1:擊飛)
        bossAttack1_RepelDistance = 0;//Boss攻擊1_擊退/擊飛距離    
        bossAttack1_Effect = "Pain";//Boss攻擊1_效果(受擊者播放的動畫名稱)
        bossAttack1_FloatSpeed = 30;//Boss攻擊1_攻擊飛行速度
        bossAttack1_LifeTime = 1.5f;//Boss攻擊1_生存時間

        //Boss 攻擊2
        bossAttack2_Damge = 9;//Boss攻擊2_傷害
        bossAttack2_RepelDirection = 0;//Boss攻擊2_擊退方向(0:擊退 1:擊飛)
        bossAttack2_RepelDistance = 0;//Boss攻擊2_擊退/擊飛距離    
        bossAttack2_Effect = "Pain";//Boss攻擊2_效果(受擊者播放的動畫名稱)
        bossAttack2_FloatSpeed = 35;//Boss攻擊2_攻擊飛行速度
        bossAttack2_LifeTime = 0.85f;//Boss攻擊2_生存時間

        //Boss 攻擊3
        bossAttack3_Damge = 48;//Boss攻擊3_傷害
        bossAttack3_RepelDirection = 0;//Boss攻擊3_擊退方向(0:擊退 1:擊飛)
        bossAttack3_RepelDistance = 0;//Boss攻擊3_擊退/擊飛距離    
        bossAttack3_Effect = "Pain";//Boss攻擊3_效果(受擊者播放的動畫名稱)        
        bossAttack3_ForwardDistance = 0;//Boss攻擊3_攻擊範圍中心點距離物件前方
        bossAttack3_attackRadius = 10;//Boss攻擊3_攻擊半徑
        bossAttack3_IsAttackBehind = true;//Boss攻擊3_是否攻擊背後敵人
        #endregion
}
}

/// <summary>
/// 遊戲數值中心
/// </summary>
[CreateAssetMenu(fileName = "NumericalValue", menuName = "ScriptableObjects/NumericalValue", order = 1)]
public class ScriptableObject_NumericalValue : ScriptableObject
{
    public GameData_NumericalValue numericalValue;
}
