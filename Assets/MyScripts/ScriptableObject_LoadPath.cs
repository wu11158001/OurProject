using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// 遊戲物件路徑管理
/// </summary>
[System.Serializable]
public class GameData_LoadPath
{
    [Header("開始場景")]
    public string startVideo;//開始影片
    public string roleSelect_Button;//選擇腳色按鈕
    public string roleSelect_Sprite;//腳色選擇圖片

    [Header("載入場景")]
    public string LoadBackground_1;//載入場景背景_1

    [Header("小地圖")]
    public string miniMapMatirial_Floor;//小地圖材質(地板)
    public string miniMapMatirial_Object;//小地圖材質(物件)
    public string miniMapMatirial_Player;//小地圖材質(玩家)
    public string miniMapMatirial_Enemy;//小地圖材質(敵人)
    public string miniMapPoint;//小地圖(點)

    [Header("玩家")]
    public string playerCharacters;//玩家腳色
    public string playerSkill_1;//玩家技能_1

    [Header("骷顱士兵")]
    public string SkeletonSoldier;//骷顱士兵

    private GameData_LoadPath()
    {
        //開始場景
        startVideo = "Video/StartVideo";
        roleSelect_Button = "SelectRoleScreen/RoleSelect_Button";//選擇腳色按鈕
        roleSelect_Sprite = "Sprites/StartScene/SelectRoleScreen/Role";//腳色選擇圖片

        //載入場景
        LoadBackground_1 = "Sprites/LoadScene/LoadBackground_1";//載入場景背景

        //小地圖
        miniMapMatirial_Floor = "Matirials/MiniMap/MiniMpa_Floor";//小地圖材質(地板)
        miniMapMatirial_Object = "Matirials/MiniMap/MiniMpa_Object";//小地圖材質(物件)
        miniMapMatirial_Player = "Matirials/MiniMap/MiniMap_Player";//小地圖材質(玩家)
        miniMapMatirial_Enemy = "Matirials/MiniMap/MiniMap_Enemy";//小地圖材質(敵人)
        miniMapPoint = "MiniMap/MiniMapPoint";//小地圖(點)

        //玩家
        playerCharacters = "Characters/PlayerCharacters_1";//玩家腳色
        playerSkill_1 = "ShootObject/PlayerSkill_1";//玩家技能_1

        //骷顱士兵
        SkeletonSoldier = "Characters/SkeletonSoldier";//骷顱士兵
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
