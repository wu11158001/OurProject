using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

/// <summary>
/// 開始場景管理
/// </summary>
public class StartSceneUI : MonoBehaviour
{
    static StartSceneUI startSceneUI;
    public static StartSceneUI Instance => startSceneUI;

    GameData_LoadPath loadPath;
    GameData_NumericalValue numericalValue;
    VideoPlayer videoPlayer;

    [Header("開始畫面")]
    Transform startScreen;//startScreen UI控制        
    Text startTip_Text;//提示文字
    float startTip_Text_alpha;////提示文字Alpha
    int startTipGlintControl;//閃爍控制

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

    [Header("關卡畫面")]
    Transform levelScreen;//levelScreen UI控制
    Button level_1_Button;//關卡1_按鈕

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
    }

    /// <summary>
    /// 初始狀態
    /// </summary>
    private void OnInital()
    {
        GameDataManagement.Instance.selectRoleNumber = 0;//選擇的腳色編號
    }

    /// <summary>
    /// 開始畫面籌備
    /// </summary>
    void OnStartScreenPrepare()
    {
        //影片
        videoPlayer = Camera.main.GetComponent<VideoPlayer>();
        videoPlayer.clip = Resources.Load<VideoClip>(loadPath.startVideo);

        //開始畫面
        startScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "StartScreen");//startScreen UI控制        
        startTip_Text = ExtensionMethods.FindAnyChild<Text>(transform, "StartTip_Text");//提示文字

        //畫面UI控制
        startScreen.gameObject.SetActive(false);        
    }

    /// <summary>
    /// 選角畫面籌備
    /// </summary>
    void OnChooseRoleScreenPrepare()
    {
        //選擇腳色畫面
        selectRoleScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "SelectRoleScreen");////SelectRoleScreen UI控制        
        roleConfirm_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RoleConfirm_Button");//腳色確定按鈕
        roleConfirm_Button.onClick.AddListener(OnRoleConfirm);
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
        levelScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "LevelScreen");//levelScreen UI控制
        level_1_Button = ExtensionMethods.FindAnyChild<Button>(transform, "Level_1_Button");//關卡1_按鈕
        level_1_Button.onClick.AddListener(() => { OnLoadGameScene(level: 1); });

        levelScreen.gameObject.SetActive(false);
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
                    startScreen.gameObject.SetActive(false);
                    selectRoleScreen.gameObject.SetActive(true);
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

    #region 選角畫面
    /// <summary>
    /// 腳色確定
    /// </summary>
    void OnRoleConfirm()
    {
        selectRoleScreen.gameObject.SetActive(false);
        levelScreen.gameObject.SetActive(true);
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
    /// 載入關卡
    /// </summary>
    /// <param name="level">選擇的關卡</param>
    void OnLoadGameScene(int level)
    {
        levelScreen.gameObject.SetActive(false);
        StartCoroutine(LoadScene.Instance.OnLoadScene("GameScene" + level));
    }
    #endregion
}
