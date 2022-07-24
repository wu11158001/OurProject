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
    CharactersCollision charactersCollision;
    GameData_NumericalValue NumericalValue;

    //碰撞框
    Vector3 boxCenter;
    Vector3 boxSize;

    //移動
    bool isLockMove;//是否鎖住移動
    float inputX;//輸入X值
    float inputZ;//輸入Z值
    Vector3 forwardVector;//前方向量
    Vector3 horizontalCross;//水平軸

    //跳躍
    bool isJump;//是否跳躍

    //普通攻擊
    bool isNormalAttack;//是否普通攻擊
    int normalAttackNumber;//普通攻擊編號

    //跳躍攻擊
    bool isJumpAttack;//是否跳躍攻擊

    //技能攻擊
    bool isSkillAttack;//是否技能攻擊

    private void Awake()
    {       
        gameObject.layer = LayerMask.NameToLayer("Player");//設定Layer                

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

        //鼠標
       /* Cursor.visible = false;//鼠標隱藏
        Cursor.lockState = CursorLockMode.Locked;//鎖定中央*/

        //碰撞框
        boxCenter = GetComponent<BoxCollider>().center;
        boxSize = GetComponent<BoxCollider>().size;

        //移動
        forwardVector = transform.forward;               
    }
   
    void Update()
    {        
        OnInput();
        OnJumpControl();
        OnAttackControl();
        OnJumpBehavior();

        if (!isNormalAttack && !isSkillAttack)
        {
            OnMovementControl();            
        }  
    }

    /// <summary>
    /// 技能攻擊行為
    /// </summary>
    void OnSkillAttackBehavior()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        AttackBehavior attack = AttackBehavior.Instance;
        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率

        //判斷目前普通攻擊編號
        switch (normalAttackNumber)
        {            
            case 1://技能1
                GameObject obj = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber("playerSkill_1_Numbering"), GameSceneManagement.Instance.loadPath.playerCharactersSkill_1);//產生物件
                obj.transform.position = transform.position + boxCenter;
                
                //設定AttackBehavior Class數值                
                attack.function = new Action(attack.OnSetShootFunction);//設定執行函式
                attack.performObject = obj;//執行攻擊的物件(自身/射出物件) 
                attack.speed = NumericalValue.playerSkillAttack_1_FlyingSpeed;//飛行速度
                attack.diration = transform.forward;//飛行方向
                attack.lifeTime = NumericalValue.playerSkillAttack_1_LifeTime;//生存時間                                                                                             
                attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
                attack.damage = NumericalValue.playerSkillAttack_1_Damage * rate;//造成傷害
                attack.animationName = NumericalValue.playerSkillAttack_1_Effect;//攻擊效果(受擊者播放的動畫名稱)
                attack.direction = NumericalValue.playerSkillAttack_1_RepelDirection;//擊退方向((0:擊退 1:擊飛))
                attack.repel = NumericalValue.playerSkillAttack_1_Repel;//擊退距離
                attack.isCritical = isCritical;//是否爆擊
                GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)             
                break;
            case 2://技能2               
                attack.function = new Action(attack.OnSetHitFunction);//設定執行函式
                attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
                attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
                attack.damage = NumericalValue.playerSkillAttack_2_Damage * rate;//造成傷害 
                attack.animationName = NumericalValue.playerSkillAttack_2_Effect;//攻擊效果(播放動畫名稱)
                attack.direction = NumericalValue.playerSkillAttack_2_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
                attack.repel = NumericalValue.playerSkillAttack_2_Repel;//擊退距離
                attack.boxSize = NumericalValue.playerSkillAttack_2_BoxSize * transform.lossyScale.x;//近身攻擊框Size
                attack.isCritical = isCritical;//是否爆擊
                GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)                   
                break;
            case 3://技能3                
                attack.function = new Action(attack.OnSetHitFunction);//設定執行函式
                attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
                attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
                attack.damage = NumericalValue.playerSkillAttack_3_Damage * rate;//造成傷害 
                attack.animationName = NumericalValue.playerSkillAttack_3_Effect;//攻擊效果(播放動畫名稱)
                attack.direction = NumericalValue.playerSkillAttack_3_RepelDirection;//擊退方向(0:擊退, 1:擊飛)
                attack.repel = NumericalValue.playerSkillAttack_3_Repel;//擊退距離
                attack.boxSize = NumericalValue.playerSkillAttack_3_BoxSize * transform.lossyScale.x;//近身攻擊框Size
                attack.isCritical = isCritical;//是否爆擊
                GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)     
                break;
        }
    }
 
    /// <summary>
    /// 跳躍攻擊行為
    /// </summary>
    void OnJumpAttackBehavior()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率

        //設定AttackBehavior Class數值
        AttackBehavior attack = AttackBehavior.Instance;
        attack.function = new Action(attack.OnSetHitFunction);//設定執行函式
        attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
        attack.damage = NumericalValue.playerJumpAttackDamage * rate;//造成傷害 
        attack.animationName = NumericalValue.playerJumpAttackEffect;//攻擊效果(播放動畫名稱)
        attack.direction = NumericalValue.playerJumpAttackRepelDirection;//擊退方向(0:擊退, 1:擊飛)
        attack.repel = NumericalValue.playerJumpAttackRepelDistance;//擊退距離
        attack.boxSize = NumericalValue.playerJumpAttackBoxSize * transform.lossyScale.x;//近身攻擊框Size
        attack.isCritical = isCritical;//是否爆擊
        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)   
    }

    /// <summary>
    /// 普通攻擊行為
    /// </summary>
    void OnNormalAttackBehavior()
    {
        //連線模式
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        //攻擊移動
         transform.position = transform.position + transform.forward * NumericalValue.playerNormalAttackMoveDistance[normalAttackNumber - 1] * Time.deltaTime;

         bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//是否爆擊
         float rate = isCritical ? NumericalValue.criticalBonus : 1;//爆擊攻擊提升倍率

         //設定AttackBehavior Class數值
         AttackBehavior attack = AttackBehavior.Instance;
         attack.function = new Action(attack.OnSetHitFunction);//設定執行函式
         attack.performObject = gameObject;//執行攻擊的物件(自身/射出物件)                                                                                            
         attack.layer = LayerMask.LayerToName(gameObject.layer);//攻擊者layer
         attack.damage = NumericalValue.playerNormalAttackDamge[normalAttackNumber - 1] * rate;//造成傷害 
         attack.animationName = NumericalValue.playerNormalAttackEffect[normalAttackNumber - 1];//攻擊效果(播放動畫名稱)
         attack.direction = NumericalValue.playerNormalAttackRepelDirection[normalAttackNumber - 1];//擊退方向(0:擊退, 1:擊飛)
         attack.repel = NumericalValue.playerNormalAttackRepelDistance[normalAttackNumber - 1];//擊退距離
         attack.boxSize = NumericalValue.playerNormalAttackBoxSize[normalAttackNumber - 1] * transform.lossyScale.x;//近身攻擊框Size
         attack.isCritical = isCritical;//是否爆擊
         GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//加入List(執行)           
    }    

    /// <summary>
    /// 攻擊控制
    /// </summary>
    void OnAttackControl()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        //普通攻擊
        if (Input.GetMouseButton(0) && !info.IsTag("SkillAttack"))
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
                if (info.IsTag("NormalAttack") || info.IsTag("SkillAttack") || info.IsTag("JumpAttack"))
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

                    if (info.IsTag("JumpAttack"))
                    {                        
                        animator.SetBool("JumpAttack", isJumpAttack);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "JumpAttack", isJumpAttack);
                    }  
                }              
            }
        }     

        //技能攻擊中關閉普通攻擊
        if(isNormalAttack && info.IsTag("SkillAttack") && info.normalizedTime > 0.35f)
        {
            isNormalAttack = false;

            animator.SetBool("NormalAttack", isNormalAttack);
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "NormalAttack", isNormalAttack);
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
            isLockMove = false;
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
        if(Input.GetKeyDown(KeyCode.Space) && !isJump)
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
    bool isSendRun;
    /// <summary>
    /// 移動控制
    /// </summary>
    void OnMovementControl()
    {                
        //轉向
        float maxRadiansDelta = 0.055f;
        if (inputX != 0 && inputZ != 0) transform.forward = Vector3.RotateTowards(transform.forward, (horizontalCross * inputX) + (forwardVector * inputZ), maxRadiansDelta, maxRadiansDelta);//斜邊
        else if (inputX != 0) transform.forward = Vector3.RotateTowards(transform.forward, horizontalCross * inputX, maxRadiansDelta, maxRadiansDelta);//左右
        else if (inputZ != 0) transform.forward = Vector3.RotateTowards(transform.forward, forwardVector * inputZ, maxRadiansDelta, maxRadiansDelta);//前後      
        
        float inputValue = Mathf.Abs(inputX) + Mathf.Abs(inputZ);//輸入值

        //跳躍中不可再增加推力
        if (isJump && inputValue == 0) isLockMove = true;       
        if (isLockMove) inputValue = 0;

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
}
