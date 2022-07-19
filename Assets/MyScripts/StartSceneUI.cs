using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// 開始場景管理
/// </summary>
public class StartSceneUI : MonoBehaviourPunCallbacks
{
    static StartSceneUI startSceneUI;
    public static StartSceneUI Instance => startSceneUI;

    GameData_LoadPath loadPath;
    GameData_NumericalValue numericalValue;
    VideoPlayer videoPlayer;

    [Header("開始畫面")]
    Image background_Image;//背景
    Transform startScreen;//startScreen UI控制        
    Text startTip_Text;//提示文字
    float startTip_Text_alpha;////提示文字Alpha
    int startTipGlintControl;//閃爍控制    

    [Header("選擇模式畫面")]
    Transform selectModeScreen;//SelectModeScreen UI控制    
    Transform modeSelectBackground_Image;//ModeSelectBackground_Image UI控制 
    Button modeSingle_Button;//單人模式按鈕
    Button modeConnect_Button;//連線模式按鈕
    Text modeTip_Text;//提示文字
    InputField nickName_InputField;//暱稱輸入框

    [Header("選擇腳色畫面")]
    Transform selectRoleScreen;//SelectRoleScreen UI控制
    Button roleConfirm_Button;//腳色確定按鈕    
    [Header("選擇腳色畫面/腳色選擇按鈕")]
    bool isSlideRoleButton;//是否滑動腳色按鈕    
    GameObject roleSelect_Button;//腳色選擇按鈕
    Sprite[] roleSelect_Sprite;//腳色選擇圖片
    Button roleSelectRight_Button;//腳色按鈕移動(右)
    Button roleSelectLeft_Button;//腳色按鈕移動(左)
    List<Transform> roleSelectButton_List = new List<Transform>();//記錄所有腳色選擇按鈕
    Image roleSelectBackGround_Image;//腳色選擇背景
    float roleSelectButtonSizeX;//腳色選擇按鈕SizeX
    float roleSelectButtonSpacing;//腳色選擇按鈕間距
    float roleSelectButtonSlideSpeed;//腳色滑動按鈕速度
    float mouseX;//Input MouseX    
    Image rolePicture_Image;//選擇的腳色(大圖)
    [Header("選擇腳色畫面/Buff")]
    int[] equipBuff;//裝備的Buff
    Text EquipBuff_Text;//裝備的Buff文字
    public BuffDrop buffBox_1;//Buff裝備框_1
    public BuffDrop buffBox_2;//Buff裝備框_2
    Button roleBack_Button;//返回按鈕

    [Header("關卡畫面")]
    Transform levelScreen;//LevelScreen UI控制
    Button level1_Button;//關卡1_按鈕
    int selectLevel;//選擇的關卡
    Button levelBack_Button;//返回按鈕

    [Header("連線模式畫面")]
    Transform conncetModeScreen;//ConncetModScreen UI控制
    Button createRoom_Button;//創建房間按鈕
    Button randomRoom_Button;//隨機房間按鈕
    Button specifyRoom_Button;//指定房間按鈕
    InputField room_InputField;//加入房間名稱輸入框
    Button connectModeBack_Button;//返回按鈕

    [Header("連線房間畫面")]
    Transform connectRoomScreen;//connectRoomScreen UI控制
    Button connectRoomBack_Button;//返回按鈕

    void Awake()
    {
        if(startSceneUI != null)
        {
            Destroy(this);
            return;
        }
        startSceneUI = this;
                
        loadPath = GameDataManagement.Instance.loadPath;
        numericalValue = GameDataManagement.Instance.numericalValue;
    }

    void Start()
    {       
        OnInital();
        OnStartScreenPrepare();
        OnChooseRoleScreenPrepare();
        OnLevelScreenPrepare();
        OnSelectModeScreenPrepare();
        OnConnectModeScreenPrepare();
        OnConnectRoomScreenPrepare();
    }

    /// <summary>
    /// 初始狀態
    /// </summary>
    private void OnInital()
    {
        GameDataManagement.Instance.selectRoleNumber = 0;//選擇的腳色編號
        selectLevel = 0;//選擇的關卡
    }

    /// <summary>
    /// 開始畫面籌備
    /// </summary>
    void OnStartScreenPrepare()
    {
        
        videoPlayer = Camera.main.GetComponent<VideoPlayer>();//影片
        videoPlayer.clip = Resources.Load<VideoClip>(loadPath.startVideo);        
        startScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "StartScreen");//startScreen UI控制        
        startTip_Text = ExtensionMethods.FindAnyChild<Text>(transform, "StartTip_Text");//提示文字
        background_Image = ExtensionMethods.FindAnyChild<Image>(transform, "Background_Image");//背景
        background_Image.gameObject.SetActive(false);

        //畫面UI控制
        startScreen.gameObject.SetActive(false);        
    }

    /// <summary>
    /// 選擇模式畫面籌備
    /// </summary>
    void OnSelectModeScreenPrepare()
    {
        selectModeScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "SelectModeScreen");//selectModeScreen UI控制
        modeSelectBackground_Image = ExtensionMethods.FindAnyChild<Transform>(transform, "ModeSelectBackground_Image");//ModeSelectBackground_Image UI控制                                                                                                    
        modeSingle_Button = ExtensionMethods.FindAnyChild<Button>(transform, "ModeSingle_Button");//單人模式按鈕
        modeSingle_Button.onClick.AddListener(OnIntoSelectRoleScreen);
        modeConnect_Button = ExtensionMethods.FindAnyChild<Button>(transform, "ModeConnect_Button");//連線模式按鈕
        modeConnect_Button.onClick.AddListener(OnOpenConnectModeScreen);
        modeTip_Text = ExtensionMethods.FindAnyChild<Text>(transform, "ModeTip_Text");//提示文字
        modeTip_Text.enabled = false;
        nickName_InputField = ExtensionMethods.FindAnyChild<InputField>(transform, "NickName_InputField");//暱稱輸入框

        selectModeScreen.gameObject.SetActive(false);
    }

    /// <summary>
    /// 選角畫面籌備
    /// </summary>
    void OnChooseRoleScreenPrepare()
    {
        //選擇腳色畫面
        selectRoleScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "SelectRoleScreen");////SelectRoleScreen UI控制        
        roleConfirm_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RoleConfirm_Button");//腳色確定按鈕
        roleConfirm_Button.onClick.AddListener(OnIntoLeaveScreen);
        roleBack_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RoleBack_Button");//返回按鈕
        roleBack_Button.onClick.AddListener(OnSelectRoleScreenBackButton);
        //選擇腳色畫面/腳色選擇按鈕
        roleSelectRight_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RoleSelectRight_Button");////腳色按鈕移動(右)
        roleSelectRight_Button.onClick.AddListener(delegate { OnRoleButtonMove(direction:1); });
        roleSelectLeft_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RoleSelectLeft_Button");//腳色按鈕移動(左)
        roleSelectLeft_Button.onClick.AddListener(delegate { OnRoleButtonMove(direction :-1); });
        roleSelect_Button = Resources.Load<GameObject>(loadPath.roleSelect_Button);//腳色選擇按鈕
        roleSelect_Sprite = Resources.LoadAll<Sprite>(loadPath.roleSelect_Sprite);//腳色選擇圖片
        roleSelectBackGround_Image = ExtensionMethods.FindAnyChild<Image>(transform, "RoleSelectBackGround_Image");//腳色選擇背景
        roleSelectButtonSizeX = roleSelect_Button.GetComponent<RectTransform>().rect.width;//腳色選擇按鈕SizeX
        roleSelectButtonSpacing = 20;//腳色選擇按鈕間距        

        //產生腳色選擇按鈕
        for (int i = 0; i < roleSelect_Sprite.Length; i++)
        {
            Transform roleButton = Instantiate(roleSelect_Button).GetComponent<Transform>();
            roleButton.name = "RoleButton" + i;
            roleButton.SetParent(roleSelectBackGround_Image.transform);           
            roleButton.localPosition = new Vector3(roleSelectButtonSpacing + ((roleSelectButtonSizeX + roleSelectButtonSpacing) * i), 0, 0);
            roleButton.GetComponent<Image>().sprite = roleSelect_Sprite[i];

            OnSetRoleButtonFunction(roleButton.GetComponent<Button>(), i);            
            roleSelectButton_List.Add(roleButton);
        }
        
        //選擇的腳色(大圖)
        rolePicture_Image = ExtensionMethods.FindAnyChild<Image>(transform, "RolePicture_Image");
        rolePicture_Image.sprite = roleSelectButton_List[0].GetComponent<Image>().sprite;

        equipBuff = new int[2];//裝備的Buff

        //Buff拖拉圖片
        for (int i = 1; i <= 6; i++)
        {
            BuffButtonDrag buff =  ExtensionMethods.FindAnyChild<BuffButtonDrag>(transform, "Buff_" + i + "_Image").GetComponent<BuffButtonDrag>();
            buff.buffAble = i;//給予Buff能力編號
        }       

        //Buff裝備框
        buffBox_1 = ExtensionMethods.FindAnyChild<BuffDrop>(transform, "SelectBuffBackground_1_Image").GetComponent<BuffDrop>();
        buffBox_1.buffBoxName = "buffBoxLeft";//判斷裝備的Buff
        buffBox_2 = ExtensionMethods.FindAnyChild<BuffDrop>(transform, "SelectBuffBackground_2_Image").GetComponent<BuffDrop>();
        buffBox_2.buffBoxName = "buffBoxRight";//判斷裝備的Buff

        //裝備的Buff文字
        EquipBuff_Text = ExtensionMethods.FindAnyChild<Text>(transform, "EquipBuff_Text").GetComponent<Text>();
        EquipBuff_Text.text = "";

        selectRoleScreen.gameObject.SetActive(false);
    }    

    /// <summary>
    /// 關卡畫面籌備
    /// </summary>
    void OnLevelScreenPrepare()
    {
        levelScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "LevelScreen");//LevelScreen UI控制 
        level1_Button = ExtensionMethods.FindAnyChild<Button>(transform, "Level1_Button");//關卡1_按鈕
        level1_Button.onClick.AddListener(() => { OnSelectLecel(level: 1); });
        levelBack_Button = ExtensionMethods.FindAnyChild<Button>(transform, "LevelBack_Button");//返回按鈕
        levelBack_Button.onClick.AddListener(OnLevelScreenBackButton);

        levelScreen.gameObject.SetActive(false);
    }   

    /// <summary>
    /// 連線模式籌備
    /// </summary>
    void OnConnectModeScreenPrepare()
    {        
        conncetModeScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "ConncetModeScreen");//ConncetModScreen UI控制
        createRoom_Button = ExtensionMethods.FindAnyChild<Button>(transform, "CreateRoom_Button");//創建房間按鈕
        createRoom_Button.onClick.AddListener(OnCreateRoom);
        randomRoom_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RandomRoom_Button");//隨機房間按鈕
        randomRoom_Button.onClick.AddListener(OnRandomRoom);
        specifyRoom_Button = ExtensionMethods.FindAnyChild<Button>(transform, "SpecifyRoom_Button");//指定房間按鈕
        specifyRoom_Button.onClick.AddListener(OnSpecifyRoom);
        room_InputField = ExtensionMethods.FindAnyChild<InputField>(transform, "Room_InputField");//加入房間名稱輸入框
        connectModeBack_Button = ExtensionMethods.FindAnyChild<Button>(transform, "ConnectModeBack_Button");//返回按鈕
        connectModeBack_Button.onClick.AddListener(OnConnectModeBackButton);

        conncetModeScreen.gameObject.SetActive(false);
    }

    /// <summary>
    /// 連線房間畫面籌備
    /// </summary>
    void OnConnectRoomScreenPrepare()
    {
        connectRoomScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "ConnectRoomScreen");//ConnectRoomScreen UI控制
        connectRoomBack_Button = ExtensionMethods.FindAnyChild<Button>(transform, "ConnectRoomBack_Button");//返回按鈕
        connectRoomBack_Button.onClick.AddListener(OnConnectRoomScreenBackButton);

        connectRoomScreen.gameObject.SetActive(false);
    }

    void Update()
    {
        OnStopVideo();
        OnTipTextGlintControl();
        OnSlideRoleButton();
    }

    #region 開始畫面  
    /// <summary>
    /// 影片停止
    /// </summary>
    void OnStopVideo()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Stop();
                startScreen.gameObject.SetActive(true);
            }
            else
            {
                if (startScreen.gameObject.activeSelf)
                {
                    background_Image.gameObject.SetActive(true);
                    selectModeScreen.gameObject.SetActive(true);
                    startScreen.gameObject.SetActive(false);                    
                }
            }
        }
    }

    /// <summary>
    /// 提示文字閃爍控制
    /// </summary>
    void OnTipTextGlintControl()
    {
        if (startTip_Text != null)
        {
            startTip_Text_alpha += startTipGlintControl * Time.deltaTime;
            if (startTip_Text_alpha >= 1) startTipGlintControl = -1;
            if (startTip_Text_alpha <= 0) startTipGlintControl = 1;
            Color col = startTip_Text.color;
            col.a = startTip_Text_alpha;
            startTip_Text.color = col;
        }
    }
    #endregion

    #region 選擇模式畫面   
    /// <summary>
    /// 進入選擇腳色畫面
    /// </summary>
    void OnIntoSelectRoleScreen()
    {
        selectRoleScreen.gameObject.SetActive(true);
        selectModeScreen.gameObject.SetActive(false);
    }

    /// <summary>
    /// 開啟連線模式畫面
    /// </summary>
    void OnOpenConnectModeScreen()
    {                
        modeTip_Text.enabled = true;
        modeSelectBackground_Image.gameObject.SetActive(false);

        PhotonNetwork.ConnectUsingSettings();//設定連線
    }
    /// <summary>
    /// 登入成功
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("photon連線成功");

        conncetModeScreen.gameObject.SetActive(true);
        modeSelectBackground_Image.gameObject.SetActive(true);
        modeTip_Text.enabled = false;
        selectModeScreen.gameObject.SetActive(false);

        if (nickName_InputField.text == "") PhotonNetwork.NickName = "訪客" + Random.Range(0, 1000);
        else PhotonNetwork.NickName = nickName_InputField.text;        
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    #endregion

    #region 選角畫面
    /// <summary>
    /// 進入關卡畫面
    /// </summary>
    void OnIntoLeaveScreen()
    {
        levelScreen.gameObject.SetActive(true);
        selectRoleScreen.gameObject.SetActive(false);               
    }

    /// <summary>
    /// 選擇腳色畫面返回按鈕
    /// </summary>
    void OnSelectRoleScreenBackButton()
    {
        selectModeScreen.gameObject.SetActive(true);
        selectRoleScreen.gameObject.SetActive(false);
    }

    /// <summary>
    /// 設定裝備的Buff
    /// </summary>
    /// <param name="boxName">Buff裝備框</param>
    /// <param name="buff">增加的Buff編號</param>
    public void OnSetEquipBuff(string boxName, int buff)
    {
        switch (boxName)
        {
            case "buffBoxLeft":
                equipBuff[0] = buff - 1;
                break;
            case "buffBoxRight":
                equipBuff[1] = buff - 1;
                break;
        }        

        //設定文字
        string text = "";
        if (equipBuff[0] >= 0) text = numericalValue.buffAbleString[equipBuff[0]] + "+" + numericalValue.buffAbleValue[equipBuff[0]] + "\n";
        else text = "";        
        if (equipBuff[1] >= 0) text = text + numericalValue.buffAbleString[equipBuff[1]] + "+" + numericalValue.buffAbleValue[equipBuff[1]];
        else text = text + "";

        EquipBuff_Text.text = text;

        GameDataManagement.Instance.equipBuff[0] = equipBuff[0];
        GameDataManagement.Instance.equipBuff[1] = equipBuff[1];
    }

    /// <summary>
    /// 設定腳色按鈕Function
    /// </summary>
    /// <param name="roleButton">腳色按鈕</param>
    /// <param name="i">編號</param>
    void OnSetRoleButtonFunction(Button roleButton, int i)
    {
        roleButton.onClick.AddListener(delegate { OnClickRoleButton(i); });
    }

    /// <summary>
    /// 點擊腳按鈕
    /// </summary>
    /// <param name="numbrt">編號(選擇的腳色)</param>
    void OnClickRoleButton(int number)
    {
        rolePicture_Image.sprite = roleSelectButton_List[number].GetComponent<Image>().sprite;//設定選擇的腳色(大圖)
        GameDataManagement.Instance.selectRoleNumber = number;
    }

    /// <summary>
    /// 腳色按鈕移動(左右按鈕)
    /// </summary>
    /// <param name="direction">移動方向</param>
    void OnRoleButtonMove(int direction)
    {
        for (int i = 0; i < roleSelectButton_List.Count; i++)
        {
            roleSelectButton_List[i].localPosition = new Vector3(roleSelectButton_List[i].localPosition.x + direction * (roleSelectButtonSizeX + (roleSelectButtonSpacing )), 0, 0);
        }
    }
    
    /// <summary>
    /// 滑動腳色按鈕
    /// </summary>
    void OnSlideRoleButton()
    {
        //判斷是否在背景上
        if (RectTransformUtility.RectangleContainsScreenPoint(roleSelectBackGround_Image.GetComponent<RectTransform>(), Input.mousePosition))
        {
            if (Input.GetMouseButtonDown(0))
            {
                isSlideRoleButton = true;
                roleSelectButtonSlideSpeed = 40;
            }
        }

        if (isSlideRoleButton)
        {
            mouseX = Input.GetAxis("Mouse X");
            if (Input.GetMouseButtonUp(0)) isSlideRoleButton = false;
        }
        else 
        {
            if(roleSelectButtonSlideSpeed > 0) roleSelectButtonSlideSpeed -= 80 * Time.deltaTime;//滑動速度衰退
            if (roleSelectButtonSlideSpeed <= 0) roleSelectButtonSlideSpeed = 0;
        }

        //滑動腳色按鈕
        for (int i = 0; i < roleSelectButton_List.Count; i++)
        {            
            roleSelectButton_List[i].localPosition = roleSelectButton_List[i].localPosition + Vector3.right * mouseX * roleSelectButtonSlideSpeed;

            //邊界設定
            if (roleSelectButton_List[i].localPosition.x <= -roleSelectButtonSizeX)
            {
                roleSelectButton_List[i].localPosition = new Vector3(((roleSelectButtonSizeX + roleSelectButtonSpacing) * (roleSelectButton_List.Count - 1) + ((roleSelectButtonSizeX + roleSelectButtonSpacing) + roleSelectButton_List[i].localPosition.x)), 0, 0);
            }
            if (roleSelectButton_List[i].localPosition.x >= (roleSelectButtonSizeX + roleSelectButtonSpacing) * (roleSelectButton_List.Count - 1))
            {
                roleSelectButton_List[i].localPosition = new Vector3((-roleSelectButtonSizeX - roleSelectButtonSpacing) + (roleSelectButton_List[i].localPosition.x - (roleSelectButtonSizeX + roleSelectButtonSpacing) * (roleSelectButton_List.Count - 1)), 0, 0);
            }
        }
    }
    #endregion

    #region 關卡畫面
    /// <summary>
    /// 選擇關卡
    /// </summary>
    /// <param name="level">選擇的關卡</param>
    void OnSelectLecel(int level)
    {
        selectLevel = level;//選擇的關卡
        levelScreen.gameObject.SetActive(false);
        StartCoroutine(LoadScene.Instance.OnLoadScene("GameScene" + selectLevel));
    }   

    /// <summary>
    /// 關卡畫面返回按鈕
    /// </summary>
    void OnLevelScreenBackButton()
    {
        selectRoleScreen.gameObject.SetActive(true);
        levelScreen.gameObject.SetActive(false);
    }
    #endregion

    #region 連線模式畫面   
    /// <summary>
    /// 返回按鈕
    /// </summary>
    void OnConnectModeBackButton()
    {
        selectModeScreen.gameObject.SetActive(true);
        conncetModeScreen.gameObject.SetActive(false);
        PhotonNetwork.Disconnect();
    }

    //離線觸發
    public override void OnDisconnected(DisconnectCause cause)
    {        
        Debug.Log("已離線");
    }
    /// <summary>
    /// 創建房間
    /// </summary>
    void OnCreateRoom()
    {
        //(防間名稱, 創建房間選擇(MaxPlayers = 最大人數), 大廳類型)
        PhotonNetwork.CreateRoom(PhotonNetwork.NickName, new Photon.Realtime.RoomOptions { MaxPlayers = 4 }, null);

        createRoom_Button.enabled = false;
        randomRoom_Button.enabled = false;
    }

    /// <summary>
    /// 隨機房間
    /// </summary>
    void OnRandomRoom()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    /// <summary>
    /// 指定房間
    /// </summary>
    void OnSpecifyRoom()
    {
        PhotonNetwork.JoinRoom(room_InputField.text);
    }

    /// <summary>
    /// 加入房間觸發
    /// </summary>
    public override void OnJoinedRoom()
    {
        Debug.Log("加入" + PhotonNetwork.CurrentRoom.Name + "房間");

        connectRoomScreen.gameObject.SetActive(true);
        conncetModeScreen.gameObject.SetActive(false);

    }
    #endregion

    #region 連線房間
    /// <summary>
    /// 連線房間返回按鈕
    /// </summary>
    void OnConnectRoomScreenBackButton()
    {
        selectModeScreen.gameObject.SetActive(true);
        connectRoomScreen.gameObject.SetActive(false);
        PhotonNetwork.Disconnect();
    }
    #endregion
}
