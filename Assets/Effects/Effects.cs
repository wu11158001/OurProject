using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public GameObject weapon;
    public GameObject effects;     //定位特效位置(因為不想用GameObject.Find)     
    Animator anim;                 //對應角色動作組件
    AnimatorStateInfo animInfo;    //獲得動作狀態(節省腳本用)   
    ParticleSystem NormalAttack_1;
    ParticleSystem NormalAttack_2;
    ParticleSystem NormalAttack_3;
    ParticleSystem SkillAttack_1;
    ParticleSystem SkillAttack_2;
    ParticleSystem SkillAttack_3;
    ParticleSystem hit;

    Color baseColor;
    float rColor, gColor, bColor;
    float intensity;
    float fov;

    void Start()
    {
        anim = gameObject.transform.GetComponent<Animator>();                             //獲得角色動作組件         
        NormalAttack_1 = effects.transform.GetChild(0).GetComponent<ParticleSystem>();    //獲得特效組件;
        NormalAttack_2 = effects.transform.GetChild(1).GetComponent<ParticleSystem>();    //獲得特效組件
        NormalAttack_3 = effects.transform.GetChild(2).GetComponent<ParticleSystem>();    //獲得特效組件;
        SkillAttack_1 = effects.transform.GetChild(3).GetComponent<ParticleSystem>();    //獲得特效組件;
        SkillAttack_2 = effects.transform.GetChild(4).GetComponent<ParticleSystem>();    //獲得特效組件;
        SkillAttack_3 = effects.transform.GetChild(5).GetComponent<ParticleSystem>();    //獲得特效組件;
        hit = effects.transform.GetChild(6).GetComponent<ParticleSystem>();              //命中效果
        StarShakeSet();                                                                 //畫面震盪
        fov =Camera.main.fieldOfView;

        //武器發光，戰士弓箭手
        if (anim.runtimeAnimatorController.name == "1_Warrior"|| anim.runtimeAnimatorController.name == "3_Archer")
        {
            baseColor = weapon.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor");
            intensity = 1f;
            rColor = 0.933f;
            gColor = 0.933f;
            bColor = 0.933f;
        }
    }

    void Update()
    {
        // effects.transform.localPosition = new Vector3(0.2075253f, 0.8239655f, 0.4717751f);   //防意外
        animInfo = anim.GetCurrentAnimatorStateInfo(0);                                      //節省廢話
        UpdaSnake();                                                                       //畫面震盪 
      //  PowerWindownView();
        if (anim.runtimeAnimatorController.name == "1_Warrior")
        {
            WarNormalAttack1();
            WarNormalAttack3();
            WarSkillAttack1();
            WarSkillAttack2();
            WarSkillAttack3();
        }
        if (anim.runtimeAnimatorController.name == "2_Magician")
        {
            MagSkillAttack1();
         //   MagSkillAttack3();
        }
    }



    void MagSkillAttack1()
    {
        var idelName = "Attack.SkillAttack_1";
        float delay = 0.01f;
        var effect = SkillAttack_1;
        if (animInfo.IsName(idelName) && animInfo.normalizedTime > delay && !effect.isPlaying) effect.Play();
    }

    void MagSkillAttack3()
    {
        var idelName = "Attack.SkillAttack_3";
        float delay = 0.01f;
        var effect = SkillAttack_3;
        if (animInfo.IsName(idelName) && animInfo.normalizedTime > delay && !effect.isPlaying)
        {
            effect.Play();
            pWindownView = true;
            if (animInfo.IsName(idelName) && animInfo.normalizedTime >= 1)
            {
                //    pWindownView = false;
            }
        }
    }


    #region 戰士

    void WarNormalAttack1()
    {
        var idelName = "Attack.NormalAttack_1";
        if (animInfo.IsName(idelName))
        {
            intensity += intensity * 20f * Time.deltaTime;   //變換速度
            if (intensity >= 15f) intensity = 15f;  //亮度
            if (animInfo.normalizedTime > 0.5)
            {
                intensity -= intensity * 50f * Time.deltaTime;
                if (intensity <= 1f) intensity = 1f;
            }
        }
        weapon.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", baseColor * intensity);


        //var idelName = "Attack.NormalAttack_1";         //動作名稱
        //float delay = 0.35f;                            //控制播放時間點，面板務必保持為0   
        //var effect = NormalAttack_1;                    //特效名稱
        //DoEffects(idelName, delay, effect);
    }

    void WarNormalAttack3()
    {
        var idelName = "Attack.NormalAttack_3";         //動作名稱      
        var effect = NormalAttack_3;                    //特效名稱
        float delay0 = 0.01f;
        var NormalAttack_30 = effect.transform.GetChild(0).GetComponent<ParticleSystem>();  //地光
        DoEffects(idelName, delay0, NormalAttack_30);

        float delay1 = 0.2f;
        var NormalAttack_31 = effect.transform.GetChild(1).GetComponent<ParticleSystem>();  //風
        if (animInfo.IsName(idelName) && animInfo.normalizedTime > delay1 && !NormalAttack_31.isPlaying)
        {
            NormalAttack_31.Play();
            if (animInfo.normalizedTime > delay1 + 0.1f) effect.Stop();
        }

        var NormalAttack_35 = effect.transform.GetChild(6).GetComponent<ParticleSystem>();
        float delay5 = 0.35f;                            //控制播放時間點，面板務必保持為0   
        DoEffects(idelName, delay5, NormalAttack_35);

        var NormalAttack_36 = effect.transform.GetChild(7).GetComponent<ParticleSystem>();
        float delay6 = 0.35f;                            //控制播放時間點，面板務必保持為0   
        DoEffects(idelName, delay6, NormalAttack_36);


        if (animInfo.IsName(idelName))
        {
            baseColor = new Color(baseColor.r, gColor, bColor); ;
            gColor += gColor * 20f * Time.deltaTime;   //變換速度
            bColor += bColor * 20f * Time.deltaTime;   //變換速度
            if (gColor >= 1f) gColor = 1f;
            if (bColor >= 1.5f) bColor = 1.5f;

            intensity += intensity * 20f * Time.deltaTime;   //變換速度
            if (intensity >= 15f) intensity = 15f;  //亮度
            if (animInfo.normalizedTime > 0.5)
            {
                intensity -= intensity * 50f * Time.deltaTime;
                if (intensity <= 1f) intensity = 1f;

                gColor -= gColor * 50f * Time.deltaTime;
                if (gColor <= 0.933f) gColor = 0.933f;
                bColor -= bColor * 50f * Time.deltaTime;
                if (bColor <= 0.933f) bColor = 0.933f;
            }
        }
        weapon.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", baseColor * intensity);

    }

    void WarSkillAttack1()
    {
        var idelName = "Attack.SkillAttack_1";         //動作名稱
        float delay = 0.7f;                            //控制播放時間點，面板務必保持為0   
        var effect = SkillAttack_1;                    //特效名稱

        if (animInfo.IsName(idelName))
        {
            baseColor = new Color(rColor, gColor, bColor); ;
            rColor += rColor * 20f * Time.deltaTime;   //變換速度
            gColor += gColor * 20f * Time.deltaTime;   //變換速度
            if (rColor >= 2f) rColor = 2f;
            if (gColor >= 1.5f) gColor = 1.5f;

            intensity += intensity * 20f * Time.deltaTime;   //變換速度
            if (intensity >= 5f) intensity = 5f;  //亮度
            if (animInfo.normalizedTime > 0.5)
            {
                intensity -= intensity * 50f * Time.deltaTime;
                if (intensity <= 1f) intensity = 1f;

                rColor -= rColor * 20f * Time.deltaTime;
                if (rColor <= 0.933f) rColor = 0.933f;
                gColor -= gColor * 20f * Time.deltaTime;
                if (gColor <= 0.933f) gColor = 0.933f;
            }
        }
        weapon.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", baseColor * intensity);

        if (animInfo.IsName(idelName) && animInfo.normalizedTime > delay && !effect.isPlaying)
        {
            effect.Play();
            isshakeCamera = true;          //畫面震盪
            if (animInfo.normalizedTime > delay + 0.1f) effect.Stop();
        }
        else effect.Stop();
    }

    void WarSkillAttack2()
    {
        var idelName = "Attack.SkillAttack_2";         //動作名稱
        var skill = SkillAttack_2;

        if (animInfo.IsName(idelName))
        {
            baseColor = new Color(rColor, gColor, bColor); ;
            rColor += rColor * 20f * Time.deltaTime;   //變換速度
            gColor -= gColor * 20f * Time.deltaTime;   //變換速度
            bColor -= bColor * 20f * Time.deltaTime;   //變換速度
            if (rColor >= 10) rColor = 10;
            if (gColor <= 0) gColor = 0;
            if (bColor <= 0) bColor = 0;

            intensity += intensity * 20f * Time.deltaTime;   //變換速度
            if (intensity >= 2f) intensity = 2f;  //亮度

            if (animInfo.normalizedTime > 0.5)
            {
                intensity -= intensity * 30f * Time.deltaTime;
                if (intensity <= 2f) intensity = 2f;

                rColor -= rColor * 30f * Time.deltaTime;
                if (rColor <= 0.933f) rColor = 0.933f;
                gColor = 0.933f;
                bColor = 0.933f;
            }
        }
        weapon.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", baseColor * intensity);




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

    void WarSkillAttack3()
    {
        var idelName = "Attack.SkillAttack_3";         //動作名稱
        var skill = SkillAttack_3;


        if (animInfo.IsName(idelName))
        {
            baseColor = new Color(baseColor.r, gColor, bColor); ;
            gColor += gColor * 20f * Time.deltaTime;   //變換速度
            bColor += bColor * 20f * Time.deltaTime;   //變換速度
            if (gColor >= 1f) gColor = 1f;
            if (bColor >= 2f) bColor = 2f;

            intensity += intensity * 20f * Time.deltaTime;   //變換速度
            if (intensity >= 15f) intensity = 15f;  //亮度
            if (animInfo.normalizedTime > 0.5)
            {
                intensity -= intensity * 50f * Time.deltaTime;
                if (intensity <= 1f) intensity = 1f;

                gColor -= gColor * 50f * Time.deltaTime;
                if (gColor <= 0.933f) gColor = 0.933f;
                bColor -= bColor * 50f * Time.deltaTime;
                if (bColor <= 0.933f) bColor = 0.933f;
            }
        }
        weapon.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", baseColor * intensity);

        var SkillAttack_30 = skill.transform.GetChild(0).GetComponent<ParticleSystem>();
        float delay = 0.001f;                            //SkillAttack_30特效播放時間點，面板務必保持為0        
        if (animInfo.IsName(idelName) && animInfo.normalizedTime > delay && !SkillAttack_30.isPlaying) SkillAttack_30.Play();

        //var SkillAttack_31 = skill.transform.GetChild(1).GetComponent<ParticleSystem>();
        //float delay1 = 0.2f;                            //SkillAttack_31特效播放時間點
        //DoEffects(idelName, delay1, SkillAttack_31);

        //var SkillAttack_32 = skill.transform.GetChild(2).GetComponent<ParticleSystem>();
        //float delay2 = 0.4f;                             //SkillAttack_32特效播放時間點，面板務必保持為0
        //DoEffects(idelName, delay2, SkillAttack_32);

        var SkillAttack_33 = skill.transform.GetChild(3).GetComponent<ParticleSystem>();
        float delay3 = 0.7f;
        if (animInfo.IsName(idelName) && animInfo.normalizedTime > delay3 && !SkillAttack_33.isPlaying)
        {
            SkillAttack_33.Play();
            isshakeCamera = true;          //畫面震盪
        }
        else SkillAttack_33.Stop();
    }

    #endregion

    void DoEffects(string idelName, float delay, ParticleSystem effect)
    {
        if (animInfo.IsName(idelName) && animInfo.normalizedTime > delay && !effect.isPlaying)
        {
            effect.Play();
            if (animInfo.normalizedTime > delay + 0.1f) effect.Stop();
        }
        else effect.Stop();
    }



    #region 畫面縮放

    bool pWindownView = false;
    float xTime = 0.4f;                                      //拉近速度，越小越慢    
    float yTime = 12f;                                    //恢復速度，越大越快
    void PowerWindownView()
    {
        if (pWindownView)
        {
            isshakeCamera = true;          //畫面震盪
            Camera.main.fieldOfView -= Camera.main.fieldOfView * xTime * Time.deltaTime;
            if (Camera.main.fieldOfView <= 35f)
            {
                Camera.main.fieldOfView = 35f;
                pWindownView = false;
            }
        }
        else
        {
            Camera.main.fieldOfView += Camera.main.fieldOfView * yTime * Time.deltaTime;
            if (Camera.main.fieldOfView >= fov)
            {
                isshakeCamera = false;
                Camera.main.fieldOfView = fov;
            }
        }

        //   isshakeCamera = true;          //畫面震盪
        //var star = new Vector3(Screen.width / 2, Screen.height / 2);
        //star = Camera.main.ScreenToWorldPoint(star);
        //var n = gameObject.transform.GetChild(0).position - star;
        //xTime -= xTime * Time.deltaTime;
        //Camera.main.transform.forward = n;
        //Camera.main.transform.position -= Camera.main.transform.position * n.magnitude * yTime * Time.deltaTime;
        //if (xTime <= 0f)
        //{
        //    xTime = 2.5f;
        //    isshakeCamera = false;
        //    pWindownView = false;
        //    return;
        //}
        //}
    }
    #endregion

    #region 命中效果

    public void HitEffect(GameObject player, Collider hitPos)
    {
        Vector3 star = player.transform.GetChild(0).position;
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
    #endregion

    #region 命中震盪   
    private float shakeTime = 0.0f;
    private float fps = 20.0f;
    private float frameTime = 0.0f;
    private float shakeDelta = 0.005f;
    // public Camera cam;  不掛鏡頭，減少依賴
    bool isshakeCamera = false;

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
