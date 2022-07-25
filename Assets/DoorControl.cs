using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    public GameObject player;
    int diration;
    float value;

    private void Start()
    {
        if (transform.localEulerAngles.y == 0) diration = 1;
        else diration = -1;
    }

    void Update()
    {
        if(player != null && value < 90)
        {
            if(Mathf.Abs(transform.position.x - player.transform.position.x) < 5 && Mathf.Abs(transform.position.z - player.transform.position.z) < 5)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + (diration * 30 * Time.deltaTime), transform.localEulerAngles.z);
                value += 30 * Time.deltaTime;
            }
        }
    }
}
