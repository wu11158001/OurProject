using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorNA3 : MonoBehaviour
{
    private Animator anim;  //對應角色動作組件
    float delay = 0.55f;       //控制播放時間點，面板務必保持為0        
    ParticleSystem NormalAttack_3;    //特效名稱
    ParticleSystem NormalAttack_31;    //特效名稱
    ParticleSystem NormalAttack_3ps;    //特效名稱

    void Start()
    {
        var pos = GameObject.Find("1_Warrior(Clone)");   //測試用
        gameObject.transform.SetParent(pos.transform);   //測試用

        anim = gameObject.transform.parent.GetComponent<Animator>();   //獲得角色動作組件
        NormalAttack_3 = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();    //獲得特效組件
        NormalAttack_31 = gameObject.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();    //獲得特效組件
        NormalAttack_3ps = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();    //獲得特效組件
    }

    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack.NormalAttack_3") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > delay)
        {
            if (!NormalAttack_3.isPlaying)
            {
                NormalAttack_3.Play();
                NormalAttack_31.Play();
                NormalAttack_3ps.Play();
            }

            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > delay + 0.1f)
            {
                NormalAttack_3.Stop();
                NormalAttack_31.Stop();
                NormalAttack_3ps.Stop();
            }
        }
        else
        {
            NormalAttack_3.Stop();
            NormalAttack_31.Stop();
            NormalAttack_3ps.Stop();
        }
        gameObject.transform.position = gameObject.transform.parent.position;
        gameObject.transform.GetChild(0).localEulerAngles = new Vector3(86.427f, 15.791f, 393.043f);
        gameObject.transform.GetChild(0).GetChild(0).localEulerAngles = new Vector3(5.105f, 178.448f, 25.118f);
        gameObject.transform.GetChild(1).localEulerAngles = new Vector3(-94.184f, 44.50301f, -44.16699f);

    }
}
