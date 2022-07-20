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

    [Header("玩家 普通攻擊")]
    public float[] playerNormalAttackDamge;//玩家普通攻擊傷害
    public float[] playerNormalAttackMoveDistance;//玩家普通攻擊 移動距離
    public float[] playerNormalAttackRepelDistance;//玩家普通攻擊 擊退/擊飛距離
    public int[] playerNormalAttackRepelDirection;//玩家普通攻擊擊退方向(0:擊退 1:擊飛)
    public string[] playerNormalAttackEffect;//玩家普通攻擊效果(受擊者播放的動畫名稱)
    public Vector3[] playerNormalAttackBoxSize;//玩家普通攻擊框Size

    [Header("玩家 跳躍攻擊")]
    public float playerJumpAttackDamage;//玩家跳躍攻擊傷害
    public string playerJumpAttackEffect;//玩家跳躍攻擊效果(受擊者播放的動畫名稱)
    public float playerJumpAttackRepelDistance;//玩家跳躍攻擊 擊退距離
    public Vector3 playerJumpAttackBoxSize;//玩家跳躍攻擊框Size
    public int playerJumpAttackRepelDirection;//玩家跳躍攻擊擊退方向(0:擊退 1:擊飛)

    [Header("玩家 技能攻擊_1")]
    public float playerSkillAttack_1_Damage;//技能攻擊_1_攻擊傷害
    public string playerSkillAttack_1_Effect;//技能攻擊_1_攻擊效果(受擊者播放的動畫名稱)
    public float playerSkillAttack_1_FlyingSpeed;//技能攻擊_1_物件飛行速度
    public float playerSkillAttack_1_LifeTime;//技能攻擊_1_生存時間
    public float playerSkillAttack_1_Repel;//技能攻擊_1_擊退距離
    public int playerSkillAttack_1_RepelDirection;//玩家技能攻擊_1_擊退方向(0:擊退 1:擊飛)

    [Header("玩家 技能攻擊_2")]
    public float playerSkillAttack_2_Damage;//技能攻擊_2_攻擊傷害
    public string playerSkillAttack_2_Effect;//技能攻擊_2_攻擊效果(受擊者播放的動畫名稱)    
    public float playerSkillAttack_2_Repel;//技能攻擊_2_擊退距離
    public int playerSkillAttack_2_RepelDirection;//玩家技能攻擊_2_擊退方向(0:擊退 1:擊飛)
    public Vector3 playerSkillAttack_2_BoxSize;//玩家技能攻擊_2_攻擊框Size

    [Header("玩家 技能攻擊_3")]
    public float playerSkillAttack_3_Damage;//技能攻擊_3_攻擊傷害
    public string playerSkillAttack_3_Effect;//技能攻擊_3_攻擊效果(受擊者播放的動畫名稱)    
    public float playerSkillAttack_3_Repel;//技能攻擊_3_擊退距離
    public int playerSkillAttack_3_RepelDirection;//玩家技能攻擊_3_擊退方向(0:擊退 1:擊飛)
    public Vector3 playerSkillAttack_3_BoxSize;//玩家技能攻擊_3_攻擊框Size

    [Header("玩家 骷顱士兵")]
    public float skeletonSoldierHp;//骷顱士兵生命值

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
        playerMoveSpeed = 10;//玩家移動速度        
        playerJumpForce = 16;//玩家跳躍力
        playerCriticalRate = 30;//玩家暴擊率

        //玩家 普通攻擊
        playerNormalAttackDamge = new float[] { 10, 10, 15 };//玩家普通攻擊傷害
        playerNormalAttackMoveDistance = new float[] { 50, 50, 0 };//玩家普通攻擊移動距離
        playerNormalAttackRepelDistance = new float[] { 70, 80, 18 };//玩家普通攻擊 擊退/擊飛距離
        playerNormalAttackRepelDirection = new int[] { 0, 0, 1 };//玩家普通攻擊擊退方向(0:擊退 1:擊飛)
        playerNormalAttackEffect = new string[] { "Pain", "Pain", "Pain" };//玩家普通攻擊效果(受擊者播放的動畫名稱)
        playerNormalAttackBoxSize = new Vector3[] { new Vector3(1, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, 1) };//玩家普通攻擊攻擊框Size        

        //玩家 跳躍攻擊
        playerJumpAttackDamage = 10;//玩家跳躍攻擊傷害
        playerJumpAttackEffect = "KnockBack";//玩家跳躍攻擊效果(受擊者播放的動畫名稱)
        playerJumpAttackRepelDistance = 50;//玩家跳躍攻擊 擊退距離
        playerJumpAttackBoxSize = new Vector3(1, 0.5f, 1);//玩家跳躍攻擊攻擊框Size
        playerJumpAttackRepelDirection = 0;//玩家跳躍攻擊擊退方向(0:擊退 1:擊飛)

        //玩家 技能攻擊_1
        playerSkillAttack_1_Damage = 33;//技能攻擊_1_攻擊傷害
        playerSkillAttack_1_Effect = "KnockBack";//技能攻擊_1_攻擊效果(受擊者播放的動畫名稱)
        playerSkillAttack_1_FlyingSpeed = 11.5f;//技能攻擊_1_物件飛行速度
        playerSkillAttack_1_LifeTime = 0.75f;//技能攻擊_1_生存時間
        playerSkillAttack_1_Repel = 70;//技能攻擊_1_擊退距離
        playerSkillAttack_1_RepelDirection = 0;//玩家普通攻擊擊退方向(0:擊退 1:擊飛)

        //玩家 技能攻擊_2
        playerSkillAttack_2_Damage = 44;//技能攻擊_1_攻擊傷害
        playerSkillAttack_2_Effect = "Pain";//技能攻擊_1_攻擊效果(受擊者播放的動畫名稱)
        playerSkillAttack_2_Repel = 18;//技能攻擊_1_擊退距離
        playerSkillAttack_2_RepelDirection = 1;//玩家普通攻擊擊退方向(0:擊退 1:擊飛)
        playerSkillAttack_2_BoxSize = new Vector3(1, 1, 1);//玩家技能攻擊_2_攻擊框Size

        //玩家 技能攻擊_3
        playerSkillAttack_3_Damage = 55;//技能攻擊_1_攻擊傷害
        playerSkillAttack_3_Effect = "KnockBack";//技能攻擊_1_攻擊效果(受擊者播放的動畫名稱)
        playerSkillAttack_3_Repel = 100;//技能攻擊_1_擊退距離
        playerSkillAttack_3_RepelDirection = 0;//玩家普通攻擊擊退方向(0:擊退 1:擊飛)
        playerSkillAttack_3_BoxSize = new Vector3(1, 1, 1);//玩家技能攻擊_2_攻擊框Size

        //骷顱士兵
        skeletonSoldierHp = 300;//骷顱士兵生命值
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
