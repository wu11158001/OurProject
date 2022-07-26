using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorEffects : MonoBehaviour
{
    private Animator anim;  //對應角色動作組件
    public GameObject effects;  //定位特效位置(因為不想用GameObject.Find) 
    ParticleSystem NormalAttack_1;
    ParticleSystem NormalAttack_3;

    void Start()
    {
        anim = gameObject.transform.GetComponent<Animator>();   //獲得角色動作組件
        NormalAttack_1 = effects.transform.GetChild(0).GetComponent<ParticleSystem>();    //獲得特效組件;
        NormalAttack_3 = effects.transform.GetChild(1).GetComponent<ParticleSystem>();    //獲得特效組件;
    }

    void Update()
    {
        effects.transform.localPosition = new Vector3(0.2075253f, 0.8239655f, 0.4717751f);
        WarNormalAttack1();
        WarNormalAttack3();
    }

    void WarNormalAttack1()
    {
        float delay = 0.35f;       //控制播放時間點，面板務必保持為0   
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack.NormalAttack_1") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > delay)
        {
            if (!NormalAttack_1.isPlaying)
            {
                NormalAttack_1.Play();

            }
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > delay + 0.1f)
            {
                NormalAttack_1.Stop();
            }
        }
        else
        {
            NormalAttack_1.Stop();
        }
    }

    void WarNormalAttack3()
    {
        float delay = 0.55f;       //控制播放時間點，面板務必保持為0
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack.NormalAttack_3") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > delay)
        {
            if (!NormalAttack_3.isPlaying)
            {
                NormalAttack_3.Play();
            }
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > delay + 0.1f)
            {
                NormalAttack_3.Stop();
            }
        }
        else
        {
            NormalAttack_3.Stop();
        }
    }
}
