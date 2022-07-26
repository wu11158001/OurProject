using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorNA1 : MonoBehaviour
{
    private Animator anim;  //對應角色動作組件
    float delay = 0.35f;       //控制播放時間點，面板務必保持為0        
    ParticleSystem NormalAttack_1;    //特效名稱
    ParticleSystem NormalAttack_1ps;    //特效名稱

    void Start()
    {
        var pos = GameObject.Find("1_Warrior(Clone)");   //測試用
        gameObject.transform.SetParent(pos.transform);   //測試用

        anim = gameObject.transform.parent.GetComponent<Animator>();   //獲得角色動作組件
        NormalAttack_1 = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();    //獲得特效組件
        NormalAttack_1ps = gameObject.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();    //獲得特效組件
    }

    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack.NormalAttack_1") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > delay)
        {
            if (!NormalAttack_1.isPlaying)
            {
                NormalAttack_1.Play();
                NormalAttack_1ps.Play();
            }

            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > delay + 0.1f)
            {
                NormalAttack_1.Stop();
                NormalAttack_1ps.Stop();
            }
        }
        else
        {
            NormalAttack_1.Stop();
        }
        gameObject.transform.position = gameObject.transform.parent.position;
        gameObject.transform.GetChild(0).localEulerAngles = new Vector3(125.485f, 284.455f, -5.64801f);
        gameObject.transform.GetChild(0).GetChild(0).localEulerAngles = new Vector3(3.294599f, 3.294599f, -3.294599f);
    }


}
