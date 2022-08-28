using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviourPunCallbacks
{
    static GameSceneUI gameSceneUI;
    public static GameSceneUI Instance => gameSceneUI;
       
    [Header("玩家生命條")]        
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
    bool isGameOver;//是否遊戲結束

    [Header("提示文字")]
    public Text tip_Text;//提示文字
    float tipTime;//文字顯示時間
    public Text task_Text;//任務文字

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
        //玩家生命條
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

        //其他
        tip_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Tip_Text");//提示文字
        tip_Text.color = new Color(tip_Text.color.r, tip_Text.color.g, tip_Text.color.b, tipTime);
        task_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Task_Text");//任務文字
    }
        
    void Update()
    {        
        OnPlayerLifeBarBehavior();//玩家生命條行為
        OnOptionsUI();//選項介面
        OnTipText();//提示文字
    }
  
    /// <summary>
    /// 設定遊戲結束UI
    /// </summary>
    /// <param name="clearance">是否過關</param>
    public void OnSetGameOverUI(bool clearance)
    {
        isGameOver = true;//遊戲結束

        //開啟選項
        if (isOptions)
        {
            isOptions = false;
            options.gameObject.SetActive(isOptions);
        }

        //開啟遊戲結束UI
        gameOver.gameObject.SetActive(isGameOver);

        //結果文字
        if (clearance) gameOverResult_Text.text = "過關";
        else gameOverResult_Text.text = "失敗";

        //顯示滑鼠
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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

            //顯示滑鼠
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;            
        }

        if(isOptions)
        {
            //音量
            GameDataManagement.Instance.musicVolume = volume_Scrollbar.value;
            audioSource.volume = GameDataManagement.Instance.musicVolume;
        }
    }

    /// <summary>
    /// 離開遊戲
    /// </summary>
    void OnLeaveGame()
    {
        isOptions = false;
        options.gameObject.SetActive(isOptions);

        //連線
        if (GameDataManagement.Instance.isConnect)
        {
            if (PhotonNetwork.IsMasterClient)
            {               
                PhotonConnect.Instance.OnSendGameTip("室長: " + PhotonNetwork.NickName + " 離開遊戲\n《遊戲結束...》");
            }
            else PhotonConnect.Instance.OnSendGameTip("玩家: " + PhotonNetwork.NickName + " 離開遊戲");
        }
        
        StartCoroutine(LoadScene.Instance.OnLoadScene("StartScene"));        
    }

    /// <summary>
    /// 繼續遊戲
    /// </summary>
    void OnContinueGame()
    {
        isOptions = false;
        options.gameObject.SetActive(isOptions);

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
}
