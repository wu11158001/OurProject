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
    float inputValue;//總輸入值
    Vector3 forwardVector;//前方向量
    Vector3 horizontalCross;//水平軸
    bool isSendRun;//是否已發送移動動畫       

    //跳躍
    bool isJump;//是否跳躍
    Vector3 jumpForward;//跳躍前方向量
    bool isRunJump;//跳躍前是否向前

    //攻擊
    bool isNormalAttack;//是否普通攻擊        
    bool isSkillAttack;//是否技能攻擊    
    int normalAttackNumber;//普通攻擊編號
    public int GetNormalAttackNumber => normalAttackNumber;
    bool isJumpAttack;//是否跳躍攻擊
    public bool isJumpAttackDown;//跳躍攻擊下降


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
        if (!charactersCollision.isDie && !GameSceneUI.Instance.isOptions && !info.IsName("Pain"))
        {            
            OnJumpControl();
            OnAttackControl();

            if (!isJumpAttack && !isNormalAttack && !isSkillAttack && !info.IsName("Pain"))
            {
                if(!info.IsName("Dodge")) OnMovementControl();
                else
                {
                    if (inputValue > 0)
                    {
                        inputValue = 0;
                        animator.SetFloat("Run", inputValue);
                    }
                }
                
                OnDodgeControl();
            } 
            
        }

        OnJumpHehavior();
        OnInput();

        if (isJumpAttackDown) OnJumpAttackMove();
    }

    
    public void OnJumpAttackMove()
    {
        transform.position = transform.position + Vector3.down * 20 * Time.deltaTime;//急速下降
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
            //跳躍攻擊
            if (isJump && !isJumpAttack)
            {
                isJumpAttack = true;

                animator.SetBool("JumpAttack", isJumpAttack);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "JumpAttack", isJumpAttack);
                return;
            }

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

            //普通攻擊(第一次攻擊)
            if (!isSkillAttack && !isNormalAttack && !isJumpAttack)
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

                    
                }              
            }
        }

        if (info.IsTag("JumpAttack") && info.normalizedTime >= 1)
        {
            isJump = false;
            isNormalAttack = false;
            isJumpAttack = false;

            animator.SetBool("JumpAttack", isJumpAttack);
            animator.SetBool("Jump", isJump);
            animator.SetBool("NormalAttack", isJump);
            if (GameDataManagement.Instance.isConnect)
            {
                PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "JumpAttack", isJumpAttack);
                PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Jump", isJump);
                PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "NormalAttack", isNormalAttack);
            }
        }

        //技能攻擊中關閉普通攻擊
        if (isNormalAttack && info.normalizedTime > 0.35f && info.IsTag("SkillAttack") || info.IsTag("SkillAttack-2"))
        {
            isNormalAttack = false;
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
            //transform.position = transform.position + transform.forward * GameDataManagement.Instance.numericalValue.playerDodgeSeppd * Time.deltaTime;
        }

            //閃躲結束
        if (info.IsName("Dodge") && info.normalizedTime > 1)
        {
            animator.SetBool("Dodge", false);
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Dodge", false);       
        }
    }

    /// <summary>
    /// 跳躍控制
    /// </summary>
    void OnJumpControl()
    {        
        if (Input.GetKeyDown(KeyCode.Space) && !isJump && !isNormalAttack && !isSkillAttack && !info.IsName("Dodge"))
        {
            //先向上一點
            transform.position = transform.position + Vector3.up * NumericalValue.playerJumpForce * Time.deltaTime;

            jumpForward = transform.forward;//跳躍前方向量
            if (inputValue != 0) isRunJump = true;
            
            isJump = true;
            isNormalAttack = false;

            if (charactersCollision.floating_List.Count > 0) charactersCollision.floating_List.Clear();
            charactersCollision.floating_List.Add(new CharactersFloating { target = transform, force = NumericalValue.playerJumpForce, gravity = NumericalValue.gravity });//浮空List
            
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
    /// 跳躍行為
    /// </summary>
    void OnJumpHehavior()
    {      
        if (info.IsTag("Jump") && info.normalizedTime > 0.5f || info.IsTag("JumpAttack"))
        {
            float boxCollisionDistance = boxSize.x < boxSize.z ? boxSize.x / 2 : boxSize.z / 2;
            LayerMask mask = LayerMask.GetMask("StageObject");
            RaycastHit hit;
            if (Physics.BoxCast(transform.position + Vector3.up * boxSize.y, new Vector3(boxCollisionDistance - 0.06f, 0.01f, boxCollisionDistance - 0.06f), -transform.up, out hit, Quaternion.Euler(transform.localEulerAngles), boxSize.y + 0.3f, mask))
            {
                if (isJump || isJumpAttack)
                {
                    
                    isRunJump = false;                
                    charactersCollision.floating_List.Clear();

                    if (isJump)
                    {
                        isJump = false;
                        animator.SetBool("Jump", isJump);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Jump", isJump);
                    }
                    if (isJumpAttack) isJumpAttackDown = false;                   
                }
            }
        }   
    }       
     
    /// <summary>
    /// 移動控制
    /// </summary>
    void OnMovementControl()
    {        
        if (!info.IsName("JumpAttack"))
        {
            //轉向
            float maxRadiansDelta = 0.15f;//轉向角度
            if (inputX != 0 && inputZ != 0) transform.forward = Vector3.RotateTowards(transform.forward, (horizontalCross * inputX) + (forwardVector * inputZ), maxRadiansDelta, maxRadiansDelta);//斜邊
            else if (inputX != 0) transform.forward = Vector3.RotateTowards(transform.forward, horizontalCross * inputX, maxRadiansDelta, maxRadiansDelta);//左右
            else if (inputZ != 0) transform.forward = Vector3.RotateTowards(transform.forward, forwardVector * inputZ, maxRadiansDelta, maxRadiansDelta);//前後      
        }               

        inputValue = Mathf.Abs(inputX) + Mathf.Abs(inputZ);//輸入值
        if (inputValue > 1) inputValue = 1;

        if (isJump)
        {
            if (isRunJump) inputValue = Mathf.Abs(inputX) + Mathf.Abs(inputZ);//輸入值            
            if (inputValue > 1) inputValue = 1;
            if (inputValue < 0) inputValue = 0;

            transform.position = transform.position + jumpForward * inputValue * NumericalValue.playerMoveSpeed * Time.deltaTime;
            return;
        }

        //移動
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
        info = animator.GetCurrentAnimatorStateInfo(0);

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
        Gizmos.DrawWireSphere(transform.position + box.center + transform.forward * 0, 1.3f);        
        Gizmos.DrawWireCube(transform.position + box.center + transform.forward * 2.5f, new Vector3(1, 1, 4));*/
        
    }
}
