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
    public void OnConnectSetting()
    {
        Debug.Log("準備連線");

        PhotonNetwork.ConnectUsingSettings();//設定連線
        PhotonNetwork.AutomaticallySyncScene = true;//開啟自動同步場景
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

    /// <summary>
    /// 設定暱稱
    /// </summary>
    /// <param name="nickName">暱稱</param>
    public void OnSetNickName(string nickName)
    {
        //設定暱稱
        if (nickName == "") PhotonNetwork.NickName = "訪客" + UnityEngine.Random.Range(0, 1000);
        else PhotonNetwork.NickName = nickName;
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
        if (GameDataManagement.Instance.stage == GameDataManagement.Stage.開始場景)
        {
            StartSceneUI.Instance.OnRoomLevelText(level);
            GameDataManagement.Instance.selectLevelNumber = level;
        }
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
            
            photonView.RPC("OnLoadScene", RpcTarget.All, level);
            /*PhotonNetwork.LoadLevel("LevelScene" + level);
            PhotonNetwork.CurrentRoom.IsOpen = false;//關閉房間*/
        }
        else isStartGame = false;

        return isStartGame;
    }
    #endregion

    #region 遊戲中
    /// <summary>
    /// 載入場景
    /// </summary>
    /// <param name="level">場景編號</param>
    [PunRPC]
    void OnLoadScene(int level)
    {
        StartCoroutine(LoadScene.Instance.OnLoadScene_Connect(level));
    }

    void OnStartIntoGame(int level)
    {
        PhotonNetwork.LoadLevel("LevelScene" + level);
        PhotonNetwork.CurrentRoom.IsOpen = false;//關閉房間
    }
    /// <summary>
    /// 房主交換觸發
    /// </summary>
    /// <param name="newMasterClient"></param>
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.LogError("房主交換" + newMasterClient.ToString());
    }

    /// <summary>
    /// 發送更新任務
    /// </summary>
    /// <param name="enemyName">擊倒物件名稱</param>
    public void OnSendRenewTask(string enemyName)
    {
        photonView.RPC("OnRenewTask", RpcTarget.Others, enemyName);
    }

    /// <summary>
    /// 更新任務
    /// </summary>
    /// <param name="enemyName">擊倒物件名稱</param>
    /// <param name="info">發送者資訊</param>
    [PunRPC]
    void OnRenewTask(string enemyName, PhotonMessageInfo info)
    {
        //GameSceneManagement.Instance.taskNumber += 1;//已擊殺怪物數量
        GameSceneUI.Instance.SetEnemyLifeBarActive = false;
        GameSceneManagement.Instance.OnTaskText();//任務文字
        GameSceneUI.Instance.OnSetTip($"{enemyName}已擊倒", 5);//設定提示文字
    }

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
    /// <param name="repel">擊退距離</param>
    /// <param name="attackerObjectID">攻擊者物件ID</param>
    /// <param name="attackerID">攻擊者ID</param>
    public void OnSendGetHit(int targetID, Vector3 position, Quaternion rotation, float damage,bool isCritical, float repel, int attackerObjectID, int attackerID)
    {        
        photonView.RPC("OnGetHit", RpcTarget.Others, targetID, position, rotation, damage, isCritical, repel, attackerObjectID, attackerID);
    }

    /// <summary>
    /// 受擊訊息
    /// </summary>
    /// <param name="targetID">目標ID</param>
    /// <param name="position">位置</param>
    /// <param name="rotation">選轉</param>
    /// <param name="damage">受到傷害</param>
    /// <param name="isCritical">是否爆擊</param>
    /// <param name="repel">擊退距離</param>
    /// <param name="attackerObjectID">攻擊者物件ID</param>
    /// <param name="attackerID">攻擊者ID</param>
    [PunRPC]
    void OnGetHit(int targetID, Vector3 position, Quaternion rotation, float damage, bool isCritical, float repel, int attackerObjectID, int attackerID)
    {

        GameSceneManagement.Instance.OnConnectGetHit(targetID, position, rotation, damage, isCritical, repel, attackerObjectID, attackerID);
    }

    /// <summary>
    /// 發送據點受擊訊息
    /// </summary>
    /// <param name="targetID">目標ID</param>
    /// <param name="damage">受到傷害</param>
    public void OnSendStrongholdGetHit(int targetID, float damage)
    {
        photonView.RPC("OnStrongholdGetHit", RpcTarget.Others, targetID, damage);
    }

    /// <summary>
    /// 據點受擊訊息
    /// </summary>
    /// <param name="targetID">目標ID</param>
    /// <param name="damage">受到傷害</param>
    [PunRPC]
    void OnStrongholdGetHit(int targetID, float damage)
    {
        GameSceneManagement.Instance.OnConnectStrongholdGetHit(targetID, damage);
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
    /// 發送動畫訊息
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="targetID">動畫更換目標ID</param>
    /// <param name="anmationName">執行動畫名稱</param>
    /// <param name="animationType">動畫Type</param>
    public void OnSendAniamtion<T>(int targetID, string anmationName, T animationType)
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

    /// <summary>
    /// 發送創建Boss激活
    /// </summary>
    public void OnSendBossActive()
    {
        photonView.RPC("OnBossActive", RpcTarget.Others);
    }

    /// <summary>
    /// 創建Boss
    /// </summary>
    [PunRPC]
    void OnBossActive()
    {
        GameSceneManagement.Instance.isCreateBoss = true;
        //是房主
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject.FindObjectOfType<BossAI>().OnActive();
        }        
    }

    /// <summary>
    /// 發送其他玩家生命條
    /// </summary>
    /// <param name="nickName">玩家暱稱</param>
    /// <param name="hpProportion">生命比例</param>
    public void OnSendOtherPlayerLifeBar(string nickName, float hpProportion)
    {
        photonView.RPC("OnOtherPlayerLifeBare", RpcTarget.Others, nickName, hpProportion);
    }

    /// <summary>
    /// 其他玩家生命條
    /// </summary>
    /// <param name="nickName">玩家暱稱</param>
    /// <param name="hpProportion">生命比例</param>
    [PunRPC]
    void OnOtherPlayerLifeBare(string nickName, float hpProportion)
    {
        GameSceneUI.Instance.OnSetOtherPlayerLifeBar(nickName, hpProportion);
    }

    /// <summary>
    /// 發送遊戲時間
    /// </summary>
    /// <param name="gameTime">遊戲時間</param>
    public void OnSendGameTime(float gameTime)
    {
        photonView.RPC("OnGameTime", RpcTarget.All, gameTime);
    }

    /// <summary>
    /// 遊戲時間
    /// </summary>
    /// <param name="gameTime">遊戲時間</param>
    [PunRPC]
    void OnGameTime(float gameTime)
    {
        //遊戲時間
        int minute = (int)gameTime / 60;
        int second = (int)gameTime % 60;
        
        GameSceneUI.Instance.playGameTimeOver_Text.text = $"遊 戲 時 間 : {minute} 分 {second} 秒"; 
    }

    /// <summary>
    /// 發送遊戲分數
    /// </summary>
    /// <param name="nickName">暱稱</param>
    /// <param name="MaxCombo">最大連擊</param>
    /// <param name="killNumber">擊殺數</param>
    /// <param name="accumulationDamage">累積傷害</param>
    /// <param name="gameTime">遊戲時間</param>
    public void OnSendGameScoring(string nickName, int MaxCombo, int killNumber, float accumulationDamage)
    {
        photonView.RPC("OnGameScoring", RpcTarget.All, nickName, MaxCombo, killNumber, accumulationDamage);
    }

    /// <summary>
    /// 遊戲分數
    /// </summary>
    /// <param name="nickName">暱稱</param>
    /// <param name="MaxCombo">最大連擊</param>
    /// <param name="killNumber">擊殺數</param>
    /// <param name="accumulationDamage">累積傷害</param>    
    [PunRPC]
    void OnGameScoring(string nickName, int MaxCombo, int killNumber, float accumulationDamage)
    {
        //清空List
        List<string> playerList = new List<string>();

        //紀錄玩家暱稱
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            playerList.Add(PhotonNetwork.PlayerList[i].NickName);
        }

        GameSceneUI.Instance.OnConnectGameOver(playerList, nickName, MaxCombo, killNumber, accumulationDamage);
    }

    /// <summary>
    /// 發送玩家死亡訊息
    /// </summary>
    public void OnSendPlayerDie()
    {
        photonView.RPC("OnPlayerDie", RpcTarget.All);
    }

    /// <summary>
    /// 玩家死亡訊息
    /// </summary>
    [PunRPC]
    void OnPlayerDie()
    {
        GameSceneManagement.Instance.lifePlayerNumber--;

        if(GameSceneManagement.Instance.lifePlayerNumber <= 0)
        {
            //遊戲結果文字
            GameSceneManagement.Instance.isGameOver = true;
            GameSceneUI.Instance.OnSetGameResult(true, "失 敗");
            GameSceneManagement.Instance.OnSetGameOver(false);
            //GameSceneUI.Instance.connectGameOverResult_Text.text = " 失 敗 總 結 ";
            //設定遊戲結束
            StartCoroutine(GameSceneManagement.Instance.OnSetGameOver(false));
        }
    }

    /// <summary>
    /// 發送進入下一關
    /// </summary>
    public void OnSendIntoNexttLevel()
    {
        photonView.RPC("OnIntoNextLevel", RpcTarget.All);
    }

    /// <summary>
    /// 進入下一關
    /// </summary>
    [PunRPC]
    void OnIntoNextLevel()
    {
        /* if (PhotonNetwork.IsMasterClient)
         {                      
             if(GameDataManagement.Instance.selectLevelNumber == 11) StartCoroutine(LoadScene.Instance.OnLoadScene_Connect(12));
             if (GameDataManagement.Instance.selectLevelNumber == 12) StartCoroutine(LoadScene.Instance.OnLoadScene_Connect(13));
         }*/

        PhotonNetwork.AutomaticallySyncScene = true;//自動同步場景

        if (GameSceneManagement.Instance.isVictory)
        {
            if (GameDataManagement.Instance.selectLevelNumber == 11)
            {
                GameDataManagement.Instance.selectLevelNumber = 12;
                StartCoroutine(LoadScene.Instance.OnLoadScene_Connect(12));
                return;
            }

            if (GameDataManagement.Instance.selectLevelNumber == 12) StartCoroutine(LoadScene.Instance.OnLoadScene_Connect(13));
        }
        else StartCoroutine(LoadScene.Instance.OnLoadScene_Connect(13));

    }

    /// <summary>
    /// 發送玩家暱稱與ID
    /// </summary>
    /// <param name="nickName">暱稱</param>
    /// <param name="id">ID</param>
    public void OnSendPlayerNickNmaeAndID(string nickName, int id)
    {
        photonView.RPC("OnPlayerNickNameAndId", RpcTarget.All, nickName, id);
    }

    /// <summary>
    /// 玩家暱稱與ID
    /// </summary>
    /// <param name="nickName"></param>
    /// <param name="id"></param>
    [PunRPC]
    void OnPlayerNickNameAndId(string nickName, int id)
    {
        GameSceneManagement.Instance.OnCreatePlayerNameObject(nickName, id);
    }    
    #endregion
}
