using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuardBossNA3 : MonoBehaviour
{
    Animator anim;                 //對應角色動作組件
    AnimatorStateInfo animInfo;    //獲得動作狀態(節省腳本用)   
    bool testMode = false;
    void Start()
    {
        anim = gameObject.transform.GetComponentInParent<Animator>();
        gameObject.GetComponent<ParticleSystem>().Stop();
        if (SceneManager.GetActiveScene().name.Equals("EffectScene")) testMode = true;
    }


    void Update()
    {
        animInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (animInfo.IsName("Attack.Attack3") && animInfo.normalizedTime > 0.1 && animInfo.normalizedTime < 0.2 && !gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying)
        {
            gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        }
        if (animInfo.IsName("Attack.Attack3") && animInfo.normalizedTime > 0.1 && animInfo.normalizedTime < 0.4 && !gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().isPlaying)
        {
            gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        }
        if (animInfo.IsName("Attack.Attack3") && animInfo.normalizedTime >= 0.2)
        {
            gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        }
        if (animInfo.IsName("Attack.Attack3") && animInfo.normalizedTime >= 0.4)
        {
            gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
        }


        if (testMode) TestMode(); //測試模式，用在編輯特效       
    }

    void TestMode()
    {
        gameObject.transform.GetComponentInParent<AI>().enabled = false;
        gameObject.transform.GetComponentInParent<CharactersCollision>().enabled = false;
        gameObject.transform.GetComponentInParent<ConnectObject>().enabled = false;
        gameObject.transform.GetComponentInParent<GuardBoss_Exclusive>().enabled = false;

        //if (!animInfo.IsName("Attack.Attack3"))
        //{
        //    anim.Play("Attack.Attack3");
        //}
    }
}
