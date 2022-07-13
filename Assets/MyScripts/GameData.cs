using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 遊戲資料
/// </summary>
public class GameData : MonoBehaviour
{
    static GameData gameData;
    public static GameData Instance => gameData;

    Dictionary<string, float> gameData_Float_Dictionary = new Dictionary<string, float>();//紀錄遊戲數值(Float)
    Dictionary<string, float[]> gameData_FloatArray_Dictionary = new Dictionary<string, float[]>();//紀錄遊戲數值(FloatArray)

    Dictionary<string, string> gameData_String_Dictionary = new Dictionary<string, string>();//紀錄遊戲數值(String)
    Dictionary<string, string[]> gameData_StringArray_Dictionary = new Dictionary<string, string[]>();//紀錄遊戲數值(StringArray)

    Dictionary<string, Vector3> gameData_Vectorg_Dictionary = new Dictionary<string, Vector3>();//紀錄遊戲數值(Vector)
    Dictionary<string, Vector3[]> gameData_VectorgArray_Dictionary = new Dictionary<string, Vector3[]>();//紀錄遊戲數值(VectorArray)

    //共通
    static float gravity;//重力

    //玩家
    static float playerHp;//玩家生命值
    static float playerMoveSpeed;//玩家移動速度
    static float playerJumpForce;//玩家跳躍力

    //玩家 普通攻擊
    static float[] playerNormalAttackDamge;//玩家普通攻擊傷害
    static float[] playerNormalAttackMoveDistance;//玩家普通攻擊移動距離
    static float[] playerNormalAttackRepelDistance;//玩家普通攻擊 擊退/擊飛距離
    static float[] playerNormalAttackRepelDirection;//玩家普通攻擊方向(0:擊退 1:擊飛)
    static string[] playerNormalAttackEffect;//玩家普通攻擊效果(受擊者播放的動畫名稱)
    static Vector3[] playerNormalAttackBoxSize;//玩家普通攻擊攻擊框Size

    //玩家 跳躍攻擊
    static float playerJumpAttackDamage;//玩家跳躍攻擊傷害
    static string playerJumpAttackEffect;//玩家跳躍攻擊效果(受擊者播放的動畫名稱)
    static float playerJumpAttackRepelDistance;//玩家跳躍攻擊 擊退距離
    static Vector3 playerJumpAttackBoxSize;//玩家跳躍攻擊攻擊框Size

    //技能攻擊_1
    static float playerSkillAttack_1_Damage;//技能攻擊_1_攻擊傷害
    static string playerSkillAttack_1_Effect;//技能攻擊_1_攻擊效果(受擊者播放的動畫名稱)
    static float playerSkillAttack_1_FlyingSpeed;//技能攻擊_1_物件飛行速度
    static float playerSkillAttack_1_LifeTime;//技能攻擊_1_生存時間
    static float playerSkillAttack_1_Repel;//技能攻擊_1_擊退距離

    //骷顱士兵
    static float skeletonSoldierHp;//骷顱士兵生命值

    private void Awake()
    {
        if(gameData != null)
        {
            Destroy(this);
            return;
        }
        gameData = this;

        //共通
        gravity = 9.8f;//重力
        gameData_Float_Dictionary.Add("gravity", gravity);

        //玩家
        playerHp = 100;//玩家生命值
        playerMoveSpeed = 10;//玩家移動速度        
        playerJumpForce = 16;//玩家跳躍力
        gameData_Float_Dictionary.Add("playerHp", playerHp);
        gameData_Float_Dictionary.Add("playerMoveSpeed", playerMoveSpeed);
        gameData_Float_Dictionary.Add("playerJumpForce", playerJumpForce);

        //玩家 普通攻擊
        playerNormalAttackDamge = new float[] { 10, 10, 15};//玩家普通攻擊傷害
        playerNormalAttackMoveDistance = new float[] { 50, 50, 0 };//玩家普通攻擊移動距離
        playerNormalAttackRepelDistance = new float[] { 70, 80, 18};//玩家普通攻擊 擊退/擊飛距離
        playerNormalAttackRepelDirection = new float[] { 0, 0, 1};//玩家普通攻擊擊中效果(0:擊退 1:擊飛)
        playerNormalAttackEffect = new string[] { "Pain", "Pain", "Pain"};//玩家普通攻擊效果(受擊者播放的動畫名稱)
        playerNormalAttackBoxSize = new Vector3[] { new Vector3(1, 1, 1), new Vector3(0.5f, 1, 0.5f), new Vector3(2, 2, 2)};//玩家普通攻擊攻擊框Size        
        gameData_FloatArray_Dictionary.Add("playerNormalAttackDamge", playerNormalAttackDamge);
        gameData_FloatArray_Dictionary.Add("playerNormalAttackMoveDistance", playerNormalAttackMoveDistance);
        gameData_FloatArray_Dictionary.Add("playerNormalAttackRepelDistance", playerNormalAttackRepelDistance);
        gameData_FloatArray_Dictionary.Add("playerNormalAttackRepelDirection", playerNormalAttackRepelDirection);
        gameData_StringArray_Dictionary.Add("playerNormalAttackEffect", playerNormalAttackEffect);
        gameData_VectorgArray_Dictionary.Add("playerNormalAttackBoxSize", playerNormalAttackBoxSize);

        //玩家 跳躍攻擊
        playerJumpAttackDamage = 10;//玩家跳躍攻擊傷害
        playerJumpAttackEffect = "KnockBack";//玩家跳躍攻擊效果(受擊者播放的動畫名稱)
        playerJumpAttackRepelDistance = 50;//玩家跳躍攻擊 擊退距離
        playerJumpAttackBoxSize = new Vector3(1, 0.5f, 1);//玩家跳躍攻擊攻擊框Size
        gameData_Float_Dictionary.Add("playerJumpAttackDamage", playerJumpAttackDamage);
        gameData_String_Dictionary.Add("playerJumpAttackEffect", playerJumpAttackEffect);
        gameData_Float_Dictionary.Add("playerJumpAttackRepelDistance", playerJumpAttackRepelDistance);
        gameData_Vectorg_Dictionary.Add("playerJumpAttackBoxSize", playerJumpAttackBoxSize);

        //技能攻擊_1
        playerSkillAttack_1_Damage = 33;//技能攻擊_1_攻擊傷害
        playerSkillAttack_1_Effect = "KnockBack";//技能攻擊_1_攻擊效果(受擊者播放的動畫名稱)
        playerSkillAttack_1_FlyingSpeed = 11.5f;//技能攻擊_1_物件飛行速度
        playerSkillAttack_1_LifeTime = 0.75f;//技能攻擊_1_生存時間
        playerSkillAttack_1_Repel = 70;//技能攻擊_1_擊退距離
        gameData_Float_Dictionary.Add("playerSkillAttack_1_Damage", playerSkillAttack_1_Damage);
        gameData_String_Dictionary.Add("playerSkillAttack_1_Effect", playerSkillAttack_1_Effect);
        gameData_Float_Dictionary.Add("playerSkillAttack_1_FlyingSpeed", playerSkillAttack_1_FlyingSpeed);
        gameData_Float_Dictionary.Add("playerSkillAttack_1_LifeTime", playerSkillAttack_1_LifeTime);
        gameData_Float_Dictionary.Add("playerSkillAttack_1_Repel", playerSkillAttack_1_Repel);

        //骷顱士兵
        skeletonSoldierHp = 50;//骷顱士兵生命值
        gameData_Float_Dictionary.Add("skeletonSoldierHp", skeletonSoldierHp);
    }

    /// <summary>
    /// 獲取數值(Float)
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    public float OnGetFloatValue (string search)
    {
        float value = 0;

        foreach(var data in gameData_Float_Dictionary)
        {         
            if(data.Key == search)
            {                
                value = data.Value;
            }
        }

        return value;
    }

    /// <summary>
    /// 獲取數值(Float Array)
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    public float[] OnGetFloatArrayValue(string search)
    {
        float[] value = new float[] { };

        foreach (var data in gameData_FloatArray_Dictionary)
        {
            if (data.Key == search)
            {
                value = data.Value;
            }
        }

        return value;
    }

    /// <summary>
    /// 獲取數值(String)
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    public string OnGetStringValue(string search)
    {
        string value = "";

        foreach (var data in gameData_String_Dictionary)
        {
            if (data.Key == search)
            {
                value = data.Value;
            }
        }

        return value;
    }

    /// <summary>
    /// 獲取數值(String Array)
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    public string[] OnGetStringArrayValue(string search)
    {
        string[] value = new string[] { };

        foreach (var data in gameData_StringArray_Dictionary)
        {
            if (data.Key == search)
            {
                value = data.Value;
            }
        }

        return value;
    }

    /// <summary>
    /// 獲取數值(Vector)
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    public Vector3 OnGetVectorValue(string search)
    {
        Vector3 value = new Vector3();

        foreach (var data in gameData_Vectorg_Dictionary)
        {
            if (data.Key == search)
            {
                value = data.Value;
            }
        }

        return value;
    }

    /// <summary>
    /// 獲取數值(Vector Array)
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    public Vector3[] OnGetVectorArrayValue(string search)
    {
        Vector3[] value = new Vector3[] { };

        foreach (var data in gameData_VectorgArray_Dictionary)
        {
            if (data.Key == search)
            {
                value = data.Value;
            }
        }

        return value;
    }
}
