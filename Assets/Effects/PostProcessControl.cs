using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessControl : MonoBehaviour
{
    public PostProcessProfile postProcessProfile;
    float FLV = 1f;

    void Start()
    {
        postProcessProfile.GetSetting<DepthOfField>().focusDistance.value = 1f;
        postProcessProfile.GetSetting<DepthOfField>().aperture.value = 5f;
        postProcessProfile.GetSetting<DepthOfField>().focalLength.value = FLV;
    }


    void Update()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");
        if (mx >= 1 || my >= 1)
        {
            FLV += FLV * 100f * Time.deltaTime;
            if (FLV >= 30f) FLV = 30f;
        }
        else FLV = 1f;

    }
}
