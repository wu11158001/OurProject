using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviourPunCallbacks
{
    static GameSceneUI gameSceneUI;
    public static GameSceneUI Instance => gameSceneUI;

    [Header("MiniMap")]
    Transform miniMap;//小地圖

    [Header("敵人生命條")]
    Transform enemyLifeBar;//EnemyLifeBar UI控制
    Image enemyLifeBarFront_Image;//生命條(前)
    Text enemyLifeBarName_Text;//敵人名稱

    [Header("玩家生命條")]
    Text playerNickName;//玩家暱稱
    Sprite[] allPlayerHeadStickers;//所有玩家頭像
    Image headStickers_Image;//玩家頭像
    Image playerLifeBarFront_Image;//生命條(前)
    Image playerLifeBarMid_Image;//生命條(中)
    float playerHpProportion;

    [Header("玩家Buff")]
    Sprite[] buffSprites;//Buff圖片
    Image playerBuff_1;//玩家裝備Buff1
    Image playerBuff_2;//玩家裝備Buff2

    [Header("選項")]
    Transform options;//Options UI控制    
    Button leaveGame_Button;//離開遊戲按鈕
    Button continueGame_Button;//繼續遊戲按鈕
    AudioSource audioSource;//音樂撥放器
    Scrollbar volume_Scrollbar;//音量ScrollBar
    public bool isOptions;//是否開起選項介面

    [Header("遊戲結束")]    
    Transform gameOver;//GameOver UI控制
    Text gameOverResult_Text;//遊戲結果文字
    Button backToStart_Button;//返回按鈕(確認按鈕)
    public bool isGameOver;//是否遊戲結束
    Transform gameResult;//GameResult UI控制
    Text gameResult_Text;//遊戲結果文字

    [Header("提示文字")]
    Image tipBackground_Image;//提示文字背景
    public Text tip_Text;//提示文字
    float tipTime;//文字顯示時間

    [Header("任務")]
    Transform task;//Task UI控制
    public Text task_Text;//任務文字

    [Header("分數")]
    Text killNumber_Text;//擊殺數文字
    public int killNumber;//擊殺數
    Text comboNumber_Text;//連擊數文字
    public int comboNumber;//連擊數
    float comboLifeTime;//連擊數文字時間
    Image comboBackground_Image;//連擊數文字背景

    [Header("計分板")]
    Text playGameTime_Text;//遊戲時間文字
    Text maxKillNumber_Text;//最大擊殺數文字
    Text maxCombolNumber_Text;//最大連擊數文字
    Text accumulationDamageNumber_Text;//累積傷害
    public int MaxCombo;//最大連擊數
    float playerGameTime;//遊戲時間
    public float accumulationDamage;//累積傷害

    [Header("連線玩家")]
    Transform connectUI;//Connect UI 控制
    Transform player1;//Player1 UI控制
    Transform player2;//Player2 UI控制
    Transform player3;//Player3 UI控制
    Text player1Name_Text;//玩家1暱稱
    Text player2Name_Text;//玩家2暱稱
    Text player3Name_Text;//玩家3暱稱
    Image player1LifeBarFront_Image;//玩家1血條
    Image player2LifeBarFront_Image;//玩家2血條
    Image player3LifeBarFront_Image;//玩家3血條
    Image[] allPlayerLifeBar;//所有玩家血條
    Image player1Head_Image;//玩家1頭像
    Image player2Head_Image;//玩家2頭像
    Image player3Head_Image;//玩家3頭像
    Image[] allPlayerHead_Image;//所有玩家頭像

    [Header("連線記分板")]
    Transform connectGameOver;//ConnectGameOver UI控制
    public Text connectGameOverResult_Text;//連線遊戲結果
    Transform player1_Over;//Player1_Over UI控制
    Transform player2_Over;//Player2_Over UI控制
    Transform player3_Over;//Player3_Over UI控制
    Transform player4_Over;//Player4_Over UI控制
    Image player1_OverHead;//Player1頭像
    Image player2_OverHead;//Player2頭像
    Image player3_OverHead;//Player3頭像
    Image player4_OverHead;//Player4頭像
    Image[] allPlayer1_OverHead;
    Text player1_OverNickName;//Player1暱稱
    Text player2_OverNickName;//Player2暱稱
    Text player3_OverNickName;//Player3暱稱
    Text player4_OverNickName;//Player4暱稱
    Text[] allPlayer4_OverNickName;
    Text player1_MaxKillNumber_Text;//Player1擊殺數
    Text player2_MaxKillNumber_Text;//Player2擊殺數
    Text player3_MaxKillNumber_Text;//Player3擊殺數
    Text player4_MaxKillNumber_Text;//Player4擊殺數
    Text[] allPlayer_MaxKillNumber_Text;
    Text player1_MaxCombolNumber_Text;//Player1最大連擊數
    Text player2_MaxCombolNumber_Text;//Player2最大連擊數
    Text player3_MaxCombolNumber_Text;//Player3最大連擊數
    Text player4_MaxCombolNumber_Text;//Player4最大連擊數
    Text[] allPlayer4_MaxCombolNumber_Text;
    Text player1_AccumulationDamageNumber_Text;//Player1累積傷害
    Text player2_AccumulationDamageNumber_Text;//Player2累積傷害
    Text player3_AccumulationDamageNumber_Text;//Player3累積傷害
    Text player4_AccumulationDamageNumber_Text;//Player4累積傷害
    Text[] allPlayer_AccumulationDamageNumber_Text;
    Button backToStartOver_Button;//回到start場景按鈕

    void Awake()
    {
        if(gameSceneUI != null)
        {
            Destroy(this);
            return;
        }
        gameSceneUI = this;
    }

    void Start()
    {
        //小地圖
        miniMap = ExtensionMethods.FindAnyChild<Transform>(transform, "MiniMap");//小地圖
        if (GameDataManagement.Instance.selectLevelNumber == 12) miniMap.gameObject.SetActive(false);

        //敵人生命條        
        enemyLifeBarName_Text = ExtensionMethods.FindAnyChild<Text>(transform, "EnemyLifeBarName_Text");//敵人名稱
        enemyLifeBarFront_Image = ExtensionMethods.FindAnyChild<Image>(transform, "EnemyLifeBarFront_Image");//生命條(前)
        enemyLifeBar = ExtensionMethods.FindAnyChild<Transform>(transform, "EnemyLifeBar");//EnemyLifeBar UI控制
        enemyLifeBar.gameObject.SetActive(false);

        //玩家生命條
        playerNickName = ExtensionMethods.FindAnyChild<Text>(transform, "PlayerNickName");//玩家暱稱
        if (GameDataManagement.Instance.isConnect)
        {
            playerNickName.text = PhotonNetwork.NickName;
        }
        else playerNickName.enabled = false;
        allPlayerHeadStickers = Resources.LoadAll<Sprite>("Sprites/PlayerHeadStickers");//所有玩家頭像
        headStickers_Image = ExtensionMethods.FindAnyChild<Image>(transform, "HeadStickers_Image");//玩家頭像
        headStickers_Image.sprite = allPlayerHeadStickers[GameDataManagement.Instance.selectRoleNumber];
        playerHpProportion = 1;
        playerLifeBarFront_Image = ExtensionMethods.FindAnyChild<Image>(transform, "PlayerLifeBarFront_Image");//生命條(前)
        playerLifeBarFront_Image.fillAmount = playerHpProportion;
        playerLifeBarMid_Image = ExtensionMethods.FindAnyChild<Image>(transform, "PlayerLifeBarMid_Image");//生命條(中)
        playerLifeBarMid_Image.fillAmount = playerHpProportion;

        //玩家Buff
        buffSprites = Resources.LoadAll<Sprite>("Sprites/Buff");
        playerBuff_1 = ExtensionMethods.FindAnyChild<Image>(transform, "PlayerBuff_1");//玩家裝備Buff1        
        playerBuff_2 = ExtensionMethods.FindAnyChild<Image>(transform, "PlayerBuff_2");//玩家裝備Buff2
        Image[] buffs = new Image[] { playerBuff_1, playerBuff_2 };
        for (int i = 0; i < GameDataManagement.Instance.equipBuff.Length; i++)
        {
            if (GameDataManagement.Instance.equipBuff[i] >= 0) buffs[i].sprite = buffSprites[GameDataManagement.Instance.equipBuff[i]];
            else
            {
                buffs[i].enabled = false;//關閉沒有裝備的buff圖片

                //交換位置
                if (GameDataManagement.Instance.equipBuff[0] < 0)
                {
                    Vector3 pos = buffs[0].rectTransform.localPosition;
                    buffs[1].rectTransform.localPosition = pos;
                }
            }
        }

        //選項
        options = ExtensionMethods.FindAnyChild<Transform>(transform, "Options");//Options UI控制
        options.gameObject.SetActive(false);
        leaveGame_Button = ExtensionMethods.FindAnyChild<Button>(transform, "LeaveGame_Button");//離開遊戲按鈕
        leaveGame_Button.onClick.AddListener(OnLeaveGame);
        continueGame_Button = ExtensionMethods.FindAnyChild<Button>(transform, "ContinueGame_Button");//繼續遊戲按鈕
        continueGame_Button.onClick.AddListener(OnContinueGame);
        audioSource = Camera.main.GetComponent<AudioSource>();//音樂撥放器        
        audioSource.volume = GameDataManagement.Instance.musicVolume;
        audioSource.Play();
        volume_Scrollbar = ExtensionMethods.FindAnyChild<Scrollbar>(transform, "Volume_Scrollbar");//音量ScrollBar
        volume_Scrollbar.value = GameDataManagement.Instance.musicVolume;

        //遊戲結束                
        gameOver = ExtensionMethods.FindAnyChild<Transform>(transform, "GameOver");//GameOver UI控制
        gameOver.gameObject.SetActive(false);
        gameOverResult_Text = ExtensionMethods.FindAnyChild<Text>(transform, "GameOverResult_Text");//遊戲結果文字
        backToStart_Button = ExtensionMethods.FindAnyChild<Button>(transform, "BackToStart_Button");//返回按鈕(確認按鈕)        
        backToStart_Button.onClick.AddListener(OnLeaveGame);
        gameResult = ExtensionMethods.FindAnyChild<Transform>(transform, "GameResult");//GameResult UI控制
        gameResult_Text = ExtensionMethods.FindAnyChild<Text>(transform, "GameResult_Text"); ;//遊戲結果文字
        gameResult.gameObject.SetActive(false);        

        //其他
        tipBackground_Image = ExtensionMethods.FindAnyChild<Image>(transform, "TipBackground_Image");//提示文字背景
        tipBackground_Image.color = new Color(tipBackground_Image.color.r, tipBackground_Image.color.g, tipBackground_Image.color.b, tipTime); 
        tip_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Tip_Text");//提示文字
        tip_Text.color = new Color(tip_Text.color.r, tip_Text.color.g, tip_Text.color.b, tipTime);

        //任務
        task = ExtensionMethods.FindAnyChild<Transform>(transform, "Task");//Task UI控制
        task_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Task_Text");//任務文字

        //分數
        killNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "KillNumber_Text");//擊殺數文字
        killNumber_Text.text = "擊 殺 數 : " + killNumber;
        comboNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "ComboNumber_Text");//連擊數文字
        comboNumber_Text.enabled = false;        
        comboBackground_Image = ExtensionMethods.FindAnyChild<Image>(transform, "ComboBackground_Image");//連擊數文字背景
        comboBackground_Image.enabled = false;

        //計分板
        playGameTime_Text = ExtensionMethods.FindAnyChild<Text>(transform, "PlayGameTime_Text");//遊戲時間文字
        maxKillNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "MaxKillNumber_Text");//最大擊殺數文字
        maxCombolNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "MaxCombolNumber_Text");//最大連擊數文字
        accumulationDamageNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "AccumulationDamageNumber_Text");//累積傷害文字

        //連線記分板
        connectGameOver = ExtensionMethods.FindAnyChild<Transform>(transform, "ConnectGameOver");//ConnectGameOver UI控制
        connectGameOver.gameObject.SetActive(false);
        connectGameOverResult_Text = ExtensionMethods.FindAnyChild<Text>(transform, "ConnectGameOverResult_Text");//連線遊戲結果
        connectGameOverResult_Text.text = " 勝 利 總 結 ";
        player1_Over = ExtensionMethods.FindAnyChild<Transform>(transform, "Player1_Over");//Player1_Over UI控制
        player2_Over = ExtensionMethods.FindAnyChild<Transform>(transform, "Player2_Over");//Player2_Over UI控制
        player3_Over = ExtensionMethods.FindAnyChild<Transform>(transform, "Player3_Over");//Player3_Over UI控制
        player4_Over = ExtensionMethods.FindAnyChild<Transform>(transform, "Player4_Over");//Player4_Over UI控制
        Transform[] allPlayerOver = new Transform[] { player1_Over, player2_Over, player3_Over, player4_Over };
        if (GameDataManagement.Instance.isConnect)
        {
            for (int i = PhotonNetwork.CurrentRoom.PlayerCount; i < allPlayerOver.Length; i++)
            {
                allPlayerOver[i].gameObject.SetActive(false);
            }
        }
        player1_OverHead = ExtensionMethods.FindAnyChild<Image>(transform, "Player1_OverHead");//Player1頭像
        player2_OverHead = ExtensionMethods.FindAnyChild<Image>(transform, "Player2_OverHead");//Player2頭像
        player3_OverHead = ExtensionMethods.FindAnyChild<Image>(transform, "Player3_OverHead");//Player3頭像
        player4_OverHead = ExtensionMethods.FindAnyChild<Image>(transform, "Player4_OverHead");//Player4頭像
        allPlayer1_OverHead = new Image[] { player1_OverHead, player2_OverHead, player3_OverHead, player4_OverHead };
        player1_OverNickName = ExtensionMethods.FindAnyChild<Text>(transform, "Player1_OverNickName");//Player1暱稱
        player2_OverNickName = ExtensionMethods.FindAnyChild<Text>(transform, "Player2_OverNickName");//Player2暱稱
        player3_OverNickName = ExtensionMethods.FindAnyChild<Text>(transform, "Player3_OverNickName");//Player3暱稱
        player4_OverNickName = ExtensionMethods.FindAnyChild<Text>(transform, "Player4_OverNickName");//Player4暱稱
        allPlayer4_OverNickName = new Text[] { player1_OverNickName, player2_OverNickName, player3_OverNickName, player4_OverNickName };
        player1_MaxKillNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player1_MaxKillNumber_Text");//Player1擊殺數
        player2_MaxKillNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player2_MaxKillNumber_Text");//Player2擊殺數
        player3_MaxKillNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player3_MaxKillNumber_Text");//Player3擊殺數
        player4_MaxKillNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player4_MaxKillNumber_Text");//Player4擊殺數
        allPlayer_MaxKillNumber_Text = new Text[] { player1_MaxKillNumber_Text, player2_MaxKillNumber_Text, player3_MaxKillNumber_Text, player4_MaxKillNumber_Text };
        player1_MaxCombolNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player1_MaxCombolNumber_Text");//Player1最大連擊數
        player2_MaxCombolNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player2_MaxCombolNumber_Text");//Player2最大連擊數
        player3_MaxCombolNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player3_MaxCombolNumber_Text");//Player3最大連擊數
        player4_MaxCombolNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player4_MaxCombolNumber_Text");//Player4最大連擊數
        allPlayer4_MaxCombolNumber_Text = new Text[] { player1_MaxCombolNumber_Text, player2_MaxCombolNumber_Text, player3_MaxCombolNumber_Text, player4_MaxCombolNumber_Text };
        player1_AccumulationDamageNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player1_AccumulationDamageNumber_Text");//Player1累積傷害
        player2_AccumulationDamageNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player2_AccumulationDamageNumber_Text");//Player2累積傷害
        player3_AccumulationDamageNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player3_AccumulationDamageNumber_Text");//Player3累積傷害
        player4_AccumulationDamageNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player4_AccumulationDamageNumber_Text");//Player4累積傷害
        allPlayer_AccumulationDamageNumber_Text = new Text[] { player1_AccumulationDamageNumber_Text, player2_AccumulationDamageNumber_Text, player3_AccumulationDamageNumber_Text, player4_AccumulationDamageNumber_Text };
        backToStartOver_Button = ExtensionMethods.FindAnyChild<Button>(transform, "BackToStartOver_Button");//回到start場景按鈕
        backToStartOver_Button.onClick.AddListener(OnLeaveGame);

        //連線玩家
        connectUI = ExtensionMethods.FindAnyChild<Transform>(transform, "ConnectUI");//ConnectUI 控制
        if (!GameDataManagement.Instance.isConnect) connectUI.gameObject.SetActive(false);
        if (GameDataManagement.Instance.isConnect)
        {
            connectUI.gameObject.SetActive(true);

            player1 = ExtensionMethods.FindAnyChild<Transform>(transform, "Player1");//Player1 UI控制
            player2 = ExtensionMethods.FindAnyChild<Transform>(transform, "Player2");//Player1 UI控制
            player3 = ExtensionMethods.FindAnyChild<Transform>(transform, "Player3");//Player1 UI控制            
            Transform[] allPlayerUI = new Transform[] { player1, player2, player3 };            
            for (int i = PhotonNetwork.CurrentRoom.PlayerCount - 1; i < allPlayerUI.Length; i++)
            {
                allPlayerUI[i].gameObject.SetActive(false);
            }

            player1Name_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player1Name_Text");//玩家3暱稱
            player2Name_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player2Name_Text");//玩家3暱稱
            player3Name_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player3Name_Text");//玩家3暱稱
            Text[] allPlayerNickName = new Text[] { player1Name_Text, player2Name_Text, player3Name_Text };
            player1LifeBarFront_Image = ExtensionMethods.FindAnyChild<Image>(transform, "Player1LifeBarFront_Image");//玩家1血條
            player2LifeBarFront_Image = ExtensionMethods.FindAnyChild<Image>(transform, "Player2LifeBarFront_Image");//玩家2血條
            player3LifeBarFront_Image = ExtensionMethods.FindAnyChild<Image>(transform, "Player3LifeBarFront_Image");//玩家3血條
            allPlayerLifeBar = new Image[] { player1LifeBarFront_Image, player2LifeBarFront_Image, player3LifeBarFront_Image };
            player1Head_Image = ExtensionMethods.FindAnyChild<Image>(transform, "Player1Head_Image");//玩家1頭像
            player2Head_Image = ExtensionMethods.FindAnyChild<Image>(transform, "Player2Head_Image");//玩家2頭像
            player3Head_Image = ExtensionMethods.FindAnyChild<Image>(transform, "Player3Head_Image");//玩家3頭像
            allPlayerHead_Image = new Image[] { player1Head_Image, player2Head_Image, player3Head_Image };//所有玩家頭像
            bool isTouchSelf = false;
            for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount - 1; i++)
            {
                allPlayerLifeBar[i].fillAmount = 1;                

                if (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.NickName)
                {
                    isTouchSelf = true;
                    allPlayerNickName[i].text = PhotonNetwork.PlayerList[i + 1].NickName;                    
                    allPlayerHead_Image[i].sprite = allPlayerHeadStickers[GameDataManagement.Instance.allConnectPlayerSelectRole[i + 1]];                    
                    continue;
                }

                if (isTouchSelf)
                {
                    allPlayerNickName[i].text = PhotonNetwork.PlayerList[i + 1].NickName;
                    allPlayerHead_Image[i].sprite = allPlayerHeadStickers[GameDataManagement.Instance.allConnectPlayerSelectRole[i + 1]];                    
                }
                else
                {
                    allPlayerNickName[i].text = PhotonNetwork.PlayerList[i].NickName;
                    allPlayerHead_Image[i].sprite = allPlayerHeadStickers[GameDataManagement.Instance.allConnectPlayerSelectRole[i]];                    
                }
            }           
        }        
    }
        
    void Update()
    {        
        OnPlayerLifeBarBehavior();//玩家生命條行為
        OnOptionsUI();//選項介面
        OnTipText();//提示文字
        OnComboLifeTime();//連擊數生存時間
        OnPlayGameTime();//遊戲時間
    }

    /// <summary>
    /// 設定其他玩家生命條
    /// </summary>
    /// <param name="nickName">玩家暱稱</param>
    /// <param name="hpProportion">生命比例</param>
    public void OnSetOtherPlayerLifeBar(string nickName, float hpProportion)
    {        
        int number = 0;
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            if (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.NickName)
            {                
                continue;
            }
            if (PhotonNetwork.PlayerList[i].NickName == nickName)
            {
                allPlayerLifeBar[number].fillAmount = hpProportion;
                return;
            }
            number++;
        }
    }

    /// <summary>
    /// 設定敵人生命條
    /// </summary>
    /// <param name="name">敵人名稱</param>
    /// <param name="value">生命比例</param>
    public void OnSetEnemyLifeBarValue(string name, float value)
    {      
        enemyLifeBarName_Text.text = name;//設定名稱
        enemyLifeBarFront_Image.fillAmount = value;//設定比例
    }

    /// <summary>
    /// 設定敵人生命條顯示
    /// </summary>
    public bool SetEnemyLifeBarActive { set { enemyLifeBar.gameObject.SetActive(value); } }

    /// <summary>
    /// 遊戲結束關閉物件
    /// </summary>
    public void OnGameOverCloseObject()
    {
        //關閉任務UI
        task.gameObject.SetActive(false);
    }

    /// <summary>
    /// 設定遊戲結束UI
    /// </summary>
    /// <param name="clearance">是否過關</param>
    public void OnSetGameOverUI(bool clearance)
    {
        isGameOver = true;//遊戲結束
        if(!clearance) Time.timeScale = 0;

        //開啟選項
        if (isOptions)
        {
            isOptions = false;
            options.gameObject.SetActive(isOptions);
        }        

        //顯示滑鼠
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //結果文字
        if (clearance) gameOverResult_Text.text = " 勝 利 總 結 ";
        else gameOverResult_Text.text = " 失 敗 總 結 ";

        //遊戲時間
        int minute = (int)playerGameTime / 60;
        int second = (int)playerGameTime % 60;
        playGameTime_Text.text = $"遊 戲 時 間 : {minute} 分 {second} 秒";

        //最大擊殺數
        maxKillNumber_Text.text = $"最 大 擊 殺 數 : {killNumber}";

        //最大連擊數
        maxCombolNumber_Text.text = $"最 大 連 擊 數 : {MaxCombo}";

        //累積傷害
        accumulationDamageNumber_Text.text = $"累 積 傷 害 : {((int)accumulationDamage).ToString()}";

        //開啟遊戲結束UI
        if(GameDataManagement.Instance.isConnect)
        {
            connectGameOver.gameObject.SetActive(true);
            //遊戲時間
            Text playGameTimeOver_Text = ExtensionMethods.FindAnyChild<Text>(transform, "PlayGameTimeOver_Text");//遊戲時間
            playGameTimeOver_Text.text = $"遊 戲 時 間 : {minute} 分 {second} 秒";            
        }
        else gameOver.gameObject.SetActive(isGameOver);
    }

    /// <summary>
    /// 連線遊戲結束
    /// </summary>
    /// <param name="playerList">玩家列表</param>
    /// <param name="nickName">暱稱</param>
    /// <param name="MaxCombo">最大連擊</param>
    /// <param name="killNumber">擊殺數</param>
    /// <param name="accumulationDamage">累積傷害</param>
    public void OnConnectGameOver(List<string> playerList, string nickName, int MaxCombo, int killNumber, float accumulationDamage)
    {
        if (!PhotonNetwork.IsMasterClient) backToStartOver_Button.gameObject.SetActive(false);

        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {                      
            if(nickName == playerList[i])
            {
                allPlayer1_OverHead[i].sprite = allPlayerHeadStickers[GameDataManagement.Instance.allConnectPlayerSelectRole[i]];
                allPlayer4_OverNickName[i].text = playerList[i];
                allPlayer_MaxKillNumber_Text[i].text = $"最 大 擊 殺 數 : {killNumber}";
                allPlayer4_MaxCombolNumber_Text[i].text = $"最 大 連 擊 數 : {MaxCombo}";
                allPlayer_AccumulationDamageNumber_Text[i].text = $"累 積 傷 害 : {((int)accumulationDamage).ToString()}";
            }
        }
    }

    /// <summary>
    /// 遊戲時間
    /// </summary>
    void OnPlayGameTime()
    {
        playerGameTime = Time.realtimeSinceStartup;//遊戲時間
    }

    /// <summary>
    /// 連擊數生存時間
    /// </summary>
    void OnComboLifeTime()
    {
        if (comboLifeTime > 0)
        {
            comboLifeTime -= Time.deltaTime; ;//連擊數文字時間

            if(comboNumber_Text.fontSize > 50) comboNumber_Text.fontSize -= 2;//文字縮小
            comboNumber_Text.color = new Color(comboNumber_Text.color.r, comboNumber_Text.color.g, comboNumber_Text.color.b, comboLifeTime);//文字
            comboBackground_Image.color = new Color(comboBackground_Image.color.r, comboBackground_Image.color.g, comboBackground_Image.color.b, comboLifeTime);//背景

            if (comboLifeTime <= 0)
            {
                comboNumber = 0;
                comboNumber_Text.enabled = false;
            }
        }
    }

    /// <summary>
    /// 設定連擊數
    /// </summary>
    public void OnSetComboNumber()
    {
        //文字
        comboLifeTime = 3;
        comboNumber++;//擊殺數
        comboNumber_Text.text = "連擊 : " + comboNumber;
        comboNumber_Text.enabled = true;
        comboNumber_Text.fontSize = 80;

        //背景
        comboBackground_Image.enabled = true;

        //最大連擊數
        if (MaxCombo < comboNumber) MaxCombo = comboNumber;
    }

    /// <summary>
    /// 設定擊殺數
    /// </summary>
    public void OnSetKillNumber()
    {
        killNumber++;//擊殺數
        killNumber_Text.text = "擊 殺 數 : " + killNumber;
    }

    /// <summary>
    /// 設定玩家生命比例
    /// </summary>
    public float SetPlayerHpProportion { set { playerHpProportion = value; } }

    /// <summary>
    /// 玩家生命條行為
    /// </summary>
    void OnPlayerLifeBarBehavior()
    {
        if (playerHpProportion <= 0) playerHpProportion = 0;//玩家生命比例

        playerLifeBarFront_Image.fillAmount = playerHpProportion;//生命條(前)
        if (playerLifeBarFront_Image.fillAmount < playerLifeBarMid_Image.fillAmount)//生命條(中)
        {
            playerLifeBarMid_Image.fillAmount -= 0.5f * Time.deltaTime;
        }
    }

    /// <summary>
    /// 選項介面
    /// </summary>
    void OnOptionsUI()
    {
        //開啟選項介面 && 遊戲結束不可按
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            isOptions = !isOptions;                                
            options.gameObject.SetActive(isOptions);

            if (isOptions)
            {
                HitNumber[] hitNumbers = GameObject.FindObjectsOfType<HitNumber>();
                for (int i = 0; i < hitNumbers.Length; i++)
                {
                    Destroy(hitNumbers[i].gameObject);
                }

                if (!GameDataManagement.Instance.isConnect) Time.timeScale = 0;              

                //顯示滑鼠                
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Time.timeScale = 1;
                
                //顯示滑鼠                
                Cursor.lockState = CursorLockMode.Locked;
            }

            Cursor.visible = isOptions;
        }

        if (isOptions)
        {    
            //音量
            GameDataManagement.Instance.musicVolume = volume_Scrollbar.value;
            audioSource.volume = GameDataManagement.Instance.musicVolume;;
        }  
    }

    /// <summary>
    /// 離開遊戲
    /// </summary>
    void OnLeaveGame()
    {
        isOptions = false;
        backToStartOver_Button.enabled = false;//關閉按鈕(避免連按)
        options.gameObject.SetActive(isOptions);
        Time.timeScale = 1;
        
        //連線
        if (GameDataManagement.Instance.isConnect)
        {
            if (PhotonNetwork.IsMasterClient)
            {               
                PhotonConnect.Instance.OnSendGameTip("室長 : " + PhotonNetwork.NickName + " 離開遊戲\n《遊戲結束...》");
            }
            else PhotonConnect.Instance.OnSendGameTip("玩家 : " + PhotonNetwork.NickName + " 離開遊戲");
        }

        //判定是否過關
        if (GameDataManagement.Instance.isConnect && GameSceneManagement.Instance.isVictory)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonConnect.Instance.OnSendIntoNexttLevel();
        }
        else StartCoroutine(LoadScene.Instance.OnLoadScene("StartScene"));
    }

    /// <summary>
    /// 繼續遊戲
    /// </summary>
    void OnContinueGame()
    {
        isOptions = false;
        options.gameObject.SetActive(isOptions);
        
        if(!GameDataManagement.Instance.isConnect)Time.timeScale = 1;

        //顯示滑鼠
        Cursor.visible = isOptions;
        Cursor.lockState = CursorLockMode.Locked;
    }   

    /// <summary>
    /// 設定提示文字
    /// </summary>
    /// <param name="tip">提示文字</param>
    /// <param name="showTime">提示時間</param>
    public void OnSetTip(string tip, float showTime)
    {
        tip_Text.text = tip;
        tipTime = showTime;
    }

    /// <summary>
    /// 提示文字
    /// </summary>
    void OnTipText()
    {
        if (tipTime > 0)
        {
            tipTime -= Time.deltaTime;

            tip_Text.color = new Color(tip_Text.color.r, tip_Text.color.g, tip_Text.color.b, tipTime);
            tipBackground_Image.color = new Color(tipBackground_Image.color.r, tipBackground_Image.color.g, tipBackground_Image.color.b, tipTime);
        }
    }

    /// <summary>
    /// 設定任務文字
    /// </summary>
    /// <param name="taskValue">任務文字</param>
    public void OnSetTaskText(string taskValue)
    {
        task_Text.text = taskValue;
    }

    /// <summary>
    /// 設定遊戲結果
    /// </summary>
    /// <param name="active">是否顯示</param>
    /// <param name="result">遊戲結果文字</param>
    public void OnSetGameResult(bool active, string result)
    {        
        gameResult.gameObject.SetActive(active);
        gameResult_Text.text = result;
    }
}
