using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongholdFire : MonoBehaviour
{
    //public ParticleSystem fire1;
    //public ParticleSystem fire2;
    //public ParticleSystem fire3;
    //public ParticleSystem fire4;
    //public ParticleSystem fire5;
    //public ParticleSystem fire6;
    //public ParticleSystem fire7;
    //public ParticleSystem fire8;
    //public ParticleSystem fire9;
    //public ParticleSystem fire10;
    //public ParticleSystem fire11;

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

    void Start()
    {
        //fire1.SetActive(false); . .Stop();
        //fire2.Stop();
        //fire3.Stop();
        //fire4.Stop();
        //fire5.Stop();
        //fire6.Stop();
        //fire7.Stop();
        //fire8.Stop();
        //fire9.Stop();
        //fire10.Stop();
        //fire11.Stop();
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

    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<Stronghold>().hp < gameObject.GetComponent<Stronghold>().maxHp * 0.7f)
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
            if (!fire6.GetComponent<ParticleSystem>().isPlaying) fire6.GetComponent<ParticleSystem>().Play();
        }
        if (gameObject.GetComponent<Stronghold>().hp < gameObject.GetComponent<Stronghold>().maxHp * 0.35f)
        {
            fire6.SetActive(true);
            fire7.SetActive(true);
            fire8.SetActive(true);
            fire9.SetActive(true);
            fire10.SetActive(true);
            fire11.SetActive(true);
            if (!fire6.GetComponent<ParticleSystem>().isPlaying) fire6.GetComponent<ParticleSystem>().Play();
            if (!fire7.GetComponent<ParticleSystem>().isPlaying) fire7.GetComponent<ParticleSystem>().Play();
            if (!fire8.GetComponent<ParticleSystem>().isPlaying) fire8.GetComponent<ParticleSystem>().Play();
            if (!fire9.GetComponent<ParticleSystem>().isPlaying) fire9.GetComponent<ParticleSystem>().Play();
            if (!fire10.GetComponent<ParticleSystem>().isPlaying) fire10.GetComponent<ParticleSystem>().Play();
            if (!fire11.GetComponent<ParticleSystem>().isPlaying) fire11.GetComponent<ParticleSystem>().Play();
        }

        if (gameObject.GetComponent<Stronghold>().hp <= gameObject.GetComponent<Stronghold>().maxHp * 0.15f)
        {
            fire1.SetActive(false);
            fire2.SetActive(false);
            fire10.SetActive(false);
            fire11.SetActive(false);
            fire1.GetComponent<ParticleSystem>().Stop();
            fire2.GetComponent<ParticleSystem>().Stop();
            fire10.GetComponent<ParticleSystem>().Stop();
            fire11.GetComponent<ParticleSystem>().Stop();
        }
    }
}
