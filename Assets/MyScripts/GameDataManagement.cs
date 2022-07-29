using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 遊戲資料中心(數值/路徑)
/// </summary>
public class GameDataManagement : MonoBehaviour
{
    static GameDataManagement gameDataManagement;
    public static GameDataManagement Instance => gameDataManagement;

    /// <summary>
    /// 目前場景
    /// </summary>
    public enum Stage {開始場景,遊戲場景}
    public Stage stage = new Stage();

    [Header("資源中心")]
    public GameData_NumericalValue numericalValue;//遊戲數值
    public GameData_LoadPath loadPath;//遊戲物件(路徑)

    [Header("紀錄遊戲資料")]
    public bool isConnect;//是否連線
    public bool isNotFirstIntoGame;//是否第一次進入遊戲
    public int selectRoleNumber;//選擇的腳色編號
    public int selectLevelNumber;//選擇的關卡編號
    public int[] equipBuff;//裝備的Buff

    void Awake()
    {
        if(gameDataManagement != null)
        {
            Destroy(this);
            return;
        }
        gameDataManagement = this;
        DontDestroyOnLoad(gameObject);

        numericalValue = Resources.Load<ScriptableObject_NumericalValue>("ScriptableObject/NumericalValue").numericalValue;
        loadPath = Resources.Load<ScriptableObject_LoadPath>("ScriptableObject/LoadPath").loadPath;

        //紀錄遊戲資料
        equipBuff = new int[2] { -1, -1};
    }   
}
