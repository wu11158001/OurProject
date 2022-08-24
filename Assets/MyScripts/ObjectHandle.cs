using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物件處理
/// </summary>
public class ObjectHandle
{
    static ObjectHandle objectHandle;
    public static ObjectHandle GetObjectHandle => objectHandle;

    List<List<TemporaryObject>> searchGameObject_List = new List<List<TemporaryObject>>();//紀錄所有遊戲物件(開啟/關閉用)    
    List<GameObject> cerateGameObject_List = new List<GameObject>();//創建物件(重新創建用)

    /// <summary>
    /// 建構子
    /// </summary>
    public ObjectHandle()
    {
        objectHandle = this;
    }
    
    /// <summary>
    /// 創建物件
    /// </summary>
    /// <param name="path">載入路徑</param>
    /// <returns></returns>
    public int OnCreateObject(string path)
    {                
        TemporaryObject temp = new TemporaryObject();

        //判斷是否為連線模式
        if (GameDataManagement.Instance.isConnect) temp.obj = PhotonConnect.Instance.OnCreateObject(path);
        else temp.obj = GameObject.Instantiate(Resources.Load(path) as GameObject);//產生物件

        temp.obj.SetActive(false);//關閉物件

        //存下物件
        List<TemporaryObject> temp_List = new List<TemporaryObject>();//臨時存放
        temp_List.Add(temp);
        cerateGameObject_List.Add(temp.obj);//存放物件(重新創建用)
        searchGameObject_List.Add(temp_List);//存放項目(開啟/關閉用)

        return searchGameObject_List.Count - 1;//回傳物件編號
    }

    /// <summary>
    /// 開啟物件
    /// </summary>
    /// <param name="number">物件編號</param>
    /// <param name="path">prefab路徑</param>
    /// <returns></returns>
    public GameObject OnOpenObject(int number, string path)
    {
        if (number < 0 || number > searchGameObject_List.Count) return null;//防呆

        List<TemporaryObject> getGameObject_List = searchGameObject_List[number];//取出物件

        for (int i = 0; i < getGameObject_List.Count; i++)
        {
            if(!getGameObject_List[i].obj.activeSelf)//若物件處於關閉狀態
            {
                //連線模式
                if (GameDataManagement.Instance.isConnect)
                {
                    if (getGameObject_List[i].obj.GetComponent<HitNumber>() == null)
                    {
                        PhotonConnect.Instance.OnSendObjectActive(getGameObject_List[i].obj, true);                        
                    }
                }
                
                getGameObject_List[i].obj.SetActive(true);//開啟物件
                return getGameObject_List[i].obj;//回傳物件
            }
        }

        //超過目前數量
        TemporaryObject temp = new TemporaryObject();//暫存物件        
        if (GameDataManagement.Instance.isConnect)//判斷是否為連線模式
        {
            temp.obj = PhotonConnect.Instance.OnCreateObject(path);//創建物件
            PhotonConnect.Instance.OnSendObjectActive(temp.obj, true);            
        }
        else
        {
            temp.obj = GameObject.Instantiate(cerateGameObject_List[number]) as GameObject;//創建新物件(複製物件)               
        }

        temp.obj.SetActive(true);//開啟物件
        searchGameObject_List[number].Add(temp);//存下物件
        return temp.obj;//回傳新物件
    }
}

/// <summary>
/// 暫存物件
/// </summary>
public class TemporaryObject
{
    public GameObject obj;
}
