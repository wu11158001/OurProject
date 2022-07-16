using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 腳色碰撞
/// </summary>
public class CharactersCollision : MonoBehaviour
{
    Animator animator;
    AnimatorStateInfo info;
    GameData_NumericalValue NumericalValue;

    //碰撞框
    Vector3 boxCenter;
    Vector3 boxSize;

    //數值
    float Hp;//生命值

    public List<CharactersFloating> floating_List = new List<CharactersFloating>();//浮空/跳躍List

    private void Awake()
    {
        animator = GetComponent<Animator>();        
    }

    void Start()
    {
        NumericalValue = GameDataManagement.Instance.numericalValue;

        //碰撞框
        boxCenter = GetComponent<BoxCollider>().center;
        boxSize = GetComponent<BoxCollider>().size;


        //數值
        switch(gameObject.tag)
        {
            case "Player":
                Hp = NumericalValue.playerHp;
                break;
            case "SkeletonSoldier":                
                Hp = NumericalValue.skeletonSoldierHp;
                break;
        }      
    }

    void Update()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        OnCollisionControl();
        OnAnimationOver();
        OnFloation();        
    }

    /// <summary>
    /// 浮空
    /// </summary>
    void OnFloation()
    {
        //浮空/跳躍
        for (int i = 0; i < floating_List.Count; i++)
        {
            floating_List[i].OnFloating();
        }

        //碰撞偵測
        LayerMask mask = LayerMask.GetMask("StageObject");
        if (Physics.CheckBox(transform.position + boxCenter, new Vector3(boxSize.x / 4, boxSize.y / 2, boxSize.z / 4), Quaternion.identity, mask))
        {
            floating_List.Clear();//清除List
        }
    }

    /// <summary>
    /// 受到攻擊
    /// </summary>
    /// <param name="attacker">攻擊者物件</param>
    /// <param name="layer">攻擊者layer</param>
    /// <param name="damage">造成傷害</param>
    /// <param name="animationName">播放動畫名稱</param>
    /// <param name="knockDirection">擊中效果(0:擊退, 1:擊飛)</param>
    /// <param name="repel">擊退距離</param>
    public void OnGetHit(GameObject attacker, LayerMask layer, float damage, string animationName, int knockDirection, float repel)
    {               
        
        //判斷受擊對象
        if (gameObject.layer == LayerMask.NameToLayer("Player") && layer == LayerMask.NameToLayer("Enemy") || 
            gameObject.layer == LayerMask.NameToLayer("Enemy") && layer == LayerMask.NameToLayer("Player"))
        {
            Hp -= damage;//生命值減少
            transform.forward = -attacker.transform.forward;//面向攻擊者

            //判斷擊中效果
            switch (knockDirection)
            {
                case 0://擊退
                    transform.position = transform.position + attacker.transform.forward * repel * Time.deltaTime;//擊退
                    break;
                case 1://擊飛
                    floating_List.Add(new CharactersFloating { target = transform, force = repel, gravity = NumericalValue.gravity });//浮空List
                    break;
            }

            //重複觸發動畫
            if (info.IsTag(animationName))
            {
                StartCoroutine(OnAniamtionRepeatTrigger(animationName));
                return;
            }

            //狀態改變(關閉前一個動畫)
            if (info.IsTag("KnockBack") && animationName == "Pain") animator.SetBool("KnockBack", false);
            if (info.IsTag("Pain") && animationName == "KnockBack") animator.SetBool("Pain", false);

            animator.SetBool(animationName, true);
        }        
    }

    /// <summary>
    /// 碰撞控制
    /// </summary>
    void OnCollisionControl()
    {
        //射線方向
        Vector3[] rayDiration = new Vector3[] { transform.forward,
                                                transform.forward - transform.right,
                                                transform.right,
                                                transform.right + transform.forward,
                                               -transform.forward,
                                               -transform.forward + transform.right,
                                               -transform.right,
                                               -transform.right -transform.forward };

        //碰撞偵測
        LayerMask mask = LayerMask.GetMask("StageObject");
        RaycastHit hit;
        for (int i = 0; i < rayDiration.Length; i++)
        {
            if (Physics.BoxCast(transform.position + boxCenter, boxSize/2 * transform.localScale.x, rayDiration[i], out hit, transform.rotation, NumericalValue.boxCollisionDistance, mask))
            {
                transform.position = transform.position - rayDiration[i] * (NumericalValue.boxCollisionDistance - hit.distance);
            }
        }

        //牆壁碰撞(第2層)
        if(Physics.CheckBox(transform.position + boxCenter, new Vector3(boxSize.x / 2, boxSize.y / 4, boxSize.z / 2), Quaternion.identity, mask))
        {
            transform.position = transform.position + transform.forward * 5 * Time.deltaTime;
        }

        //地板碰撞
        if (Physics.CheckBox(transform.position + boxCenter, new Vector3(boxSize.x / 4, boxSize.y / 2, boxSize.z / 4), Quaternion.identity, mask))
        {
            if (Physics.Raycast(transform.position + boxCenter, -transform.up, out hit, boxSize.y / 2, mask))//地板碰撞(第2層)
            {
                transform.position = transform.position + transform.up * (boxSize.y / 2 - 0.01f - hit.distance);
            }            
        }
        else
        {
            //重力
            transform.position = transform.position - Vector3.up * NumericalValue.gravity * Time.deltaTime;
        }
    }

    /// <summary>
    /// 動畫結束
    /// </summary>
    void OnAnimationOver()
    {
        if (info.IsTag("Pain") && info.normalizedTime >= 1) animator.SetBool("Pain", false);
        if (info.IsTag("KnockBack") && info.normalizedTime >= 1) animator.SetBool("KnockBack", false);       
    }

    /// <summary>
    /// 動畫重複觸發
    /// </summary>
    /// <param name="aniamtionName">動畫名稱</param>
    /// <returns></returns>
    IEnumerator OnAniamtionRepeatTrigger(string aniamtionName)
    {
        animator.SetBool(aniamtionName, false);
        yield return new WaitForSeconds(0.03f);
        animator.SetBool(aniamtionName, true);
    }
}