using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEffects : MonoBehaviour
{
    public GameObject ragonTongue02;
    public GameObject fireBallPos;
    public GameObject fireBreathPos;
    public GameObject boomPos;

    //龍非固定。對位
    public GameObject PosLLeg;  //左腿
    public GameObject PosRLeg;   //右腿
    public GameObject PosLClav;  //左翅
    public GameObject PosRClav;  //右翅
                                 // public GameObject Pos4;  //喉


    Animator anim;                 //對應角色動作組件
    AnimatorStateInfo animInfo;    //獲得動作狀態(節省腳本用)   
    ParticleSystem a01;
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
                                              && animInfo.normalizedTime <= 0.3)
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
}


