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

    [Header("玩家")]
    public string playerCharacters_1;//玩家腳色_1
    public string playerSkill_1;//玩家技能_1

    private GameData_LoadPath()
    {
        //開始場景
        startVideo = "Video/StartVideo";
        roleSelect_Button = "ChooseRoleScreen/RoleSelect_Button";//選擇腳色按鈕
        roleSelect_Sprite = "Sprites/StartScene/ChooseRoleScreen/Role";//腳色選擇圖片

        //載入場景
        LoadBackground_1 = "Sprites/LoadScene/LoadBackground_1";//載入場景背景

        //玩家
        playerCharacters_1 = "Characters/PlayerCharacters_1";//玩家腳色_1
        playerSkill_1 = "Skill/PlayerSkill_1";//玩家技能_1
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
