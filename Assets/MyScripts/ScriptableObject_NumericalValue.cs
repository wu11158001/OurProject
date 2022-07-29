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

    [Header("玩家")]
    public float playerHp;//玩家生命值
    public float playerMoveSpeed;//玩家移動速度
    public float playerJumpForce;//玩家跳躍力
    public float playerCriticalRate;//玩家暴擊率
    public float playerDodgeSeppd;//玩家閃躲速度

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
    public float warriorSkillAttack_1_Damge;//戰士技能1_傷害
    public int warriorSkillAttack_1_RepelDirection;//戰士技能1_擊退方向(0:擊退 1:擊飛)
    public float warriorSkillAttack_1_RepelDistance;//戰士技能1_擊退/擊飛距離    
    public string warriorSkillAttack_1_Effect;//戰士技能1_效果(受擊者播放的動畫名稱)
    public float warriorSkillAttack_1_FlightSpeed;//戰士技能1_飛行速度
    public float warriorSkillAttack_1_LifeTime;//戰士技能1_生存時間

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
    public string archerSkillAttack_1_Effect;//弓箭手技能攻擊攻擊1_效果(受擊者播放的動畫名稱)
    public float archerSkillAttack_1_FlightSpeed;//弓箭手技能攻擊攻擊1_飛行速度
    public float archerSkillAttack_1_LifeTime;//弓箭手技能攻擊攻擊1_生存時間

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

    [Header("敵人")]
    public float enemyHp;//骷顱士兵生命值

    /// <summary>
    /// 建構子
    /// </summary>
    private GameData_NumericalValue()
    {
        //共通
        gravity = 9.8f;//重力
        criticalBonus = 1.3f;//報擊傷害加成
        levelNames = new string[] { "Level[1]:我是第一關", "Level[2]:第二關還沒做", "Level[3]:第三關在哪"};//關卡名稱

        //Buff增加數值
        buffAbleString = new string[] { "生命值", "傷害", "防禦", "移動", "吸血", "回血" };//Buff增益文字
        buffAbleValue = new float[] { 100, 10, 5, 5, 3, 1};//Buff增益數值

        //攝影機
        distance = 2.6f;//與玩家距離        
        limitUpAngle = 35;//限制向上角度
        limitDownAngle = 20;//限制向下角度

        //玩家
        playerHp = 300;//玩家生命值
        playerMoveSpeed = 5;//玩家移動速度        
        playerJumpForce = 14.5f;//玩家跳躍力
        playerCriticalRate = 15;//玩家暴擊率
        playerDodgeSeppd = 3;//玩家閃躲速度

        #region 戰士
        //戰士 普通攻擊1
        warriorNormalAttack_1_Damge = 10;//戰士普通攻擊1_傷害
        warriorNormalAttack_1_RepelDirection = 0;//戰士普通攻擊1_擊退方向(0:擊退 1:擊飛)
        warriorNormalAttack_1_RepelDistance = 50;//戰士普通攻擊1_擊退/擊飛距離    
        warriorNormalAttack_1_Effect = "Pain";//戰士普通攻擊1_效果(受擊者播放的動畫名稱)            
        warriorNormalAttack_1_ForwardDistance = 1.3f;//戰士普通攻擊1_攻擊範圍中心點距離物件前方
        warriorNormalAttack_1_attackRadius = 1.2f;//戰士普通攻擊1_攻擊半徑    
        warriorNormalAttack_1_IsAttackBehind = false;//戰士普通攻擊1_是否攻擊背後敵人

        //戰士 普通攻擊2
        warriorNormalAttack_2_Damge = 11;//戰士普通攻擊1_傷害
        warriorNormalAttack_2_RepelDirection = 0;//戰士普通攻擊1_擊退方向(0:擊退 1:擊飛)
        warriorNormalAttack_2_RepelDistance = 50;//戰士普通攻擊1_擊退/擊飛距離    
        warriorNormalAttack_2_Effect = "Pain";//戰士普通攻擊1_效果(受擊者播放的動畫名稱)            
        warriorNormalAttack_2_ForwardDistance = 1.3f;//戰士普通攻擊1_攻擊範圍中心點距離物件前方
        warriorNormalAttack_2_attackRadius = 1.2f;//戰士普通攻擊1_攻擊半徑    
        warriorNormalAttack_2_IsAttackBehind = false;//戰士普通攻擊1_是否攻擊背後敵人

        //戰士 普通攻擊3
        warriorNormalAttack_3_Damge = 12;//戰士普通攻擊1_傷害
        warriorNormalAttack_3_RepelDirection = 0;//戰士普通攻擊1_擊退方向(0:擊退 1:擊飛)
        warriorNormalAttack_3_RepelDistance = 50;//戰士普通攻擊1_擊退/擊飛距離    
        warriorNormalAttack_3_Effect = "Pain";//戰士普通攻擊1_效果(受擊者播放的動畫名稱)            
        warriorNormalAttack_3_ForwardDistance = 0.5f;//戰士普通攻擊1_攻擊範圍中心點距離物件前方
        warriorNormalAttack_3_attackRadius = 1.55f;//戰士普通攻擊1_攻擊半徑    
        warriorNormalAttack_3_IsAttackBehind = true;//戰士普通攻擊1_是否攻擊背後敵人

        //戰士 跳躍攻擊
        warriorJumpAttack_Damage = 11;//戰士跳躍攻擊_傷害
        warriorJumpAttack_RepelDirection = 0;//戰士跳躍攻擊_擊退方向(0:擊退 1:擊飛)
        warriorJumpAttac_kRepelDistance = 50;//戰士跳躍攻擊_擊退距離
        warriorJumpAttack_Effect = "Pain";//戰士跳躍攻擊效果(受擊者播放的動畫名稱)
        warriorJumpAttack_ForwardDistance = 0.77f;//戰士普通攻擊3_攻擊範圍中心點距離物件前方
        warriorJumpAttack_attackRadius = 1.0f;//戰士普通攻擊3_攻擊半徑
        warriorJumpAttack_IsAttackBehind = false;//戰士普通攻擊3_是否攻擊背後敵人

        //戰士 技能攻擊1
        warriorSkillAttack_1_Damge = 9;//戰士技能1_傷害
        warriorSkillAttack_1_RepelDirection = 0;//戰士技能1_擊退方向(0:擊退 1:擊飛)
        warriorSkillAttack_1_RepelDistance = 50;//戰士技能1_擊退/擊飛距離    
        warriorSkillAttack_1_Effect = "Pain";//戰士技能1_效果(受擊者播放的動畫名稱)
        warriorSkillAttack_1_FlightSpeed = 20;//戰士技能1_飛行速度
        warriorSkillAttack_1_LifeTime = 0.2f;//戰士技能1_生存時間

        //戰士 技能攻擊2
        warriorSkillAttack_2_Damge = 13;//戰士技能攻擊2_傷害
        warriorSkillAttack_2_RepelDirection = 0;//戰士技能攻擊2_擊退方向(0:擊退 1:擊飛)
        warriorSkillAttack_2_RepelDistance = 50;//戰士技能攻擊2_擊退/擊飛距離    
        warriorSkillAttack_2_Effect = "Pain";//戰士普技能攻擊2_效果(受擊者播放的動畫名稱)        
        warriorSkillAttack_2_ForwardDistance = 1.3f;//戰士技能攻擊2_攻擊範圍中心點距離物件前方
        warriorSkillAttack_2_attackRadius = 1.2f;//戰士技能攻擊2_攻擊半徑
        warriorSkillAttack_2_IsAttackBehind = false;//戰士技能攻擊2_是否攻擊背後敵人

        //戰士 技能攻擊3
        warriorSkillAttack_3_Damge = new float[] { 11, 12, 15};//戰士技能攻擊3_傷害
        warriorSkillAttack_3_RepelDirection = new int[] { 0, 0, 1};//戰士技能攻擊3_擊退方向(0:擊退 1:擊飛)
        warriorSkillAttack_3_RepelDistance = new float[] { 50, 50, 15};//戰士技能攻擊3_擊退/擊飛距離    
        warriorSkillAttack_3_Effect = new string[] { "Pain", "Pain", "Pain" };//戰士普技能攻擊3_效果(受擊者播放的動畫名稱)        
        warriorSkillAttack_3_ForwardDistance = new float[] { 0, 0, 0};//戰士技能攻擊3_攻擊範圍中心點距離物件前方
        warriorSkillAttack_3_attackRadius = new float[] { 1.4f, 1.4f, 5};//戰士技能攻擊3_攻擊半徑
        warriorSkillAttack_3_IsAttackBehind = new bool[] { true, true, true};//戰士技能攻擊3_是否攻擊背後敵人
        #endregion

        #region 弓箭手
        //弓箭手 普通攻擊
        archerNormalAttack_Damge = new float[] { 11, 11, 16 };//弓箭手普通攻擊_傷害
        archerNormalAttack_RepelDirection = new int[] { 0, 0, 0 };//弓箭手普通攻擊_擊退方向(0:擊退 1:擊飛)
        archerNormalAttack_RepelDistance = new float[] { 30, 30, 30 };//弓箭手普通攻擊_擊退/擊飛距離        
        archerNormalAttack_Effect = new string[] { "Pain", "Pain", "Pain" };//弓箭手普通攻擊_效果(受擊者播放的動畫名稱)     
        archerNormalAttack_FloatSpeed = new float[] { 50, 50, 50};//弓箭手普通攻擊飛行速度
        archerNormalAttack_LifeTime = new float[] { 0.3f, 0.3f, 0.3f};//弓箭手普通攻擊生存時間

        //弓箭手 跳躍攻擊
        archerJumpAttack_Damage = 11;//弓箭手跳躍攻擊傷害
        archerJumpAttack_RepelDirection = 0;//弓箭手跳躍攻擊擊退方向(0:擊退 1:擊飛)
        archerJumpAttack_RepelDistance = 50;//弓箭手跳躍攻擊 擊退距離
        archerJumpAttack_Effect = "Pain";//弓箭手跳躍攻擊效果(受擊者播放的動畫名稱)
        archerJumpAttack_ForwardDistance = 0;//弓箭手跳躍攻擊_攻擊範圍中心點距離物件前方
        archerJumpAttack_attackRadius = 1.2f;//弓箭手跳躍攻擊_攻擊半徑
        archerJumpAttack_IsAttackBehind = false;//弓箭手跳躍攻擊_是否攻擊背後敵人

        //弓箭手 技能攻擊1
        archerSkillAttack_1_Damage = 11;//弓箭手技能攻擊攻擊1_傷害
        archerSkillAttack_1_RepelDirection = 0;//弓箭手技能攻擊攻擊1_擊退方向(0:擊退 1:擊飛)
        archerSkillAttack_1_Repel = 13;//弓箭手技能攻擊攻擊1_擊退距離        
        archerSkillAttack_1_Effect = "Pain";//弓箭手技能攻擊攻擊1_效果(受擊者播放的動畫名稱)
        archerSkillAttack_1_FlightSpeed = 50;//弓箭手技能攻擊攻擊1_飛行速度
        archerSkillAttack_1_LifeTime = 0.3f;//弓箭手技能攻擊攻擊1_生存時間

        //弓箭手 技能攻擊2
        archerSkillAttack_2_Damge = 11;//弓箭手技能攻擊2_傷害
        archerSkillAttack_2_RepelDirection = 0;//弓箭手技能攻擊2_擊退方向(0:擊退 1:擊飛)
        archerSkillAttack_2_RepelDistance = 50;//弓箭手技能攻擊2_擊退/擊飛距離    
        archerSkillAttack_2_Effect = "Pain";//弓箭手技能攻擊2_效果(受擊者播放的動畫名稱)        
        archerSkillAttack_2_ForwardDistance = 0;//弓箭手技能攻擊2_攻擊範圍中心點距離物件前方
        archerSkillAttack_2_attackRadius = 1.3f;//弓箭手技能攻擊2_攻擊半徑
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

        //敵人
        enemyHp = 1000;//骷顱士兵生命值
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
