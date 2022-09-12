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
    static Animator playerAnimator;//觀看玩家Animator

    float totalVertical;//記錄垂直移動量
    float distance;//與玩家距離
    bool isCollsion;//是否碰撞

    [Header("SmoothDamp")]
    Vector3 velocity;
    [SerializeField] float smoothTime;

    [Header("速度")]
    public float rotateSpeed;//選轉速度
    public float lerpSpeed;//lerp速度
    

    [Header("等待時間")]
    [SerializeField] public float waitMoveTime; //等待移動時間
    float waitTime;//等待移動時間(計時器)    
    bool isWait;//是否有等待過

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

    private void Start()
    {
        transform.position = new Vector3(285,-13, -25);//初始位置

        lerpSpeed = 4f;//選轉速度
        rotateSpeed = 0.80f;//選轉速度
        waitMoveTime = 1;//等待移動時間

        //SmoothDamp
        velocity = Vector3.zero;
        smoothTime = 0.3f;
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
            playerAnimator = lookPoint.parent.GetComponent<Animator>();
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

        forwardVector = Quaternion.AngleAxis(mouseX * rotateSpeed, Vector3.up) * forwardVector;//前方向量

        //限制上下角度
        totalVertical += mouseY * rotateSpeed;//記錄垂直移動量
        if (totalVertical > NumericalValue.limitDownAngle) totalVertical = NumericalValue.limitDownAngle;
        if (totalVertical < -NumericalValue.limitUpAngle) totalVertical = -NumericalValue.limitUpAngle;

        Vector3 tempRotate = Vector3.Cross(Vector3.up, forwardVector);//獲取水平軸
        Vector3 RotateVector = Quaternion.AngleAxis(totalVertical, -tempRotate) * forwardVector;//最後選轉向量
        RotateVector.Normalize();

        Vector3 moveTarget = lookPoint.position - RotateVector * NumericalValue.distance;//移動目標
        //Vector3 moveTarget = Vector3.Lerp(transform.position, lookPoint.position - RotateVector * NumericalValue.distance, lerpSpeed);//攝影機靠近減速
        //攝影機障礙物偵測        
        LayerMask mask = LayerMask.GetMask("StageObject");
        if (Physics.SphereCast(lookPoint.position, 0.1f, -RotateVector, out RaycastHit hit, NumericalValue.distance, mask))
        {
            //lerpSpeed = 0.25f;//lerp速度
            if (!isCollsion) isCollsion = true;
            moveTarget = Vector3.Lerp(transform.position, lookPoint.position - RotateVector * hit.distance, lerpSpeed * Time.deltaTime);//攝影機靠近減速
            //moveTarget = lookPoint.position - RotateVector * hit.distance;//攝影機靠近

            //碰到"StageObject"攝影機移動位置改為(觀看物件位置 - 與碰撞物件距離)                                    
            /*if (distance < 0.5f && (lookPoint.transform.position - hit.transform.position).magnitude < 2.6f) moveTarget = lookPoint.position + Vector3.up * 0.49f;//距離玩家太近          
            else moveTarget = Vector3.Lerp(transform.position, lookPoint.position - RotateVector * hit.distance, lerpSpeed);//攝影機靠近減速*/
        }     
        else
        {
            if (isCollsion && distance < NumericalValue.distance)//攝影機離開障礙物減速
            {
                moveTarget = Vector3.Lerp(transform.position, lookPoint.position - RotateVector * NumericalValue.distance, lerpSpeed * Time.deltaTime);
            }
            else//一般狀態
            {
                //moveTarget = Vector3.Lerp(transform.position, lookPoint.position - RotateVector * NumericalValue.distance, lerpSpeed * Time.deltaTime);                
                //moveTarget = Vector3.SmoothDamp(transform.position, lookPoint.position - RotateVector * NumericalValue.distance, ref velocity, smoothTime);
            }
        }
        
        transform.position = moveTarget;
        transform.forward = lookPoint.position - transform.position; 
    }
}
