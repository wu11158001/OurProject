using System;
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

    GameData_NumericalValue NumericalValue;

    static Transform lookPoint;//攝影機觀看點
    static Vector3 forwardVector;//前方向量
    float totalVertical;//記錄垂直移動量
    float distance;//與玩家距離
    bool isCollsion;//是否碰撞

    void Awake()
    {
        if (cameraControl != null)
        {
            Destroy(this);
            return;
        }
        cameraControl = this;

        NumericalValue = GameDataManagement.Instance.numericalValue;
    }

    private void LateUpdate()
    {
        if (lookPoint != null) OnCameraControl();
    }

    /// <summary>
    /// 設定攝影機觀看點
    /// </summary>
    public static Transform SetLookPoint
    {
        set
        {
            lookPoint = value;//設定觀看物件 
            forwardVector = lookPoint.forward;
        }
    }

    /// <summary>
    /// 攝影機控制
    /// </summary>
    void OnCameraControl()
    {
        float mouseX = Input.GetAxis("Mouse X");//滑鼠X軸
        float mouseY = Input.GetAxis("Mouse Y");//滑鼠Y軸
        distance = (lookPoint.position - transform.position).magnitude;//與玩家距離

        forwardVector = Quaternion.AngleAxis(mouseX, Vector3.up) * forwardVector;//前方向量

        //限制上下角度
        totalVertical += mouseY;//記錄垂直移動量
        if (totalVertical > NumericalValue.limitDownAngle) totalVertical = NumericalValue.limitDownAngle;
        if (totalVertical < -NumericalValue.limitUpAngle) totalVertical = -NumericalValue.limitUpAngle;

        Vector3 tempRotate = Vector3.Cross(Vector3.up, forwardVector);//獲取水平軸
        Vector3 RotateVector = Quaternion.AngleAxis(totalVertical, -tempRotate) * forwardVector;//最後選轉向量
        RotateVector.Normalize();

        Vector3 moveTarget = lookPoint.position - RotateVector * NumericalValue.distance;//移動目標

        //攝影機障礙物偵測
        float lerpSpeed = 0.35f;//lerp速度
        LayerMask mask = LayerMask.GetMask("StageObject");
        if (Physics.SphereCast(lookPoint.position, 0.1f, -RotateVector, out RaycastHit hit, NumericalValue.distance, mask))
        {
            if (!isCollsion) isCollsion = true;
            
            //碰到"StageObject"攝影機移動位置改為(觀看物件位置 - 與碰撞物件距離)                                    
            if (distance < 0.5f && (lookPoint.transform.position - hit.transform.position).magnitude < 2.6f) moveTarget = lookPoint.position + Vector3.up * 0.49f;//距離玩家太近          
            else moveTarget = Vector3.Lerp(transform.position, lookPoint.position - RotateVector * hit.distance, lerpSpeed);//攝影機靠近減速     
        }
        else
        {
            if (isCollsion && distance < NumericalValue.distance)//攝影機後拉遠減速
            {                
                moveTarget = Vector3.Lerp(transform.position, lookPoint.position - RotateVector * NumericalValue.distance, lerpSpeed);                
                if (distance > NumericalValue.distance - 0.03f) isCollsion = false;
            }
        }
        
        transform.position = moveTarget;
        transform.forward = lookPoint.position - transform.position;                
    }
}
