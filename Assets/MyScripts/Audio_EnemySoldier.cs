using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_EnemySoldier : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audioClip;
    [SerializeField] float volume;

    void Start()
    {
        audioSource.volume = volume;
    }

    /// <summary>
    /// ¼½©ñ­µ®Ä
    /// </summary>
    void OnAudioPlayer(int number )
    {
        audioSource.clip = audioClip[number];
        audioSource.Play();
    }
}
