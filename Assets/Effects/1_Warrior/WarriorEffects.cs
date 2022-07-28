using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorEffects : MonoBehaviour
{
    public GameObject effects;     //定位特效位置(因為不想用GameObject.Find)     
    Animator anim;                 //對應角色動作組件
    AnimatorStateInfo animInfo;    //獲得動作狀態(節省腳本用)   
    ParticleSystem NormalAttack_1;
    ParticleSystem NormalAttack_3;
    ParticleSystem SkillAttack_3;

    void Start()
    {
        anim = gameObject.transform.GetComponent<Animator>();                             //獲得角色動作組件       
        NormalAttack_1 = effects.transform.GetChild(0).GetComponent<ParticleSystem>();    //獲得特效組件;
        NormalAttack_3 = effects.transform.GetChild(1).GetComponent<ParticleSystem>();    //獲得特效組件;
        SkillAttack_3 = effects.transform.GetChild(2).GetComponent<ParticleSystem>();    //獲得特效組件;
    }

    void Update()
    {
        // effects.transform.localPosition = new Vector3(0.2075253f, 0.8239655f, 0.4717751f);   //防意外
        animInfo = anim.GetCurrentAnimatorStateInfo(0);                                      //節省廢話
        WarNormalAttack1();
        WarNormalAttack3();
        WarSkillAttack3();
    }

    void WarNormalAttack1()
    {
        float delay = 0.35f;       //控制播放時間點，面板務必保持為0   
        if (animInfo.IsName("Attack.NormalAttack_1") && animInfo.normalizedTime > delay)
        {
            if (!NormalAttack_1.isPlaying) NormalAttack_1.Play();
            if (animInfo.normalizedTime > delay + 0.1f) NormalAttack_1.Stop();
        }
        else NormalAttack_1.Stop();
    }

    void WarNormalAttack3()
    {
        float delay = 0.55f;       //控制播放時間點，面板務必保持為0
        if (animInfo.IsName("Attack.NormalAttack_3") && animInfo.normalizedTime > delay)
        {
            if (!NormalAttack_3.isPlaying) NormalAttack_3.Play();
            if (animInfo.normalizedTime > delay + 0.1f) NormalAttack_3.Stop();
        }
        else NormalAttack_3.Stop();
    }

    void WarSkillAttack3()
    {
        var SkillAttack_30 = SkillAttack_3.transform.GetChild(0).GetComponent<ParticleSystem>();
        float delay = 0.1f;
        if (animInfo.IsName("Attack.SkillAttack_3") && animInfo.normalizedTime > delay)
        {
            if (!SkillAttack_30.isPlaying) SkillAttack_30.Play();
            if (animInfo.normalizedTime > delay + 0.1f) SkillAttack_30.Stop();
        }
        else SkillAttack_30.Stop();

        var SkillAttack_31 = SkillAttack_3.transform.GetChild(1).GetComponent<ParticleSystem>();
        float delay1 = 0.4f;
        if (animInfo.IsName("Attack.SkillAttack_3") && animInfo.normalizedTime > delay1)
        {
            if (!SkillAttack_31.isPlaying) SkillAttack_31.Play();
            if (animInfo.normalizedTime > delay1 + 0.1f) SkillAttack_31.Stop();
        }
        else SkillAttack_31.Stop();

        var SkillAttack_32 = SkillAttack_3.transform.GetChild(2).GetComponent<ParticleSystem>();
        float delay2 = 0.7f;
        if (animInfo.IsName("Attack.SkillAttack_3") && animInfo.normalizedTime > delay2)
        {
            Debug.Log("11");
            if (!SkillAttack_32.isPlaying) SkillAttack_32.Play();
            if (animInfo.normalizedTime > delay2 + 0.1f) SkillAttack_32.Stop();
        }
        else SkillAttack_32.Stop();
    }
}
