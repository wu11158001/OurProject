using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 遊戲管理中心
/// </summary>
public class GameManagement : MonoBehaviour
{
    static GameManagement gameManagement;
    public static GameManagement Instance => gameManagement;
    ObjectHandle objectHandle = new ObjectHandle();
    GameData_LoadPath loadPath;

    Dictionary<string, int> objectNumber_Dictionary = new Dictionary<string, int>();//記錄所有物件編號
    public List<AttackBehavior> AttackBehavior_List = new List<AttackBehavior>();//紀錄所有攻擊行為    

    //物件編號 玩家
    static int playerNumber;//玩家
    static int playerSkill_1_Number;//玩家技能1

    void Awake()
    {       
        if(gameManagement != null)
        {
            Destroy(this);
            return;
        }
        gameManagement = this;
        objectHandle = ObjectHandle.GetObjectHandle;
        loadPath = GameDataManagement.Insrance.loadPath;

        //玩家腳色_1
        playerNumber = objectHandle.OnCreateObject(loadPath.playerCharacters_1);
        objectNumber_Dictionary.Add("playerNumber", playerNumber);
        GameObject player = objectHandle.OnOpenObject(playerNumber);//產生玩家
        player.transform.position = new Vector3(0, 0.5f, 0);

        //玩家技能_1
        playerSkill_1_Number = objectHandle.OnCreateObject(loadPath.playerSkill_1);
        objectNumber_Dictionary.Add("playerSkill_1_Number", playerSkill_1_Number);
    }

    void Update()
    {        
        OnAttackBehavior();
    }

    //攻擊行為
    void OnAttackBehavior()
    {
        for (int i = 0; i < AttackBehavior_List.Count; i++)
        {
            AttackBehavior_List[i].function.Invoke();            
        }
    }

    /// <summary>
    /// 獲取物件編號
    /// </summary>
    /// <param name="objectNmae">要開啟的物件名稱</param>
    /// <returns></returns>
    public int OnGetObjectNumber(string objectNmae)
    {
        int value = -1;

        foreach (var obj in objectNumber_Dictionary)
        {
            if(obj.Key == objectNmae)
            {
                value = obj.Value;
            }
        }
        
        return value;
    }

    /// <summary>
    /// 要求開啟物件
    /// </summary>
    /// <param name="number">物件編號</param>
    /// <returns></returns>
    public GameObject OnRequestOpenObject(int number)
    {        
        GameObject obj = objectHandle.OnOpenObject(number);//開啟物件
        return obj;//回傳物件
    }
}
