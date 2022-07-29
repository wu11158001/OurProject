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
        var idelName = "Attack.NormalAttack_1";         //動作名稱
        float delay = 0.35f;                            //控制播放時間點，面板務必保持為0   
        var effect = NormalAttack_1;                    //特效名稱
        DoEffects(idelName, delay, effect);
    }

    void WarNormalAttack3()
    {
        var idelName = "Attack.NormalAttack_3";         //動作名稱
        float delay = 0.55f;                            //控制播放時間點，面板務必保持為0   
        var effect = NormalAttack_3;                    //特效名稱
        DoEffects(idelName, delay, effect);
    }

    void WarSkillAttack3()
    {
        var idelName = "Attack.SkillAttack_3";         //動作名稱
        var skill = SkillAttack_3;                     //三個不同時間播放特效

        var SkillAttack_30 = skill.transform.GetChild(0).GetComponent<ParticleSystem>();
        float delay = 0.1f;                            //SkillAttack_30特效播放時間點，面板務必保持為0        
        DoEffects(idelName, delay, SkillAttack_30);

        var SkillAttack_31 = skill.transform.GetChild(1).GetComponent<ParticleSystem>();
        float delay1 = 0.3f;                            //SkillAttack_31特效播放時間點
        DoEffects(idelName, delay1, SkillAttack_31);

        var SkillAttack_32 = skill.transform.GetChild(2).GetComponent<ParticleSystem>();
        float delay2 = 0.7f;                             //SkillAttack_32特效播放時間點，面板務必保持為0
        DoEffects(idelName, delay2, SkillAttack_32);
    }

    void DoEffects(string idelName, float delay, ParticleSystem effect)
    {

        var SkillAttack_32 = SkillAttack_3.transform.GetChild(2).GetComponent<ParticleSystem>();
        float delay2 = 0.7f;
        if (animInfo.IsName("Attack.SkillAttack_3") && animInfo.normalizedTime > delay2)
        {            
            if (!SkillAttack_32.isPlaying) SkillAttack_32.Play();
            if (animInfo.normalizedTime > delay2 + 0.1f) SkillAttack_32.Stop();
        }
        else effect.Stop();
    }
}
