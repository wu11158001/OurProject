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
    /// 隨機或創建房間設定
    /// </summary>
    public void OnRandomOrCreateRoomRoomSetting()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    /// <summary>
    /// 加入隨機房間失敗觸發
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError("加入隨機房間失敗:" + returnCode + ":" + message);

        StartSceneUI.Instance.OnConnectModeSettingTip(tip: "加入隨機房間失敗");
    }

    /// <summary>
    /// 指定房間設定
    /// </summary>
    /// <param name="roomName">欲加入房間名</param>
    public void OnSpecifyRoomSetting(string roomName)
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
        Debug.LogError("加入房間失敗:" + returnCode + ":" + message);

        StartSceneUI.Instance.OnConnectModeSettingTip(tip: "找不到房間");
    }

    /// <summary>
    /// 加入房間觸發
    /// </summary>
    public override void OnJoinedRoom()
    {
        Debug.Log("加入" + PhotonNetwork.CurrentRoom.Name + "房間");

        StartSceneUI.Instance.OnTidyConnectModeUI();
        OnReFreshPlayerNickName();
        OnSendRoomPlayerCharacters();
    }

    /// <summary>
    /// 有玩家進入房間
    /// </summary>
    /// <param name="newPlayer">新玩家</param>

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        OnReFreshPlayerNickName();
        OnSendRoomPlayerCharacters();
    }

    /// <summary>
    /// 有玩家離開房間
    /// </summary>
    /// <param name="otherPlayer">離開玩家</param>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        OnReFreshPlayerNickName();       
        OnSendRoomPlayerCharacters();
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
        
        StartSceneUI.Instance.OnRefreshRoomPlayerNickName(playerList, PhotonNetwork.NickName);
    }

    /// <summary>
    /// 發送房間玩家腳色
    /// </summary>
    public void OnSendRoomPlayerCharacters()
    {
        photonView.RPC("OnRefreshPlayerCharacters", RpcTarget.All, PhotonNetwork.NickName, GameDataManagement.Instance.selectRoleNumber);
    }

    /// <summary>
    /// 刷新玩家腳色
    /// </summary>
    /// <param name="nickName"></param>
    /// <param name="characters"></param>
    [PunRPC]
    void OnRefreshPlayerCharacters(string nickName, int characters )
    {        
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
           if(PhotonNetwork.PlayerList[i].NickName == nickName)
            {
                StartSceneUI.Instance.OnRefreshPlayerCharacters(i, characters);
                return;
            }
        }
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
    #endregion
}
