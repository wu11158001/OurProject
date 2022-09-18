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
    public Dictionary<int, GameObject> connectObject_Dictionary = new Dictionary<int, GameObject>();//記錄所有連線物件
    public List<AttackMode> AttackMode_List = new List<AttackMode>();//紀錄所有攻擊行為    
    public List<int> AllPlayerID_List = new List<int>();//紀錄連線玩家ID
    public GameObject thisPlayerObject;//本地玩家物件

    //敵人出生點
    Transform[] enemySoldiers1_Stage1Point;//敵人士兵1_階段1出生點
    Transform[] enemySoldiers2_Stage1Point;//敵人士兵2_階段1出生點
    Transform[] guardBoss_Stage2Point;//城門首衛Boss_階段2出生點
    Transform[] enemySoldiers1_Stage4Point;//敵人士兵1_階段3出生點
    Transform[] enemySoldiers2_Stage4Point;//敵人士兵2_階段3出生點
    Transform[] enemySoldiers3_Stage4Point;//敵人士兵3_階段3出生點
    Transform[] enemySoldiers2_Stage5Point;//敵人士兵2_階段4出生點
    Transform[] enemySoldiers3_Stage5Point;//敵人士兵3_階段4出生點

    //我方出生點
    Transform[] allianceSoldier1_Stage1Point;//我方士兵1_階段1出生點
    Transform[] allianceSoldier1_Stage4Point;//我方士兵1_階段4出生點

    //第2關出生點
    [SerializeField]Transform[] brithPoint_Level12;
    float level12CreateSoldierTime;//產生時間
    [SerializeField] float level12SoldierTime;//產生時間(計時器)

    //任務    
    public bool isGameOver;//是否遊戲結束
    public bool isVictory;//是否過關
    public string[] taskText;//各階段任務文字
    public string[] tipTaskText;//提示任務文字
    public int taskStage;//目前任務階段
    public int[] taskNeedNumber;//各階段任務所需數量
    public int taskNumber;//已完成任務數量
    GameObject strongholdStage3;//第3階段據點
    public GameObject[] strongholdStage4;//第4階段據點
    GameObject strongholdStage5;//第5階段據點
                                
    public bool isCreateBoss;//是否已創建Boss
    public int lifePlayerNumber;//生存的玩家數量    

    //可控制城門
    float gateSpeed;//城門移動速度
    bool[] stageGateOpen;//個階段城門開啟狀態
    GameObject stage1_Gate;//階段1城門

    public Color thiscolor;

    [Header("特別使用")]
    public GameObject BossTargetObject;//Boss追隨目標物件

    void Awake()
    {
        Time.timeScale = 1;

        if (gameSceneManagement != null)
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

        //可控制城門
        stageGateOpen = new bool[] { false };//個階段城門開啟狀態
        gateSpeed = 1;//城門移動速度
        stage1_Gate = GameObject.Find("Stage1_Gate");//階段1城門

        int number = 0;

        //玩家腳色
        number = objectHandle.OnCreateObject(loadPath.allPlayerCharacters[GameDataManagement.Instance.selectRoleNumber]);//產生至物件池
        objectNumber_Dictionary.Add("playerNumbering", number);//添加至紀錄中
        GameObject player = OnRequestOpenObject(OnGetObjectNumber("playerNumbering"), loadPath.allPlayerCharacters[GameDataManagement.Instance.selectRoleNumber]);//開啟物件
        OnSetMiniMapPoint(player.transform, loadPath.miniMapMatirial_Player);//設定小地圖點點
        
        if (!GameDataManagement.Instance.isConnect)//未連線位置
        {
            if (GameDataManagement.Instance.selectLevelNumber == 11)//第1關
            {
                player.transform.position = new Vector3(348f, -23.8f, -25f);
                player.transform.rotation = Quaternion.Euler(0, -85, 0);//設定選轉
            }
            if (GameDataManagement.Instance.selectLevelNumber == 12)//第2關
            {
                player.transform.position = new Vector3(17, -0.5f, -15f);
                player.transform.rotation = Quaternion.Euler(0, 270, 0);//設定選轉
            }
        }
        else//連線位置
        {
            lifePlayerNumber = PhotonNetwork.CurrentRoom.PlayerCount;//生存的玩家數量

            if (GameDataManagement.Instance.selectLevelNumber == 11)//第1關
            {
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    if (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.NickName)
                    {
                        player.transform.position = new Vector3(345, -23.9f, -27.8f + (i * 1.5f));
                        player.transform.rotation = Quaternion.Euler(0, -85, 0);//設定選轉
                    }
                }
            }
            if (GameDataManagement.Instance.selectLevelNumber == 12)//第2關
            {
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    if (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.NickName)
                    {
                        player.transform.position = new Vector3(17, -0.5f, -15 + (i * 1.5f));
                        player.transform.rotation = Quaternion.Euler(0, -60, 0);//設定選轉
                    }
                }
            }
        }

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

        //Boss物件
        number = objectHandle.OnCreateObject(loadPath.bossAttack1);//攻擊1物件
        objectNumber_Dictionary.Add("bossAttack1", number);//添加至紀錄中
        number = objectHandle.OnCreateObject(loadPath.bossAttack2);//攻擊2物件
        objectNumber_Dictionary.Add("bossAttack2", number);//添加至紀錄中
        number = objectHandle.OnCreateObject(loadPath.bossAttack3);//攻擊3物件
        objectNumber_Dictionary.Add("bossAttack3", number);//添加至紀錄中
        number = objectHandle.OnCreateObject(loadPath.bossAttack4);//攻擊4物件
        objectNumber_Dictionary.Add("bossAttack4", number);//添加至紀錄中

        #region 產生士兵
        //產生同盟士兵1
        number = objectHandle.OnCreateObject(loadPath.allianceSoldier_1);//產生至物件池
        objectNumber_Dictionary.Add("allianceSoldier_1", number);////添加至紀錄中

        //產生敵人士兵1
        number = objectHandle.OnCreateObject(loadPath.enemySoldier_1);//產生至物件池
        objectNumber_Dictionary.Add("enemySoldier_1", number);////添加至紀錄中

        //產生敵人士兵2
        number = objectHandle.OnCreateObject(loadPath.enemySoldier_2);//產生至物件池
        objectNumber_Dictionary.Add("enemySoldier_2", number);////添加至紀錄中

        //產生敵人士兵3
        number = objectHandle.OnCreateObject(loadPath.enemySoldier_3);//產生至物件池
        objectNumber_Dictionary.Add("enemySoldier_3", number);////添加至紀錄中

        //產生城門守衛Boss     
        number = objectHandle.OnCreateObject(loadPath.guardBoss);//產生至物件池
        objectNumber_Dictionary.Add("enemyGuardBoss", number);////添加至紀錄中
        #endregion

        #region 第1關
        if (GameDataManagement.Instance.selectLevelNumber == 11)
        {
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
            guardBoss_Stage2Point = new Transform[GameObject.Find("GuardBoss_Stage2Point").transform.childCount];
            for (int i = 0; i < GameObject.Find("GuardBoss_Stage2Point").transform.childCount; i++)
            {
                guardBoss_Stage2Point[i] = GameObject.Find("GuardBoss_Stage2Point").transform.GetChild(i);
            }
            //敵人士兵1_階段4出生點
            enemySoldiers1_Stage4Point = new Transform[GameObject.Find("EnemySoldiers1_Stage4Point").transform.childCount];
            for (int i = 0; i < GameObject.Find("EnemySoldiers1_Stage4Point").transform.childCount; i++)
            {
                enemySoldiers1_Stage4Point[i] = GameObject.Find("EnemySoldiers1_Stage4Point").transform.GetChild(i);
            }

            //敵人士兵2_階段4出生點
            enemySoldiers2_Stage4Point = new Transform[GameObject.Find("EnemySoldiers2_Stage4Point").transform.childCount];
            for (int i = 0; i < GameObject.Find("EnemySoldiers2_Stage4Point").transform.childCount; i++)
            {
                enemySoldiers2_Stage4Point[i] = GameObject.Find("EnemySoldiers2_Stage4Point").transform.GetChild(i);
            }

            //敵人士兵3_階段3出生點
            enemySoldiers3_Stage4Point = new Transform[GameObject.Find("EnemySoldiers3_Stage4Point").transform.childCount];
            for (int i = 0; i < GameObject.Find("EnemySoldiers3_Stage4Point").transform.childCount; i++)
            {
                enemySoldiers3_Stage4Point[i] = GameObject.Find("EnemySoldiers3_Stage4Point").transform.GetChild(i);
            }

            //我方士兵1_階段4出生點
            allianceSoldier1_Stage4Point = new Transform[GameObject.Find("AllianceSoldier1_Stage4Point").transform.childCount];
            for (int i = 0; i < GameObject.Find("AllianceSoldier1_Stage4Point").transform.childCount; i++)
            {
                allianceSoldier1_Stage4Point[i] = GameObject.Find("AllianceSoldier1_Stage4Point").transform.GetChild(i);
            }

            //敵人士兵2_階段4出生點
            enemySoldiers2_Stage5Point = new Transform[GameObject.Find("EnemySoldiers2_Stage5Point").transform.childCount];
            for (int i = 0; i < GameObject.Find("EnemySoldiers2_Stage5Point").transform.childCount; i++)
            {
                enemySoldiers2_Stage5Point[i] = GameObject.Find("EnemySoldiers2_Stage5Point").transform.GetChild(i);
            }

            //敵人士兵3_階段4出生點
            enemySoldiers3_Stage5Point = new Transform[GameObject.Find("EnemySoldiers3_Stage5Point").transform.childCount];
            for (int i = 0; i < GameObject.Find("EnemySoldiers3_Stage5Point").transform.childCount; i++)
            {
                enemySoldiers3_Stage5Point[i] = GameObject.Find("EnemySoldiers3_Stage5Point").transform.GetChild(i);
            }
            #endregion

            #region 我方同盟出生點
            allianceSoldier1_Stage1Point = new Transform[GameObject.Find("AllianceSoldier1_Stage1Point").transform.childCount];
            for (int i = 0; i < GameObject.Find("AllianceSoldier1_Stage1Point").transform.childCount; i++)
            {
                allianceSoldier1_Stage1Point[i] = GameObject.Find("AllianceSoldier1_Stage1Point").transform.GetChild(i);
            }
            #endregion

            //初始階段創建敵人
            OnInitialCreateEnemy();

            //任務
            taskNumber = -1;//已完成任務數量
            tipTaskText = new string[] { "前進戰場\n擊破該區據點", "擊倒城門守衛", "擊破城門機關", "進入城內\n擊破所有據點", "前進破壞龍族水晶" };//提示任務文字
            taskText = new string[] { "已擊破據點 : ", "已擊倒守衛 : ", "已擊破機關 : ", "已擊破據點 : ", "已破壞水晶 : " };//個階段任務文字
            //各階段任務所需擊殺數
            taskNeedNumber = new int[] { 2,//階段1
                                     guardBoss_Stage2Point.Length,//階段2
                                     1,//階段3
                                     3,//階段4
                                     1};//階段5

            //任務提示
            StartCoroutine(OnTaskTipText(taskTipValue: tipTaskText[taskStage].ToString()));

            //任務文字
            OnTaskText();

            //任務物件
            strongholdStage3 = GameObject.Find("Stronghold_Enemy3");//第3階段據點
            strongholdStage3.SetActive(false);
            for (int i = 0; i < strongholdStage4.Length; i++)//第4階段據點
            {
                strongholdStage4[i].SetActive(false);
            }
            strongholdStage5 = GameObject.Find("Stronghold_Enemy7");//第5階段據點
            strongholdStage5.SetActive(false);
        }
        #endregion

        #region 第2關
        if (GameDataManagement.Instance.selectLevelNumber == 12)
        {
            //非連線 || 是房主
            if (!GameDataManagement.Instance.isConnect || PhotonNetwork.IsMasterClient)
            {
                brithPoint_Level12 = new Transform[GameObject.Find("BrithPoint_Level12").transform.childCount];
                for (int i = 0; i < brithPoint_Level12.Length; i++)
                {
                    brithPoint_Level12[i] = GameObject.Find("BrithPoint_Level12").transform.GetChild(i);
                }

                level12CreateSoldierTime = 10;//產生時間
                level12SoldierTime = level12CreateSoldierTime;//產生時間(計時器)

                GameSceneManagement.Instance.OnCreateBoss();//產生boss
            }

            taskNumber = -1;//已完成任務數量
            tipTaskText = new string[] { "擊 殺 火 龍" };//提示任務文字
            taskText = new string[] { "擊 殺 火 龍" };//個階段任務文字
            //各階段任務所需擊殺數
            taskNeedNumber = new int[] { 1 };//階段1

            //任務提示
            StartCoroutine(OnTaskTipText(taskTipValue: tipTaskText[taskStage].ToString()));

            //任務文字
            OnTaskText();
        }
        #endregion
    }

    void Update()
    {
        OnAttackBehavior();//攻擊行為
        OnGate();//可控制城門
        OnLevel12CreateSoldier();//第2關產生士兵
    }

    #region 任務
    /// <summary>
    /// 第2關產生士兵
    /// </summary>
   void OnLevel12CreateSoldier()
    {
        if (GameDataManagement.Instance.selectLevelNumber == 12 && !isGameOver)
        {
            //非連線 || 是房主
            if (!GameDataManagement.Instance.isConnect || PhotonNetwork.IsMasterClient)
            {
                level12SoldierTime -= Time.deltaTime;//產生時間(計時器)

                if(level12SoldierTime <= 0)
                {
                    level12SoldierTime = level12CreateSoldierTime;

                    StartCoroutine(OnDelayCreateInitalSoldier("enemySoldier_1", loadPath.enemySoldier_1, brithPoint_Level12[0], "Enemy"));
                    StartCoroutine(OnDelayCreateInitalSoldier("enemySoldier_1", loadPath.enemySoldier_1, brithPoint_Level12[1], "Enemy"));
                    StartCoroutine(OnDelayCreateInitalSoldier("enemySoldier_1", loadPath.enemySoldier_1, brithPoint_Level12[2], "Enemy"));
                    StartCoroutine(OnDelayCreateInitalSoldier("enemySoldier_3", loadPath.enemySoldier_3, brithPoint_Level12[3], "Enemy"));
                    StartCoroutine(OnDelayCreateInitalSoldier("enemySoldier_3", loadPath.enemySoldier_3, brithPoint_Level12[4], "Enemy"));
                    
                }
            }
        }
    }

    /// <summary>
    /// 產生Boss
    /// </summary>
    public void OnCreateBoss()
    {
        //守衛Boss物件
        int number = objectHandle.OnCreateObject(loadPath.boss);//攻擊1物件
        objectNumber_Dictionary.Add("boss", number);//添加至紀錄中
        GameObject AIObject = OnRequestOpenObject(OnGetObjectNumber("boss"), loadPath.boss);//開啟物件
        AIObject.transform.position = new Vector3(4.8f, 2.5f, -3f);//設定位置
        //AIObject.transform.position = new Vector3(76f, -11f, -28f);//設定位置
        AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        AIObject.tag = "Enemy";//設定Tag
        AIObject.layer = LayerMask.NameToLayer("Boss");//設定Layer        
        //OnSetMiniMapPoint(AIObject.transform, loadPath.miniMapMatirial_TaskObject);//設定小地圖點點 
    }

    /// <summary>
    /// 可控制城門
    /// </summary>
    void OnGate()
    {
        if (taskStage == 3)//第3階段過關
        {
            //if (stage1_Gate.GetComponent<BoxCollider>().enabled) stage1_Gate.GetComponent<BoxCollider>().enabled = false;
            if (stage1_Gate.transform.position.y < -12)
            {
                //玩家在範圍內
                if (Physics.CheckSphere(stage1_Gate.transform.position, 20, 1 << LayerMask.NameToLayer("Player")))
                {
                    if (!stageGateOpen[0])
                    {
                        stage1_Gate.GetComponent<AudioSource>().Play();
                        stageGateOpen[0] = true;
                    }
                }                

                //階段1城門開啟
                if (stageGateOpen[0]) stage1_Gate.transform.position = stage1_Gate.transform.position + Vector3.up * gateSpeed * Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// 初始階段創建敵人/任務流程
    /// </summary>
    void OnInitialCreateEnemy()
    {
        //非連線 || 是房主
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
        {
            //關卡1
            if (GameDataManagement.Instance.selectLevelNumber == 11)
            {
                //int number = 0;
                //GameObject AIObject = null;

                //判斷目前任務階段
                switch (taskStage)
                {
                    case 0://階段1
                        // 產生同盟士兵1                                                    
                        for (int i = 0; i < allianceSoldier1_Stage1Point.Length; i++)
                        {
                            StartCoroutine(OnDelayCreateInitalSoldier("allianceSoldier_1", loadPath.allianceSoldier_1, allianceSoldier1_Stage1Point[i], "Alliance"));
                            /*AIObject = OnRequestOpenObject(OnGetObjectNumber("allianceSoldier_1"), loadPath.allianceSoldier_1);//開啟物件
                            AIObject.transform.position = allianceSoldier1_Stage1Point[i].position;//設定位置
                            AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            AIObject.tag = "Alliance";//設定Tag
                            AIObject.layer = LayerMask.NameToLayer("Alliance");//設定Layer
                            AIObject.GetComponent<CharactersCollision>().OnInitial();//初始化
                            AIObject.GetComponent<AI>().OnInitial();//初始化*/
                            //OnSetMiniMapPoint(AIObject.transform, loadPath.miniMapMatirial_OtherPlayer);//設定小地圖點點
                        }
                        //產生敵人士兵1
                        for (int i = 0; i < enemySoldiers1_Stage1Point.Length; i++)
                        {
                            StartCoroutine(OnDelayCreateInitalSoldier("enemySoldier_1", loadPath.enemySoldier_1, enemySoldiers1_Stage1Point[i], "Enemy"));
                            /*AIObject = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_1"), loadPath.enemySoldier_1);//開啟物件
                            AIObject.transform.position = enemySoldiers1_Stage1Point[i].position;//設定位置
                            AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            AIObject.tag = "Enemy";//設定Tag
                            AIObject.layer = LayerMask.NameToLayer("Enemy");//設定Layer
                            AIObject.GetComponent<CharactersCollision>().OnInitial();//初始化
                            AIObject.GetComponent<AI>().OnInitial();//初始化   */
                            //OnSetMiniMapPoint(AIObject.transform, loadPath.miniMapMatirial_Enemy);//設定小地圖點點
                        }
                        //產生敵人士兵2                      
                        for (int i = 0; i < enemySoldiers2_Stage1Point.Length; i++)
                        {
                            StartCoroutine(OnDelayCreateInitalSoldier("enemySoldier_2", loadPath.enemySoldier_2, enemySoldiers2_Stage1Point[i], "Enemy"));
                            /*AIObject = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_2"), loadPath.enemySoldier_2);//開啟物件
                            AIObject.transform.position = enemySoldiers2_Stage1Point[i].position;//設定位置
                            AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            AIObject.tag = "Enemy";//設定Tag
                            AIObject.layer = LayerMask.NameToLayer("Enemy");//設定Layer
                            AIObject.GetComponent<CharactersCollision>().OnInitial();//初始化
                            AIObject.GetComponent<AI>().OnInitial();//初始化*/
                           // OnSetMiniMapPoint(AIObject.transform, loadPath.miniMapMatirial_Enemy);//設定小地圖點點
                        }
                        break;
                    case 1://階段2                       
                           //產生城門守衛Boss
                        for (int i = 0; i < guardBoss_Stage2Point.Length; i++)
                        {
                            StartCoroutine(OnDelayCreateInitalSoldier("enemyGuardBoss", loadPath.guardBoss, guardBoss_Stage2Point[i], "Enemy"));
                            /*AIObject = OnRequestOpenObject(OnGetObjectNumber("enemyGuardBoss"), loadPath.guardBoss);//開啟物件
                            AIObject.transform.position = guardBoss_Stage2Point[i].position;//設定位置
                            AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            AIObject.tag = "Enemy";//設定Tag
                            AIObject.layer = LayerMask.NameToLayer("Enemy");//設定Layer
                            AIObject.GetComponent<CharactersCollision>().OnInitial();//初始化
                            AIObject.GetComponent<AI>().OnInitial();//初始化    */
                            //OnSetMiniMapPoint(AIObject.transform, loadPath.miniMapMatirial_TaskObject);//設定小地圖點點                            
                        }
                        break;
                    case 2://階段3 
                        //開啟機關
                        strongholdStage3.SetActive(true);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendObjectActive(strongholdStage3, true);
                        break;
                    case 3://階段4
                        for (int i = 0; i < strongholdStage4.Length; i++)//第4階段據點
                        {
                            strongholdStage4[i].SetActive(true);
                            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendObjectActive(strongholdStage4[i], true);
                        }
                        // 產生同盟士兵1                       
                        for (int i = 0; i < allianceSoldier1_Stage4Point.Length; i++)
                        {
                            StartCoroutine(OnDelayCreateInitalSoldier("allianceSoldier_1", loadPath.allianceSoldier_1, allianceSoldier1_Stage4Point[i], "Alliance"));
                            /*AIObject = OnRequestOpenObject(OnGetObjectNumber("allianceSoldier_1"), loadPath.allianceSoldier_1);//開啟物件
                            AIObject.transform.position = allianceSoldier1_Stage4Point[i].position;//設定位置
                            AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            AIObject.tag = "Alliance";//設定Tag
                            AIObject.layer = LayerMask.NameToLayer("Alliance");//設定Layer
                            AIObject.GetComponent<CharactersCollision>().OnInitial();//初始化
                            AIObject.GetComponent<AI>().OnInitial();//初始化*/
                           // OnSetMiniMapPoint(AIObject.transform, loadPath.miniMapMatirial_OtherPlayer);//設定小地圖點點
                        }
                        //產生敵人士兵1
                        for (int i = 0; i < enemySoldiers1_Stage4Point.Length; i++)
                        {
                            StartCoroutine(OnDelayCreateInitalSoldier("enemySoldier_1", loadPath.enemySoldier_1, enemySoldiers1_Stage4Point[i], "Enemy"));
                            /*AIObject = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_1"), loadPath.enemySoldier_1);//開啟物件
                            AIObject.transform.position = enemySoldiers1_Stage4Point[i].position;//設定位置
                            AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            AIObject.tag = "Enemy";//設定Tag
                            AIObject.layer = LayerMask.NameToLayer("Enemy");//設定Layer
                            AIObject.GetComponent<CharactersCollision>().OnInitial();//初始化
                            AIObject.GetComponent<AI>().OnInitial();//初始化   */
                            //OnSetMiniMapPoint(AIObject.transform, loadPath.miniMapMatirial_Enemy);//設定小地圖點點
                        }
                        //產生敵人士兵2                      
                        for (int i = 0; i < enemySoldiers2_Stage4Point.Length; i++)
                        {
                            StartCoroutine(OnDelayCreateInitalSoldier("enemySoldier_2", loadPath.enemySoldier_2, enemySoldiers2_Stage4Point[i], "Enemy"));
                            /*AIObject = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_2"), loadPath.enemySoldier_2);//開啟物件
                            AIObject.transform.position = enemySoldiers2_Stage4Point[i].position;//設定位置
                            AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            AIObject.tag = "Enemy";//設定Tag
                            AIObject.layer = LayerMask.NameToLayer("Enemy");//設定Layer
                            AIObject.GetComponent<CharactersCollision>().OnInitial();//初始化
                            AIObject.GetComponent<AI>().OnInitial();//初始化*/
                            //OnSetMiniMapPoint(AIObject.transform, loadPath.miniMapMatirial_Enemy);//設定小地圖點點
                        }
                        //產生敵人士兵3                      
                        for (int i = 0; i < enemySoldiers3_Stage4Point.Length; i++)
                        {
                            StartCoroutine(OnDelayCreateInitalSoldier("enemySoldier_3", loadPath.enemySoldier_3, enemySoldiers3_Stage4Point[i], "Enemy"));
                           /* AIObject = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_3"), loadPath.enemySoldier_3);//開啟物件
                            AIObject.transform.position = enemySoldiers3_Stage4Point[i].position;//設定位置
                            AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            AIObject.tag = "Enemy";//設定Tag
                            AIObject.layer = LayerMask.NameToLayer("Enemy");//設定Layer
                            AIObject.GetComponent<CharactersCollision>().OnInitial();//初始化
                            AIObject.GetComponent<AI>().OnInitial();//初始化*/
                           // OnSetMiniMapPoint(AIObject.transform, loadPath.miniMapMatirial_Enemy);//設定小地圖點點
                        }
                        break;
                    case 4://階段5 
                        //擊破水晶
                        strongholdStage5.SetActive(true);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendObjectActive(strongholdStage5, true);

                        //產生敵人士兵2                      
                        for (int i = 0; i < enemySoldiers2_Stage5Point.Length; i++)
                        {
                            StartCoroutine(OnDelayCreateInitalSoldier("enemySoldier_2", loadPath.enemySoldier_2, enemySoldiers2_Stage5Point[i], "Enemy"));
                            /*AIObject = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_2"), loadPath.enemySoldier_2);//開啟物件
                            AIObject.transform.position = enemySoldiers2_Stage5Point[i].position;//設定位置
                            AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            AIObject.tag = "Enemy";//設定Tag
                            AIObject.layer = LayerMask.NameToLayer("Enemy");//設定Layer
                            AIObject.GetComponent<CharactersCollision>().OnInitial();//初始化
                            AIObject.GetComponent<AI>().OnInitial();//初始化                                                   */
                        }

                        //產生敵人士兵3                      
                        for (int i = 0; i < enemySoldiers3_Stage5Point.Length; i++)
                        {
                            StartCoroutine(OnDelayCreateInitalSoldier("enemySoldier_3", loadPath.enemySoldier_3, enemySoldiers3_Stage5Point[i], "Enemy"));
                            /*AIObject = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_3"), loadPath.enemySoldier_3);//開啟物件
                            AIObject.transform.position = enemySoldiers3_Stage5Point[i].position;//設定位置
                            AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            AIObject.tag = "Enemy";//設定Tag
                            AIObject.layer = LayerMask.NameToLayer("Enemy");//設定Layer
                            AIObject.GetComponent<CharactersCollision>().OnInitial();//初始化
                            AIObject.GetComponent<AI>().OnInitial();//初始化 */                                                  
                        }
                        break;
                }
            }

            //關卡2
            if (GameDataManagement.Instance.selectLevelNumber == 12)
            {

            }
        }
    }

    /// <summary>
    /// 產生士兵
    /// </summary>
    /// <param name="createPoint"></param>
    /// <param name="objTag"></param>
    public void OnCreateSoldier(Transform createPoint, string objTag)
    {
        //非連線 || 是房主
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
        {
            #region 我方據點
            if (objTag == "Alliance")
            {
                //產生敵人士兵1
                for (int i = 0; i < 2; i++)
                {
                    StartCoroutine(OnDelayCreateSoldier_Alliance("allianceSoldier_1", loadPath.allianceSoldier_1, createPoint, objTag, i, UnityEngine.Random.Range(0.0f, 1.5f)));
                }
            }
            #endregion

            //判斷目前任務階段
            switch (taskStage)
            {
                case 0://階段1
                    #region 敵人據點
                    if (objTag == "Enemy")
                    {
                        //產生敵人士兵1
                        for (int i = 0; i < 3; i++)
                        {
                            StartCoroutine(OnDelayCreateSoldier_Enemy("enemySoldier_1", loadPath.enemySoldier_1, createPoint, objTag, i, UnityEngine.Random.Range(0.0f, 1.5f)));
                        }
                        //產生敵人士兵2
                        for (int j = 2; j < 4; j++)
                        {
                            StartCoroutine(OnDelayCreateSoldier_Enemy("enemySoldier_2", loadPath.enemySoldier_2, createPoint, objTag, j, UnityEngine.Random.Range(0.0f, 1.5f)));
                        }
                    }
                    #endregion                    
                    break;
                case 3://階段4
                    #region 敵人據點
                    if (objTag == "Enemy")
                    {
                        //產生敵人士兵1
                        for (int i = 0; i < 1; i++)
                        {
                            StartCoroutine(OnDelayCreateSoldier_Enemy("enemySoldier_1", loadPath.enemySoldier_1, createPoint, objTag, i, UnityEngine.Random.Range(0.0f, 1.5f)));
                        }
                        //產生敵人士兵2
                        for (int j = 1; j < 2; j++)
                        {
                            StartCoroutine(OnDelayCreateSoldier_Enemy("enemySoldier_2", loadPath.enemySoldier_2, createPoint, objTag, j, UnityEngine.Random.Range(0.0f, 1.5f)));
                        }
                        //產生敵人士兵3
                        for (int k = 2; k < 4; k++)
                        {
                            StartCoroutine(OnDelayCreateSoldier_Enemy("enemySoldier_3", loadPath.enemySoldier_3, createPoint, objTag, k, UnityEngine.Random.Range(0.0f, 1.5f)));
                        }
                    }
                    #endregion       
                    break;
                case 4://階段5
                    #region 敵人據點
                    if (objTag == "Enemy")
                    {
                        //產生敵人士兵1
                        for (int i = 0; i < 1; i++)
                        {
                            StartCoroutine(OnDelayCreateSoldier_Enemy("enemySoldier_1", loadPath.enemySoldier_1, createPoint, objTag, i, UnityEngine.Random.Range(0.0f, 1.5f)));
                        }
                        //產生敵人士兵2
                        for (int j = 1; j < 2; j++)
                        {
                            StartCoroutine(OnDelayCreateSoldier_Enemy("enemySoldier_2", loadPath.enemySoldier_2, createPoint, objTag, j, UnityEngine.Random.Range(0.0f, 1.5f)));
                        }
                        //產生敵人士兵3
                        for (int k = 2; k < 5; k++)
                        {
                            StartCoroutine(OnDelayCreateSoldier_Enemy("enemySoldier_3", loadPath.enemySoldier_3, createPoint, objTag, k, UnityEngine.Random.Range(0.0f, 1.5f)));
                        }
                    }
                    #endregion       
                    break;
            }
        }
    }

    /// <summary>
    /// 延遲產生士兵_我方
    /// </summary>
    /// <param name="soldierName">士兵名稱</param>
    /// <param name="soldierPath">士兵路徑</param>
    /// <param name="createPoint">產生據點</param>
    /// <param name="objTag">據點Tag</param>
    /// <param name="number">目前數量</param>
    /// <param name="waitTime">等待時間</param>
    /// <returns></returns>
    IEnumerator OnDelayCreateSoldier_Alliance(string soldierName, string soldierPath, Transform createPoint, string objTag, int number, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        GameObject AIObject = null;
        AIObject = OnRequestOpenObject(OnGetObjectNumber(soldierName), soldierPath);//開啟物件
        AIObject.transform.position = createPoint.position + createPoint.forward * (-2 + (number * 1.5f));//設定位置
        AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        AIObject.tag = objTag;//設定Tag
        AIObject.layer = LayerMask.NameToLayer(objTag);//設定Layer         
        AIObject.GetComponent<CharactersCollision>().OnInitial();//初始化
        AIObject.GetComponent<AI>().OnInitial();//初始化
    }

    /// <summary>
    /// 延遲產生士兵_敵方
    /// </summary>
    /// <param name="soldierName">士兵名稱</param>
    /// <param name="soldierPath">士兵路徑</param>
    /// <param name="createPoint">產生據點</param>
    /// <param name="objTag">據點Tag</param>
    /// <param name="number">目前數量</param>
    /// <param name="waitTime">等待時間</param>
    /// <returns></returns>
    IEnumerator OnDelayCreateSoldier_Enemy(string soldierName, string soldierPath, Transform createPoint, string objTag, int number, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        GameObject AIObject = null;
        AIObject = OnRequestOpenObject(OnGetObjectNumber(soldierName), soldierPath);//開啟物件
        //AIObject.transform.position = createPoint.position + createPoint.forward * (10 + (number * 2f));//設定位置
        //AIObject.transform.position = (createPoint.position + createPoint.forward * 10) + (createPoint.right * (number * 1.5f));//設定位置
        AIObject.transform.position = (createPoint.position + createPoint.forward * 10 ) + (createPoint.right * (number * -1.5f)) + (Vector3.up * 1);//設定位置
        AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        AIObject.tag = objTag;//設定Tag
        AIObject.layer = LayerMask.NameToLayer(objTag);//設定Layer         
        AIObject.GetComponent<CharactersCollision>().OnInitial();//初始化
        AIObject.GetComponent<AI>().OnInitial();//初始化
    }

    /// <summary>
    /// 延遲產生初始士兵
    /// </summary>
    /// <param name="soldierName">士兵名稱</param>
    /// <param name="soldierPath">士兵路徑</param>
    /// <param name="createPoint">產生點</param>
    /// <param name="objTag">ObjTag</param>
    /// <returns></returns>
    IEnumerator OnDelayCreateInitalSoldier(string soldierName, string soldierPath, Transform createPoint, string objTag)
    {
        yield return new WaitForSeconds(0.1f);

        GameObject AIObject = null;
        AIObject = OnRequestOpenObject(OnGetObjectNumber(soldierName), soldierPath);//開啟物件
        //AIObject.transform.position = createPoint.position + createPoint.forward * (10 + (number * 2f));//設定位置
        //AIObject.transform.position = (createPoint.position + createPoint.forward * 10) + (createPoint.right * (number * 1.5f));//設定位置
        AIObject.transform.position = createPoint.position;//設定位置
        AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        AIObject.tag = objTag;//設定Tag
        AIObject.layer = LayerMask.NameToLayer(objTag);//設定Layer         
        AIObject.GetComponent<CharactersCollision>().OnInitial();//初始化
        AIObject.GetComponent<AI>().OnInitial();//初始化
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
        taskNumber++;//已完成任務數量

        //任務判定
        OnJudgeTask();

        //設定任務文字
        if (taskStage < taskNeedNumber.Length)//未過關
        {
            GameSceneUI.Instance.OnSetTaskText(taskValue: taskText[taskStage] + "\n" + taskNumber + " / " + taskNeedNumber[taskStage]);
        }
    }

    /// <summary>
    /// 任務判定
    /// </summary>
    void OnJudgeTask()
    {
        //已擊殺怪物數量 >= 任務所需擊殺數
        if (taskNumber >= taskNeedNumber[taskStage])
        {
            taskStage += 1;//目前任務階段

            if (taskStage >= taskNeedNumber.Length)//過關
            {
                //StartCoroutine(OnTaskTipText(taskTipValue: "勝利"));//任務提示
                AI[] ais = GameObject.FindObjectsOfType<AI>();
                for (int i = 0; i < ais.Length; i++)
                {
                    ais[i].gameObject.SetActive(false);
                }

                
                GameSceneUI.Instance.OnSetGameResult(true, "勝 利");
                StartCoroutine(OnSetGameOver(true));//設定遊戲結束
            }
            else//進入下階段
            {
                taskNumber = 0;//已完成任務數量                                
                StartCoroutine(OnTaskTipText(taskTipValue: tipTaskText[taskStage]));//任務提示   
                GameSceneUI.Instance.OnSetTaskText(taskValue: taskText[taskStage] + "\n" + taskNumber + " / " + taskNeedNumber[taskStage]);

                //初始階段創建敵人
                OnInitialCreateEnemy();
            }
        }
    }

    /// <summary>
    /// 設定遊戲結束
    /// </summary>
    /// <param name="result">結果</param>
    /// <returns></returns>
    public IEnumerator OnSetGameOver(bool result)
    {
        //遊戲結束關閉物件
        GameSceneUI.Instance.OnGameOverCloseObject();

        //連線
        if(GameDataManagement.Instance.isConnect)
        {
            PhotonConnect.Instance.OnSendGameScoring(PhotonNetwork.NickName, GameSceneUI.Instance.MaxCombo, GameSceneUI.Instance.killNumber, GameSceneUI.Instance.accumulationDamage);
            if (PhotonNetwork.IsMasterClient) PhotonConnect.Instance.OnSendGameTime(GameSceneUI.Instance.playerGameTime);
        }

        yield return new WaitForSeconds(4);

        PhotonNetwork.AutomaticallySyncScene = true;//自動同步場景
        isGameOver = true;
        isVictory = result;//是否過關
        GameSceneUI.Instance.OnSetGameResult(false, "");

        //設定遊戲結束UI
        GameSceneUI.Instance.OnSetGameOverUI(clearance: result);
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
            if (obj.Key == objectNmae)
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
        obj.transform.localPosition = new Vector3(itemBoxCenter.x, -1, itemBoxCenter.z);

        /*//Size
        if (item.gameObject.layer != LayerMask.NameToLayer("Player") && item.gameObject.layer != LayerMask.NameToLayer("Enemy"))
        {
            Vector3 itemBoxSize = item.GetComponent<BoxCollider>().size;
            obj.transform.localScale = new Vector3(itemBoxSize.x, itemBoxSize.z, 1);
        }
        else obj.transform.localScale = new Vector3(5, 5, 5);*/

        if (item.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            obj.transform.localScale = new Vector3(12, 12, 12);//玩家
            obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, 0, obj.transform.localPosition.z);
        }
        else if (item.GetComponent<CharactersCollision>().isTaskObject)
        {
            obj.transform.localScale = new Vector3(25, 25, 25);//任務
            obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, -2, obj.transform.localPosition.z);
        }
        else
        {
            obj.transform.localScale = new Vector3(10, 10, 10);
            obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, -3, obj.transform.localPosition.z);
        }


        //選轉
        obj.transform.localEulerAngles = new Vector3(90, 0, 0);

        //材質球(顏色)
        obj.GetComponent<Renderer>().material = Resources.Load<Material>(materialPath);
    }
    #endregion

    #region 連線
    /// <summary>
    /// 設定Boss目標物件
    /// </summary>
    /// <param name="id"></param>
    public void OnSetBossTarget(int id)
    {
        BossTargetObject = connectObject_Dictionary[id];
    }

    /// <summary>
    /// 紀錄所有連線玩家
    /// </summary>
    /// <param name="id">物件ID</param>
    public void OnRecordConnectPlayer(int id)
    {
        AllPlayerID_List.Add(id);        
    }
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
        if (connectObject_Dictionary[targetID] != null)
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
    }

    /// <summary>
    /// 連線受擊
    /// </summary>
    /// <param name="targetID">目標ID</param>
    /// <param name="position">位置</param>
    /// <param name="rotation">選轉</param>
    /// <param name="damage">受到傷害</param>
    /// <param name="isCritical">是否爆擊</param>
    /// <param name="repel">擊退距離</param>
    /// <param name="attackerObjectID">攻擊者物件ID</param>
    /// <param name="attackerID">攻擊者ID</param>
    public void OnConnectGetHit(int targetID, Vector3 position, Quaternion rotation, float damage, bool isCritical, float repel, int attackerObjectID, int attackerID)
    {
        CharactersCollision collision = connectObject_Dictionary[targetID].GetComponent<CharactersCollision>();
        if (collision != null)
        {
            GameObject attackObj = connectObject_Dictionary[attackerObjectID].gameObject;
            GameObject attacker = connectObject_Dictionary[attackerID].gameObject;
            collision.OnConnectOtherGetHit(position, rotation, damage, isCritical, repel, attackObj, attacker);
        }
    }

    /// <summary>
    /// 連線據點受擊
    /// </summary>
    /// <param name="targetID">目標ID</param>
    /// <param name="damage">受到傷害</param>
    public void OnConnectStrongholdGetHit(int targetID, float damage)
    {
        Stronghold stronghold = connectObject_Dictionary[targetID].GetComponent<Stronghold>();

        if (stronghold != null)
        {
            stronghold.OnConnectGetHit(damage);
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

    /// <summary>
    /// 創建玩家名稱物件
    /// </summary>
    /// <param name="nickName">名稱</param>
    /// <param name="id">ID</param>
    public void OnCreatePlayerNameObject(string nickName, int id)
    {
        //名稱物件
        ObjectName objectName = Instantiate(Resources.Load<GameObject>(GameDataManagement.Instance.loadPath.objectName)).GetComponent<ObjectName>();//名稱物件
        objectName.OnSetName(connectObject_Dictionary[id].transform, nickName, thiscolor, 1.85f);        
    }
    #endregion
}
