using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    //private Animator anim;
    //public ParticleSystem normalAttack_1;
    ParticleSystem effect;
    public float srartTime;//開始攻擊時間
    public float overTime;//開始攻擊時間
    float thisTime;

    void Start()
    {
        //anim = gameObject.GetComponent<Animator>();
        effect = GetComponent<ParticleSystem>();
        //effect.Stop();
      //  effect.transform.localPosition=new Vector3(-0.328889f, 1.086004f, 1.013481f);
    }


    void Update()
    {
        ParticleNormalAttack_1();

    }

    void ParticleNormalAttack_1()
    {
        /*if (anim.GetCurrentAnimatorStateInfo(0).IsName("NormalAttack_1") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > normalAttack_1_Delay)
        {           
            normalAttack_1.gameObject.GetComponent<ParticleSystem>().Play();
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > normalAttack_1_Delay + 0.1f)
            {               
                normalAttack_1.gameObject.GetComponent<ParticleSystem>().Stop();
            }
        }
        else
        {           
            normalAttack_1.gameObject.GetComponent<ParticleSystem>().Stop();
        }*/
        thisTime += Time.deltaTime;
        if (thisTime > srartTime && !effect.isPlaying)
        {
            effect.Play();
            if (thisTime > overTime)
            {
                thisTime = 0;
                effect.gameObject.SetActive(false);
            }
        }
        effect.transform.localPosition = new Vector3(-0.328889f, 1.086004f, 1.013481f);
        //else gameObject.SetActive(false);
    }
}
