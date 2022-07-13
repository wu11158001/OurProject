using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 骷顱士兵控制
/// </summary>
public class SkeletonSoldierControl : MonoBehaviour
{
    Animator animator;
    
    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");//設定Layer

        animator = GetComponent<Animator>();

        if (GetComponent<CharactersCollision>() == null) gameObject.AddComponent<CharactersCollision>();
    }
}
