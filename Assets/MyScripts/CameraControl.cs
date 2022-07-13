using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攝影機控制
/// </summary>
public class CameraControl : MonoBehaviour
{
    static CameraControl cameraControl;
    public static CameraControl Instance => cameraControl;

    Transform lookPoint;//攝影機觀看點
    float distance;//與玩家距離
    Vector3 forwardVector;//前方向量
    float totalVertical;//記錄垂直移動量
    float limitUpAngle;//限制向上角度
    float limitDownAngle;//限制向下角度

    void Start()
    {        
        if(cameraControl != null)
        {
            Destroy(this);
            return;
        }
        cameraControl = this;

        lookPoint = GameObject.Find("CameraLookPoint").transform;
        distance = 2.6f;//與玩家距離
        forwardVector = lookPoint.forward;//前方向量
        limitUpAngle = 35;//限制向上角度
        limitDownAngle = 20;//限制向下角度
    }

    // Update is called once per frame
    void Update()
    {
        OnCameraControl();
    }

    /// <summary>
    /// 攝影機控制
    /// </summary>
    void OnCameraControl()
    {
        float mouseX = Input.GetAxis("Mouse X");//滑鼠X軸
        float mouseY = Input.GetAxis("Mouse Y");//滑鼠Y軸

        forwardVector = Quaternion.AngleAxis(mouseX, Vector3.up) * forwardVector;//前方向量

        //限制上下角度
        totalVertical += mouseY;//記錄垂直移動量
        if (totalVertical > limitDownAngle) totalVertical = limitDownAngle;
        if (totalVertical < -limitUpAngle) totalVertical = -limitUpAngle;

        Vector3 tempRotate = Vector3.Cross(Vector3.up, forwardVector);//獲取水平軸
        Vector3 RotateVector = Quaternion.AngleAxis(totalVertical, -tempRotate) * forwardVector;//最後選轉向量
        RotateVector.Normalize();

        Vector3 moveTarget = lookPoint.position - RotateVector * distance;//移動目標

        //攝影機障礙物偵測
        LayerMask mask = LayerMask.GetMask("StageObject");        
        if (Physics.SphereCast(lookPoint.position, 0.08f, -RotateVector, out RaycastHit hit, distance, mask))
        {
            //碰到"StageObject"攝影機移動位置改為(觀看物件位置 - 與碰撞物件距離)
            moveTarget = lookPoint.position - RotateVector * hit.distance;            
        }
        
        transform.position = moveTarget;
        transform.forward = lookPoint.position - transform.position;        
    }
}
