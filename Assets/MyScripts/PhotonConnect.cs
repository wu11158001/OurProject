using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

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
        if (nickName == "") PhotonNetwork.NickName = "訪客" + Random.Range(0, 1000);
        else PhotonNetwork.NickName = nickName;
    }

    /// <summary>
    /// 登入成功觸發
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("連線成功");

        StartSceneUI.Instance.OnIsConnected();
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
        StartSceneUI.Instance.OnConnectModeBackButton();
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

        StartSceneUI.Instance.OnConnectModeSettingTip(tip: "創建房間失敗");
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

        StartSceneUI.Instance.OnConnectModeSettingTip(tip: "目前沒有房間");
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

        StartSceneUI.Instance.OnConnectModeSettingTip(tip: "找不到房間");
    }

    /// <summary>
    /// 加入房間觸發
    /// </summary>
    public override void OnJoinedRoom()
    {
        Debug.Log("加入" + PhotonNetwork.CurrentRoom.Name + "房間");

        StartSceneUI.Instance.OnTidyConnectModeUI(roomName: PhotonNetwork.CurrentRoom.Name);
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
    void OnReFreshPlayerNickName()
    {
        //清空List
        List<string> playerList = new List<string>();

        //紀錄玩家暱稱
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            playerList.Add(PhotonNetwork.PlayerList[i].NickName);
        }

        StartSceneUI.Instance.OnRefreshRoomPlayerNickName(playerList, PhotonNetwork.NickName, PhotonNetwork.IsMasterClient);
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
                StartSceneUI.Instance.OnRefreshPlayerCharacters(i, characters);
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
        StartSceneUI.Instance.OnGetRoomChatMessage(info.Sender.NickName + ":" + message);
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
        StartSceneUI.Instance.OnRoomLevelText(level);
    }

    /// <summary>
    /// 開始遊戲
    /// </summary>
    /// <param name="level">進入關卡編號</param>
    public bool OnStartGame(int level)
    {
        bool isStartGame = false;

        PhotonNetwork.LoadLevel("LevelScene" + level);
        //2人以上開始
        /*if (PhotonNetwork.CurrentRoom.PlayerCount > 1) PhotonNetwork.LoadLevel("LevelScene" + level);
        else isStartGame = false;*/

        return isStartGame;
    }
    #endregion

    #region 遊戲中
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
    /// <param name="obj">物件ID</param>
    /// <param name="active">激活狀態</param>
    public void OnSendObjectActive(GameObject obj, bool active)
    {
        int id = obj.GetComponent<PhotonView>().ViewID;
        photonView.RPC("OnObjectActive", RpcTarget.Others, id, active);
    }

    /// <summary>
    /// 物件激活
    /// </summary>
    /// <param name="id">物件ID</param>
    /// <param name="active">激活狀態</param>
    /// <param name="info">傳送者訊息</param>
    [PunRPC]
    void OnObjectActive(int id, bool active, PhotonMessageInfo info)
    {
        GameSceneManagement.Instance.OnConnectObjectActive(id, active);
    }
    #endregion
}
