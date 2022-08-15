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

    [Header("戰士")]
    public string warriorSkillAttack_1;//技能攻擊_1

    [Header("弓箭手")]
    public string archerNormalAttack_1;//普通攻擊_1
    public string archerNormalAttack_2;//普通攻擊_2
    public string archerNormalAttack_3;//普通攻擊_3
    public string[] archerAllNormalAttack;//所有普通攻擊
    public string archerSkilllAttack_1;//技能攻擊_1

    [Header("法師")]
    public string magicianNormalAttack_1;//普通攻擊_1

    [Header("敵人")]
    public string enemySoldier_1;//敵人士兵1
    public string enemySoldier_2;//敵人士兵2

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

        //戰士
        warriorSkillAttack_1 = "Prefab/ShootObject/Warrior/WarriorSkillAttack_1";//技攻擊能_1

        //弓箭手
        archerNormalAttack_1 = "Prefab/ShootObject/Archer/ArcherNormalAttack_1";//普通攻擊_1
        archerNormalAttack_2 = "Prefab/ShootObject/Archer/ArcherNormalAttack_2";//普通攻擊_2
        archerNormalAttack_3 = "Prefab/ShootObject/Archer/ArcherNormalAttack_3";//普通攻擊_3
        archerAllNormalAttack = new string[] { archerNormalAttack_1 , archerNormalAttack_2 , archerNormalAttack_3 };//所有普通攻擊物件
        archerSkilllAttack_1 = "Prefab/ShootObject/Archer/ArcherSkilllAttack_1";//技能攻擊_1

        //法師
        magicianNormalAttack_1 = "Prefab/ShootObject/Magician/MagicianNormalAttack_1";//普通攻擊_1

        //敵人
        enemySoldier_1 = "Prefab/Characters/Enemy/EnemySoldier_1";//敵人士兵1
        enemySoldier_2 = "Prefab/Characters/Enemy/EnemySoldier_2";//敵人士兵2


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
