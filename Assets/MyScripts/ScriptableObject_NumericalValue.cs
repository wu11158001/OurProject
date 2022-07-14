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

    [Header("攝影機")]
    public float distance;//與玩家距離
    public float limitUpAngle;//限制向上角度
    public float limitDownAngle;//限制向下角度

    [Header("玩家")]
    public float playerHp;//玩家生命值
    public float playerMoveSpeed;//玩家移動速度
    public float playerJumpForce;//玩家跳躍力

    [Header("玩家 普通攻擊")]
    public float[] playerNormalAttackDamge;//玩家普通攻擊傷害
    public float[] playerNormalAttackMoveDistance;//玩家普通攻擊 移動距離
    public float[] playerNormalAttackRepelDistance;//玩家普通攻擊 擊退/擊飛距離
    public int[] playerNormalAttackRepelDirection;//玩家普通攻擊擊退方向(0:擊退 1:擊飛)
    public string[] playerNormalAttackEffect;//玩家普通攻擊效果(受擊者播放的動畫名稱)
    public Vector3[] playerNormalAttackBoxSize;//玩家普通攻擊攻擊框Size

    [Header("玩家 跳躍攻擊")]
    public float playerJumpAttackDamage;//玩家跳躍攻擊傷害
    public string playerJumpAttackEffect;//玩家跳躍攻擊效果(受擊者播放的動畫名稱)
    public float playerJumpAttackRepelDistance;//玩家跳躍攻擊 擊退距離
    public Vector3 playerJumpAttackBoxSize;//玩家跳躍攻擊攻擊框Size
    public int playerJumpAttackRepelDirection;//玩家跳躍攻擊擊退方向(0:擊退 1:擊飛)

    [Header("玩家 技能攻擊_1")]
    public float playerSkillAttack_1_Damage;//技能攻擊_1_攻擊傷害
    public string playerSkillAttack_1_Effect;//技能攻擊_1_攻擊效果(受擊者播放的動畫名稱)
    public float playerSkillAttack_1_FlyingSpeed;//技能攻擊_1_物件飛行速度
    public float playerSkillAttack_1_LifeTime;//技能攻擊_1_生存時間
    public float playerSkillAttack_1_Repel;//技能攻擊_1_擊退距離
    public int playerSkillAttack_1_RepelDirection;//玩家技能攻擊擊退方向(0:擊退 1:擊飛)

    [Header("玩家 骷顱士兵")]
    public float skeletonSoldierHp;//骷顱士兵生命值

    /// <summary>
    /// 建構子
    /// </summary>
    public GameData_NumericalValue()
    {
        //共通
        gravity = 9.8f;//重力
        boxCollisionDistance = 0.5f;//碰撞框距離(與牆面距離)

        //攝影機
        distance = 2.6f;//與玩家距離        
        limitUpAngle = 35;//限制向上角度
        limitDownAngle = 20;//限制向下角度

        //玩家
        playerHp = 100;//玩家生命值
        playerMoveSpeed = 10;//玩家移動速度        
        playerJumpForce = 16;//玩家跳躍力

        //玩家 普通攻擊
        playerNormalAttackDamge = new float[] { 10, 10, 15 };//玩家普通攻擊傷害
        playerNormalAttackMoveDistance = new float[] { 50, 50, 0 };//玩家普通攻擊移動距離
        playerNormalAttackRepelDistance = new float[] { 70, 80, 18 };//玩家普通攻擊 擊退/擊飛距離
        playerNormalAttackRepelDirection = new int[] { 0, 0, 1 };//玩家普通攻擊擊退方向(0:擊退 1:擊飛)
        playerNormalAttackEffect = new string[] { "Pain", "Pain", "Pain" };//玩家普通攻擊效果(受擊者播放的動畫名稱)
        playerNormalAttackBoxSize = new Vector3[] { new Vector3(1, 1, 1), new Vector3(0.5f, 1, 0.5f), new Vector3(2, 2, 2) };//玩家普通攻擊攻擊框Size        

        //玩家 跳躍攻擊
        playerJumpAttackDamage = 10;//玩家跳躍攻擊傷害
        playerJumpAttackEffect = "KnockBack";//玩家跳躍攻擊效果(受擊者播放的動畫名稱)
        playerJumpAttackRepelDistance = 50;//玩家跳躍攻擊 擊退距離
        playerJumpAttackBoxSize = new Vector3(1, 0.5f, 1);//玩家跳躍攻擊攻擊框Size
        playerJumpAttackRepelDirection = 0;//玩家跳躍攻擊擊退方向(0:擊退 1:擊飛)

        //技能攻擊_1
        playerSkillAttack_1_Damage = 33;//技能攻擊_1_攻擊傷害
        playerSkillAttack_1_Effect = "KnockBack";//技能攻擊_1_攻擊效果(受擊者播放的動畫名稱)
        playerSkillAttack_1_FlyingSpeed = 11.5f;//技能攻擊_1_物件飛行速度
        playerSkillAttack_1_LifeTime = 0.75f;//技能攻擊_1_生存時間
        playerSkillAttack_1_Repel = 70;//技能攻擊_1_擊退距離
        playerSkillAttack_1_RepelDirection = 0;//玩家普通攻擊擊退方向(0:擊退 1:擊飛)

        //骷顱士兵
        skeletonSoldierHp = 50;//骷顱士兵生命值
    }
}

[CreateAssetMenu(fileName = "NumericalValue", menuName = "ScriptableObjects/NumericalValue", order = 1)]
public class ScriptableObject_NumericalValue : ScriptableObject
{
    public GameData_NumericalValue numericalValue;
}
