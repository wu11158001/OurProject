using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// 玩家控制
/// </summary>
public class PlayerControl : MonoBehaviourPunCallbacks
{
    Animator animator;
    AnimatorStateInfo info;
    CharactersCollision charactersCollision;
    GameData_NumericalValue NumericalValue;    

    //碰撞框
    Vector3 boxCenter;
    Vector3 boxSize;

    //移動
    float inputX;//輸入X值
    float inputZ;//輸入Z值
    Vector3 forwardVector;//前方向量
    Vector3 horizontalCross;//水平軸
    bool isSendRun;//是否已發送移動動畫

    //跳躍
    bool isJump;//是否跳躍

    //攻擊
    bool isNormalAttack;//是否普通攻擊    
    bool isJumpAttack;//是否跳躍攻擊
    bool isSkillAttack;//是否技能攻擊    
    int normalAttackNumber;//普通攻擊編號
    public int GetNormalAttackNumber => normalAttackNumber;

    private void Awake()
    {       
        gameObject.layer = LayerMask.NameToLayer("Player");//設定Layer                
        gameObject.tag = "Player";//設定Tag

        animator = GetComponent<Animator>();
        
        if (GetComponent<CharactersCollision>() == null) gameObject.AddComponent<CharactersCollision>();
        charactersCollision = GetComponent<CharactersCollision>();

        //連線 && 不是自己的
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            GameSceneManagement.Instance.OnSetMiniMapPoint(transform, GameSceneManagement.Instance.loadPath.miniMapMatirial_OtherPlayer);//設定小地圖點點
            this.enabled = false;
            return;
        }
    }

    void Start()
    {
        NumericalValue = GameDataManagement.Instance.numericalValue;

        //設定攝影機觀看點
        CameraControl.SetLookPoint = ExtensionMethods.FindAnyChild<Transform>(transform, "CameraLookPoint");

        //小地圖攝影機
        GameObject miniMap_Camera = GameObject.Find("MiniMap_Camera");
        miniMap_Camera.transform.SetParent(transform);
        miniMap_Camera.transform.localPosition = new Vector3(0, 10, 0);               

        //碰撞框
        boxCenter = GetComponent<BoxCollider>().center;
        boxSize = GetComponent<BoxCollider>().size;

        //移動
        forwardVector = transform.forward;

        //鼠標
         Cursor.visible = false;//鼠標隱藏
         Cursor.lockState = CursorLockMode.Locked;//鎖定中央
    }

    void Update()
    {
        //不是死亡狀態 & 沒有開啟選項介面
        if (!charactersCollision.isDie && !GameSceneUI.Instance.isOptions)
        {            
            OnJumpControl();
            OnAttackControl();
            OnJumpBehavior();

            if (!isNormalAttack && !isSkillAttack && !info.IsName("Pain"))
            {
                OnMovementControl();
                OnDodgeControl();
            } 
        }

        OnInput();
    }        

    /// <summary>
    /// 攻擊控制
    /// </summary>
    void OnAttackControl()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        //普通攻擊
        if (Input.GetMouseButton(0) && !info.IsTag("SkillAttack") && !info.IsTag("SkillAttack-2") && !info.IsName("Dodge"))
        {
            //技能攻擊
            if (Input.GetMouseButtonDown(1))
            {
                //普通攻擊時間內按下
                if (info.IsTag("NormalAttack") && info.normalizedTime < 1)
                {                                       
                    isSkillAttack = true;                    
                }
            }

            //等待普通攻擊結束再執行技能
            if (isSkillAttack && info.IsTag("NormalAttack") && info.normalizedTime >= 1)
            {
                //轉向               
                if (inputX != 0 && inputZ != 0) transform.forward = (horizontalCross * inputX) + (forwardVector * inputZ);//斜邊
                else if (inputX != 0) transform.forward = horizontalCross * inputX;//左右
                else if (inputZ != 0) transform.forward = forwardVector * inputZ;//前後
                
                animator.SetBool("SkillAttack", isSkillAttack);
                if(GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "SkillAttack", isSkillAttack);
                return;
            }            

            //跳躍攻擊
            if (isJump)
            {
                isJumpAttack = true;

                animator.SetBool("JumpAttack", isJumpAttack);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "JumpAttack", isJumpAttack);
                return;
            }                        

            //普通攻擊(第一次攻擊)
            if (!isSkillAttack && !isNormalAttack)
            {
                isNormalAttack = true;
                normalAttackNumber = 1;                

                animator.SetBool("NormalAttack", isNormalAttack);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "NormalAttack", isNormalAttack);
            }

            //切換普通攻擊招式
            if (info.IsTag("NormalAttack") && info.normalizedTime >= 1)
            {                                
                normalAttackNumber++;//普通攻擊編號                
                if (normalAttackNumber > 3) normalAttackNumber = 0;
                
                //轉向               
                if (inputX != 0 && inputZ != 0) transform.forward = (horizontalCross * inputX) + (forwardVector * inputZ);//斜邊
                else if (inputX != 0) transform.forward = horizontalCross * inputX;//左右
                else if (inputZ != 0) transform.forward = forwardVector * inputZ;//前後

                animator.SetInteger("NormalAttackNumber", normalAttackNumber);
                if (GameDataManagement.Instance.isConnect)  PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "NormalAttackNumber", normalAttackNumber);
            }          
        }       
        else
        {
            //動畫/攻擊結束
            if (info.normalizedTime >= 1)
            {                                
                if (info.IsTag("NormalAttack") || info.IsTag("SkillAttack") || info.IsTag("SkillAttack-2") || info.IsTag("JumpAttack"))
                {                    
                    normalAttackNumber = 0;//普通攻擊編號                    
                    isNormalAttack = false;
                    isSkillAttack = false;
                    isJumpAttack = false;

                    animator.SetInteger("NormalAttackNumber", normalAttackNumber);
                    if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "NormalAttackNumber", normalAttackNumber);

                    if (info.IsTag("NormalAttack"))
                    {
                        animator.SetBool("NormalAttack", isNormalAttack);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "NormalAttack", isNormalAttack);
                    }

                    if (info.IsTag("SkillAttack"))
                    {
                        animator.SetBool("SkillAttack", isSkillAttack);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "SkillAttack", isSkillAttack);
                    }

                    if (info.IsTag("SkillAttack-2"))
                    {
                        animator.SetBool("SkillAttack", isSkillAttack);
                        animator.SetBool("SkillAttack-2", false);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "SkillAttack", isSkillAttack);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "SkillAttack-2", false);
                    }

                    if (info.IsTag("JumpAttack"))
                    {                        
                        animator.SetBool("JumpAttack", isJumpAttack);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "JumpAttack", isJumpAttack);
                    }  
                }              
            }
        }     

        //技能攻擊中關閉普通攻擊
        if(isNormalAttack && info.normalizedTime > 0.35f && info.IsTag("SkillAttack") || info.IsTag("SkillAttack-2"))
        {
            isNormalAttack = false;

            animator.SetBool("NormalAttack", isNormalAttack);
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "NormalAttack", isNormalAttack);
        }           
    }      
    
    /// <summary>
    /// 閃躲控制
    /// </summary>
    void OnDodgeControl()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        //閃躲控制
        if (info.IsName("Idle") || info.IsName("Run"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                animator.SetBool("Dodge", true);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Dodge", true);
            }
        }

        //閃躲移動
        if (info.IsName("Dodge") && info.normalizedTime < 1)
        {
            transform.position = transform.position + transform.forward * GameDataManagement.Instance.numericalValue.playerDodgeSeppd * Time.deltaTime;
        }

            //閃躲結束
        if (info.IsName("Dodge") && info.normalizedTime > 1)
        {
            animator.SetBool("Dodge", false);
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Dodge", false);       
        }
    }

    /// <summary>
    /// 跳躍行為
    /// </summary>
    void OnJumpBehavior()
    {       
        if (isJump) StartCoroutine(OnWaitJump());          
    }

    /// <summary>
    /// 等待跳躍(避免無法觸發動畫)
    /// </summary>
    /// <returns></returns>
    IEnumerator OnWaitJump()
    {
        yield return new WaitForSeconds(0.1f);

        //碰撞偵測
        LayerMask mask = LayerMask.GetMask("StageObject");
        if (Physics.CheckBox(transform.position + boxCenter, new Vector3(boxSize.x / 4, boxSize.y / 2, boxSize.z / 4), Quaternion.identity, mask))
        {
            isJump = false;

            animator.SetBool("Jump", isJump);
            animator.SetBool("JumpAttack", isJump);
            if (GameDataManagement.Instance.isConnect)
            {
                PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Jump", isJump);
                PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "JumpAttack", isJump);
            }
        }
    }

    /// <summary>
    /// 跳躍控制
    /// </summary>
    void OnJumpControl()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (Input.GetKeyDown(KeyCode.Space) && !isJump && !isNormalAttack && !isSkillAttack && !info.IsName("Dodge"))
        {
            charactersCollision.floating_List.Add(new CharactersFloating { target = transform, force = NumericalValue.playerJumpForce, gravity = NumericalValue.gravity });//浮空List

            isJump = true;         
            isNormalAttack = false;
            animator.SetBool("NormalAttack", isNormalAttack);
            animator.SetBool("Jump", isJump);
            if (GameDataManagement.Instance.isConnect)
            {
                PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "NormalAttack", isNormalAttack);
                PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Jump", isJump);
            }
        }        
    }
    
    /// <summary>
    /// 移動控制
    /// </summary>
    void OnMovementControl()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (!info.IsName("JumpAttack"))
        {
            //轉向
            float maxRadiansDelta = 0.075f;//轉向角度
            if (inputX != 0 && inputZ != 0) transform.forward = Vector3.RotateTowards(transform.forward, (horizontalCross * inputX) + (forwardVector * inputZ), maxRadiansDelta, maxRadiansDelta);//斜邊
            else if (inputX != 0) transform.forward = Vector3.RotateTowards(transform.forward, horizontalCross * inputX, maxRadiansDelta, maxRadiansDelta);//左右
            else if (inputZ != 0) transform.forward = Vector3.RotateTowards(transform.forward, forwardVector * inputZ, maxRadiansDelta, maxRadiansDelta);//前後      
        }
        if (info.IsName("Dodge")) return;

        float inputValue = Mathf.Abs(inputX) + Mathf.Abs(inputZ);//輸入值
        
        //移動
        if (inputValue > 1) inputValue = 1;
        transform.position = transform.position + transform.forward * inputValue * NumericalValue.playerMoveSpeed * Time.deltaTime;

        animator.SetFloat("Run", inputValue);
        if (GameDataManagement.Instance.isConnect)
        {
            if (inputValue > 0.1f && !isSendRun)
            {
                isSendRun = true;
                PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Run", inputValue);
            }
            if (inputValue < 0.1f && isSendRun)
            {
                isSendRun = false;
                PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Run", 0.0f);
            }
        }
    }           

    /// <summary>
    /// 輸入值
    /// </summary>
    void OnInput()
    {        
        inputX = Input.GetAxis("Horizontal");//輸入X值
        inputZ = Input.GetAxis("Vertical");//輸入Z值

        forwardVector = Quaternion.AngleAxis(Input.GetAxis("Mouse X"), Vector3.up) * forwardVector;//前方向量
        horizontalCross = Vector3.Cross(Vector3.up, forwardVector);//水平軸       

        //滑鼠
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Cursor.visible = !Cursor.visible;//鼠標 顯示/隱藏
            if (!Cursor.visible) Cursor.lockState = CursorLockMode.Locked;//鎖定中央
            else Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnDrawGizmos()
    {
        /*BoxCollider box = GetComponent<BoxCollider>();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + box.center + transform.forward * 3, 1.8f);        
        Gizmos.DrawWireCube(transform.position + box.center, new Vector3(box.size.x /1.3f, box.size.y, box.size.z/1.3f));*/
    }
}
