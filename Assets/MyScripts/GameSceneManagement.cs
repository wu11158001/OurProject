using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 遊戲場景管理中心
/// </summary>
public class GameSceneManagement : MonoBehaviourPunCallbacks
{
    static GameSceneManagement gameSceneManagement;
    public static GameSceneManagement Instance => gameSceneManagement;
    ObjectHandle objectHandle = new ObjectHandle();
    public GameData_LoadPath loadPath;

    Dictionary<string, int> objectNumber_Dictionary = new Dictionary<string, int>();//記錄所有物件編號
    public List<AttackBehavior> AttackBehavior_List = new List<AttackBehavior>();//紀錄所有攻擊行為    

    Dictionary<int, GameObject> connectObject_Dixtionary = new Dictionary<int, GameObject>();//記錄所有連線物件

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

        int number = 0;

        //玩家腳色
        number = objectHandle.OnCreateObject(loadPath.allPlayerCharacters[GameDataManagement.Instance.selectRoleNumber]);//產生至物件池
        objectNumber_Dictionary.Add("playerNumbering", number);//添加至紀錄中
        GameObject player = OnRequestOpenObject(OnGetObjectNumber("playerNumbering"), loadPath.allPlayerCharacters[GameDataManagement.Instance.selectRoleNumber]);//開啟物件
        player.transform.position = new Vector3(0, 0.7f, 0);////設定位置
        OnSetMiniMapPoint(player.transform, loadPath.miniMapMatirial_Player);//設定小地圖點點   

        //玩家腳色1_技能1
        number = objectHandle.OnCreateObject(loadPath.playerCharactersSkill_1);////產生至物件池
        objectNumber_Dictionary.Add("playerSkill_1_Numbering", number);//添加至紀錄中

        //骷顱士兵
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
        {
            number = objectHandle.OnCreateObject(loadPath.SkeletonSoldier);//產生至物件池
            objectNumber_Dictionary.Add("skeletonSoldierNumbering", number);////添加至紀錄中
            GameObject skeletonSoldier = OnRequestOpenObject(OnGetObjectNumber("skeletonSoldierNumbering"), loadPath.SkeletonSoldier);//開啟物件
            skeletonSoldier.transform.position = new Vector3(3, 1.7f, 2);//設定位置
            OnSetMiniMapPoint(skeletonSoldier.transform, loadPath.miniMapMatirial_Enemy);//設定小地圖點點
        }
        
        //其他
        number = objectHandle.OnCreateObject(loadPath.hitNumber);//產生至物件池;//擊中數字
        objectNumber_Dictionary.Add("hitNumberNumbering", number);////添加至紀錄中
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
    /// <param name="path">prefab路徑</param>
    /// <returns></returns>
    public GameObject OnRequestOpenObject(int number, string path)
    {        
        GameObject obj = objectHandle.OnOpenObject(number, path);//開啟物件
        return obj;//回傳物件
    }

    /// <summary>
    /// 設定小地圖點點
    /// </summary>
    /// <param name="item">要添加的物件</param>
    /// <param name="item">點的材質路徑</param>
    public void OnSetMiniMapPoint(Transform item, string materialPath)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>(loadPath.miniMapPoint));
        obj.transform.localEulerAngles = new Vector3(90, 0, 0);
        obj.transform.SetParent(item);
        MiniMapPoint map = obj.GetComponent<MiniMapPoint>();
        map.pointMaterial = Resources.Load<Material>(materialPath);
    }  

    /// <summary>
    /// 紀錄連線物件
    /// </summary>
    /// <param name="id">物件ID</param>
    /// <param name="obj">物件</param>
    public void OnRecordConnectObject(int id, GameObject obj)
    {
        connectObject_Dixtionary.Add(id, obj);
    }

    /// <summary>
    /// 連線物件激活狀態
    /// </summary>
    /// <param name="id">物件ID</param>
    /// <param name="active">激活狀態</param>
    public void OnConnectObjectActive(int id, bool active)
    {
        foreach(var obj in connectObject_Dixtionary)
        {
            if (obj.Key == id) obj.Value.SetActive(active);
        }        
    }

    /// <summary>
    /// 獲取連線物件
    /// </summary>
    /// <param name="id">物件ID</param>
    /// <returns></returns>
    public GameObject OnGetConnectObject(int id)
    {
        GameObject theObj = null;

        foreach (var obj in connectObject_Dixtionary)
        {
            if (obj.Key == id) theObj = obj.Value;
        }
        return theObj;
    }

    /// <summary>
    /// 連線生命數值
    /// </summary>
    /// <param name="numberID">數字物件ID</param>
    /// <param name="targetID">受擊目標ID</param>
    /// <param name="damage">受到傷害</param>
    /// <param name="isCritical">是否爆擊</param>
    /// <param name="lifeBarID">生命條物件ID</param>
    /// <param name="HpProportion">生命比例</param>
    public void OnConnectLifeValue(int numberID, int targetID, float damage, bool isCritical, int lifeBarID, float HpProportion)
    {   
        Transform target = null;

        //目標物件
        foreach (var obj in connectObject_Dixtionary)
        {
            if (obj.Key == targetID) target = obj.Value.transform;
        }

        //擊中數字
        foreach (var hitNumber in connectObject_Dixtionary)
        {
            if (hitNumber.Key == numberID) hitNumber.Value.GetComponent<HitNumber>().OnSetValue(target: target, damage: damage, color: isCritical ? Color.yellow : Color.red);
        }

        //頭頂生命條
        foreach (var lifeBar in connectObject_Dixtionary)
        {
            if (lifeBar.Key == lifeBarID) lifeBar.Value.GetComponent<LifeBar_Characters>().SetValue = HpProportion;
        }
    }
}
