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
    public List<AttackMode> AttackBehavior_List = new List<AttackMode>();//紀錄所有攻擊行為    

    Dictionary<int, GameObject> connectObject_Dictionary = new Dictionary<int, GameObject>();//記錄所有連線物件

    void Awake()
    {       
        if(gameSceneManagement != null)
        {
            Destroy(this);
            return;
        }
        gameSceneManagement = this;

        GameDataManagement.Instance.stage = GameDataManagement.Stage.遊戲場景;

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
        player.transform.position = new Vector3(222, -22, -60);//設定位置
        player.transform.rotation = Quaternion.Euler(0, -60, 0);//設定選轉
        OnSetMiniMapPoint(player.transform, loadPath.miniMapMatirial_Player);//設定小地圖點點   

        //戰士物件
        number = objectHandle.OnCreateObject(loadPath.warriorSkillAttack_1);//戰士技能攻擊_1物件
        objectNumber_Dictionary.Add("warriorSkillAttack_1", number);//添加至紀錄中

        //弓箭手物件
        number = objectHandle.OnCreateObject(loadPath.archerNormalAttack_1);//普通攻擊_1物件
        objectNumber_Dictionary.Add("archerNormalAttack_1", number);//添加至紀錄中
        number = objectHandle.OnCreateObject(loadPath.archerNormalAttack_2);//普通攻擊_2物件
        objectNumber_Dictionary.Add("archerNormalAttack_2", number);//添加至紀錄中
        number = objectHandle.OnCreateObject(loadPath.archerNormalAttack_3);//普通攻擊_3物件
        objectNumber_Dictionary.Add("archerNormalAttack_3", number);//添加至紀錄中
        number = objectHandle.OnCreateObject(loadPath.archerSkilllAttack_1);//技能攻擊_1物件
        objectNumber_Dictionary.Add("archerSkilllAttack_1", number);//添加至紀錄中

        //法師物件
        number = objectHandle.OnCreateObject(loadPath.magicianNormalAttack_1);//普通攻擊_1物件
        objectNumber_Dictionary.Add("magicianNormalAttack_1", number);//添加至紀錄中

        //敵人士兵1
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
        {
            number = objectHandle.OnCreateObject(loadPath.enemySoldier_1);//產生至物件池
            objectNumber_Dictionary.Add("enemySoldier_1", number);////添加至紀錄中

            for (int i = 0; i < 1; i++)
            {                
                GameObject enemy = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_1"), loadPath.enemySoldier_1);//開啟物件
                enemy.transform.position = new Vector3(188, -24, -37);//設定位置
                enemy.transform.rotation = Quaternion.Euler(0, 90, 0);
                OnSetMiniMapPoint(enemy.transform, loadPath.miniMapMatirial_Enemy);//設定小地圖點點
            }
        }
    }

    void Update()
    {        
        OnAttackBehavior();   
        
        if(Input.GetKeyDown(KeyCode.O))
        {
            if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
            {
                GameObject enemy = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_1"), loadPath.enemySoldier_1);//開啟物件
                CharactersCollision collision = enemy.GetComponent<CharactersCollision>();
                if (collision != null) collision.OnInitial();//初始化
                enemy.transform.position = new Vector3(24 + 2 * 1, 2f, 40);//設定位置                
                OnSetMiniMapPoint(enemy.transform, loadPath.miniMapMatirial_Enemy);//設定小地圖點點
            }
        }
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
        obj.transform.localPosition = new Vector3(itemBoxCenter.x, 0, itemBoxCenter.z);

        //Size
        if (item.gameObject.layer != LayerMask.NameToLayer("Player") && item.gameObject.layer != LayerMask.NameToLayer("Enemy"))
        {
            Vector3 itemBoxSize = item.GetComponent<BoxCollider>().size;
            obj.transform.localScale = new Vector3(itemBoxSize.x, itemBoxSize.z, 1);
        }
        else obj.transform.localScale = new Vector3(1, 1, 1);

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
        connectObject_Dictionary.Add(id, obj);
    }

    /// <summary>
    /// 連線物件激活狀態
    /// </summary>
    /// <param name="targetID">目標ID</param>
    /// <param name="active">激活狀態</param>
    public void OnConnectObjectActive(int targetID, bool active)
    {
        connectObject_Dictionary[targetID].SetActive(active);

        if (active)
        {
            CharactersCollision collision = connectObject_Dictionary[targetID].GetComponent<CharactersCollision>();
            if (collision != null) collision.OnInitial();//初始化                
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
        Animator animator = connectObject_Dictionary[targetID].GetComponent<Animator>();
        if (animator != null)
        {
            switch (animationType.GetType().Name)
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
                case "String":
                    animator.SetTrigger(anmationName);
                    break;
            }
        }
    }

    /// <summary>
    /// 連線受擊
    /// </summary>
    /// <param name="targetID">目標ID</param>
    /// <param name="position">位置</param>
    /// <param name="rotation">選轉</param>
    /// <param name="damage">受到傷害</param>
    /// <param name="isCritical">是否爆擊</param>
    public void OnConnectGetHit(int targetID, Vector3 position, Quaternion rotation, float damage, bool isCritical)
    {       
        CharactersCollision collision = connectObject_Dictionary[targetID].GetComponent<CharactersCollision>();
        if (collision != null)
        {
            collision.OnConnectOtherGetHit(position, rotation, damage, isCritical);
        }
    }

    /// <summary>
    /// 連線治療
    /// </summary>
    /// <param name="targetID">目標ID</param>
    /// <param name="heal">治療量</param>
    /// <param name="isCritical">是否爆擊</param>
    public void OnConnectGetHeal(int targetID, float heal, bool isCritical)
    {
        CharactersCollision collision = connectObject_Dictionary[targetID].GetComponent<CharactersCollision>();
        if (collision != null)
        {
            collision.OnConnectOtherGetHeal(heal, isCritical);
        }
    }
    #endregion
}
