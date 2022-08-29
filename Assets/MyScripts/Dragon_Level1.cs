using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon_Level1 : MonoBehaviour
{
    [SerializeField] Transform rotateAroundTarger;//³òÂ¶¥Ø¼Ð
    public Transform SetRotateAroundTarger { set { rotateAroundTarger = value; } }

    [Header("¼Æ­È")]
    [SerializeField] float aroundSpeed;//³òÂ¶³t«×
    [SerializeField] float distance;//¶ZÂ÷ª±®a¶ZÂ÷

    void Start()
    {
        aroundSpeed = 5;//³òÂ¶³t«×
        distance = 20;//¶ZÂ÷ª±®a¶ZÂ÷
    }
  
    void Update()
    {
        if (rotateAroundTarger != null)
        {
            Vector3 thisPos = transform.position;
            thisPos.y = 0;
            Vector3 targetPos = rotateAroundTarger.position;
            targetPos.y = 0;
            Vector3 forward = thisPos - targetPos;

            //transform.right = forward;
            
            
            /*if((thisPos - targetPos).magnitude > distance)
            {
                transform.position = transform.position - transform.right * aroundSpeed / 2 * Time.deltaTime;
            }*/               
        }

        transform.position = transform.position + transform.forward * aroundSpeed * Time.deltaTime;
    }
}
