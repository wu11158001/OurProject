using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

/// <summary>
/// 開始場景管理
/// </summary>
public class StartSceneManagement : MonoBehaviour
{
    VideoPlayer videoPlayer;
    Canvas canvas;

    //提示文字
    Text tip_Text;
    float tip_Text_alpha;
    int glintControl;//閃爍控制

    void Start()
    {
        //影片
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.clip = Resources.Load<VideoClip>("Video/StartVideo");

        //UI
        canvas = GameObject.Find("StartScene_UI").GetComponent<Canvas>();
        canvas.enabled = false;
        tip_Text = GameObject.Find("Tip_Text").GetComponent<Text>();
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
                canvas.enabled = true;
            }
            else
            {
                canvas.enabled = false;
                StartCoroutine(LoadScene.OnLoadScene("LobbyScene"));
            }
        }
    }

    /// <summary>
    /// 提示文字閃爍控制
    /// </summary>
    void OnTipTextGlintControl()
    {
        tip_Text_alpha += glintControl * Time.deltaTime;
        if (tip_Text_alpha >= 1) glintControl = -1;
        if (tip_Text_alpha <= 0) glintControl = 1;
        Color col = tip_Text.color;
        col.a = tip_Text_alpha;
        tip_Text.color = col;
    }
}
