using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Boss : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPlayAudio(int number)
    {
        audioSource.clip = audioClips[number];
        audioSource.Play();
    }
}
