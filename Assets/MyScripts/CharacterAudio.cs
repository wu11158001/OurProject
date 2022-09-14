using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    AudioSource audioSource;

    [Header("音效")]
    [SerializeField] AudioClip[] thisAudioClips;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="number">音效編號</param>
    void OnPlayClip(int number)
    {
        if (number >= 0 && number < thisAudioClips.Length)
        {
            audioSource.clip = thisAudioClips[number];
            audioSource.Play();
        }
        else Debug.LogError("錯誤音效編號:" + number);
    }
}
