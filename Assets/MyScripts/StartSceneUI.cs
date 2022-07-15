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
    VideoPlayer videoPlayer;

    [Header("開始畫面")]
    Transform startScreen;//startScreen UI控制        
    Text startTip_Text;//提示文字
    float startTip_Text_alpha;////提示文字Alpha
    int startTipGlintControl;//閃爍控制

    [Header("選擇腳色畫面")]
    Transform chooseRoleScreen;//chooseRoleScreen UI控制
    Button roleConfirm_Button;//腳色確定按鈕
    Transform roleSelectBackGround_Image;//腳色選擇背景
    GameObject roleSelect_Button;//腳色選擇按鈕
    Sprite[] roleSelect_Sprite;//腳色選擇圖片
    public List<Transform> roleSelectButton_List = new List<Transform>();//記錄所有腳色選擇按鈕

    void Awake()
    {
        if(startSceneUI != null)
        {
            Destroy(this);
            return;
        }
        startSceneUI = this;
    }

    void Start()
    {
        loadPath = GameDataManagement.Insrance.loadPath;

        OnStartScreenPrepare();
        OnChooseRoleScreenPrepare();        
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

        startScreen.gameObject.SetActive(false);
    }

    /// <summary>
    /// 選角畫面籌備
    /// </summary>
    void OnChooseRoleScreenPrepare()
    {
        //選擇腳色畫面
        chooseRoleScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "ChooseRoleScreen");////chooseRoleScreen UI控制        
        roleConfirm_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RoleConfirm_Button");//腳色確定按鈕
        roleConfirm_Button.onClick.AddListener(OnRoleConfirm);
        roleSelectBackGround_Image = ExtensionMethods.FindAnyChild<Transform>(transform, "RoleSelectBackGround_Image");//腳色選擇背景
        roleSelect_Button = Resources.Load<GameObject>(loadPath.roleSelect_Button);//腳色選擇按鈕
        roleSelect_Sprite = Resources.LoadAll<Sprite>(loadPath.roleSelect_Sprite);//腳色選擇圖片     

        //產生腳色選擇按鈕
        for (int i = 0; i < roleSelect_Sprite.Length; i++)
        {
            Transform role = Instantiate(roleSelect_Button).GetComponent<Transform>();
            role.SetParent(roleSelectBackGround_Image);           
            role.localPosition = new Vector3(10 + 160 * i, 0, 0);
            role.GetComponent<Image>().sprite = roleSelect_Sprite[i];

            roleSelectButton_List.Add(role);
        }

        chooseRoleScreen.gameObject.SetActive(false);
    }

    void Update()
    {
        OnStopVideo();
        OnTipTextGlintControl();
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
                startScreen.gameObject.SetActive(false);
                chooseRoleScreen.gameObject.SetActive(true);                
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
        chooseRoleScreen.gameObject.SetActive(false);
    }
    #endregion
}
