using UnityEngine;

/// <summary>
/// 遊戲數值
/// </summary>
[System.Serializable]
public class GameData_NumericalValue
{
    [Header("共通")]
    public float gravity;//重力
    public float boxCollisionDistance;//碰撞框距離(與牆面距離)
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

    [Header("戰士 普通攻擊")]
    public float[] warriorNormalAttackDamge;//玩家普通攻擊傷害
    public float[] warriorNormalAttackMoveDistance;//玩家普通攻擊 移動距離
    public float[] warriorNormalAttackRepelDistance;//玩家普通攻擊 擊退/擊飛距離
    public int[] warriorNormalAttackRepelDirection;//玩家普通攻擊擊退方向(0:擊退 1:擊飛)
    public string[] warriorNormalAttackEffect;//玩家普通攻擊效果(受擊者播放的動畫名稱)
    public Vector3[] warriorNormalAttackBoxSize;//玩家普通攻擊框Size

    [Header("戰士 跳躍攻擊")]
    public float warriorJumpAttackDamage;//玩家跳躍攻擊傷害
    public string warriorJumpAttackEffect;//玩家跳躍攻擊效果(受擊者播放的動畫名稱)
    public float warriorJumpAttackRepelDistance;//玩家跳躍攻擊 擊退距離
    public Vector3 warriorJumpAttackBoxSize;//玩家跳躍攻擊框Size
    public int warriorJumpAttackRepelDirection;//玩家跳躍攻擊擊退方向(0:擊退 1:擊飛)

    [Header("戰士 技能攻擊")]
    public float[] warriorSkillAttackDamage;//技能攻擊傷害
    public string[] warriorSkillAttackEffect;//技能攻擊攻擊效果(受擊者播放的動畫名稱)    
    public int[] warriorSkillAttackRepelDirection;//玩家技能攻擊擊退方向(0:擊退 1:擊飛)
    public float[] warriorSkillAttackRepel;//技能攻擊擊退距離    
    public Vector3[] warriorSkillAttackBoxSize;//玩家技能攻擊攻擊框Size

    [Header("玩家 骷顱士兵")]
    public float enemyHp;//骷顱士兵生命值

    /// <summary>
    /// 建構子
    /// </summary>
    private GameData_NumericalValue()
    {
        //共通
        gravity = 9.8f;//重力
        boxCollisionDistance = 0.5f;//碰撞框距離(與牆面距離)
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
        playerCriticalRate = 30;//玩家暴擊率
        playerDodgeSeppd = 3;//玩家閃躲速度

        //戰士 普通攻擊
        warriorNormalAttackDamge = new float[] { 10, 10, 15 };//戰士普通攻擊傷害
        warriorNormalAttackMoveDistance = new float[] { 50, 50, 50 };//戰士攻擊移動距離
        warriorNormalAttackRepelDistance = new float[] { 120, 80, 60 };//戰士普通攻擊 擊退/擊飛距離
        warriorNormalAttackRepelDirection = new int[] { 0, 0, 0 };//戰士普通攻擊擊退方向(0:擊退 1:擊飛)
        warriorNormalAttackEffect = new string[] { "Pain", "Pain", "Pain" };//戰士普通攻擊效果(受擊者播放的動畫名稱)
        warriorNormalAttackBoxSize = new Vector3[] { new Vector3(1.5f, 1.5f, 1.5f), new Vector3(1.5f, 1.5f, 1.5f), new Vector3(1.5f, 1.5f, 1.5f) };//戰士普通攻擊攻擊框Size        

        //戰士 跳躍攻擊
        warriorJumpAttackDamage = 10;//戰士跳躍攻擊傷害
        warriorJumpAttackEffect = "Pain";//戰士跳躍攻擊效果(受擊者播放的動畫名稱)
        warriorJumpAttackRepelDistance = 50;//戰士跳躍攻擊 擊退距離
        warriorJumpAttackBoxSize = new Vector3(1.5f, 1f, 1.5f);//戰士跳躍攻擊攻擊框Size
        warriorJumpAttackRepelDirection = 0;//戰士跳躍攻擊擊退方向(0:擊退 1:擊飛)

        //戰士 技能攻擊
        warriorSkillAttackDamage = new float[] { 10, 20, 30 };//技能攻擊傷害
        warriorSkillAttackEffect = new string[] { "Pain", "Pain", "Pain" };//技能攻擊攻擊效果(受擊者播放的動畫名稱)    
        warriorSkillAttackRepelDirection = new int[] { 0, 0, 0 };//技能攻擊擊退方向(0:擊退 1:擊飛)
        warriorSkillAttackRepel = new float[] { 50, 50, 50};//技能攻擊擊退距離        
        warriorSkillAttackBoxSize = new Vector3[] { new Vector3(1.5f, 1.5f, 1.5f), new Vector3(1.5f, 1.5f, 1.5f) , new Vector3(1.5f, 1.5f, 1.5f) };//技能攻擊攻擊框Size         

        //骷顱士兵
        enemyHp = 50;//骷顱士兵生命值
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
