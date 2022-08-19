using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessControl : MonoBehaviour
{
    public PostProcessProfile postProcessProfile;
    float FLV = 1f;

    void Start()
    {
        postProcessProfile.GetSetting<DepthOfField>().focusDistance.value = 2.5f;
        postProcessProfile.GetSetting<DepthOfField>().aperture.value = 0.1f;        
    }


    void Update()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");
        if (mx >= 9 || my >= 8)
        {
            FLV += FLV * 1000f* Time.deltaTime;
            if (FLV >= 15) FLV = 15;           
        }

        if (mx < 0.01f && my < 0.01f)
        {
            FLV -= FLV *1f*  Time.deltaTime;
            if (FLV <= 1f) FLV = 1f;
        }
        postProcessProfile.GetSetting<DepthOfField>().focalLength.value = FLV;
    }
}
