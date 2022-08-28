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

    [Header("法師 技能攻擊2")]
    public float[] magicianSkillAttack_2_Damge;//法師技能攻擊2_傷害
    public int[] magicianSkillAttack_2_RepelDirection;//法師技能攻擊2_擊退方向(0:擊退 1:擊飛)
    public float[] magicianSkillAttack_2_RepelDistance;//法師技能攻擊2_擊退/擊飛距離    
    public string[] magicianSkillAttack_2_Effect;//法師技能攻擊2_效果(受擊者播放的動畫名稱)        
    public float[] magicianSkillAttack_2_ForwardDistance;//法師技能攻擊2_攻擊範圍中心點距離物件前方
    public float[] magicianSkillAttack_2_attackRadius;//法師技能攻擊2_攻擊半徑
    public bool[] magicianSkillAttack_2_IsAttackBehind;//法師技能攻擊2_是否攻擊背後敵人

    [Header("法師 技能攻擊3")]
    public float magicianSkillAttack_3_Damge;//法師技能攻擊3_傷害
    public int magicianSkillAttack_3_RepelDirection;//法師技能攻擊3_擊退方向(0:擊退 1:擊飛)
    public float magicianSkillAttack_3_RepelDistance;//法師技能攻擊3_擊退/擊飛距離    
    public string magicianSkillAttack_32_Effect;//法師技能攻擊3_效果(受擊者播放的動畫名稱)        
    public float magicianSkillAttack_3_ForwardDistance;//法師技能攻擊3_攻擊範圍中心點距離物件前方
    public float magicianSkillAttack_3_attackRadius;//法師技能攻擊3_攻擊半徑
    public bool magicianSkillAttack_3_IsAttackBehind;//法師技能攻擊3_是否攻擊背後敵人
    #endregion

    #region 敵人士兵1
    [Header("敵人士兵 攻擊1")]
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

    [Header("敵人士兵1")]
    public float enemySoldier1_Hp;//敵人士兵1_生命值
    [Header("城門守衛Boss")]
    public float guardBoss_Hp;//城門守衛Boss_生命值

    /// <summary>
    /// 建構子
    /// </summary>
    private GameData_NumericalValue()
    {
        //共通
        gravity = 6.8f;//重力
        criticalBonus = 1.3f;//報擊傷害加成
        levelNames = new string[] { "橫掃千軍", "人中之龍"};//關卡名稱

        //Buff增加數值
        buffAbleString = new string[] { "生命", "傷害", "防禦", "吸血", "移動", "受療" };//Buff增益文字
        buffAbleValue = new float[] { 30, 25, 25, 20, 25, 25};//Buff增益數值(%)

        //攝影機
        distance = 2.6f;//與玩家距離        
        limitUpAngle = 35;//限制向上角度
        limitDownAngle = 13;//限制向下角度
        //cameraAngle = 20;//攝影機角度

        //玩家
        playerHp = 500;//玩家生命值
        playerMoveSpeed = 6.3f;//玩家移動速度        
        playerJumpForce = 11.05f;//玩家跳躍力
        playerCriticalRate = 15;//玩家暴擊率
        playerDodgeSeppd = 6.3f;//玩家閃躲速度
        playerSelfHealTime = 5;//玩家自身回復時間(秒)

        #region 戰士
        //戰士 普通攻擊1
        warriorNormalAttack_1_Damge = 30;//戰士普通攻擊1_傷害
        warriorNormalAttack_1_RepelDirection = 0;//戰士普通攻擊1_擊退方向(0:擊退 1:擊飛)
        warriorNormalAttack_1_RepelDistance = 37;//戰士普通攻擊1_擊退/擊飛距離    
        warriorNormalAttack_1_Effect = "Pain";//戰士普通攻擊1_效果(受擊者播放的動畫名稱)            
        warriorNormalAttack_1_ForwardDistance = 1.3f;//戰士普通攻擊1_攻擊範圍中心點距離物件前方
        warriorNormalAttack_1_attackRadius = 1.2f;//戰士普通攻擊1_攻擊半徑    
        warriorNormalAttack_1_IsAttackBehind = false;//戰士普通攻擊1_是否攻擊背後敵人

        //戰士 普通攻擊2
        warriorNormalAttack_2_Damge = 33;//戰士普通攻擊1_傷害
        warriorNormalAttack_2_RepelDirection = 0;//戰士普通攻擊1_擊退方向(0:擊退 1:擊飛)
        warriorNormalAttack_2_RepelDistance = 37;//戰士普通攻擊1_擊退/擊飛距離    
        warriorNormalAttack_2_Effect = "Pain";//戰士普通攻擊1_效果(受擊者播放的動畫名稱)            
        warriorNormalAttack_2_ForwardDistance = 1.3f;//戰士普通攻擊1_攻擊範圍中心點距離物件前方
        warriorNormalAttack_2_attackRadius = 1.2f;//戰士普通攻擊1_攻擊半徑    
        warriorNormalAttack_2_IsAttackBehind = false;//戰士普通攻擊1_是否攻擊背後敵人

        //戰士 普通攻擊3
        warriorNormalAttack_3_Damge = 36;//戰士普通攻擊1_傷害
        warriorNormalAttack_3_RepelDirection = 0;//戰士普通攻擊1_擊退方向(0:擊退 1:擊飛)
        warriorNormalAttack_3_RepelDistance = 45;//戰士普通攻擊1_擊退/擊飛距離    
        warriorNormalAttack_3_Effect = "Pain";//戰士普通攻擊1_效果(受擊者播放的動畫名稱)            
        warriorNormalAttack_3_ForwardDistance = 0.5f;//戰士普通攻擊1_攻擊範圍中心點距離物件前方
        warriorNormalAttack_3_attackRadius = 1.55f;//戰士普通攻擊1_攻擊半徑    
        warriorNormalAttack_3_IsAttackBehind = false;//戰士普通攻擊1_是否攻擊背後敵人

        //戰士 跳躍攻擊
        warriorJumpAttack_Damage = 37;//戰士跳躍攻擊_傷害
        warriorJumpAttack_RepelDirection = 0;//戰士跳躍攻擊_擊退方向(0:擊退 1:擊飛)
        warriorJumpAttac_kRepelDistance = 50;//戰士跳躍攻擊_擊退距離
        warriorJumpAttack_Effect = "Pain";//戰士跳躍攻擊效果(受擊者播放的動畫名稱)
        warriorJumpAttack_ForwardDistance = 0.77f;//戰士普通攻擊3_攻擊範圍中心點距離物件前方
        warriorJumpAttack_attackRadius = 1.3f;//戰士普通攻擊3_攻擊半徑
        warriorJumpAttack_IsAttackBehind = false;//戰士普通攻擊3_是否攻擊背後敵人

        //戰士 技能攻擊1
        warriorSkillAttack_1_Damge = 50;//戰士技能攻擊2_傷害
        warriorSkillAttack_1_RepelDirection = 0;//戰士技能攻擊2_擊退方向(0:擊退 1:擊飛)
        warriorSkillAttack_1_RepelDistance = 80;//戰士技能攻擊2_擊退/擊飛距離    
        warriorSkillAttack_1_Effect = "Pain";//戰士普技能攻擊2_效果(受擊者播放的動畫名稱)        
        warriorSkillAttack_1_ForwardDistance = 1.6f;//戰士技能攻擊2_攻擊範圍中心點距離物件前方
        warriorSkillAttack_1_attackRadius = 1.65f;//戰士技能攻擊2_攻擊半徑
        warriorSkillAttack_1_IsAttackBehind = false;//戰士技能攻擊2_是否攻擊背後敵人

        //戰士 技能攻擊2
        warriorSkillAttack_2_Damge = 28;//戰士技能攻擊2_傷害
        warriorSkillAttack_2_RepelDirection = 0;//戰士技能攻擊2_擊退方向(0:擊退 1:擊飛)
        warriorSkillAttack_2_RepelDistance = 45;//戰士技能攻擊2_擊退/擊飛距離    
        warriorSkillAttack_2_Effect = "Pain";//戰士普技能攻擊2_效果(受擊者播放的動畫名稱)        
        warriorSkillAttack_2_ForwardDistance = 1.3f;//戰士技能攻擊2_攻擊範圍中心點距離物件前方
        warriorSkillAttack_2_attackRadius = 1.2f;//戰士技能攻擊2_攻擊半徑
        warriorSkillAttack_2_IsAttackBehind = false;//戰士技能攻擊2_是否攻擊背後敵人

        //戰士 技能攻擊3
        warriorSkillAttack_3_Damge = new float[] { 16, 16, 35};//戰士技能攻擊3_傷害
        warriorSkillAttack_3_RepelDirection = new int[] { 0, 0, 0};//戰士技能攻擊3_擊退方向(0:擊退 1:擊飛)
        warriorSkillAttack_3_RepelDistance = new float[] { 50, 50, 90};//戰士技能攻擊3_擊退/擊飛距離    
        warriorSkillAttack_3_Effect = new string[] { "Pain", "Pain", "Pain" };//戰士普技能攻擊3_效果(受擊者播放的動畫名稱)        
        warriorSkillAttack_3_ForwardDistance = new float[] { 0.7f, 0.7f, 0};//戰士技能攻擊3_攻擊範圍中心點距離物件前方
        warriorSkillAttack_3_attackRadius = new float[] { 1.4f, 1.4f, 4.5f};//戰士技能攻擊3_攻擊半徑
        warriorSkillAttack_3_IsAttackBehind = new bool[] { false, false, true};//戰士技能攻擊3_是否攻擊背後敵人
        #endregion

        #region 弓箭手
        //弓箭手 普通攻擊
        archerNormalAttack_Damge = new float[] { 11, 11, 16 };//弓箭手普通攻擊_傷害
        archerNormalAttack_RepelDirection = new int[] { 0, 0, 0 };//弓箭手普通攻擊_擊退方向(0:擊退 1:擊飛)
        archerNormalAttack_RepelDistance = new float[] { 25, 25, 25 };//弓箭手普通攻擊_擊退/擊飛距離        
        archerNormalAttack_Effect = new string[] { "Pain", "Pain", "Pain" };//弓箭手普通攻擊_效果(受擊者播放的動畫名稱)     
        archerNormalAttack_FloatSpeed = new float[] { 30, 30, 30};//弓箭手普通攻擊飛行速度
        archerNormalAttack_LifeTime = new float[] { 0.6f, 0.6f, 0.6f};//弓箭手普通攻擊生存時間

        //弓箭手 跳躍攻擊
        archerJumpAttack_Damage = 11;//弓箭手跳躍攻擊傷害
        archerJumpAttack_RepelDirection = 0;//弓箭手跳躍攻擊擊退方向(0:擊退 1:擊飛)
        archerJumpAttack_RepelDistance = 25;//弓箭手跳躍攻擊 擊退距離
        archerJumpAttack_Effect = "Pain";//弓箭手跳躍攻擊效果(受擊者播放的動畫名稱)
        archerJumpAttack_ForwardDistance = 0;//弓箭手跳躍攻擊_攻擊範圍中心點距離物件前方
        archerJumpAttack_attackRadius = 1.3f;//弓箭手跳躍攻擊_攻擊半徑
        archerJumpAttack_IsAttackBehind = true;//弓箭手跳躍攻擊_是否攻擊背後敵人

        //弓箭手 技能攻擊1
        archerSkillAttack_1_Damage = 11;//弓箭手技能攻擊1_傷害
        archerSkillAttack_1_RepelDirection = 5;//弓箭手技能攻擊1_擊退方向(0:擊退 1:擊飛)
        archerSkillAttack_1_Repel = 13;//弓箭手技能攻擊1_擊退距離        
        archerSkillAttack_1_Effect = "Pain";//弓箭手技能攻擊1_效果(受擊者播放的動畫名稱)
        archerSkillAttack_1_FlightSpeed = 30;//弓箭手技能攻擊1_飛行速度
        archerSkillAttack_1_LifeTime = 0.4f;//弓箭手技能攻擊1_生存時間

        //弓箭手 技能攻擊2
        archerSkillAttack_2_Damge = 11;//弓箭手技能攻擊2_傷害
        archerSkillAttack_2_RepelDirection = 0;//弓箭手技能攻擊2_擊退方向(0:擊退 1:擊飛)
        archerSkillAttack_2_RepelDistance = 30;//弓箭手技能攻擊2_擊退/擊飛距離    
        archerSkillAttack_2_Effect = "Pain";//弓箭手技能攻擊2_效果(受擊者播放的動畫名稱)        
        archerSkillAttack_2_ForwardDistance = 1.3f;//弓箭手技能攻擊2_攻擊範圍中心點距離物件前方
        archerSkillAttack_2_attackRadius = 1.2f;//弓箭手技能攻擊2_攻擊半徑
        archerSkillAttack_2_IsAttackBehind = false;//弓箭手技能攻擊2_是否攻擊背後敵人

        //弓箭手 技能攻擊3
        archerSkillAttack_3_Damge = 5;//弓箭手技能攻擊3_傷害
        archerSkillAttack_3_RepelDirection = 1;//弓箭手技能攻擊3_擊退方向(0:擊退 1:擊飛)
        archerSkillAttack_3_RepelDistance = 0;//弓箭手技能攻擊3_擊退/擊飛距離    
        archerSkillAttack_3_Effect = "Pain";//弓箭手技能攻擊3_效果(受擊者播放的動畫名稱)        
        archerSkillAttack_3_ForwardDistance = 0;//弓箭手技能攻擊3_攻擊範圍中心點距離物件前方
        archerSkillAttack_3_attackRadius = 5.0f;//弓箭手技能攻擊3_攻擊半徑
        archerSkillAttack_3_IsAttackBehind = true;//弓箭手技能攻擊3_是否攻擊背後敵人
        #endregion

        #region 法師
        //法師普通攻擊1
        magicianNormalAttack_1_Damage = 7;//法師普通攻擊1_傷害
        magicianNormalAttack_1_RepelDirection = 0;//法師普通攻擊1_擊退方向(0:擊退 1:擊飛)
        magicianNormalAttack_1_Repel = 5;//法師普通攻擊1_擊退距離        
        magicianNormalAttack_1_Effect = "Pain";//法師普通攻擊1_效果(受擊者播放的動畫名稱)
        magicianNormalAttack_1_FlightSpeed = 40;//法師普通攻擊1_飛行速度
        magicianNormalAttack_1_LifeTime = 1f;//法師普通攻擊1_生存時間

        //法師普通攻擊2
        magicianNormalAttack_2_Damge = 5;//法師普通攻擊2_傷害
        magicianNormalAttack_2_RepelDirection = 0;//法師普通攻擊2_擊退方向(0:擊退 1:擊飛)
        magicianNormalAttack_2_RepelDistance = 10;//法師普通攻擊2_擊退/擊飛距離    
        magicianNormalAttack_2_Effect = "Pain";//法師普通攻擊2_效果(受擊者播放的動畫名稱)        
        magicianNormalAttack_2_ForwardDistance = 1.5f;//法師普通攻擊2_攻擊範圍中心點距離物件前方
        magicianNormalAttack_2_attackRange = new Vector3(1, 1f, 3.3f);//法師普通攻擊2_攻擊範圍
        magicianNormalAttack_2_IsAttackBehind = false;//法師普通攻擊2_是否攻擊背後敵人

        //法師普通攻擊3
        magicianNormalAttack_3_Damge = 33;//法師普通攻擊3_傷害
        magicianNormalAttack_3_RepelDirection = 0;//法師普通攻擊3_擊退方向(0:擊退 1:擊飛)
        magicianNormalAttack_3_RepelDistance = 25;//法師普通攻擊3_擊退/擊飛距離    
        magicianNormalAttack_3_Effect = "Pain";//法師普通攻擊3_效果(受擊者播放的動畫名稱)        
        magicianNormalAttack_3_ForwardDistance = 2.7f;//法師普通攻擊3_攻擊範圍中心點距離物件前方
        magicianNormalAttack_3_attackRadius = 2.3f;//法師普通攻擊3_攻擊半徑
        magicianNormalAttack_3_IsAttackBehind = false;//法師普通攻擊3_是否攻擊背後敵人

        //法師跳躍攻擊
        magicianJumpAttack_Damage = 28;//法師跳躍攻擊_傷害
        magicianJumpAttack_RepelDirection = 0;//法師跳躍攻擊_擊退方向(0:擊退 1:擊飛)  
        magicianJumpAttack_RepelDistance = 40;//法師跳躍攻擊_擊退距離
        magicianJumpAttack_Effect = "Pain";//法師跳躍攻擊_效果(受擊者播放的動畫名稱)
        magicianJumpAttack_ForwardDistance = 0;//法師跳躍攻擊_攻擊範圍中心點距離物件前方
        magicianJumpAttack_attackRadius = 1.3f;//法師跳躍攻擊_攻擊半徑
        magicianJumpAttack_IsAttackBehind = true;//法師跳躍攻擊_是否攻擊背後敵人

        //法師技能攻擊1
        magicianSkillAttack_1_HealValue = 10;//法師普通攻擊1_治療量(%)    
        magicianSkillAttack_1_ForwardDistance = 0;//法師普通攻擊1_治療範圍中心點距離物件前方
        magicianSkillAttack_1_attackRange = 5;//法師普通攻擊1_治療半徑
        magicianSkillAttack_1_IsAttackBehind = true;//法師普通攻擊1_是否治療背後盟友

        //法師技能攻擊2
        magicianSkillAttack_2_Damge = new float[] { 4, 4, 5, 4, 5, 4, 4, 7, 9, 12};//法師技能攻擊2_傷害
        magicianSkillAttack_2_RepelDirection = new int[] { 0, 0, 0, 0 ,0, 0, 0, 0, 0, 0};//法師技能攻擊2_擊退方向(0:擊退 1:擊飛)
        magicianSkillAttack_2_RepelDistance = new float[] { 25, 5, 5, 5, 5, 5, 5, 15, 10, 65};//法師技能攻擊2_擊退/擊飛距離    
        magicianSkillAttack_2_Effect = new string[] { "Pain", "Pain", "Pain", "Pain", "Pain", "Pain", "Pain", "Pain", "Pain", "Pain" };//法師技能攻擊2_效果(受擊者播放的動畫名稱)        
        magicianSkillAttack_2_ForwardDistance = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};//法師技能攻擊2_攻擊範圍中心點距離物件前方
        magicianSkillAttack_2_attackRadius = new float[] { 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f};//法師技能攻擊2_攻擊半徑
        magicianSkillAttack_2_IsAttackBehind = new bool[] { false, false, false, false, false, false, false, false, false, false };//法師技能攻擊2_是否攻擊背後敵人

        //法師技能攻擊3
        magicianSkillAttack_3_Damge = 52;//法師技能攻擊3_傷害
        magicianSkillAttack_3_RepelDirection = 0;//法師技能攻擊3_擊退方向(0:擊退 1:擊飛)
        magicianSkillAttack_3_RepelDistance = 45;//法師技能攻擊3_擊退/擊飛距離    
        magicianSkillAttack_32_Effect = "Pain";//法師技能攻擊3_效果(受擊者播放的動畫名稱)        
        magicianSkillAttack_3_ForwardDistance = 3;//法師技能攻擊3_攻擊範圍中心點距離物件前方
        magicianSkillAttack_3_attackRadius = 1.8f;//法師技能攻擊3_攻擊半徑
        magicianSkillAttack_3_IsAttackBehind = false;//法師技能攻擊3_是否攻擊背後敵人
        #endregion

        #region 敵人士兵1
        //敵人士兵1 攻擊1
        enemySoldier1_Attack_1_Damge = 9;//敵人士兵1_攻擊1_傷害
        enemySoldier1_Attack_1_RepelDirection = 0;//敵人士兵1_攻擊1_擊退方向(0:擊退 1:擊飛)
        enemySoldier1_Attack_1_RepelDistance = 30;//敵人士兵1_攻擊1_擊退/擊飛距離    
        enemySoldier1_Attack_1_Effect = "Pain";//敵人士兵1_攻擊1_效果(受擊者播放的動畫名稱)        
        enemySoldier1_Attack_1_ForwardDistance = 1.4f;//敵人士兵1_攻擊1_攻擊範圍中心點距離物件前方
        enemySoldier1_Attack_1_attackRadius = 1.3f;//敵人士兵1_攻擊1_攻擊半徑
        enemySoldier1_Attack_1_IsAttackBehind = false;//敵人士兵1_攻擊1_是否攻擊背後敵人

        //敵人士兵1 攻擊2
        enemySoldier1_Attack_2_Damge = 15;//敵人士兵1_攻擊2_傷害
        enemySoldier1_Attack_2_RepelDirection = 0;//敵人士兵1_攻擊2_擊退方向(0:擊退 1:擊飛)
        enemySoldier1_Attack_2_RepelDistance = 45;//敵人士兵1_攻擊2_擊退/擊飛距離    
        enemySoldier1_Attack_2_Effect = "Pain";//敵人士兵1_攻擊2_效果(受擊者播放的動畫名稱)        
        enemySoldier1_Attack_2_ForwardDistance = 0;//敵人士兵1_攻擊2_攻擊範圍中心點距離物件前方
        enemySoldier1_Attack_2_attackRadius = 1.7f;//敵人士兵1_攻擊2_攻擊半徑
        enemySoldier1_Attack_2_IsAttackBehind = true;//敵人士兵1_攻擊2_是否攻擊背後敵人

        //敵人士兵1 攻擊3
        enemySoldier1_Attack_3_Damge = 6;//敵人士兵1_攻擊3_傷害
        enemySoldier1_Attack_3_RepelDirection = 0;//敵人士兵1_攻擊3_擊退方向(0:擊退 1:擊飛)
        enemySoldier1_Attack_3_RepelDistance = 20;//敵人士兵1_攻擊3_擊退/擊飛距離    
        enemySoldier1_Attack_3_Effect = "Pain";//敵人士兵1_攻擊3_效果(受擊者播放的動畫名稱)        
        enemySoldier1_Attack_3_ForwardDistance = 1.4f;//敵人士兵1_攻擊3_攻擊範圍中心點距離物件前方
        enemySoldier1_Attack_3_attackRadius = 1.3f;//敵人士兵1_攻擊3_攻擊半徑
        enemySoldier1_Attack_3_IsAttackBehind = false;//敵人士兵1_攻擊3_是否攻擊背後敵人
        #endregion

        //敵人
        enemySoldier1_Hp = 130;//敵人士兵1_生命值
        guardBoss_Hp = 500;//城門守衛Boss_生命值
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
