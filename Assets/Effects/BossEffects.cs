using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEffects : MonoBehaviour
{
    public GameObject ragonTongue02;
    public GameObject fireBallPos;
    public GameObject fireBreathPos;
    public GameObject boomPos;
    public GameObject flyAttackPos;

    Vector3 flyAttackOnY; //飛行攻擊對地面特效的Y值

    //龍非固定。對位
    public GameObject PosLLeg;  //左腿
    public GameObject PosRLeg;   //右腿
    public GameObject PosLClav;  //左翅
    public GameObject PosRClav;  //右翅
                                 // public GameObject Pos4;  //喉


    Animator anim;                 //對應角色動作組件
    AnimatorStateInfo animInfo;    //獲得動作狀態(節省腳本用)   
    ParticleSystem a01;   //我知道code有點亂，想到什麼寫什麼XD
    ParticleSystem a02;
    ParticleSystem a03;
    ParticleSystem a04;
    void Start()
    {
        anim = gameObject.transform.GetComponent<Animator>();                             //獲得角色動作組件         
        a01 = fireBallPos.transform.GetChild(0).GetComponent<ParticleSystem>();    //獲得特效組件;
        a02 = fireBallPos.transform.GetChild(1).GetComponent<ParticleSystem>();    //獲得特效組件;
        a03 = fireBallPos.transform.GetChild(2).GetComponent<ParticleSystem>();    //獲得特效組件;
        a04 = fireBallPos.transform.GetChild(3).GetComponent<ParticleSystem>();    //獲得特效組件;
    }


    void Update()
    {
        animInfo = anim.GetCurrentAnimatorStateInfo(0);
        FireBall();
        FireBreath();
        Boom();
        FlyAttack();

    }

    void FireBall()
    {
        if (animInfo.IsName("Attack.Attack1") && animInfo.normalizedTime <= 0.5
                                     && !a01.isPlaying) a01.Play();

        if (animInfo.IsName("Attack.Attack1")
                                      && animInfo.normalizedTime <= 0.3
                                     && !a02.isPlaying) a02.Play();

        if (animInfo.IsName("Attack.Attack1") && animInfo.normalizedTime > 0.6
                                    && animInfo.normalizedTime <= 0.65
                                   && !a03.isPlaying) a03.Play();

        if (animInfo.IsName("Attack.Attack1") && animInfo.normalizedTime > 0.45    //火球
                                      && animInfo.normalizedTime <= 0.5
                                     && !a04.isPlaying)
        {
            if (GameDataManagement.Instance.isConnect)  //如果連線
            {
                //8是朝腳底
                a04.transform.forward = GameSceneManagement.Instance.BossTargetObject.GetComponent<Effects>().breathHere.transform.position - ragonTongue02.transform.position;
            }
            else
            {
                a04.transform.forward = gameObject.GetComponent<BossAI>().GetTarget().GetComponent<Effects>().breathHere.transform.position - ragonTongue02.transform.position;
            }

            a04.Play();
        }

        if (!GameDataManagement.Instance.isConnect && animInfo.IsName("Attack.Attack1"))
        {
            gameObject.GetComponent<BossAI>().OnRotateToTarget();
        }
    }
    void FireBreath()
    {
        if (animInfo.IsName("Attack.Attack2") && animInfo.normalizedTime > 0.25    //口中的火
                                              && animInfo.normalizedTime <= 0.3
                                              && !fireBreathPos.transform.GetChild(1).GetComponent<ParticleSystem>().isPlaying)
        {
            fireBreathPos.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        }


        if (animInfo.IsName("Attack.Attack2") && animInfo.normalizedTime > 0.25    //龍息
                                     && animInfo.normalizedTime <= 0.3
                                    && !fireBreathPos.transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying)
        {   //龍息的目標=從BOSSAI那裡取得玩家，再從玩家Effects取得breathHere位置    
            if (GameDataManagement.Instance.isConnect)
            {
                fireBreathPos.transform.GetChild(0).GetComponent<ParticleSystem>().transform.forward = GameSceneManagement.Instance.BossTargetObject.GetComponent<Effects>().breathHere.transform.position - ragonTongue02.transform.position;
            }
            else
            {
                fireBreathPos.transform.GetChild(0).GetComponent<ParticleSystem>().transform.forward = gameObject.GetComponent<BossAI>().GetTarget().GetComponent<Effects>().breathHere.transform.position - ragonTongue02.transform.position;
            }

            fireBreathPos.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        }
        if (!GameDataManagement.Instance.isConnect && fireBreathPos.transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying)
        {
            gameObject.GetComponent<BossAI>().OnRotateToTarget();
        }

        if (animInfo.IsName("Attack.Attack2") && animInfo.normalizedTime > 0.25   //龍息
                                     && animInfo.normalizedTime <= 0.3
                                    && !fireBreathPos.transform.GetChild(2).GetComponent<ParticleSystem>().isPlaying)
        {
            //龍息的目標=從BOSSAI那裡取得玩家，再從玩家Effects取得breathHere位置   
            if (GameDataManagement.Instance.isConnect)  //如果連線
            {
                fireBreathPos.transform.GetChild(2).GetComponent<ParticleSystem>().transform.forward = GameSceneManagement.Instance.BossTargetObject.GetComponent<Effects>().breathHere.transform.position - ragonTongue02.transform.position;
            }
            else
            {
                fireBreathPos.transform.GetChild(2).GetComponent<ParticleSystem>().transform.forward = gameObject.GetComponent<BossAI>().GetTarget().GetComponent<Effects>().breathHere.transform.position - ragonTongue02.transform.position;
            }
            fireBreathPos.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
        }
        if (!GameDataManagement.Instance.isConnect && fireBreathPos.transform.GetChild(2).GetComponent<ParticleSystem>().isPlaying)
        {
            gameObject.GetComponent<BossAI>().OnRotateToTarget();
        }



    }
    void Boom()
    {
        boomPos.transform.GetChild(0).position = (PosLClav.transform.position + PosRClav.transform.position + PosLLeg.transform.position + PosRLeg.transform.position) * 0.25f;   //取腿翅中間
        boomPos.transform.GetChild(1).position = (PosLClav.transform.position + PosRClav.transform.position) * 0.5f;   //取兩翅中間
        boomPos.transform.GetChild(2).position = (PosLClav.transform.position + PosRClav.transform.position) * 0.5f;   //取兩翅中間
        boomPos.transform.GetChild(3).position = (PosLLeg.transform.position + PosRLeg.transform.position) * 0.5f;   //取兩腿中間
        boomPos.transform.GetChild(4).position = (PosLLeg.transform.position + PosRLeg.transform.position) * 0.5f;   //取兩腿中間
        boomPos.transform.GetChild(5).position = boomPos.transform.GetChild(0).position;

        if (animInfo.IsName("Attack.Attack3") && !boomPos.transform.GetChild(0).transform.GetComponent<ParticleSystem>().isPlaying)
        {
            boomPos.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        }
        if (animInfo.IsName("Attack.Attack3") && !boomPos.transform.GetChild(1).transform.GetComponent<ParticleSystem>().isPlaying)
        {
            boomPos.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        }
        if (animInfo.IsName("Attack.Attack3") && !boomPos.transform.GetChild(2).transform.GetComponent<ParticleSystem>().isPlaying)
        {
            boomPos.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
        }
        if (animInfo.IsName("Attack.Attack3") && !boomPos.transform.GetChild(3).transform.GetComponent<ParticleSystem>().isPlaying)
        {
            boomPos.transform.GetChild(3).GetComponent<ParticleSystem>().Play();
        }
        if (animInfo.IsName("Attack.Attack3") && !boomPos.transform.GetChild(4).transform.GetComponent<ParticleSystem>().isPlaying)
        {
            boomPos.transform.GetChild(4).GetComponent<ParticleSystem>().Play();
        }
        if (animInfo.IsName("Attack.Attack3") && animInfo.normalizedTime <= 0.7 && !boomPos.transform.GetChild(5).transform.GetComponent<ParticleSystem>().isPlaying)
        {
            boomPos.transform.GetChild(5).GetComponent<ParticleSystem>().Play();
        }
    }


    void FlyAttack()  //抱歉，我的CODE都沒有整理XD
    {
        if (animInfo.IsName("Attack.Attack4-1") && animInfo.normalizedTime <= 0.8    //起飛第一階段
                                              && !flyAttackPos.transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying)
        {
            flyAttackPos.transform.GetChild(0).position = (PosLClav.transform.position + PosRClav.transform.position + PosLLeg.transform.position + PosRLeg.transform.position) * 0.25f;   //取腿翅中間
            flyAttackPos.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        }
        if (animInfo.IsName("Attack.Attack4-2") && animInfo.normalizedTime > 0.4        //起飛第二階段爆炸1
                                                && animInfo.normalizedTime <= 0.45
                                                && !flyAttackPos.transform.GetChild(1).GetComponent<ParticleSystem>().isPlaying)
        {
            flyAttackPos.transform.GetChild(1).position = GetPlayerY();
            flyAttackPos.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        }
        if (animInfo.IsName("Attack.Attack4-2") && animInfo.normalizedTime > 0.5        //起飛第二階段爆炸2
                                               && animInfo.normalizedTime <= 0.55
                                               && !flyAttackPos.transform.GetChild(2).GetComponent<ParticleSystem>().isPlaying)
        {
            flyAttackPos.transform.GetChild(2).position = GetPlayerY();
            flyAttackPos.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
        }
        if (animInfo.IsName("Attack.Attack4-2") && animInfo.normalizedTime > 0.6        //起飛第二階段爆炸3
                                              && animInfo.normalizedTime <= 0.65
                                              && !flyAttackPos.transform.GetChild(3).GetComponent<ParticleSystem>().isPlaying)
        {
            flyAttackPos.transform.GetChild(3).position = GetPlayerY();
            flyAttackPos.transform.GetChild(3).GetComponent<ParticleSystem>().Play();
        }
        if (animInfo.IsName("Attack.Attack4-2") && animInfo.normalizedTime > 0.7        //起飛第二階段爆炸4
                                              && animInfo.normalizedTime <= 0.75
                                              && !flyAttackPos.transform.GetChild(4).GetComponent<ParticleSystem>().isPlaying)
        {
            flyAttackPos.transform.GetChild(4).position = GetPlayerY();
            flyAttackPos.transform.GetChild(4).GetComponent<ParticleSystem>().Play();
        }

    }

    Vector3 GetPlayerY()  //取得玩家Y值當作地面高度(不使用射腺打地面的方式)
    {
        if (GameDataManagement.Instance.isConnect)  //如果連線
        {   //取翅中間加上玩家Y(地面)
            flyAttackOnY = new Vector3((PosLClav.transform.position.x + PosRClav.transform.position.x) * 0.5f,     //x
                                        GameSceneManagement.Instance.BossTargetObject.transform.position.y,        //y
                                       (PosLClav.transform.position.z + PosRClav.transform.position.z) * 0.5f);    //z
        }
        else
        {
            flyAttackOnY = new Vector3((PosLClav.transform.position.x + PosRClav.transform.position.x) * 0.5f,     //x
                                       gameObject.GetComponent<BossAI>().GetTarget().transform.position.y,        //y
                                      (PosLClav.transform.position.z + PosRClav.transform.position.z) * 0.5f);    //z
        }
        return flyAttackOnY;
    }
}


