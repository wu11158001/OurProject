using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePath : MonoBehaviour
{
    const float radius = 0.5f;//Gizmos

    [Tooltip("鄰居節點")] public NodePath[] neighborNode = new NodePath[] { };
    
    /// <summary>
    /// 節點狀態
    /// </summary>
    public enum NodeState
    {
        開啟,
        關閉
    }
    public NodeState nodeState;

    private void OnDrawGizmos()
    {
        /*Gizmos.color = Color.black;
        Gizmos.DrawSphere(transform.position, radius);*/

        for (int i = 0; i < neighborNode.Length; i++)
        {
            Gizmos.color = Color.black;
            
            Gizmos.DrawLine(transform.position, neighborNode[i].transform.position);
        }
    }
}
