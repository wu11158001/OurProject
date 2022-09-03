using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 據點
/// </summary>
public class Stronghold : MonoBehaviourPunCallbacks
{
    public int id;
    GameData_NumericalValue NumericalValue;

    [Header("第幾階段生兵(0 == 階段1)")]
    public int stage;

    [Header("建築物名稱")]
    public string builidName;

    //生命值
   public float maxHp;
   public float hp;

    //產生士兵時間
    float createSoldierTime;//產生士兵時間
    float createTime;//產生士兵時間(計時器)

    //判斷
    bool isGetHit;//是否受攻擊

    private void Awake()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect)
        {
            id = GetComponent<PhotonView>().ViewID;
            GameSceneManagement.Instance.OnRecordConnectObject(id, gameObject);

        }
        else
        {
            Destroy(GetComponent<PhotonView>());
            Destroy(GetComponent<PhotonTransformView>());
        }
    }

    void Start()
    {
        NumericalValue = GameDataManagement.Instance.numericalValue;

        //生命值
        maxHp = NumericalValue.strongholdHp;
        hp = maxHp;

        //產生士兵時間
        createSoldierTime = 30;//產生士兵時間
        //createTime = createSoldierTime;//產生士兵時間(計時器)
    }

    void Update()
    {
        //非連線 || 是房主
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
        {
            if (hp > 0 && stage == GameSceneManagement.Instance.taskStage)
            {
                createTime -= Time.deltaTime;//產生士兵時間(計時器)
                if (createTime <= 0)
                {
                    GameSceneManagement.Instance.OnCreateSoldier(transform, gameObject.tag);
                    createTime = createSoldierTime;
                }
            }
        }     
    }

    /// <summary>
    /// 受到攻擊
    /// </summary>
    /// <param name="attackerLayer">攻擊者layer</param>
    /// <param name="damage">受到傷害</param>
    public void OnGetHit(string attackerLayer, float damage)
    {
        if (gameObject.tag == "Enemy" && attackerLayer == "Player")
        {
            isGetHit = true;//是否受攻擊

            hp -= damage;

            //連線
            if (GameDataManagement.Instance.isConnect)
            {
                PhotonConnect.Instance.OnSendStrongholdGetHit(id, damage);
            }

            //設定生命條
            GameSceneUI.Instance.OnSetEnemyLifeBarValue(builidName, hp / maxHp);
            GameSceneUI.Instance.SetEnemyLifeBarActive = true;

            if (hp <= 0)
            {
                hp = 0;

                //任務
                GameSceneManagement.Instance.OnTaskText();//任務文字                

                //連線
                if (GameDataManagement.Instance.isConnect && PhotonNetwork.IsMasterClient)
                {
                    PhotonConnect.Instance.OnSendRenewTask();//更新任務
                }

                //連線模式
                if (GameDataManagement.Instance.isConnect)
                {                    
                    PhotonConnect.Instance.OnSendObjectActive(gameObject, false);
                }

                GameSceneUI.Instance.OnSetTip($"{builidName}已擊破", 5);//設定提示文字
                GameSceneUI.Instance.SetEnemyLifeBarActive = false;//關閉生命條

                gameObject.SetActive(false);//關閉物件                
            }            
        }
    }

    /// <summary>
    /// 連線受擊
    /// </summary>
    /// <param name="damage">受到傷害</param>
    public void OnConnectGetHit(float damage)
    {
        hp -= damage;
        if (hp <= 0) hp = 0;
    }
}
