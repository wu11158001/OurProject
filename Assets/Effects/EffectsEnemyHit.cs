using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsEnemyHit : MonoBehaviour
{
    public GameObject effects;     //掛載身上的effects，以定位特效位置(因為不想用GameObject.Find)     
    ParticleSystem hit;
    void Start()
    {
        hit = effects.transform.GetChild(0).GetComponent<ParticleSystem>();              //命中效果
    }


    void Update()
    {

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
