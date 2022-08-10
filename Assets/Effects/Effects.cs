using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public GameObject effects;     //定位特效位置(因為不想用GameObject.Find)     
    Animator anim;                 //對應角色動作組件
    AnimatorStateInfo animInfo;    //獲得動作狀態(節省腳本用)   
    ParticleSystem NormalAttack_1;
    ParticleSystem NormalAttack_3;
    ParticleSystem SkillAttack_1;
    ParticleSystem SkillAttack_2;
    ParticleSystem SkillAttack_3;
    ParticleSystem hit;

    void Start()
    {
        anim = gameObject.transform.GetComponent<Animator>();                             //獲得角色動作組件        
        NormalAttack_1 = effects.transform.GetChild(0).GetComponent<ParticleSystem>();    //獲得特效組件;
        NormalAttack_3 = effects.transform.GetChild(1).GetComponent<ParticleSystem>();    //獲得特效組件;
        SkillAttack_1 = effects.transform.GetChild(2).GetComponent<ParticleSystem>();    //獲得特效組件;
        SkillAttack_2 = effects.transform.GetChild(3).GetComponent<ParticleSystem>();    //獲得特效組件;
        SkillAttack_3 = effects.transform.GetChild(4).GetComponent<ParticleSystem>();    //獲得特效組件;
        hit = effects.transform.GetChild(5).GetComponent<ParticleSystem>();              //命中效果
        StarShakeSet();                                                                 //畫面震盪
    }

    void Update()
    {
        // effects.transform.localPosition = new Vector3(0.2075253f, 0.8239655f, 0.4717751f);   //防意外
        animInfo = anim.GetCurrentAnimatorStateInfo(0);                                      //節省廢話
        WarNormalAttack1();
        WarNormalAttack3();
        WarSkillAttack1();
        WarSkillAttack2();
        WarSkillAttack3();
        UpdaSnake();                                                                       //畫面震盪 
    }

    void WarNormalAttack1()
    {
        var idelName = "Attack.NormalAttack_1";         //動作名稱
        float delay = 0.35f;                            //控制播放時間點，面板務必保持為0   
        var effect = NormalAttack_1;                    //特效名稱
        DoEffects(idelName, delay, effect);
    }

    void WarNormalAttack3()
    {
        var idelName = "Attack.NormalAttack_3";         //動作名稱
        float delay = 0.35f;                            //控制播放時間點，面板務必保持為0   
        var effect = NormalAttack_3;                    //特效名稱
        DoEffects(idelName, delay, effect);
    }

    void WarSkillAttack2()
    {
        var idelName = "Attack.SkillAttack_2";         //動作名稱
        var skill = SkillAttack_2;                     //三個不同時間播放特效

        var SkillAttack_30 = skill.transform.GetChild(0).GetComponent<ParticleSystem>();
        float delay = 0.01f;                            //SkillAttack_30特效播放時間點，面板務必保持為0        
        if (animInfo.IsName(idelName) && animInfo.normalizedTime > delay && !SkillAttack_30.isPlaying) SkillAttack_30.Play();

        var SkillAttack_31 = skill.transform.GetChild(1).GetComponent<ParticleSystem>();
        float delay1 = 0.3f;                            //SkillAttack_31特效播放時間點
        DoEffects(idelName, delay1, SkillAttack_31);

        var SkillAttack_32 = skill.transform.GetChild(2).GetComponent<ParticleSystem>();
        float delay2 = 0.5f;                             //SkillAttack_32特效播放時間點，面板務必保持為0
        DoEffects(idelName, delay2, SkillAttack_32);
    }

    void WarSkillAttack1()
    {
        var idelName = "Attack.SkillAttack_1";         //動作名稱
        float delay = 0.5f;                            //控制播放時間點，面板務必保持為0   
        var effect = SkillAttack_1;                    //特效名稱

        if (animInfo.IsName(idelName) && animInfo.normalizedTime > delay && !effect.isPlaying)
        {
            effect.Play();
            isshakeCamera = true;          //畫面震盪
            if (animInfo.normalizedTime > delay + 0.1f) effect.Stop();
        }
        else effect.Stop();
    }

    void WarSkillAttack3()
    {
        var idelName = "Attack.SkillAttack_3";         //動作名稱
        var skill = SkillAttack_3;                     //三個不同時間播放特效

        var SkillAttack_30 = skill.transform.GetChild(0).GetComponent<ParticleSystem>();
        float delay = 0.001f;                            //SkillAttack_30特效播放時間點，面板務必保持為0        
        if (animInfo.IsName(idelName) && animInfo.normalizedTime > delay && !SkillAttack_30.isPlaying) SkillAttack_30.Play();

        var SkillAttack_31 = skill.transform.GetChild(1).GetComponent<ParticleSystem>();
        float delay1 = 0.2f;                            //SkillAttack_31特效播放時間點
        DoEffects(idelName, delay1, SkillAttack_31);

        var SkillAttack_32 = skill.transform.GetChild(2).GetComponent<ParticleSystem>();
        float delay2 = 0.4f;                             //SkillAttack_32特效播放時間點，面板務必保持為0
        DoEffects(idelName, delay2, SkillAttack_32);

        var SkillAttack_33 = skill.transform.GetChild(3).GetComponent<ParticleSystem>();
        float delay3 = 0.7f;                             //SkillAttack_32特效播放時間點，面板務必保持為0
        DoEffects(idelName, delay3, SkillAttack_33);
    }

    void DoEffects(string idelName, float delay, ParticleSystem effect)
    {
        if (animInfo.IsName(idelName) && animInfo.normalizedTime > delay && !effect.isPlaying)
        {
            effect.Play();
            if (animInfo.normalizedTime > delay + 0.1f) effect.Stop();
        }
        else effect.Stop();
    }



    //畫面縮放
    float xTime = 0f;                                      //縮小的參數，非固定參數
    void PowerWindownView()
    {
        float yTime = 0.7f;                                //縮小的速率，固定參數
        if (Input.GetKey(KeyCode.P))
        {
            isshakeCamera = true;          //畫面震盪
            var star = new Vector3(Screen.width / 2, Screen.height / 2);
            star = Camera.main.ScreenToWorldPoint(star);
            var n = gameObject.transform.GetChild(3).position - star;
            xTime -= Time.deltaTime;
            Camera.main.transform.forward = n;
            Camera.main.transform.position = Camera.main.transform.position - n.normalized * xTime * yTime;
        }
        if (Input.GetKeyUp(KeyCode.P)) xTime = 0f;
    }





    #region 命中效果

    public void HitEffect(GameObject player, Collider hitPos)
    {
        Vector3 star = player.transform.GetChild(3).position;
        Vector3 dir = hitPos.transform.GetChild(0).position - star;
        if (dir.magnitude < 2)
        {
            star = new Vector3(Screen.width / 2, Screen.height / 2);
            star = Camera.main.ScreenToWorldPoint(star);
            dir = hitPos.transform.GetChild(0).position - star;
        }
        Physics.Raycast(star, dir, out RaycastHit pos, Mathf.Infinity, LayerMask.GetMask("Enemy"));
        GetHitPs().transform.position = pos.point;
        GetHitPs().Play();
        isshakeCamera = true;          //畫面震盪
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
    #endregion

    #region 命中震盪   
    private float shakeTime = 0.0f;
    private float fps = 20.0f;
    private float frameTime = 0.0f;
    private float shakeDelta = 0.005f;
    // public Camera cam;  不掛鏡頭，減少依賴
    public bool isshakeCamera = false;

    void StarShakeSet()
    {
        shakeTime = 0.2f;
        fps = 20.0f;
        frameTime = 0.03f;
        shakeDelta = 0.005f;
    }
    void UpdaSnake()
    {
        if (isshakeCamera)
        {
            if (shakeTime > 0)  //防呆
            {
                shakeTime -= Time.deltaTime;
                if (shakeTime <= 0)
                {
                    Camera.main.rect = new Rect(0.0f, 0.0f, 10.0f, 10.0f);
                    isshakeCamera = false;
                    shakeTime = 0.2f;
                    fps = 20.0f;
                    frameTime = 0.03f;
                    shakeDelta = 0.005f;
                }
                else
                {
                    frameTime += Time.deltaTime;

                    if (frameTime > 1.0 / fps)
                    {
                        frameTime = 0;
                        Camera.main.rect = new Rect(shakeDelta * (-5.0f + 5.0f * Random.value), shakeDelta * (-5.0f + 5.0f * Random.value), 1.0f, 1.0f);
                    }
                }
            }
        }
    }
    #endregion
}
