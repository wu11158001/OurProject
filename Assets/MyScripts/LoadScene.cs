using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 載入場景
/// </summary>
public class LoadScene : MonoBehaviour
{
    static LoadScene loadScene;
    public static LoadScene Instance => loadScene;
    GameData_LoadPath loadPath;

    static AsyncOperation ao;//載入場景

    static Image background;//載入背景
    static Image loadBack_Image;//載入進度條(背景)
    static Image loadFront_Image;//載入進度條(進度)
    static float loadValue;//載入進度

    private void Awake()
    {
        if (loadScene != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        loadScene = this;
    }

    void Start()
    {
        loadPath = GameDataManagement.Insrance.loadPath;

        //載入背景
        background = ExtensionMethods.FindAnyChild<Image>(transform, "Background_Image");        
        background.enabled = false;

        //載入進度條(背景)
        loadBack_Image = ExtensionMethods.FindAnyChild<Image>(transform, "LoadBack_Image");
        loadBack_Image.enabled = false;

        //載入進度條(進度)
        loadFront_Image = ExtensionMethods.FindAnyChild<Image>(transform, "LoadFront_Image");
        loadFront_Image.enabled = false;

        StartCoroutine(OnLoadScene("StartScene"));
    }

    void Update()
    {
        OnLoading();        
    }
        
    /// <summary>
    /// 載入
    /// </summary>
    void OnLoading()
    {
        if (loadFront_Image.enabled && loadFront_Image.rectTransform.localScale.x < loadValue)
        {
            if (loadValue >= 1) loadValue = 1;
            loadFront_Image.rectTransform.localScale = new Vector3(loadFront_Image.rectTransform.localScale.x + Time.deltaTime, 1, 1);//進度條
        }
    }

    /// <summary>
    /// 載入場景
    /// </summary>
    /// <param name="path">場景名稱</param>
    /// <returns></returns>
    public IEnumerator OnLoadScene(string scene)
    {       
        //判斷場景(設定背景圖)
        switch (scene)
        {
            case "StartScene":
                background.sprite = Resources.Load<Sprite>(loadPath.LoadBackground_1);
                break;
            case "GameScene":
                background.sprite = Resources.Load<Sprite>(loadPath.LoadBackground_1);
                break;
        }   

        //開啟UI
        background.enabled = true;
        loadBack_Image.enabled = true;
        loadFront_Image.enabled = true;

        ao = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);//載入場景
        if (ao == null) yield break;//沒場景以下不做

        ao.allowSceneActivation = false;//載入完成自動切換

        while (!ao.isDone)//載入未完成
        {
            loadValue = 0.5f;
            if (ao.progress > 0.89f)//載入進度
            {
                loadValue = 0.7f;
                yield return new WaitForSeconds(0.3f);

                loadValue = 0.9f;
                yield return new WaitForSeconds(0.3f);

                loadValue = 1.0f;
                yield return new WaitForSeconds(0.3f);

                //關閉UI
                background.enabled = false;
                loadBack_Image.enabled = false;
                loadFront_Image.enabled = false;

                //進入場景
                ao.allowSceneActivation = true;
            }
            yield return 0;
        }
        yield return 0;
    }
}
