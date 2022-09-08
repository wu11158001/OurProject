using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongholdFire : MonoBehaviour
{
    public GameObject stronghold;
    public GameObject fire1;
    public GameObject fire2;
    public GameObject fire3;
    public GameObject fire4;
    public GameObject fire5;
    public GameObject fire6;
    public GameObject fire7;
    public GameObject fire8;
    public GameObject fire9;
    public GameObject fire10;
    public GameObject fire11;

    float strongholdHp;
    float strongholdMaxhp;

    bool door ;

    void Start()
    {
        fire1.SetActive(false);
        fire2.SetActive(false);
        fire3.SetActive(false);
        fire4.SetActive(false);
        fire5.SetActive(false);
        fire6.SetActive(false);
        fire7.SetActive(false);
        fire8.SetActive(false);
        fire9.SetActive(false);
        fire10.SetActive(false);
        fire11.SetActive(false);
        door = false;
    }

    void Update()
    {
        strongholdHp = stronghold.gameObject.GetComponent<Stronghold>().hp;
        strongholdMaxhp = stronghold.gameObject.GetComponent<Stronghold>().maxHp;

        if (!stronghold.name.Equals("Stronghold_Enemy3"))
        {
            if (strongholdHp <= strongholdMaxhp * 0.7f && strongholdHp > strongholdMaxhp * 0.35f)
            {
                fire1.SetActive(true);
                fire2.SetActive(true);
                fire3.SetActive(true);
                fire4.SetActive(true);
                fire5.SetActive(true);
                if (!fire1.GetComponent<ParticleSystem>().isPlaying) fire1.GetComponent<ParticleSystem>().Play();
                if (!fire2.GetComponent<ParticleSystem>().isPlaying) fire2.GetComponent<ParticleSystem>().Play();
                if (!fire3.GetComponent<ParticleSystem>().isPlaying) fire3.GetComponent<ParticleSystem>().Play();
                if (!fire4.GetComponent<ParticleSystem>().isPlaying) fire4.GetComponent<ParticleSystem>().Play();
                if (!fire5.GetComponent<ParticleSystem>().isPlaying) fire5.GetComponent<ParticleSystem>().Play();
            }
            if (strongholdHp <= strongholdMaxhp * 0.35f && strongholdHp > strongholdMaxhp * 0.001f)
            {
                fire6.SetActive(true);
                fire7.SetActive(true);
                fire8.SetActive(true);
                fire9.SetActive(true);
                fire10.SetActive(true);
                if (!fire6.GetComponent<ParticleSystem>().isPlaying) fire6.GetComponent<ParticleSystem>().Play();
                if (!fire7.GetComponent<ParticleSystem>().isPlaying) fire7.GetComponent<ParticleSystem>().Play();
                if (!fire8.GetComponent<ParticleSystem>().isPlaying) fire8.GetComponent<ParticleSystem>().Play();
                if (!fire9.GetComponent<ParticleSystem>().isPlaying) fire9.GetComponent<ParticleSystem>().Play();
                if (!fire10.GetComponent<ParticleSystem>().isPlaying) fire10.GetComponent<ParticleSystem>().Play();
            }
            if (strongholdHp <= strongholdMaxhp * 0.001f || stronghold == null)
            {
                fire1.SetActive(false);
                fire2.SetActive(false);
                fire3.SetActive(false);
                fire4.SetActive(false);
                fire5.SetActive(false);
                fire6.SetActive(false);
                fire7.SetActive(false);
                fire8.SetActive(false);
                fire9.SetActive(false);
                fire10.SetActive(false);

                fire1.GetComponent<ParticleSystem>().Stop();
                fire2.GetComponent<ParticleSystem>().Stop();
                fire3.GetComponent<ParticleSystem>().Stop();
                fire4.GetComponent<ParticleSystem>().Stop();
                fire5.GetComponent<ParticleSystem>().Stop();
                fire6.GetComponent<ParticleSystem>().Stop();
                fire7.GetComponent<ParticleSystem>().Stop();
                fire8.GetComponent<ParticleSystem>().Stop();
                fire9.GetComponent<ParticleSystem>().Stop();
                fire10.GetComponent<ParticleSystem>().Stop();

                if (fire11 != null)
                {
                    fire11.SetActive(true);
                    if (!fire11.GetComponent<ParticleSystem>().isPlaying) fire11.GetComponent<ParticleSystem>().Play();
                }
            }
        }
        else   //城門石
        {
            if (stronghold.activeInHierarchy)  //當出現時(預設不存在)
            {
                if (strongholdHp <= strongholdMaxhp * 0.7f && strongholdHp > strongholdMaxhp * 0.35f)
                {
                    fire1.SetActive(true);
                    if (!fire1.GetComponent<ParticleSystem>().isPlaying) fire1.GetComponent<ParticleSystem>().Play();
                    door = true;   //兩階段判定，因為城門石初始狀態與打爆狀態相同
                }
                if (strongholdHp <= strongholdMaxhp * 0.35f && strongholdHp > strongholdMaxhp * 0.001f)
                {
                    fire6.SetActive(true);
                    if (!fire6.GetComponent<ParticleSystem>().isPlaying) fire6.GetComponent<ParticleSystem>().Play();
                }
            }
            if (door)
            {
                if (strongholdHp <= strongholdMaxhp * 0.001f || stronghold == null)
                {
                    fire1.SetActive(false);
                    fire6.SetActive(false);
                    fire1.GetComponent<ParticleSystem>().Stop();
                    fire6.GetComponent<ParticleSystem>().Stop();
                    if (fire11 != null)
                    {
                        fire11.SetActive(true);
                        if (!fire11.GetComponent<ParticleSystem>().isPlaying) fire11.GetComponent<ParticleSystem>().Play();                       
                    }
                }
            }
        }
    }
}

