using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

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
    float addMoveSpeed;//增加移動速度值
    bool isSendRun;//是否已發送移動動畫       

    //跳躍
    public bool isJump;//是否跳躍
    bool isJumpTimeCountdown;//可執行跳躍倒數(倒數時不能跳躍)
    float doJumpTime;//執行跳躍時間間隔
    float JumpTime;//執行跳躍時間間隔(計時器)   
    bool isSendClosePain;//是否已關閉受傷動畫

    //閃躲
    bool isDodge;//是否閃躲
    bool isDodgeCollision;//是否閃躲碰撞

    //攻擊
    bool isNormalAttack;//是否普通攻擊        
    bool isSkillAttack;//是否技能攻擊    
    int normalAttackNumber;//普通攻擊編號
    public int GetNormalAttackNumber => normalAttackNumber;
    bool isJumpAttack;//是否跳躍攻擊
    public bool isJumpAttackMove;//跳躍攻擊下降

    private void Awake()
    {       
        gameObject.layer = LayerMask.NameToLayer("Player");//設定Layer                
        gameObject.tag = "Player";//設定Tag

        animator = GetComponent<Animator>();
        
        //if (GetComponent<CharactersCollision>() == null) gameObject.AddComponent<CharactersCollision>();
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

        //房主
        if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.AutomaticallySyncScene = false;//關閉自動同步場景
        }

        //設定攝影機觀看點
        CameraControl.SetLookPoint = ExtensionMethods.FindAnyChild<Transform>(transform, "CameraLookPoint");

        //小地圖攝影機
        GameObject miniMap_Camera = GameObject.Find("MiniMap_Camera");
        miniMap_Camera.transform.SetParent(transform);
        miniMap_Camera.transform.localPosition = new Vector3(0, 10, 0);               

        //碰撞框
        boxCenter = GetComponent<BoxCollider>().center;
        boxSize = GetComponent<BoxCollider>().size;

        //跳躍
        doJumpTime = 1.3f;//執行跳躍時間間隔
        JumpTime = doJumpTime;//執行跳躍時間間隔(計時器)

        //移動
        forwardVector = transform.forward;

        //鼠標
         Cursor.visible = false;//鼠標隱藏
         Cursor.lockState = CursorLockMode.Locked;//鎖定中央

        //Buff
        for (int i = 0; i < GameDataManagement.Instance.equipBuff.Length; i++)
        {
            switch(GameDataManagement.Instance.equipBuff[i])
            {               
                case 2://增加防禦值
                    charactersCollision.addDefence = GameDataManagement.Instance.numericalValue.buffAbleValue[2];
                    break;
                case 3://增加吸血效果增
                    charactersCollision.isSuckBlood = true;
                    break;
                case 4://加移動速度值
                    addMoveSpeed = NumericalValue.playerMoveSpeed * (GameDataManagement.Instance.numericalValue.buffAbleValue[3] / 100);                    
                    break;
                case 5://增加回復效果
                    charactersCollision.isSelfHeal = true;
                    break;
            }         
        }  
        
        /*//判斷關卡
        if(GameDataManagement.Instance.selectLevelNumber == 1)//測試用1 else = 0
        {
            //龍圍繞玩家
            Dragon_Level1 dragon_Level1 = GameObject.Find("Dragon_Around").GetComponent<Dragon_Level1>();
            dragon_Level1.SetRotateAroundTarger = transform;
        }*/
    }

    void Update()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        //不是死亡狀態 & 不是受擊動畫
        if (!charactersCollision.isDie && !info.IsName("Pain"))
        {
            //沒有開啟選項介面
            if (!GameSceneUI.Instance.isOptions)
            {
                OnJumpControl();
                OnAttackControl();

                if (!isJumpAttack && !isNormalAttack && !isSkillAttack && !info.IsName("Pain"))
                {
                    if (!info.IsName("Dodge")) OnMovementControl();
                    else
                    {
                        if (inputValue > 0)
                        {
                            inputValue = 0;
                            animator.SetFloat("Run", inputValue);
                        }
                    }

                   // OnDodgeControl();
                }

                if (!isJumpAttack && !isSkillAttack && !info.IsName("Pain"))
                {
                    OnDodgeControl();
                }
            }

            OnFallBehavior();
        }                

        OnJumpHehavior();
        OnInput();

        if (isJumpAttackMove) OnJumpAttackMove();

       /* if (Input.GetKeyDown(KeyCode.P))        
        {
            AI[] ai = GameObject.FindObjectsOfType<AI>();

            foreach (var a in ai)
            {
                Debug.LogError("s");
                a.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);//交換房主
            }
            
        }*/
    }

    /// <summary>
    /// 落下行為
    /// </summary>
    void OnFallBehavior()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName("Fall"))
        {
            if(isJump)//關閉跳落
            {                
                isJump = false;
                animator.SetBool("Jump", isJump);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Jump", isJump);
            }
            
            if(isJumpAttack)
            {
                isJumpAttack = false;
                animator.SetBool("JumpAttack", isJumpAttack);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "JumpAttack", isJumpAttack);
            }
        }
    }

    /// <summary>
    /// 攻擊控制
    /// </summary>
    void OnAttackControl()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        //普通攻擊
        if (Input.GetMouseButton(0) && !info.IsTag("SkillAttack") && !info.IsTag("SkillAttack-2") && !info.IsName("Fall") && !isDodge)
        {           
            //跳躍攻擊
            if (isJump && !isJumpAttack)
            {
                isJumpAttack = true;

                animator.SetBool("JumpAttack", isJumpAttack);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "JumpAttack", isJumpAttack);
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

            if(info.IsTag("NormalAttack") || info.IsTag("SkillAttack"))
            {
                if(info.normalizedTime < 0.05f)
                {
                    //轉向               
                    if (inputX != 0 && inputZ != 0) transform.forward = (horizontalCross * inputX) + (forwardVector * inputZ);//斜邊
                    else if (inputX != 0) transform.forward = horizontalCross * inputX;//左右
                    else if (inputZ != 0) transform.forward = forwardVector * inputZ;//前後
                    /*float maxRadiansDelta = 0.3f;//轉向角度
                    if (inputX != 0 && inputZ != 0) transform.forward = Vector3.RotateTowards(transform.forward, (horizontalCross * inputX) + (forwardVector * inputZ), maxRadiansDelta, maxRadiansDelta);//斜邊
                    else if (inputX != 0) transform.forward = Vector3.RotateTowards(transform.forward, horizontalCross * inputX, maxRadiansDelta, maxRadiansDelta);//左右
                    else if (inputZ != 0) transform.forward = Vector3.RotateTowards(transform.forward, forwardVector * inputZ, maxRadiansDelta, maxRadiansDelta);//前後*/
                }
            }

            //等待普通攻擊結束再執行技能
            if (isSkillAttack && info.IsTag("NormalAttack") && info.normalizedTime >= 1)
            {
                /*//轉向               
                if (inputX != 0 && inputZ != 0) transform.forward = (horizontalCross * inputX) + (forwardVector * inputZ);//斜邊
                else if (inputX != 0) transform.forward = horizontalCross * inputX;//左右
                else if (inputZ != 0) transform.forward = forwardVector * inputZ;//前後*/
                
                animator.SetBool("SkillAttack", isSkillAttack);
                if(GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "SkillAttack", isSkillAttack);
                return;
            }                           

            //普通攻擊(第一次攻擊)
            if (!isSkillAttack && !isNormalAttack )//&& !isJumpAttack)
            {
                isNormalAttack = true;
                normalAttackNumber = 1;                

                animator.SetBool("NormalAttack", isNormalAttack);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "NormalAttack", isNormalAttack);
            }                        

            //切換普通攻擊招式
            if (info.IsTag("NormalAttack") && info.normalizedTime >= 1)
            {                                
                normalAttackNumber++;//普通攻擊編號                
                if (normalAttackNumber > 3) normalAttackNumber = 0;
                
                /*//轉向               
                if (inputX != 0 && inputZ != 0) transform.forward = (horizontalCross * inputX) + (forwardVector * inputZ);//斜邊
                else if (inputX != 0) transform.forward = horizontalCross * inputX;//左右
                else if (inputZ != 0) transform.forward = forwardVector * inputZ;//前後*/

                animator.SetInteger("NormalAttackNumber", normalAttackNumber);
                if (GameDataManagement.Instance.isConnect)  PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "NormalAttackNumber", normalAttackNumber);
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
                    if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "NormalAttackNumber", normalAttackNumber);

                    if (info.IsTag("NormalAttack"))
                    {                        
                        animator.SetBool("NormalAttack", isNormalAttack);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "NormalAttack", isNormalAttack);
                    }

                    if (info.IsTag("SkillAttack"))
                    {
                        animator.SetBool("SkillAttack", isSkillAttack);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "SkillAttack", isSkillAttack);
                    }

                    if (info.IsTag("SkillAttack-2"))
                    {
                        animator.SetBool("SkillAttack", isSkillAttack);
                        animator.SetBool("SkillAttack-2", false);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "SkillAttack", isSkillAttack);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "SkillAttack-2", false);
                    }                    
                }              
            }

            if (info.IsTag("JumpAttack") && isNormalAttack)
            {
                isNormalAttack = false;

                animator.SetBool("NormalAttack", isNormalAttack);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "NormalAttack", isNormalAttack);
            }
        }

        if (info.IsTag("JumpAttack") && info.normalizedTime >= 1)
        {
            isJump = false;            
            isJumpAttack = false;

            animator.SetBool("JumpAttack", isJumpAttack);
            animator.SetBool("Jump", isJump);
            
            if (GameDataManagement.Instance.isConnect)
            {
                PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "JumpAttack", isJumpAttack);
                PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Jump", isJump);                
            }
        }

        //攻擊中關閉閃躲
        if((info.IsTag("NormalAttack") && info.normalizedTime < 0.5f) && isDodge)
        {
            isDodge = false;

            animator.SetBool("Dodge", false);
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Dodge", false);
        }

        //技能攻擊中關閉普通攻擊
        if (isNormalAttack && info.normalizedTime > 0.35f && info.IsTag("SkillAttack") || info.IsTag("SkillAttack-2"))
        {
            isNormalAttack = false;
            isNormalAttack = false;

            animator.SetBool("NormalAttack", isNormalAttack);
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "NormalAttack", isNormalAttack);
        }           
    }  

    /// <summary>
    /// 閃躲控制
    /// </summary>
    void OnDodgeControl()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);
        
        //閃躲控制
        if (info.IsName("Idle") || info.IsName("Run") || (info.IsTag("NormalAttack") && info.normalizedTime > 0.6f))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                isDodge = true;
                isDodgeCollision = false;

                for (int i = 0; i < charactersCollision.GetCollisionObject.Length; i++)
                {
                    //判斷是否已經碰牆
                    if (isDodgeCollision = charactersCollision.GetCollisionObject[i]) break;                    
                }                
       
                if(info.IsTag("NormalAttack"))
                {
                    normalAttackNumber = 0;//普通攻擊編號
                    isNormalAttack = false;

                    animator.SetInteger("NormalAttackNumber", normalAttackNumber);
                    animator.SetBool("NormalAttack", isNormalAttack);
                    if (GameDataManagement.Instance.isConnect)
                    {
                        PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "NormalAttack", isNormalAttack);
                        PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "NormalAttackNumber", normalAttackNumber);
                    }
                }

                animator.SetBool("Dodge", true);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Dodge", true);                 
            }
        }

        //閃躲移動
        if (info.IsName("Dodge") && info.normalizedTime < 1)
        {
            LayerMask mask = LayerMask.GetMask("StageObject");

            //判斷是否有碰牆
            if (Physics.Raycast(transform.position + boxCenter, transform.forward, boxSize.z * boxSize.z, mask)) isDodgeCollision = true;//閃躲碰撞

            if (isDodgeCollision) transform.position = transform.position - transform.forward * 5 * Time.deltaTime;
            else transform.position = transform.position + transform.forward * GameDataManagement.Instance.numericalValue.playerDodgeSeppd * Time.deltaTime;

            //普通攻擊中
            if(isNormalAttack)
            {
                normalAttackNumber = 0;//普通攻擊編號
                isNormalAttack = false;

                animator.SetInteger("NormalAttackNumber", normalAttackNumber);
                animator.SetBool("NormalAttack", isNormalAttack);
                if (GameDataManagement.Instance.isConnect)
                {
                    PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "NormalAttack", isNormalAttack);
                    PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "NormalAttackNumber", normalAttackNumber);
                }
            }
        }

        //閃躲結束
        if (info.IsName("Dodge") && info.normalizedTime > 1)
        {
            isDodge = false;

            animator.SetBool("Dodge", false);
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Dodge", false);

            animator.SetBool("Pain", false);
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Pain", false);
        }
    }

    /// <summary>
    /// 跳躍控制
    /// </summary>
    void OnJumpControl()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumpTimeCountdown && !isJump && !isNormalAttack && !isSkillAttack && !info.IsName("Dodge") && !info.IsName("Fall") && !info.IsName("Pain"))
        {            
            isJump = true;
            isNormalAttack = false;
            isJumpTimeCountdown = true;//可執行跳躍倒數                        
            isSendClosePain = false;//是否已關閉受傷動畫

            charactersCollision.floating_List.Add(new CharactersFloating { target = transform, force = NumericalValue.playerJumpForce, gravity = NumericalValue.gravity});//浮空List

            animator.SetBool("NormalAttack", isNormalAttack);
            animator.SetBool("Jump", isJump);
            if (GameDataManagement.Instance.isConnect)
            {
                PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "NormalAttack", isNormalAttack);
                PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Jump", isJump);
            }
        }
    }

    /// <summary>
    /// 跳躍行為
    /// </summary>
    void OnJumpHehavior()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        //跳躍計時器倒數
        if (isJumpTimeCountdown)
        {
            JumpTime -= Time.deltaTime;//跳躍計時器

            if (JumpTime <= 0)
            {
                JumpTime = doJumpTime;//獲取執行跳躍時間間隔 
                isJumpTimeCountdown = false;
            }
        }

        //獲取執行跳躍時間間隔
        if (info.IsName("Jump"))
        {         
            if (doJumpTime != animator.GetCurrentAnimatorClipInfo(0).FirstOrDefault(x => x.clip.name == "Jump").clip.length + 0.4f) doJumpTime = animator.GetCurrentAnimatorClipInfo(0).FirstOrDefault(x => x.clip.name == "Jump").clip.length + 0.4f;            
        }

        if (info.IsTag("Jump") || info.IsTag("JumpAttack"))
        {           
            LayerMask mask = LayerMask.GetMask("StageObject");
            RaycastHit hit;
            if (charactersCollision.OnCollision_Floor(out hit))
            {
                if ((isJump || isJumpAttack) && info.normalizedTime > 0.9f)
                {
                    if (isJump)
                    {
                        isJump = false;
                        animator.SetBool("Jump", isJump);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Jump", isJump);
                    }
                    if (isJumpAttack) isJumpAttackMove = false;
                }
            }        
        }

        //跳躍時受攻擊
        if(isJump && info.IsTag("Pain") && !isSendClosePain)
        {            
            isSendClosePain = true;//是否已關閉受傷動畫
            animator.SetBool("Pain", false);
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Pain", false);
        }
    }

    /// <summary>
    /// 跳躍攻擊移動
    /// </summary>
    public void OnJumpAttackMove()
    {
        transform.position = transform.position + 10 * Time.deltaTime * Vector3.down;//急速下降
    }

    /// <summary>
    /// 移動控制
    /// </summary>
    void OnMovementControl()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        if (!info.IsName("JumpAttack"))
        {
            //轉向
            float maxRadiansDelta = 0.1405f;//轉向角度
            if (inputX != 0 && inputZ != 0) transform.forward = Vector3.RotateTowards(transform.forward, (horizontalCross * inputX) + (forwardVector * inputZ), maxRadiansDelta, maxRadiansDelta);//斜邊
            else if (inputX != 0) transform.forward = Vector3.RotateTowards(transform.forward, horizontalCross * inputX, maxRadiansDelta, maxRadiansDelta);//左右
            else if (inputZ != 0) transform.forward = Vector3.RotateTowards(transform.forward, forwardVector * inputZ, maxRadiansDelta, maxRadiansDelta);//前後      
        }               

        inputValue = Mathf.Abs(inputX) + Mathf.Abs(inputZ);//輸入值
        if (inputValue > 1) inputValue = 1;

        animator.SetFloat("Run", inputValue);
        if (GameDataManagement.Instance.isConnect)
        {
            if (!info.IsName("Fall"))
            {
                if (inputValue > 0.1f && !isSendRun)
                {
                    isSendRun = true;
                    PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Run", inputValue);
                }
                if (inputValue < 0.1f && isSendRun)
                {
                    isSendRun = false;
                    PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Run", 0.0f);
                }
            }
        }

        RaycastHit hit;
        if (info.IsName("Jump"))
        {
            if(charactersCollision.OnCollision_Floor(out hit))
            {
                if (hit.transform.tag == "Stairs") inputValue = (Mathf.Abs(inputX) + Mathf.Abs(inputZ)) * 0.8f;//輸入值
            }      
        }
  
        //移動
        transform.position = transform.position + transform.forward * inputValue * (NumericalValue.playerMoveSpeed + addMoveSpeed) * Time.deltaTime;

        if (inputValue > 0) charactersCollision.OnCollision_Characters(out hit);//碰撞框_腳色
    }           

    /// <summary>
    /// 輸入值
    /// </summary>
    void OnInput()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        inputX = Input.GetAxis("Horizontal");//輸入X值
        inputZ = Input.GetAxis("Vertical");//輸入Z值

        forwardVector = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * CameraControl.Instance.rotateSpeed, Vector3.up) * forwardVector;//前方向量
        horizontalCross = Vector3.Cross(Vector3.up, forwardVector);//水平軸      

        //開啟介面
        if(GameSceneUI.Instance.isOptions && info.IsName("Run"))
        {
            animator.SetFloat("Run", 0);
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Run", 0.0f);
        }

        //滑鼠
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Cursor.visible = !Cursor.visible;//鼠標 顯示/隱藏
            if (!Cursor.visible) Cursor.lockState = CursorLockMode.Locked;//鎖定中央
            else Cursor.lockState = CursorLockMode.None;
        }
    }

    public float gizmosSpherCenter;
    public float  gizmosSpherRadius;
    private void OnDrawGizmos()
    {
        BoxCollider box = GetComponent<BoxCollider>();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + box.center + transform.forward * gizmosSpherCenter, gizmosSpherRadius);        
        //Gizmos.DrawWireCube(transform.position + box.center + transform.forward * 5f, new Vector3(1, 1, 10));
        
    }
}
