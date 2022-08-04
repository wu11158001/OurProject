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
    float playerHpProportion;
    Image playerLifeBarFront_Image;//生命條(前)
    Image playerLifeBarMid_Image;//生命條(中)

    [Header("選項")]
    Transform options;//Options UI控制
    public bool isOptions;//是否開起選項介面
    Button leaveGame_Button;//離開遊戲按鈕
    Button continueGame_Button;//繼續遊戲按鈕
    AudioSource audioSource;//音樂撥放器
    Scrollbar volume_Scrollbar;//音量ScrollBar

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
    }
        
    void Update()
    {        
        OnPlayerLifeBarBehavior();
        OnOptions();       
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
    void OnOptions()
    {
        //開啟選項介面
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isOptions = !isOptions; 
            options.gameObject.SetActive(isOptions);

            Cursor.visible = true;//顯示滑鼠
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
      
        StartCoroutine(LoadScene.Instance.OnLoadScene("StartScene"));        
    }

    /// <summary>
    /// 繼續遊戲
    /// </summary>
    void OnContinueGame()
    {
        isOptions = false;
        options.gameObject.SetActive(isOptions);
    }   
}
