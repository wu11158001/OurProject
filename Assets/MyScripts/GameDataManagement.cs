using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 遊戲資料中心(數值/物件/路徑)
/// </summary>
public class GameDataManagement : MonoBehaviour
{
    static GameDataManagement gameDataManagement;
    public static GameDataManagement Insrance => gameDataManagement;

    [Header("資源中心")]
    public GameData_NumericalValue numericalValue;//遊戲數值
    public GameData_LoadPath loadPath;//遊戲物件(路徑)

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
    }
   
}
