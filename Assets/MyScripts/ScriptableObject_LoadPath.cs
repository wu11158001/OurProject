using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// 遊戲物件路徑管理
/// </summary>
[System.Serializable]
public class GameData_LoadPath
{
    [Header("開始場景")]
    public string roleSelect_Button;//選擇腳色按鈕
    public string roleSelect_Sprite;//腳色選擇圖片

    [Header("載入場景")]
    public string LoadBackground_1;//載入場景背景_1

    [Header("小地圖")]
    public string miniMapMatirial_Floor;//小地圖材質(地板)
    public string miniMapMatirial_Object;//小地圖材質(物件)
    public string miniMapMatirial_Player;//小地圖材質(玩家)
    public string miniMapMatirial_OtherPlayer;//小地圖材質(其他玩家)
    public string miniMapMatirial_Enemy;//小地圖材質(敵人)
    public string miniMapPoint;//小地圖(點)

    [Header("玩家腳色")]
    public string Warrior;//玩家腳色
    public string Magician;//玩家腳色
    public string Archer;//玩家腳色
    public string[] allPlayerCharacters;//所有玩家腳色

    [Header("玩家腳色1_技能")]
    public string playerCharactersSkill_1;//玩家腳色1_技能1

    [Header("敵人")]
    public string enemy;//敵人

    [Header("其他")]
    public string hitNumber;//擊中文字
    public string lifeBar;//生命條

    private GameData_LoadPath()
    {
        //開始場景
        roleSelect_Button = "Prefab/UI/RoleSelect_Button";//選擇腳色按鈕
        roleSelect_Sprite = "Sprites/StartScene/SelectRoleScreen/Role";//腳色選擇圖片

        //載入場景
        LoadBackground_1 = "Sprites/LoadScene/LoadBackground_1";//載入場景背景

        //小地圖
        miniMapMatirial_Floor = "Matirials/MiniMap/MiniMpa_Floor";//小地圖材質(地板)
        miniMapMatirial_Object = "Matirials/MiniMap/MiniMpa_Object";//小地圖材質(物件)
        miniMapMatirial_Player = "Matirials/MiniMap/MiniMap_Player";//小地圖材質(玩家)
        miniMapMatirial_OtherPlayer = "Matirials/MiniMap/MiniMap_OtherPlayer";//小地圖材質(其他玩家)
        miniMapMatirial_Enemy = "Matirials/MiniMap/MiniMap_Enemy";//小地圖材質(敵人)
        miniMapPoint = "Prefab/UI/MiniMapPoint";//小地圖(點)

        //玩家腳色
        Warrior = "Prefab/Characters/Player/1_Warrior";//戰士
        Magician = "Prefab/Characters/Player/2_Magician";//魔法師
        Archer = "Prefab/Characters/Player/3_Archer";//弓箭手
        allPlayerCharacters = new string[] { Warrior , Magician, Archer };//所有玩家腳色

        //玩家技能
        playerCharactersSkill_1 = "Prefab/ShootObject/PlayerCharacters1_Skill_1";//玩家腳色1_技能1

        //敵人
        enemy = "Prefab/Characters/Enemy/Enemy";//敵人

        //其他
        hitNumber = "Prefab/UI/HitNumber_Text";//擊中文字
        lifeBar = "Prefab/UI/LifeBar";//生命條
    }
}

/// <summary>
/// 遊戲物件路徑管理中心
/// </summary>
[CreateAssetMenu(fileName = "LoadPath", menuName = "ScriptableObjects/LoadPath", order = 2)]
public class ScriptableObject_LoadPath : ScriptableObject
{
    public GameData_LoadPath loadPath;
}
