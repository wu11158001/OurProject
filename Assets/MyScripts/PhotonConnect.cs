using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

/// <summary>
/// Photon連線
/// </summary>
public class PhotonConnect : MonoBehaviourPunCallbacks
{
    static PhotonConnect photonConnect;
    public static PhotonConnect Instance => photonConnect;

    void Awake()
    {
        if(photonConnect != null)
        {
            Destroy(this);
            return;
        }
        photonConnect = this;
        DontDestroyOnLoad(gameObject);
    }

    #region 連線
    /// <summary>
    /// 連線設定
    /// </summary>
    /// <param name="nickName">暱稱</param>
    public void OnConnectSetting(string nickName)
    {
        Debug.Log("準備連線");

        PhotonNetwork.ConnectUsingSettings();//設定連線
        PhotonNetwork.AutomaticallySyncScene = true;

        //設定暱稱
        if (nickName == "") PhotonNetwork.NickName = "訪客" + UnityEngine.Random.Range(0, 1000);
        else PhotonNetwork.NickName = nickName;
    }

    /// <summary>
    /// 登入成功觸發
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("連線成功");

        if (GameDataManagement.Instance.stage == GameDataManagement.Stage.開始場景)  StartSceneUI.Instance.OnIsConnected();
        GameDataManagement.Instance.isConnect = true;
    }

    /// <summary>
    /// 離線設定
    /// </summary>
    public void OnDisconnectSetting()
    {
        PhotonNetwork.Disconnect();
    }

    /// <summary>
    /// 離線觸發
    /// </summary>
    /// <param name="cause">離線原因</param>
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("已離線");       
        GameDataManagement.Instance.isConnect = false;
    }
    #endregion

    #region 房間
    /// <summary>
    /// 創建房間設定
    /// </summary>
    /// <param name="roomName"><房間名稱/param>
    public void OnCreateRoomSetting(string roomName)
    {
        if (roomName == "") roomName = PhotonNetwork.NickName;

        //(防間名稱, 創建房間選擇(MaxPlayers = 最大人數), 大廳類型)
        PhotonNetwork.CreateRoom(roomName, new Photon.Realtime.RoomOptions { MaxPlayers = 4 }, null);
    }

    /// <summary>
    /// 創建房間失敗觸發
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("創建房間失敗:" + returnCode + ":" + message);

        if(GameDataManagement.Instance.stage == GameDataManagement.Stage.開始場景) StartSceneUI.Instance.OnConnectModeSettingTip(tip: "創建房間失敗");
    }

    /// <summary>
    /// 加入隨機房間設定
    /// </summary>
    public void OnJoinRandomRoomRoomSetting()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    /// <summary>
    /// 加入隨機房間失敗觸發
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("目前沒有房間:" + returnCode + ":" + message);

        if (GameDataManagement.Instance.stage == GameDataManagement.Stage.開始場景) StartSceneUI.Instance.OnConnectModeSettingTip(tip: "目前沒有房間");
    }

    /// <summary>
    /// 加入指定房間設定
    /// </summary>
    /// <param name="roomName">欲加入房間名</param>
    public void OnJoinSpecifyRoomSetting(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    /// <summary>
    /// 加入房間失敗
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("找不到房間:" + returnCode + ":" + message);

        if (GameDataManagement.Instance.stage == GameDataManagement.Stage.開始場景)
        {
            if (returnCode == 32765) StartSceneUI.Instance.OnConnectModeSettingTip(tip: "房間已滿");
            if (returnCode == 32760) StartSceneUI.Instance.OnConnectModeSettingTip(tip: "找不到房間");
            else StartSceneUI.Instance.OnConnectModeSettingTip(tip: "找不到房間");
        }
    }

    /// <summary>
    /// 加入房間觸發
    /// </summary>
    public override void OnJoinedRoom()
    {
        Debug.Log("加入" + PhotonNetwork.CurrentRoom.Name + "房間");

        if (GameDataManagement.Instance.stage == GameDataManagement.Stage.開始場景) StartSceneUI.Instance.OnTidyConnectModeUI(roomName: PhotonNetwork.CurrentRoom.Name);
        OnReFreshPlayerNickName();
        OnSendRoomPlayerCharacters();
        if (PhotonNetwork.IsMasterClient) OnSendLevelNumber(GameDataManagement.Instance.selectLevelNumber);
    }

    /// <summary>
    /// 有玩家進入房間
    /// </summary>
    /// <param name="newPlayer">新玩家</param>

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        OnReFreshPlayerNickName();
        OnSendRoomPlayerCharacters();
        if (PhotonNetwork.IsMasterClient) OnSendLevelNumber(GameDataManagement.Instance.selectLevelNumber);
    }

    /// <summary>
    /// 有玩家離開房間
    /// </summary>
    /// <param name="otherPlayer">離開玩家</param>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        OnReFreshPlayerNickName();       
        OnSendRoomPlayerCharacters();
        if (PhotonNetwork.IsMasterClient) OnSendLevelNumber(GameDataManagement.Instance.selectLevelNumber);
    }

    /// <summary>
    /// 離開房間設定
    /// </summary>
    public void OnLeaveRoomSetting()
    {
        Debug.Log("準備離開房間");

        PhotonNetwork.LeaveRoom();
    }

    /// <summary>
    /// 離開房間觸發
    /// </summary>
    public override void OnLeftRoom()
    {
        Debug.Log("離開房間");        
    }

    /// <summary>
    /// 更新玩家暱稱
    /// </summary>    
    public void OnReFreshPlayerNickName()
    {
        //清空List
        List<string> playerList = new List<string>();

        //紀錄玩家暱稱
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            playerList.Add(PhotonNetwork.PlayerList[i].NickName);
        }

        if (GameDataManagement.Instance.stage == GameDataManagement.Stage.開始場景) StartSceneUI.Instance.OnRefreshRoomPlayerNickName(playerList, PhotonNetwork.NickName, PhotonNetwork.IsMasterClient);
    }

    /// <summary>
    /// 發送房間玩家腳色
    /// </summary>
    public void OnSendRoomPlayerCharacters()
    {        
        photonView.RPC("OnRefreshPlayerCharacters", RpcTarget.All, GameDataManagement.Instance.selectRoleNumber);
    }

    /// <summary>
    /// 刷新玩家腳色
    /// </summary>
    /// <param name="characters">腳色編號</param>
    /// <param name="info">傳送者訊息</param>
    [PunRPC]
    void OnRefreshPlayerCharacters(int characters, PhotonMessageInfo info)
    {        
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].NickName == info.Sender.NickName)
            {
                if (GameDataManagement.Instance.stage == GameDataManagement.Stage.開始場景) StartSceneUI.Instance.OnRefreshPlayerCharacters(i, characters);
                return;
            }
        }
    }

    /// <summary>
    /// 發送聊天訊息
    /// </summary>
    /// <param name="message">訊息</param>
    public void OnSendRoomChatMessage(string message)
    {
        photonView.RPC("OnRoomChatMessage", RpcTarget.All, message);
    }

    /// <summary>
    /// 房間訊息
    /// </summary>
    /// <param name="message">訊息</param>
    /// <param name="info">傳送者訊息</param>
    [PunRPC]
    void OnRoomChatMessage(string message, PhotonMessageInfo info)
    {
        if (GameDataManagement.Instance.stage == GameDataManagement.Stage.開始場景) StartSceneUI.Instance.OnGetRoomChatMessage(info.Sender.NickName + ":" + message);
    }

    /// <summary>
    /// 發送關卡編號
    /// </summary>
    /// <param name="level">選擇的關卡</param>
    public void OnSendLevelNumber(int level)
    {
        photonView.RPC("OnLevelNumber", RpcTarget.All, level);
    }

    /// <summary>
    /// 關卡編號
    /// </summary>
    /// <param name="level">選擇的關卡</param>
    /// <param name="info">傳送者訊息</param>
    [PunRPC]
    void OnLevelNumber(int level, PhotonMessageInfo info)
    {
        if (GameDataManagement.Instance.stage == GameDataManagement.Stage.開始場景) StartSceneUI.Instance.OnRoomLevelText(level);
    }

    /// <summary>
    /// 開始遊戲
    /// </summary>
    /// <param name="level">進入關卡編號</param>
    public bool OnStartGame(int level)
    {
        bool isStartGame = false;

        //2人以上開始
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            isStartGame = true;

            PhotonNetwork.LoadLevel("LevelScene" + level);
            PhotonNetwork.CurrentRoom.IsOpen = false;//關閉房間
        }
        else isStartGame = false;

        return isStartGame;
    }
    #endregion

    #region 遊戲中

    /// <summary>
    /// 發送遊戲提示文字
    /// </summary>
    /// <param name="nickName">發送者暱稱</param>
    public void OnSendGameTip(string nickName)
    {
        photonView.RPC("OnGameTip", RpcTarget.Others, nickName);
    }

    /// <summary>
    /// 遊戲提示文字
    /// </summary>
    /// <param name="nickName">發送者暱稱</param>
    /// <param name="info">傳送者訊息</param>
    [PunRPC]
    void OnGameTip(string nickName, PhotonMessageInfo info)
    {
        GameSceneUI.Instance.OnSetTip(nickName, 3);
    }

    /// <summary>
    /// 創建物件
    /// </summary>
    /// <param name="path">prefeb路徑</param>
    /// <returns></returns>
    public GameObject OnCreateObject(string path)
    {        
        return PhotonNetwork.Instantiate(path, Vector3.zero, Quaternion.identity);
    }

    /// <summary>
    /// 發送物件激活狀態
    /// </summary>
    /// <param name="obj">物件</param>
    /// <param name="active">激活狀態</param>
    public void OnSendObjectActive(GameObject obj, bool active)
    {        
        if (obj.GetComponent<PhotonView>())
        {
            int id = obj.GetComponent<PhotonView>().ViewID;
            photonView.RPC("OnObjectActive", RpcTarget.Others, id, active);
        }
    }

    /// <summary>
    /// 物件激活
    /// </summary>
    /// <param name="targetID">物件ID</param>
    /// <param name="active">激活狀態</param>
    /// <param name="info">傳送者訊息</param>
    [PunRPC]
    void OnObjectActive(int targetID, bool active, PhotonMessageInfo info)
    {
        GameSceneManagement.Instance.OnConnectObjectActive(targetID, active);
    }

    /// <summary>
    /// 發送受擊訊息
    /// </summary>
    /// <param name="targetID">目標ID</param>
    /// <param name="position">位置</param>
    /// <param name="rotation">選轉</param>
    /// <param name="damage">受到傷害</param>
    /// <param name="isCritical">是否爆擊</param>
    /// <param name="knockDirection">擊退方向</param>
    /// <param name="repel">擊退距離</param>
    /// <param name="attackerObjectID">攻擊者物件ID</param>
    public void OnSendGetHit(int targetID, Vector3 position, Quaternion rotation, float damage,bool isCritical, int knockDirection, float repel, int attackerObjectID)
    {
        photonView.RPC("OnGetHit", RpcTarget.Others, targetID, position, rotation, damage, isCritical, knockDirection, repel, attackerObjectID);
    }

    /// <summary>
    /// 受擊訊息
    /// </summary>
    /// <param name="targetID">目標ID</param>
    /// <param name="position">位置</param>
    /// <param name="rotation">選轉</param>
    /// <param name="damage">受到傷害</param>
    /// <param name="isCritical">是否爆擊</param>
    /// <param name="knockDirection">擊退方向</param>
    /// <param name="repel">擊退距離</param>
    /// <param name="attackerObjectID">攻擊者物件ID</param>
    [PunRPC]
    void OnGetHit(int targetID, Vector3 position, Quaternion rotation, float damage, bool isCritical, int knockDirection, float repel, int attackerObjectID)
    {
        GameSceneManagement.Instance.OnConnectGetHit(targetID, position, rotation, damage, isCritical, knockDirection, repel, attackerObjectID);
    }

    /// <summary>
    /// 發送受治療訊息
    /// </summary>
    /// <param name="targetID">目標ID</param>
    /// <param name="heal">治療量</param>
    /// <param name="isCritical">是否爆擊</param>
    public void OnSendGetHeal(int targetID, float heal, bool isCritical)
    {
        photonView.RPC("OnGetHeal", RpcTarget.Others, targetID, heal, isCritical);
    }

    /// <summary>
    /// 受治療訊息
    /// </summary>
    /// <param name="targetID">目標ID</param>
    /// <param name="heal">治療量</param>
    /// <param name="isCritical">是否爆擊</param>
    [PunRPC]
    void OnGetHeal(int targetID, float heal, bool isCritical)
    {
        GameSceneManagement.Instance.OnConnectGetHeal(targetID, heal, isCritical);
    }

    /// <summary>
    /// 發送動畫訊息_Boolean
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="targetID">動畫更換目標ID</param>
    /// <param name="anmationName">執行動畫名稱</param>
    /// <param name="animationType">動畫Type</param>
    public void OnSendAniamtion_Boolean<T>(int targetID, string anmationName, T animationType)
    {        
        switch (animationType.GetType().Name)
        {
            case "Boolean":                
                photonView.RPC("OnSetAniamtion_Boolean", RpcTarget.Others, targetID, anmationName, Convert.ToBoolean(animationType));
                break;
            case "Single":
                photonView.RPC("OnSetAniamtion_Single", RpcTarget.Others, targetID, anmationName, Convert.ToSingle(animationType));
                break;
            case "Int32":
                photonView.RPC("OnSetAniamtion_Int32", RpcTarget.Others, targetID, anmationName, Convert.ToInt32(animationType));
                break;
            case "String":
                photonView.RPC("OnSetAniamtion_String", RpcTarget.Others, targetID, anmationName, Convert.ToString(animationType));
                break;            
        }        
    }

    /// <summary>
    /// 設定動畫_Boolean
    /// </summary>
    /// <param name="targetID">動畫更換目標ID</param>
    /// <param name="anmationName">執行動畫名稱</param>
    /// <param name="animationType">動畫Type</param>
    [PunRPC]
    void OnSetAniamtion_Boolean(int targetID, string anmationName, bool animationType)
    {        
        GameSceneManagement.Instance.OnConnectAnimationSetting(targetID, anmationName, animationType);
    }

    /// <summary>
    /// 設定動畫_Single
    /// </summary>
    /// <param name="targetID">動畫更換目標ID</param>
    /// <param name="anmationName">執行動畫名稱</param>
    /// <param name="animationType">動畫Type</param>
    [PunRPC]
    void OnSetAniamtion_Single(int targetID, string anmationName, float animationType)
    {
        GameSceneManagement.Instance.OnConnectAnimationSetting(targetID, anmationName, animationType);
    }

    /// <summary>
    /// 設定動畫_Int32
    /// </summary>
    /// <param name="targetID">動畫更換目標ID</param>
    /// <param name="anmationName">執行動畫名稱</param>
    /// <param name="animationType">動畫Type</param>
    [PunRPC]
    void OnSetAniamtion_Int32(int targetID, string anmationName, int animationType)
    {
        GameSceneManagement.Instance.OnConnectAnimationSetting(targetID, anmationName, animationType);
    }

    /// <summary>
    /// 設定動畫_String
    /// </summary>
    /// <param name="targetID">動畫更換目標ID</param>
    /// <param name="anmationName">執行動畫名稱</param>
    /// <param name="animationType">動畫Type</param>
    [PunRPC]
    void OnSetAniamtion_String(int targetID, string anmationName, string animationType)
    {
        GameSceneManagement.Instance.OnConnectAnimationSetting(targetID, anmationName, anmationName);
    }

    /// <summary>
    /// 發送遊戲結束訊息(房主離開遊戲)
    /// </summary>
    public void OnSendGameover()
    {
        photonView.RPC("OnGameOver", RpcTarget.Others);
    }

    /// <summary>
    /// 遊戲結束
    /// </summary>
    [PunRPC]
    void OnGameOver()
    {
        StartCoroutine(LoadScene.Instance.OnLoadScene("StartScene"));
    }
    #endregion
}
