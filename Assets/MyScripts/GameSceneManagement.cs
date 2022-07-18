using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 遊戲場景管理中心
/// </summary>
public class GameSceneManagement : MonoBehaviour
{
    static GameSceneManagement gameSceneManagement;
    public static GameSceneManagement Instance => gameSceneManagement;
    ObjectHandle objectHandle = new ObjectHandle();
    GameData_LoadPath loadPath;

    Dictionary<string, int> objectNumber_Dictionary = new Dictionary<string, int>();//記錄所有物件編號
    public List<AttackBehavior> AttackBehavior_List = new List<AttackBehavior>();//紀錄所有攻擊行為    

    //物件編號 玩家
    static int playerNumbering;//玩家
    static int playerSkill_1_Numbering;//玩家技能1

    //物件編號 敵人
    static int skeletonSoldierNumbering;//骷顱士兵

    //物件編號 其他
    static int hitNumberNumbering;//擊中數字

    void Awake()
    {       
        if(gameSceneManagement != null)
        {
            Destroy(this);
            return;
        }
        gameSceneManagement = this;
        objectHandle = ObjectHandle.GetObjectHandle;
        loadPath = GameDataManagement.Instance.loadPath;
    }

    void Start()
    {
        //場景物件掛上小地圖點點
        GameObject stageObject = GameObject.Find("StageObjects");        
        Transform[] allStageObject = stageObject.GetComponentsInChildren<Transform>();
        foreach (var item in allStageObject)
        {
            if (item.GetComponent<BoxCollider>()) OnSetMiniMapPoint(item, loadPath.miniMapMatirial_Object);
        }

        //玩家腳色
        playerNumbering = objectHandle.OnCreateObject(loadPath.allPlayerCharacters[GameDataManagement.Instance.selectRoleNumber]);//產生至物件池
        objectNumber_Dictionary.Add("playerNumbering", playerNumbering);//添加至紀錄中
        GameObject player = objectHandle.OnOpenObject(playerNumbering);//產生玩家
        player.transform.position = new Vector3(0, 0.5f, 0);////設定位置
        OnSetMiniMapPoint(player.transform, loadPath.miniMapMatirial_Player);//設定小地圖點點

        //玩家腳色1_技能1
        playerSkill_1_Numbering = objectHandle.OnCreateObject(loadPath.playerCharactersSkill_1);////產生至物件池
        objectNumber_Dictionary.Add("playerSkill_1_Numbering", playerSkill_1_Numbering);//添加至紀錄中

        //骷顱士兵
        skeletonSoldierNumbering = objectHandle.OnCreateObject(loadPath.SkeletonSoldier);//產生至物件池
        objectNumber_Dictionary.Add("skeletonSoldierNumbering", skeletonSoldierNumbering);////添加至紀錄中
        GameObject skeletonSoldier = objectHandle.OnOpenObject(skeletonSoldierNumbering);//產生骷顱士兵
        skeletonSoldier.transform.position = new Vector3(3, 0.5f, 2);//設定位置
        OnSetMiniMapPoint(skeletonSoldier.transform, loadPath.miniMapMatirial_Enemy);//設定小地圖點點

        //其他
        hitNumberNumbering = objectHandle.OnCreateObject(loadPath.hitNumber);//產生至物件池;//擊中數字
        objectNumber_Dictionary.Add("hitNumberNumbering", hitNumberNumbering);////添加至紀錄中
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

    /// <summary>
    /// 設定小地圖點點
    /// </summary>
    /// <param name="item">要添加的物件</param>
    /// <param name="item">點的材質路徑</param>
    void OnSetMiniMapPoint(Transform item, string materialPath)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>(loadPath.miniMapPoint));
        obj.transform.localEulerAngles = new Vector3(90, 0, 0);
        obj.transform.SetParent(item);
        MiniMapPoint map = obj.GetComponent<MiniMapPoint>();
        map.pointMaterial = Resources.Load<Material>(materialPath);
    }
}
