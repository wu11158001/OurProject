using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStart
{
    public AStart Instance;

    WayPoints wayPoints;
    List<Vector3> pathNodes = new List<Vector3>();//紀錄路徑點

    /// <summary>
    /// 初始
    /// </summary>
    public void initial()
    {
        Instance = this;
        wayPoints = WayPoints.Instace;
    }

    /// <summary>
    /// 尋找最佳路線
    /// </summary>
    /// <param name="startPoint">開始位置</param>
    /// <param name="targetPosition">目標位置</param>
    /// <returns></returns>
    public List<Vector3> OnGetBestPoint(Vector3 startPoint, Vector3 targetPosition)
    {
        pathNodes.Clear();
        pathNodes.Add(startPoint);//初始路徑點

        Vector3[] nodes = wayPoints.GetNodesPosition;//獲取所有的節點

        float distance = 10000;//距離
        int closeNumber = 0;//最近的節點編號

        #region 第一步:尋找距離起始點最近的節點
        for (int i = 0; i < nodes.Length; i++)
        {
            float p = (startPoint - nodes[i]).magnitude;//距離

            //有障礙物跳過
            if (Physics.Linecast(startPoint, nodes[i], 1 << LayerMask.NameToLayer("Wall")))
            {

                continue;
            }

            //尋找最近的距離
            if (p < distance)
            {


                distance = p;
                closeNumber = i;
            }
        }
        #endregion

        pathNodes.Add(nodes[closeNumber]);//紀錄最近的節點

        #region 第二步:尋找最近節點的鄰居
        float closestNode = (targetPosition - nodes[closeNumber]).magnitude;//最近點到終點距離
        float nextNeighbor = (targetPosition - nodes[(wayPoints.OnGetNextIndex(closeNumber))]).magnitude;//最近點鄰居到終點距離(下個編號)
        float previousNeighbor = (targetPosition - nodes[(wayPoints.OnGetPreviousIndex(closeNumber))]).magnitude;//最近點鄰居到終點距離(前個編號)        

        bool isNext = false;//判斷編號走向
        int number;//節點編號
        if (nextNeighbor < previousNeighbor)
        {
            /*//最近節點即是最近距離
            if (closestNode < nextNeighbor)
            {
                pathNodes.Add(targetPosition);//紀錄路徑點
                return pathNodes;
            }*/

            isNext = true;//判斷編號走向
            pathNodes.Add(nodes[wayPoints.OnGetNextIndex(closeNumber)]);//紀錄路徑點

            if (OnJudegNextClosePoint(closeNumber, nodes, targetPosition)) return pathNodes;
        }
        else
        {
            /*//最近節點即是最近距離
            if (closestNode < previousNeighbor)
            {
                pathNodes.Add(targetPosition);//紀錄路徑點
                return pathNodes;
            } */

            pathNodes.Add(nodes[wayPoints.OnGetPreviousIndex(closeNumber)]);//紀錄路徑點

            if (OnJudegPreviousClosePoint(closeNumber, nodes, targetPosition)) return pathNodes;
        }
        #endregion

        #region 第三步:判斷之後的節點
        number = closeNumber;
        for (int i = 0; i < nodes.Length; i++)
        {
            if (isNext)
            {
                number = wayPoints.OnGetNextIndex(number);
                pathNodes.Add(nodes[wayPoints.OnGetNextIndex(number)]);//紀錄路徑點
                if (OnJudegNextClosePoint(number, nodes, targetPosition)) return pathNodes;
            }
            else
            {
                number = wayPoints.OnGetPreviousIndex(number);
                pathNodes.Add(nodes[wayPoints.OnGetPreviousIndex(number)]);//紀錄路徑點
                if (OnJudegPreviousClosePoint(number, nodes, targetPosition)) return pathNodes;
            }
        }
        #endregion

        pathNodes.Add(targetPosition);//目標路徑點
        return pathNodes;
    }

    /// <summary>
    /// 判斷最近距離(Next)
    /// </summary>
    /// <param name="number">節點編號</param>
    /// <param name="nodes">所有的節點</param>
    /// <param name="targetPosition">目標位置</param>
    /// <returns></returns>
    bool OnJudegNextClosePoint(int number, Vector3[] nodes, Vector3 targetPosition)
    {
        int next = wayPoints.OnGetNextIndex(number);
        float n1 = (targetPosition - nodes[wayPoints.OnGetNextIndex(number)]).magnitude;
        float n2 = (targetPosition - nodes[wayPoints.OnGetNextIndex(next)]).magnitude;

        if (n1 < n2 && !Physics.Linecast(nodes[number], targetPosition, 1 << LayerMask.NameToLayer("Wall")))
        {
            pathNodes.Add(targetPosition);//紀錄路徑點
            return true;
        }

        return false;
    }

    /// <summary>
    /// 判斷最近距離(Previous)
    /// </summary>
    /// <param name="number">節點編號</param>
    /// <param name="nodes">所有的節點</param>
    /// <param name="targetPosition">目標位置</param>
    /// <returns></returns>
    bool OnJudegPreviousClosePoint(int number, Vector3[] nodes, Vector3 targetPosition)
    {
        int next = wayPoints.OnGetPreviousIndex(number);
        float n1 = (targetPosition - nodes[wayPoints.OnGetPreviousIndex(number)]).magnitude;
        float n2 = (targetPosition - nodes[wayPoints.OnGetPreviousIndex(next)]).magnitude;

        if (n1 < n2 && !Physics.Linecast(nodes[number], targetPosition, 1 << LayerMask.NameToLayer("Wall")))
        {
            pathNodes.Add(targetPosition);//紀錄路徑點
            return true;
        }

        return false;
    }
}
