using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    static WayPoints wayPoints;
    public static WayPoints Instace => wayPoints;

    const float radius = 0.5f;

    //所有節點位置
    Vector3[] nodesPosition;
    public Vector3[] GetNodesPosition => nodesPosition;

    private void Awake()
    {
        if (wayPoints != null)
        {
            Destroy(this);
            return;
        }
        wayPoints = this;
    }

    private void Start()
    {
        nodesPosition = new Vector3[transform.childCount];

        OnSaveNode();
    }

    /// <summary>
    /// 節點存檔
    /// </summary>
    void OnSaveNode()
    {
        //創建文字檔紀錄(覆蓋)
        //StreamWriter streamWriter = new StreamWriter("Assets/AiNode.txt", false);

        //string s = "";
        for (int i = 0; i < transform.childCount; i++)
        {
            /*s = "";
            s += transform.GetChild(i).name;//節點物件名稱
            s += " ";
            s += transform.GetChild(OnGetNextIndex(i)).name;//下個節點物件名稱
            s += " ";
            s += transform.GetChild(OnGetPreviousIndex(i)).name;//前個節點物件名稱

            streamWriter.WriteLine(s);*/

            nodesPosition[i] = transform.GetChild(i).position;//紀錄節點位置
        }

        //OnSetNeighbor();
        //streamWriter.Close();
    }

    /// <summary>
    /// 獲取節點position
    /// </summary>
    /// <param name="i">節點編號</param>
    /// <returns></returns>
    public Vector3 OnGetWayPoint(int i)
    {
        return transform.GetChild(i).position;
    }

    /// <summary>
    /// 獲取下個節點編號
    /// </summary>
    /// <param name="i">節點編號</param>
    /// <returns></returns>
    public int OnGetNextIndex(int i)
    {
        if (i + 1 == transform.childCount) return 0;

        return i + 1;
    }

    /// <summary>
    /// 獲取前個節點編號
    /// </summary>
    /// <param name="i">節點編號</param>
    /// <returns></returns>
    public int OnGetPreviousIndex(int i)
    {
        if (i == 0) return transform.childCount - 1;

        return i - 1;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(OnGetWayPoint(i), radius);
            Gizmos.DrawLine(OnGetWayPoint(i), transform.GetChild(OnGetNextIndex(i)).position);
        }
    }
}
