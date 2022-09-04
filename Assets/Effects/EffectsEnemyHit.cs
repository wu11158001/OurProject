using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsEnemyHit : MonoBehaviour
{
    public GameObject effects;     //掛載身上的effects，以定位特效位置(因為不想用GameObject.Find)     
    ParticleSystem hit;
    ParticleSystem bigRipples;
    AnimatorStateInfo nowState;

    void Start()
    {
        hit = effects.transform.GetChild(0).GetComponent<ParticleSystem>();              //命中效果
        if (effects.transform.childCount > 1 && effects.transform.GetChild(1).name.Equals("BigRipples"))
        {
            bigRipples = effects.transform.GetChild(1).GetComponent<ParticleSystem>();       //落地波           
        }
    }


    void Update()
    {
        if (gameObject.transform.GetComponent<Animator>() != null)
        {
            nowState = gameObject.transform.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);  //省廢話
        }
        BigRipples();  //石頭怪的跳落地
    }

    void BigRipples()
    {
        if (bigRipples != null)
        {
            if (nowState.IsName("Attack.Attack2") && nowState.normalizedTime > 0.4
                                                  && nowState.normalizedTime <= 0.45
                                                  && !bigRipples.isPlaying)  bigRipples.Play(); 
        }
    }







    public void HitEffect(GameObject Enemy, Collider hitPos)
    {

        Vector3 star = Enemy.transform.GetChild(0).position;
        Vector3 dir = hitPos.transform.GetChild(0).position - star;
        //if (dir.magnitude < 2)
        //{
        //    star = new Vector3(Screen.width / 2, Screen.height / 2);
        //    star = Camera.main.ScreenToWorldPoint(star);
        //    dir = hitPos.transform.GetChild(0).position - star;
        //}
        Physics.Raycast(star, dir, out RaycastHit pos, Mathf.Infinity, LayerMask.GetMask("Player"));
        GetHitPs().transform.position = pos.point;
        GetHitPs().Play();
        //  isshakeCamera = true;          //畫面震盪

    }
    List<ParticleSystem> hitList = new List<ParticleSystem>();
    ParticleSystem HitPool()
    {
        ParticleSystem hitPs = Instantiate(hit);
        hitList.Add(hitPs);
        return hitPs;
    }
    ParticleSystem GetHitPs()
    {
        foreach (var hl in hitList)
        {
            if (!hl.isPlaying) return hl;
        }
        return HitPool();
    }
}
