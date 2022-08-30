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
    public List<AttackMode> AttackMode_List = new List<AttackMode>();//紀錄所有攻擊行為    

    Dictionary<int, GameObject> connectObject_Dictionary = new Dictionary<int, GameObject>();//記錄所有連線物件

    //出生點
    Transform[] enemySoldiers1_Stage1Point;//敵人士兵1_階段1出生點
    Transform[] enemySoldiers2_Stage1Point;//敵人士兵2_階段1出生點
    Transform guardBoss_Stage2Point;//城門首衛Boss_階段2出生點
    Transform[] enemySoldiers1_Stage3Point;//敵人士兵1_階段3出生點
    Transform[] enemySoldiers2_Stage3Point;//敵人士兵2_階段3出生點
    Transform[] enemySoldiers3_Stage3Point;//敵人士兵3_階段3出生點

    //任務
    string[] taskText;//各階段任務文字
    int taskStage;//目前任務階段
    public int[] taskKillNumber;//各階段任務所需擊殺數
    public int KillEnemyNumber;//已擊殺怪物數量

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
        /*//場景物件掛上小地圖點點
        GameObject stageObject = GameObject.Find("StageObjects");        
        Transform[] allStageObject = stageObject.GetComponentsInChildren<Transform>();
        foreach (var item in allStageObject)
        {
            if (item.GetComponent<BoxCollider>()) OnSetMiniMapPoint(item, loadPath.miniMapMatirial_Object);
        }*/

        int number = 0;

        //玩家腳色
        number = objectHandle.OnCreateObject(loadPath.allPlayerCharacters[GameDataManagement.Instance.selectRoleNumber]);//產生至物件池
        objectNumber_Dictionary.Add("playerNumbering", number);//添加至紀錄中
        GameObject player = OnRequestOpenObject(OnGetObjectNumber("playerNumbering"), loadPath.allPlayerCharacters[GameDataManagement.Instance.selectRoleNumber]);//開啟物件
        player.transform.position = new Vector3(227.5f, -23.6f, -23.5f);        
        player.transform.rotation = Quaternion.Euler(0, -60, 0);//設定選轉
        OnSetMiniMapPoint(player.transform, loadPath.miniMapMatirial_Player);//設定小地圖點點           

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

        //守衛Boss物件
        number = objectHandle.OnCreateObject(loadPath.guardBossAttack_1);//攻擊1物件
        objectNumber_Dictionary.Add("guardBossAttack_1", number);//添加至紀錄中

        //敵人士兵2物件
        number = objectHandle.OnCreateObject(loadPath.enemySoldier2Attack_Arrow);//弓箭物件
        objectNumber_Dictionary.Add("enemySoldier2Attack_Arrow", number);//添加至紀錄中

        #region 敵人出生點
        //敵人士兵1_階段1出生點
        enemySoldiers1_Stage1Point = new Transform[GameObject.Find("EnemySoldiers1_Stage1Point").transform.childCount];
        for (int i = 0; i < GameObject.Find("EnemySoldiers1_Stage1Point").transform.childCount; i++)
        {
            enemySoldiers1_Stage1Point[i] = GameObject.Find("EnemySoldiers1_Stage1Point").transform.GetChild(i);        
        }

        //敵人士兵2_階段1出生點
        enemySoldiers2_Stage1Point = new Transform[GameObject.Find("EnemySoldiers2_Stage1Point").transform.childCount];
        for (int i = 0; i < GameObject.Find("EnemySoldiers2_Stage1Point").transform.childCount; i++)
        {
            enemySoldiers2_Stage1Point[i] = GameObject.Find("EnemySoldiers2_Stage1Point").transform.GetChild(i);
        }

        //城門首衛Boss_階段2出生點
        guardBoss_Stage2Point = GameObject.Find("GuardBoss_Stage2Point").transform.GetChild(0);

        //敵人士兵1_階段3出生點
        enemySoldiers1_Stage3Point = new Transform[GameObject.Find("EnemySoldiers1_Stage3Point").transform.childCount];
        for (int i = 0; i < GameObject.Find("EnemySoldiers1_Stage3Point").transform.childCount; i++)
        {
            enemySoldiers1_Stage3Point[i] = GameObject.Find("EnemySoldiers1_Stage3Point").transform.GetChild(i);
        }

        //敵人士兵2_階段3出生點
        enemySoldiers2_Stage3Point = new Transform[GameObject.Find("EnemySoldiers2_Stage3Point").transform.childCount];
        for (int i = 0; i < GameObject.Find("EnemySoldiers2_Stage3Point").transform.childCount; i++)
        {
            enemySoldiers2_Stage3Point[i] = GameObject.Find("EnemySoldiers2_Stage3Point").transform.GetChild(i);
        }

        //敵人士兵3_階段3出生點
        enemySoldiers3_Stage3Point = new Transform[GameObject.Find("EnemySoldiers3_Stage3Point").transform.childCount];
        for (int i = 0; i < GameObject.Find("EnemySoldiers3_Stage3Point").transform.childCount; i++)
        {
            enemySoldiers3_Stage3Point[i] = GameObject.Find("EnemySoldiers3_Stage3Point").transform.GetChild(i);
        }
        #endregion

        //創建敵人
        OnCreateEnemy();

        //任務
        taskText = new string[] { "擊倒該區域所有怪物", "擊倒城門守衛", "擊倒城內區域所有怪物" };//個階段任務文字
        //各階段任務所需擊殺數
        taskKillNumber = new int[] { enemySoldiers1_Stage1Point.Length + enemySoldiers2_Stage1Point.Length,//階段1
                                     1,//階段1
                                     enemySoldiers1_Stage3Point.Length + enemySoldiers2_Stage3Point.Length + enemySoldiers3_Stage3Point.Length};//階段3

        //任務提示
        StartCoroutine(OnTaskTipText(taskTipValue: taskText[taskStage].ToString()));

        //任務文字
        OnTaskText();
    }

    void Update()
    {
        //攻擊行為
        OnAttackBehavior();
    }

    #region 任務
    /// <summary>
    /// 創建敵人
    /// </summary>
    void OnCreateEnemy()
    {
        //關卡1
        if(GameDataManagement.Instance.selectLevelNumber == 0)
        {
            
        }

        //關卡2
        if (GameDataManagement.Instance.selectLevelNumber == 1)
        {
            //非連線 || 是房主
            if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
            {
                int number = 0;
                GameObject enemy = null;

                //判斷目前任務階段
                switch (taskStage)
                {
                    case 0://階段1
                        //產生敵人士兵1
                        number = objectHandle.OnCreateObject(loadPath.enemySoldier_1);//產生至物件池
                        objectNumber_Dictionary.Add("enemySoldier_1", number);////添加至紀錄中
                        for (int i = 0; i < enemySoldiers1_Stage1Point.Length; i++)                   
                        {
                            enemy = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_1"), loadPath.enemySoldier_1);//開啟物件
                            enemy.transform.position = enemySoldiers1_Stage1Point[i].position;//設定位置
                            enemy.transform.rotation = Quaternion.Euler(0, 90, 0);                            
                            enemy.tag = "EnemySoldier_1";//設定Tag判斷HP
                            //OnSetMiniMapPoint(enemy.transform, loadPath.miniMapMatirial_Enemy);//設定小地圖點點
                        }

                        //產生敵人士兵2
                        number = objectHandle.OnCreateObject(loadPath.enemySoldier_2);//產生至物件池
                        objectNumber_Dictionary.Add("enemySoldier_2", number);////添加至紀錄中
                        for (int i = 0; i < enemySoldiers2_Stage1Point.Length; i++)
                        {
                            enemy = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_2"), loadPath.enemySoldier_1);//開啟物件
                            enemy.transform.position = enemySoldiers2_Stage1Point[i].position;//設定位置
                            enemy.transform.rotation = Quaternion.Euler(0, 90, 0);
                            enemy.tag = "EnemySoldier_2";//設定Tag判斷HP
                            //OnSetMiniMapPoint(enemy.transform, loadPath.miniMapMatirial_Enemy);//設定小地圖點點
                        }
                        break;
                    case 1://階段2
                        //產生城門守衛Boss     
                        number = objectHandle.OnCreateObject(loadPath.guardBoss);//產生至物件池
                        objectNumber_Dictionary.Add("enemyGuardBoss", number);////添加至紀錄中
                        //產生城門守衛Boss
                        enemy = OnRequestOpenObject(OnGetObjectNumber("enemyGuardBoss"), loadPath.guardBoss);//開啟物件
                        enemy.transform.position = guardBoss_Stage2Point.position;//設定位置
                        enemy.transform.rotation = Quaternion.Euler(0, 90, 0);
                        enemy.tag = "GuardBoss";//設定Tag判斷HP
                        break;
                    case 2://階段3
                        //產生敵人士兵1
                        for (int i = 0; i < enemySoldiers1_Stage3Point.Length; i++)
                        {
                            enemy = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_1"), loadPath.enemySoldier_1);//開啟物件
                            enemy.transform.position = enemySoldiers1_Stage3Point[i].position;//設定位置
                            enemy.transform.rotation = Quaternion.Euler(0, 90, 0);
                            enemy.GetComponent<CharactersCollision>().OnInitial();//初始化
                            enemy.GetComponent<AI>().OnInitial();//初始化
                        }
                        //產生敵人士兵2
                        for (int i = 0; i < enemySoldiers2_Stage3Point.Length; i++)
                        {
                            enemy = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_2"), loadPath.enemySoldier_2);//開啟物件
                            enemy.transform.position = enemySoldiers2_Stage3Point[i].position;//設定位置
                            enemy.transform.rotation = Quaternion.Euler(0, 90, 0);
                            enemy.GetComponent<CharactersCollision>().OnInitial();//初始化
                            enemy.GetComponent<AI>().OnInitial();//初始化
                        }
                        //產生敵人士兵3
                        number = objectHandle.OnCreateObject(loadPath.enemySoldier_3);//產生至物件池
                        objectNumber_Dictionary.Add("enemySoldier_3", number);////添加至紀錄中
                        for (int i = 0; i < enemySoldiers3_Stage3Point.Length; i++)
                        {
                            enemy = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_3"), loadPath.enemySoldier_3);//開啟物件
                            enemy.transform.position = enemySoldiers3_Stage3Point[i].position;//設定位置
                            enemy.transform.rotation = Quaternion.Euler(0, 90, 0);
                            enemy.tag = "EnemySoldier_3";//設定Tag判斷HP
                            //OnSetMiniMapPoint(enemy.transform, loadPath.miniMapMatirial_Enemy);//設定小地圖點點
                        }
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 任務提示
    /// </summary>
    /// <param name="value">任務提示文字</param>
    /// <returns></returns>
    public IEnumerator OnTaskTipText(string taskTipValue)
    {
        yield return new WaitForSeconds(2);

        //設定提示文字(任務提示)
        GameSceneUI.Instance.OnSetTip(tip: taskTipValue, showTime: 5);
    }

    /// <summary>
    /// 任務文字
    /// </summary>
    /// <param name="taskValue">任務文字</param>
    public void OnTaskText()
    {
        //任務判定
        OnJudgeTask();

        //設定任務文字
        if (taskStage < taskKillNumber.Length)//未過關
        {
            GameSceneUI.Instance.OnSetTaskText(taskValue: taskText[taskStage] + "\n目標:" + KillEnemyNumber + "/" + taskKillNumber[taskStage]);
        }
    }

    /// <summary>
    /// 任務判定
    /// </summary>
    void OnJudgeTask()
    {
        //已擊殺怪物數量 >= 任務所需擊殺數
        if (KillEnemyNumber >= taskKillNumber[taskStage])
        {            
            taskStage += 1;//目前任務階段

            if (taskStage >= taskKillNumber.Length)//過關
            {
                StartCoroutine(OnTaskTipText(taskTipValue: "過關"));//任務提示   
                GameSceneUI.Instance.OnSetTaskText(taskValue: "過關");

                StartCoroutine(OnClearance());//過關
            }
            else//進入下階段
            {
                KillEnemyNumber = 0;//已擊殺怪物數量                                
                StartCoroutine(OnTaskTipText(taskTipValue: taskText[taskStage]));//任務提示   
                GameSceneUI.Instance.OnSetTaskText(taskValue: taskText[taskStage] + "\n目標:" + KillEnemyNumber + "/" + taskKillNumber[taskStage]);

                //創建敵人
                OnCreateEnemy();
            }               
        }    
    }
    
    /// <summary>
    /// 過關
    /// </summary>
    /// <returns></returns>
    IEnumerator OnClearance()
    {
        yield return new WaitForSeconds(3);

        //設定遊戲結束UI
        GameSceneUI.Instance.OnSetGameOverUI(clearance: true);
    }

    #endregion

    #region 一般    
    /// <summary>
    /// 攻擊行為
    /// </summary>
    void OnAttackBehavior()
    {
        for (int i = 0; i < AttackMode_List.Count; i++)
        {
            AttackMode_List[i].function.Invoke();            
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
    /// <param name="knockDirection">擊退方向</param>
    /// <param name="repel">擊退距離</param>
    /// <param name="attackerObjectID">攻擊者物件ID</param>
    public void OnConnectGetHit(int targetID, Vector3 position, Quaternion rotation, float damage, bool isCritical, int knockDirection, float repel, int attackerObjectID)
    {       
        CharactersCollision collision = connectObject_Dictionary[targetID].GetComponent<CharactersCollision>();
        if (collision != null)
        {
            GameObject attackObj = connectObject_Dictionary[attackerObjectID].gameObject;
            collision.OnConnectOtherGetHit(position, rotation, damage, isCritical, knockDirection, repel, attackObj);
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
