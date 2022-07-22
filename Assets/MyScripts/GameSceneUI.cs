using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour
{
    static GameSceneUI gameSceneUI;
    public static GameSceneUI Instance => gameSceneUI;

    //玩家生命條
    float playerHpProportion;
    Image playerLifeBarFront_Image;//生命條(前)
    Image playerLifeBarMid_Image;//生命條(中)

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
    }
        
    void Update()
    {
        OnPlayerLifeBarBehavior();
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
}
