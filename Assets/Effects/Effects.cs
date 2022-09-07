using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class Effects : MonoBehaviour
{
    public GameObject weapon;      //掛載武器
    public GameObject effects;     //掛載角色身上的effects，以定位特效位置(因為不想用GameObject.Find)     
    public PostProcessProfile postProcessProfile;   //掛載PostProcessProfile

    Animator anim;                 //對應角色動作組件
    AnimatorStateInfo animInfo;    //獲得動作狀態(節省腳本用)   
    ParticleSystem NormalAttack_1;
    ParticleSystem NormalAttack_2;
    ParticleSystem NormalAttack_3;
    ParticleSystem SkillAttack_1;
    ParticleSystem SkillAttack_2;
    ParticleSystem SkillAttack_3;
    ParticleSystem hit;

    //武器發光
    Color baseColor;
    float rColor, gColor, bColor;
    float intensity;
    bool lensDistortion = false;

    //法師特效脫離角色Transform影響
    Transform playerEffectstoWorld;        //角色的父物件
    Transform magicNa2;               //要脫離的特效
    Transform magicNa30;              //要脫離的特效
    Transform magicNa31;              //要脫離的特效
    Transform magicNa32;              //要脫離的特效
    Transform magicNa33;              //要脫離的特效
    Transform magicNa34;              //要脫離的特效
    Transform magicNa35;              //要脫離的特效
    Transform magicBook;

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

        postProcessProfile.GetSetting<LensDistortion>().intensity.value = 0f;                //小魚眼

        playerEffectstoWorld = gameObject.transform.parent;                                //角色的父物件，讓特效脫離角色Transform影響      

        if (anim.runtimeAnimatorController.name == "2_Magician")                        //要脫離的特效
        {
            magicNa2 = NormalAttack_2.transform.GetChild(1);                               //攻擊法盾
            magicNa30 = NormalAttack_3.transform.GetChild(1).GetChild(1).GetChild(3);        //閃電                     
            magicNa31 = NormalAttack_3.transform.GetChild(1).GetChild(2).GetChild(3);            //閃電                    
            magicNa32 = NormalAttack_3.transform.GetChild(1).GetChild(3).GetChild(3);              //閃電 
            magicNa33 = NormalAttack_3.transform.GetChild(1).GetChild(4).GetChild(3);        //閃電                     
            magicNa34 = NormalAttack_3.transform.GetChild(1).GetChild(5).GetChild(3);            //閃電                    
            magicNa35 = NormalAttack_3.transform.GetChild(1).GetChild(6).GetChild(3);              //閃電
            magicBook = SkillAttack_1.transform.GetChild(3);                                     //魔法書
        }

        //武器發光，戰士
        if (anim.runtimeAnimatorController.name == "1_Warrior")
        {
            baseColor = weapon.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor");
            intensity = 1f;
            rColor = 0.933f;
            gColor = 0.933f;
            bColor = 0.933f;
        }

        //拖曳刀光
        if (anim.runtimeAnimatorController.name == "1_Warrior")
        {
            weapon.GetComponent<TrailRenderer>().enabled = false;
        }
    }

    void Update()
    {
        animInfo = anim.GetCurrentAnimatorStateInfo(0);                                      //節省廢話
        UpdaSnake();                                                                       //畫面震盪                                                                                            
        UpdaLensDistortion();
        if (anim.runtimeAnimatorController.name == "1_Warrior")
        {
            WarNormalAttack1();
            WarNormalAttack3();
            WarSkillAttack1();
            WarSkillAttack2();
            WarSkillAttack3();
            WeaponTrailControl();  //武器拖曳刀光
        }
        if (anim.runtimeAnimatorController.name == "2_Magician")
        {
            MagNormalAttack1();
            MagNormalAttack2();
            MagNormalAttack3();
            MagSkillAttack1();
            MagSkillAttack2();
            MagSkillAttack3();
            MagEffectsControl();   //魔法陣
        }
        if (anim.runtimeAnimatorController.name == "3_Archer")   //不一致，若判定出問題來這裡確認
        {
            ArcSkillAttack1();
            ArcSkillAttack3();
        }
    }


    //  float oSize = 0.2f;  //投影法陣
    float booksize = 0.01262763f;
    bool closeMagicBook = false;
    void MagEffectsControl()
    {
        //投影法陣，消失
        //var effect = SkillAttack_1;
        //if (!animInfo.IsName("Attack.SkillAttack_1"))  //如果不在補血狀態
        //{
        //    oSize -= oSize * 10 * Time.deltaTime;
        //    if (oSize <= 0.2f)
        //    {
        //        effect.transform.GetChild(2).gameObject.SetActive(false);
        //        oSize = 0.2f;
        //    }
        //    effect.transform.GetChild(2).GetComponent<Projector>().orthographicSize = oSize;
        //    effect.transform.GetChild(2).gameObject.transform.Rotate(0, 0, 0.5f);
        //}

        //藍色法陣在Idle時停止        
        if (animInfo.IsName("Idle") || animInfo.IsName("Attack.NormalAttack_1"))
        {
            NormalAttack_3.Stop();
        }

        //魔法書
        if (magicBook.gameObject.activeInHierarchy)  //當魔法書啟動時
        {
            if (!closeMagicBook)
            {
                booksize += booksize * 15f * Time.deltaTime;   //開始放大                
                if (booksize >= 0.01262763f) booksize = 0.01262763f;
            }

            magicBook.localScale = new Vector3(booksize, booksize, booksize);

            magicBook.Rotate(Vector3.up, 50 * Time.deltaTime, Space.World);
            magicBook.SetParent(playerEffectstoWorld);

            magicBook.position = Vector3.Lerp(magicBook.position, gameObject.transform.position + (gameObject.transform.right * (0.5f) + gameObject.transform.forward * (0.3f) + gameObject.transform.up * 2f), Time.deltaTime);
            // 偏移量，避免重疊  gameObject.transform.right場景腳色的右邊,gameObject.transform.forward 前面 gameObject.transform.up上面

        }
        else
        {
            magicBook.SetParent(SkillAttack_1.transform);
            magicBook.gameObject.SetActive(false);
            magicBook.GetChild(1).GetComponent<ParticleSystem>().Stop();   //關閉魔法書時關閉金光特效
        }
        if (animInfo.IsName("Pain"))
        {
            closeMagicBook = true;
        }
        if (closeMagicBook)
        {
            booksize -= booksize * 45f * Time.deltaTime;   //跟前面魔法陣啟動時放大有相反作用，所以值要大
            if (booksize <= 0f)
            {
                magicBook.SetParent(SkillAttack_1.transform);
                magicBook.gameObject.SetActive(false);
                magicBook.GetChild(1).GetComponent<ParticleSystem>().Stop();   //關閉魔法書時關閉金光特效
                booksize = 0.008676398f;
                closeMagicBook = false;
            }
            magicBook.localScale = new Vector3(booksize, booksize, booksize);
        }



    }

    void ArcSkillAttack1()
    {
        if (animInfo.IsName("Attack.SkillAttack_1") && animInfo.normalizedTime > 0.7 && animInfo.normalizedTime <= 0.75)
        {
            if (!SkillAttack_1.transform.GetChild(1).GetComponent<ParticleSystem>().isPlaying)
            {
                SkillAttack_1.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
            }
            ArcSa1().transform.SetParent(SkillAttack_1.transform);
            ArcSa1().transform.localPosition = SkillAttack_1.transform.GetChild(2).localPosition;
            ArcSa1().transform.forward = SkillAttack_1.transform.forward;
            ArcSa1().Play();
        }
    }

    List<ParticleSystem> arcSa1List = new List<ParticleSystem>();
    ParticleSystem ArcSa1Pool()
    {
        ParticleSystem hitPs = Instantiate(SkillAttack_1.transform.GetChild(3).GetComponent<ParticleSystem>());
        arcSa1List.Add(hitPs);
        return hitPs;
    }
    ParticleSystem ArcSa1()
    {
        foreach (var hl in arcSa1List)
        {
            if (!hl.isPlaying) return hl;
        }
        return ArcSa1Pool();
    }

    void ArcSkillAttack3()
    {
        var idelName = "Attack.SkillAttack_3";
        var effect = SkillAttack_3;
        if (animInfo.IsName(idelName) && !effect.transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying) 
                                          effect.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        if (animInfo.IsName(idelName) && animInfo.normalizedTime > 0.5 
                                      && animInfo.normalizedTime <= 0.55
                                      && !effect.transform.GetChild(1).GetComponent<ParticleSystem>().isPlaying)
                                          effect.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
    }




    #region 法師

    void MagNormalAttack1()
    {
        if (animInfo.IsName("Attack.NormalAttack_1") && animInfo.normalizedTime > 0.3 && animInfo.normalizedTime <= 0.35)
        {
            MagNa1().transform.SetParent(NormalAttack_1.transform);
            MagNa1().transform.localPosition = NormalAttack_1.transform.GetChild(0).localPosition;
            MagNa1().transform.forward = NormalAttack_1.transform.forward;
            MagNa1().Play();
        }
    }

    List<ParticleSystem> magNa1List = new List<ParticleSystem>();
    ParticleSystem MagNa1Pool()
    {
        ParticleSystem hitPs = Instantiate(NormalAttack_1.transform.GetChild(0).GetComponent<ParticleSystem>());
        magNa1List.Add(hitPs);
        return hitPs;
    }
    ParticleSystem MagNa1()
    {
        foreach (var hl in magNa1List)
        {
            if (!hl.isPlaying) return hl;
        }
        return MagNa1Pool();
    }





    void MagNormalAttack2()
    {
        if (animInfo.IsName("Attack.NormalAttack_2"))
        {
            NormalAttack_2.Play();
            magicNa2.SetParent(playerEffectstoWorld);            //特效播放之後脫離角色Transform影響
        }
        if (magicNa2.GetComponent<ParticleSystem>().isStopped)  //如果特效沒有撥放
        {
            //回到角色層級並恢復相關參數
            magicNa2.SetParent(NormalAttack_2.transform);
            magicNa2.transform.localPosition = NormalAttack_2.transform.GetChild(0).localPosition;
            magicNa2.transform.localRotation = NormalAttack_2.transform.GetChild(0).localRotation;
            magicNa2.transform.localScale = NormalAttack_2.transform.GetChild(0).localScale;
        }
    }

    void MagNormalAttack3()
    {
        if (animInfo.IsName("Attack.NormalAttack_3") && animInfo.normalizedTime <= 0.45)
        {
            NormalAttack_3.Play();
            magicNa30.SetParent(playerEffectstoWorld);            //特效播放之後脫離角色Transform影響
            magicNa31.SetParent(playerEffectstoWorld);            //特效播放之後脫離角色Transform影響
            magicNa32.SetParent(playerEffectstoWorld);            //特效播放之後脫離角色Transform影響
            magicNa33.SetParent(playerEffectstoWorld);            //特效播放之後脫離角色Transform影響
            magicNa34.SetParent(playerEffectstoWorld);            //特效播放之後脫離角色Transform影響
            magicNa35.SetParent(playerEffectstoWorld);            //特效播放之後脫離角色Transform影響
        }
        if (magicNa30.GetComponent<ParticleSystem>().isStopped)  //如果特效沒有撥放
        {
            //回到角色層級並恢復相關參數
            magicNa30.SetParent(NormalAttack_3.transform.GetChild(1).GetChild(1));
            magicNa30.transform.localPosition = NormalAttack_3.transform.GetChild(1).GetChild(1).GetChild(2).localPosition;
            magicNa30.transform.localRotation = NormalAttack_3.transform.GetChild(1).GetChild(1).GetChild(2).localRotation;
            magicNa30.transform.localScale = NormalAttack_3.transform.GetChild(1).GetChild(1).GetChild(2).localScale;
        }
        if (magicNa31.GetComponent<ParticleSystem>().isStopped)  //如果特效沒有撥放
        {
            //回到角色層級並恢復相關參數
            magicNa31.SetParent(NormalAttack_3.transform.GetChild(1).GetChild(2));
            magicNa31.transform.localPosition = NormalAttack_3.transform.GetChild(1).GetChild(2).GetChild(2).localPosition;
            magicNa31.transform.localRotation = NormalAttack_3.transform.GetChild(1).GetChild(2).GetChild(2).localRotation;
            magicNa31.transform.localScale = NormalAttack_3.transform.GetChild(1).GetChild(2).GetChild(2).localScale;
        }
        if (magicNa32.GetComponent<ParticleSystem>().isStopped)  //如果特效沒有撥放
        {
            //回到角色層級並恢復相關參數
            magicNa32.SetParent(NormalAttack_3.transform.GetChild(1).GetChild(3));
            magicNa32.transform.localPosition = NormalAttack_3.transform.GetChild(1).GetChild(3).GetChild(2).localPosition;
            magicNa32.transform.localRotation = NormalAttack_3.transform.GetChild(1).GetChild(3).GetChild(2).localRotation;
            magicNa32.transform.localScale = NormalAttack_3.transform.GetChild(1).GetChild(3).GetChild(2).localScale;
        }
        if (magicNa33.GetComponent<ParticleSystem>().isStopped)  //如果特效沒有撥放
        {
            //回到角色層級並恢復相關參數
            magicNa33.SetParent(NormalAttack_3.transform.GetChild(1).GetChild(4));
            magicNa33.transform.localPosition = NormalAttack_3.transform.GetChild(1).GetChild(4).GetChild(2).localPosition;
            magicNa33.transform.localRotation = NormalAttack_3.transform.GetChild(1).GetChild(4).GetChild(2).localRotation;
            magicNa33.transform.localScale = NormalAttack_3.transform.GetChild(1).GetChild(4).GetChild(2).localScale;
        }
        if (magicNa34.GetComponent<ParticleSystem>().isStopped)  //如果特效沒有撥放
        {
            //回到角色層級並恢復相關參數
            magicNa34.SetParent(NormalAttack_3.transform.GetChild(1).GetChild(5));
            magicNa34.transform.localPosition = NormalAttack_3.transform.GetChild(1).GetChild(5).GetChild(2).localPosition;
            magicNa34.transform.localRotation = NormalAttack_3.transform.GetChild(1).GetChild(5).GetChild(2).localRotation;
            magicNa34.transform.localScale = NormalAttack_3.transform.GetChild(1).GetChild(5).GetChild(2).localScale;
        }
        if (magicNa35.GetComponent<ParticleSystem>().isStopped)  //如果特效沒有撥放
        {
            //回到角色層級並恢復相關參數
            magicNa35.SetParent(NormalAttack_3.transform.GetChild(1).GetChild(6));
            magicNa35.transform.localPosition = NormalAttack_3.transform.GetChild(1).GetChild(6).GetChild(2).localPosition;
            magicNa35.transform.localRotation = NormalAttack_3.transform.GetChild(1).GetChild(6).GetChild(2).localRotation;
            magicNa35.transform.localScale = NormalAttack_3.transform.GetChild(1).GetChild(6).GetChild(2).localScale;
        }
    }


    void MagSkillAttack1()
    {
        var idelName = "Attack.SkillAttack_1";

        //魔法書
        if (animInfo.IsName(idelName))
        {
            //if (animInfo.normalizedTime > 0.001f && !magicBook.gameObject.activeInHierarchy) //當魔法書沒有啟動，就縮到很小，不能為0
            if (!magicBook.gameObject.activeInHierarchy)
            {
                booksize = 0.000000011f;
                magicBook.gameObject.SetActive(true);  //打開魔法書，腳本銜接法陣管理          
            }
        }


        var SkillAttack_10 = SkillAttack_1.transform.GetChild(0).GetComponent<ParticleSystem>();
        if (animInfo.IsName(idelName) && animInfo.normalizedTime > 0.2f) SkillAttack_10.Play();

        var SkillAttack_11 = SkillAttack_1.transform.GetChild(1).GetComponent<ParticleSystem>();
        if (animInfo.IsName(idelName) && animInfo.normalizedTime > 0.2f && !SkillAttack_11.isPlaying) SkillAttack_11.Play();


        //投影法陣
        //if (animInfo.IsName(idelName))
        //{
        //    SkillAttack_1.transform.GetChild(2).gameObject.SetActive(true);  //法陣     
        //    oSize += oSize * 10 * Time.deltaTime;
        //    if (oSize >= 0.8f) oSize = 0.8f;
        //    SkillAttack_1.transform.GetChild(2).GetComponent<Projector>().orthographicSize = oSize;
        //    SkillAttack_1.transform.GetChild(2).gameObject.transform.Rotate(0, 0, 0.5f);
        //    //關閉在法陣管理MagEffectsControl控制
        //}

    }

    void MagSkillAttack2()
    {
        //客製化，解決連續animator造成無法連續施法
        if (animInfo.IsName("Attack.SkillAttack_2") && animInfo.normalizedTime <= 0.001) SkillAttack_2.Stop();
        if (animInfo.IsName("Attack.SkillAttack_2") && animInfo.normalizedTime > 0.05 && animInfo.normalizedTime <= 0.1) SkillAttack_2.Play();
    }


    void MagSkillAttack3()
    {
        var idelName = "Attack.SkillAttack_3";
        float delay = 0.01f;
        var effect = SkillAttack_3;
        if (animInfo.IsName(idelName) && animInfo.normalizedTime > delay && !effect.isPlaying) effect.Play();
        if (animInfo.IsName(idelName) && animInfo.normalizedTime >= 0.1 && animInfo.IsName(idelName) && animInfo.normalizedTime <= 0.15) lensDistortion = true;
    }

    #endregion

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

    float wNa3ScaleY = 1.5f;
    void WarNormalAttack3()
    {
        var idelName = "Attack.NormalAttack_3";         //動作名稱      
        var effect = NormalAttack_3;                    //特效名稱

        if (animInfo.IsName(idelName) && animInfo.normalizedTime > 0.01f
                                      && !effect.transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying)
        {
            effect.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        }
        //if (animInfo.IsName(idelName) && animInfo.normalizedTime > 0.35f
        //                              && animInfo.normalizedTime <= 0.4f
        //                              && !effect.transform.GetChild(1).GetComponent<ParticleSystem>().isPlaying)
        //{
        //    effect.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        //}
        if (animInfo.IsName(idelName) && animInfo.normalizedTime > 0.35f
                                     && animInfo.normalizedTime <= 0.37f) effect.transform.GetChild(2).GetComponent<ParticleSystem>().Play();


        if (animInfo.IsName(idelName) && animInfo.normalizedTime > 0.35f
                                     && animInfo.normalizedTime <= 0.4f
                                     && !effect.transform.GetChild(3).GetComponent<ParticleSystem>().isPlaying)
        {
            effect.transform.GetChild(3).GetComponent<ParticleSystem>().Play();
        }

        //DoEffects(idelName, 0.35f, effect.transform.GetChild(1).GetComponent<ParticleSystem>());
        //DoEffects(idelName, 0.35f, effect.transform.GetChild(2).GetComponent<ParticleSystem>());

        //改變劍大小
        if (animInfo.IsName(idelName))
        {
            if (animInfo.normalizedTime < 0.6)
            {
                weapon.transform.localScale = new Vector3(1f, 1.5f, 1);
                weapon.transform.localPosition = new Vector3(0, 0.49f, 0);
            }
            if (animInfo.normalizedTime >= 0.6)
            {

                wNa3ScaleY -= wNa3ScaleY * 0.5f * Time.deltaTime;
                if (wNa3ScaleY <= 1f) wNa3ScaleY = 1f;
                weapon.transform.localScale = new Vector3(1, wNa3ScaleY, 1);
                weapon.transform.localPosition = new Vector3(0, 0.347f, 0);
            }
        }
        else
        {
            wNa3ScaleY -= wNa3ScaleY * 0.5f * Time.deltaTime;
            if (wNa3ScaleY <= 1f) wNa3ScaleY = 1f;
            weapon.transform.localScale = new Vector3(1, wNa3ScaleY, 1);
            weapon.transform.localPosition = new Vector3(0, 0.347f, 0);
        }

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
        float delay = 0.3f;                            //SkillAttack_30特效播放時間點，面板務必保持為0        
                                                       // if (animInfo.IsName(idelName) && animInfo.normalizedTime > delay && !SkillAttack_30.isPlaying) SkillAttack_30.Play();
        DoEffects(idelName, delay, SkillAttack_30);

        var SkillAttack_31 = skill.transform.GetChild(1).GetComponent<ParticleSystem>();
        float delay1 = 0.3f;                            //SkillAttack_31特效播放時間點
        DoEffects(idelName, delay1, SkillAttack_31);

        var SkillAttack_32 = skill.transform.GetChild(2).GetComponent<ParticleSystem>();
        float delay2 = 0.6f;                             //SkillAttack_32特效播放時間點，面板務必保持為0
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

        //拖曳刀光
        if (animInfo.IsName(idelName) && animInfo.normalizedTime <= 0.4)
        {
            weapon.GetComponent<TrailRenderer>().enabled = true;
        }
        if (animInfo.normalizedTime > 0.4)
        {
            weapon.GetComponent<TrailRenderer>().enabled = false;
        }


        if (animInfo.IsName(idelName) && animInfo.normalizedTime > 0.2f && !skill.transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying)
        {
            skill.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        }

        if (animInfo.IsName(idelName) && animInfo.normalizedTime > 0.7f && !skill.transform.GetChild(1).GetComponent<ParticleSystem>().isPlaying)
        {
            skill.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        }
        if (animInfo.IsName(idelName) && animInfo.normalizedTime > 0.7f && !skill.transform.GetChild(2).GetComponent<ParticleSystem>().isPlaying)
        {
            skill.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
        }
        if (animInfo.IsName(idelName) && animInfo.normalizedTime > 0.8f && !skill.transform.GetChild(3).GetComponent<ParticleSystem>().isPlaying)
        {
            skill.transform.GetChild(3).GetComponent<ParticleSystem>().Play();
            isshakeCamera = true;          //畫面震盪
        }



    }

    void WeaponTrailControl()
    {
        if (!animInfo.IsName("Attack.SkillAttack_3"))
        {
            weapon.GetComponent<TrailRenderer>().enabled = false;
        }
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


    #region 小魚眼
    float uldTime;
    float uldSpeed;
    void UpdaLensDistortion()
    {
        if (lensDistortion)
        {
            uldTime += 2f * Time.deltaTime;
            uldSpeed += 18f * Time.deltaTime;
            postProcessProfile.GetSetting<LensDistortion>().intensity.value = uldSpeed;
            if (uldSpeed >= 35) uldSpeed = 35;
            if (uldTime >= 2) lensDistortion = false;

        }
        if (!lensDistortion)
        {
            uldSpeed -= 1000f * Time.deltaTime;
            if (uldSpeed <= 0) uldSpeed = 0;
            uldTime = 0;
            postProcessProfile.GetSetting<LensDistortion>().intensity.value = uldSpeed;
        }
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

                    if (frameTime > 1 / fps) //震動節奏，越小越頻繁
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
