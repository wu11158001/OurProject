using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStart
{
    public AStart Instance;

    WayPoints wayPoints;
    NodePath[] allNodes;//獲取所有的節點
    List<Vector3> pathNodesList = new List<Vector3>();//紀錄路徑點
    List<NodePath> closeNodeList = new List<NodePath>();//紀錄已關閉的節點

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
        pathNodesList.Clear();//紀錄路徑點
        closeNodeList.Clear();//紀錄已關閉的節點
        pathNodesList.Add(startPoint);//初始路徑點

        allNodes = wayPoints.GetNodePaths;//獲取所有的節點

        NodePath node = null;

        //重製所有節點狀態
        for (int i = 0; i < allNodes.Length; i++)
        {
            allNodes[i].nodeState = NodePath.NodeState.開啟;
        }

        float distance = 10000;//距離
        int closeNumber = 0;//最近的節點編號

        #region 第一步:尋找距離起始點最近的節點
        for (int i = 0; i < allNodes.Length; i++)
        {
            float closestDistance = (startPoint - allNodes[i].transform.position).magnitude;//起點到節點距離

            //有障礙物跳過
            if (Physics.Linecast(startPoint, allNodes[i].transform.position, 1 << LayerMask.NameToLayer("StageObject")))
            {
                continue;
            }

            //尋找最近的距離
            if (closestDistance < distance)
            {
                distance = closestDistance;
                closeNumber = i;
            }
        }

        node = allNodes[closeNumber];//最近節點       
        //比較鄰居節點
        node = OnCompareStartNeighborNode(node: node, targetPosition: targetPosition, startPoint: startPoint);

        node.nodeState = NodePath.NodeState.關閉;//節點狀態
        closeNodeList.Add(node);//紀錄已關閉的節點
        pathNodesList.Add(node.transform.position);//初始路徑點
        #endregion


        #region 第二步:比較最近節點鄰居
        float bestDistance = 10000;//最佳距離                
        int bestNeighbor = 0;//最近的鄰居編號
        while (closeNodeList.Count < allNodes.Length)
        {
            bool isHaveBestNode = false;//是否有更近的節點
            bestNeighbor = 0;//最近的鄰居編號
            for (int i = 0; i < node.neighborNode.Length; i++)
            {
                if (node.neighborNode[i].nodeState == NodePath.NodeState.關閉) continue;               
                Vector3 nextPosition = node.transform.position;//下個節點位置
                Vector3 neighborPosition = node.neighborNode[i].transform.position;//鄰居節點位置

                float G = (nextPosition - neighborPosition).magnitude;//到下個節點位置
                float H = (neighborPosition - targetPosition).magnitude;//下個節點到目標位置
                float F = G + H;//距離

                if (F < bestDistance)
                {
                    isHaveBestNode = true;//有更近的節點
                    bestDistance = F;//最佳距離
                    bestNeighbor = i;//最近的鄰居編號                    
                }
            }            

            if (!isHaveBestNode)//沒有更近的節點
            {
                //判斷與目標路徑是否有障礙物
                if (Physics.Linecast(node.transform.position, targetPosition, 1 << LayerMask.NameToLayer("StageObject")))
                {
                    isHaveBestNode = true;//有更近的節點
                    //比較鄰居節點
                    
                    if(OnCompareNeighborNode(node: ref node, targetPosition: targetPosition))
                    {
                        pathNodesList.Add(targetPosition);//紀錄目標點
                        return pathNodesList;//回傳所有紀錄路徑點
                    }

                }
                else
                {                   
                    pathNodesList.Add(targetPosition);//紀錄目標點
                    return pathNodesList;//回傳所有紀錄路徑點
                }             
            }

            node = node.neighborNode[bestNeighbor];
            node.nodeState = NodePath.NodeState.關閉;//節點狀態
            closeNodeList.Add(node);//紀錄已關閉的節點
            pathNodesList.Add(node.transform.position);//初始路徑點
        }
        #endregion

        pathNodesList.Add(targetPosition);//紀錄目標點
        return pathNodesList;//回傳所有紀錄路徑點
    }

    /// <summary>
    /// 比較鄰居節點
    /// </summary>
    /// <param name="node">要比較的節點</param>
    /// <param name="targetPosition">目標位置</param>
    bool OnCompareNeighborNode(ref NodePath node, Vector3 targetPosition)
    {
        NodePath compareNode = node;
        float bestDistance = 10000;

        for (int i = 0; i < compareNode.neighborNode.Length; i++)
        {
            if (compareNode.neighborNode[i].nodeState == NodePath.NodeState.關閉) continue;           

            Vector3 nextPosition = compareNode.transform.position;//下個節點位置
            Vector3 neighborPosition = compareNode.neighborNode[i].transform.position;//鄰居節點位置

            float G = (nextPosition - neighborPosition).magnitude;//到下個節點位置
            float H = (neighborPosition - targetPosition).magnitude;//下個節點到目標位置
            float F = G + H;//距離

            if (F < bestDistance)
            {
                bestDistance = F;//最佳距離
                compareNode = compareNode.neighborNode[i];//更新最近節點
            }
        }
        if(compareNode == node) return true;
       
        node = compareNode;
        return false;
    }

    /// <summary>
    /// 比較起點鄰居節點
    /// </summary>
    /// <param name="node">要比較的節點</param>
    /// <param name="targetPosition">目標位置</param>
    /// <param name="targetPosition">起點位置</param>
    NodePath OnCompareStartNeighborNode(NodePath node, Vector3 targetPosition, Vector3 startPoint)
    {
        NodePath compareNode = node;

        float bestDistance = 10000;
        for (int i = 0; i < compareNode.neighborNode.Length; i++)
        {
            if (compareNode.neighborNode[i].nodeState == NodePath.NodeState.關閉) continue;
            //有障礙物跳過
            if (Physics.Linecast(startPoint, compareNode.neighborNode[i].transform.position, 1 << LayerMask.NameToLayer("StageObject")))
            {
                continue;
            }            

            Vector3 nextPosition = compareNode.transform.position;//下個節點位置
            Vector3 neighborPosition = compareNode.neighborNode[i].transform.position;//鄰居節點位置

            float G = (nextPosition - neighborPosition).magnitude;//到下個節點位置
            float H = (neighborPosition - targetPosition).magnitude;//下個節點到目標位置
            float F = G + H;//距離

            if (F < bestDistance)
            {
                bestDistance = F;//最佳距離
                compareNode = compareNode.neighborNode[i];//更新最近節點
            }
        }
        
        return compareNode;
    }
}
