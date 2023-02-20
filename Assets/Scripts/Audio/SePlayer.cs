using System;
using System.Collections.Generic;
using UnityEngine;

public class SePlayer : MonoBehaviour
{
    [SerializeField]
    private SeList clipName;
    public SeList ClipName { get { return clipName; } }

    [SerializeField]
    private int maxPlayCount = 30;
    public int MaxPlayCount { get { return maxPlayCount; } }
    
    private Queue<SePlayer> use;
    private Queue<SePlayer> unUse;

    private AudioSource audioSource; 

    private void Awake()
    {
        TryGetComponent(out audioSource);        
        audioSource.clip ??= Resources.Load<AudioClip>($"SEtest/{clipName}");    
    }
    protected virtual void Update()
    {        
        CheckIsPlay();
    }

    private void CheckIsPlay()
    {
        if(!audioSource.isPlaying)
        {
            unUse.Enqueue(use.Dequeue());
            gameObject.SetActive(false);
        }
    }
    public void SetPool((Queue<SePlayer> use, Queue<SePlayer> unuse) pool)
    {
        use = pool.use;
        unUse = pool.unuse;       
    }
    public void PlaySE()
    {
        if(!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        audioSource.Play();
    }
}
