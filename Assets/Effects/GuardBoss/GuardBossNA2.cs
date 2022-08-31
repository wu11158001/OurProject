using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardBossNA2 : MonoBehaviour
{
    Animator anim;                 //對應角色動作組件
    AnimatorStateInfo animInfo;    //獲得動作狀態(節省腳本用)   
    void Start()
    {
        anim = gameObject.transform.GetComponentInParent<Animator>();
        gameObject.GetComponent<ParticleSystem>().Stop();      
    }

    // Update is called once per frame
    void Update()
    {
        animInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (animInfo.IsName("Attack.Attack2") && animInfo.normalizedTime > 0.25 && animInfo.normalizedTime < 0.65 && !gameObject.GetComponent<ParticleSystem>().isPlaying)
        {
            gameObject.GetComponent<ParticleSystem>().Play();
        }
        if (animInfo.IsName("Attack.Attack2") && animInfo.normalizedTime >= 0.65)
        {
            gameObject.GetComponent<ParticleSystem>().Stop();
        }

       // TestMode();  //測試模式，用在編輯特效
    }

    void TestMode()
    {
        gameObject.transform.GetComponentInParent<AI>().enabled = false;
      //  anim.Play("Attack.Attack2");
    }
}
