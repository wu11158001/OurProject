using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonStone : MonoBehaviour
{
    public GameObject dragonStone;
    bool spawn;
    void Start()
    {
        gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        spawn = false;
    }
    
    void Update()
    {
        if (dragonStone!=null)
        {

        }
        if (dragonStone != null&&dragonStone.activeInHierarchy)   //因為打爆狀態跟沒出現狀態相同，所以要兩階段判斷，當spawn時才true，當true時才執行特效
        {
            spawn = true;
        }

        if (spawn)
        {
            if (dragonStone.GetComponent<Stronghold>().hp <= dragonStone.GetComponent<Stronghold>().maxHp * 0.001f || dragonStone == null)
            {
                    if (!gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying)
                    gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
               
            }
        }
    }
}
