using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEffects : MonoBehaviour
{
    public GameObject ragonTongue02;
    public GameObject firePos;
    Animator anim;                 //對應角色動作組件
    AnimatorStateInfo animInfo;    //獲得動作狀態(節省腳本用)   
    ParticleSystem Attack1;
    void Start()
    {
        anim = gameObject.transform.GetComponent<Animator>();                             //獲得角色動作組件         
        Attack1 = firePos.transform.GetChild(0).GetComponent<ParticleSystem>();    //獲得特效組件;
    }


    void Update()
    {
        animInfo = anim.GetCurrentAnimatorStateInfo(0);
      //  firePos.transform.position = ragonTongue02.transform.position;
     //   firePos.transform.forward = ragonTongue02.transform.forward;
        Fire();
    }

    void Fire()
    {
        var idelName = "Attack.Attack1";
        var effect = Attack1;
        if (animInfo.IsName(idelName)// && animInfo.normalizedTime > 0.3
                                      && animInfo.normalizedTime <= 0.4
                                     && !effect.isPlaying)
        {
            effect.transform.position = ragonTongue02.transform.position;
            effect.transform.forward = gameObject.transform.forward;
            effect.Play();
        }
    }
}
