using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectObject : MonoBehaviourPunCallbacks
{    
    public int id;
    void Awake()
    {
        //³s½u¼Ò¦¡
        if (GameDataManagement.Instance.isConnect && photonView.IsMine)
        {
            id = GetComponent<PhotonView>().ViewID;
            GameSceneManagement.Instance.OnRecordConnectObject(id, gameObject);
            gameObject.SetActive(false);
        }
    }
}
