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
    GameData_LoadPath loadPath;
    VideoPlayer videoPlayer;

    [Header("開始畫面")]
    Transform startScreen;

    //提示文字
    Text tip_Text;
    float tip_Text_alpha;
    int glintControl;//閃爍控制
  

    void Start()
    {
        loadPath = GameDataManagement.Insrance.loadPath;

        //影片
        videoPlayer = Camera.main.GetComponent<VideoPlayer>();
        videoPlayer.clip = Resources.Load<VideoClip>(loadPath.startVideo);

        //開始畫面
        startScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "StartScreen");
        startScreen.gameObject.SetActive(false);
        tip_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Tip_Text");
    }

    void Update()
    {
        OnStopVideo();
        OnTipTextGlintControl();
    }

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
                StartCoroutine(LoadScene.Instance.OnLoadScene("GameScene"));
            }
        }
    }

    /// <summary>
    /// 提示文字閃爍控制
    /// </summary>
    void OnTipTextGlintControl()
    {
        if (tip_Text != null)
        {
            tip_Text_alpha += glintControl * Time.deltaTime;
            if (tip_Text_alpha >= 1) glintControl = -1;
            if (tip_Text_alpha <= 0) glintControl = 1;
            Color col = tip_Text.color;
            col.a = tip_Text_alpha;
            tip_Text.color = col;
        }
    }
}
