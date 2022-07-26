using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 腳色浮空(跳躍)
/// </summary>
public class CharactersFloating
{
    public Transform target;//浮空物件 
    public float force;//向上力量
    public float gravity;//重力

    /// <summary>
    /// 浮空/跳躍
    /// </summary>
    /// <param name="target">浮空物件</param>
    /// <param name="force">向上力量</param>
    /// <param name="gravity">重力</param>
    public void OnFloating()
    {
        force -= gravity * Time.deltaTime;//向上力量
        if (force <= 0) force = 0;

        target.position = target.position + Vector3.up * force * Time.deltaTime;//物件向上
    }
}
