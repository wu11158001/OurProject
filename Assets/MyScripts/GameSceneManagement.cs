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
        player.transform.position = new Vector3(23, 2f, 40);////設定位置
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
    #region 一般
    /// <summary>
    /// 攻擊行為
    /// </summary>
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
        obj.transform.SetParent(item);        

        //位置
        Vector3 itemBoxCenter = item.GetComponent<BoxCollider>().center;
        obj.transform.localPosition = new Vector3(0, 0, itemBoxCenter.z);

        //Size
        Vector3 itemBoxSize = item.GetComponent<BoxCollider>().size;        
        obj.transform.localScale = new Vector3(itemBoxSize.x, itemBoxSize.z, 1);

        //選轉
        obj.transform.localEulerAngles = new Vector3(90, 0, 0);

        //材質球(顏色)
        obj.GetComponent<Renderer>().material = Resources.Load<Material>(materialPath);
    }
    #endregion

    #region 連線
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
    /// 連線受擊訊息
    /// </summary>
    /// <param name="targetID">受擊者物件ID</param>
    /// <param name="attackerID">攻擊者物件ID</param>
    /// <param name="layer">攻擊者layer</param>
    /// <param name="damage">造成傷害</param>
    /// <param name="animationName">播放動畫名稱</param>
    /// <param name="knockDirection">擊中效果(0:擊退, 1:擊飛)</param>
    /// <param name="repel">擊退距離</param>
    /// <param name="isCritical">是否爆擊</param>
    public void OnConnectGetHit(int targetID, int attackerID, string layer, float damage, string animationName, int knockDirection, float repel, bool isCritical)
    {
        GameObject attacker = null;

        //搜尋攻擊者物件
        foreach(var attack in connectObject_Dixtionary)
        {
            if (attack.Key == attackerID)
            {
                attacker = attack.Value;
                break;
            }
        }

        //搜尋受擊者物件
        foreach (var obj in connectObject_Dixtionary)
        {
            if(obj.Key == targetID)
            {                
                obj.Value.GetComponent<CharactersCollision>().OnGetHit(attacker: attacker,//攻擊者物件
                                                                       layer: layer,//攻擊者layer
                                                                       damage: damage,//造成傷害
                                                                       animationName: animationName,//攻擊效果(受擊者播放的動畫名稱)
                                                                       knockDirection: knockDirection,//擊退方向((0:擊退 1:擊飛))
                                                                       repel: repel,//擊退距離
                                                                       isCritical: isCritical);//是否爆擊
                break;
            }
        }               
    }

    /// <summary>
    /// 連線動畫設定
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="targetID">動畫更換目標ID</param>
    /// <param name="anmationName">動畫更換目標ID</param>
    /// <param name="animationType">動畫Type</param>
    public void OnConnectAnimationSetting<T>(int targetID, string anmationName, T animationType) 
    {
        
        //搜尋攻擊者物件
        foreach (var target in connectObject_Dixtionary)
        {
            if (target.Key == targetID)
            {
                Animator animator = target.Value.GetComponent<Animator>();
                switch(animationType.GetType().Name)
                {
                    case "Boolean":
                        animator.SetBool(anmationName, Convert.ToBoolean(animationType));
                        break;
                    case "Single":
                        animator.SetFloat(anmationName, Convert.ToSingle(animationType));
                        break;
                    case "Int32":
                        animator.SetInteger(anmationName, Convert.ToInt32(animationType));
                        break;
                        
                }              
            }
        }
    }

    #endregion
}
