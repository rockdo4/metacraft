using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SePlayer : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip audioClip;
    private float lifeTime;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();        
        audioClip = audioSource.clip;
        lifeTime = audioClip.length;
    }
}
