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
    private AudioClip audioClip;
    private float lifeTime;
    private float timer;
    
    private void Awake()
    {
        TryGetComponent(out audioSource);        
        audioSource.clip ??= Resources.Load<AudioClip>($"SEtest/{clipName}");        

        audioClip = audioSource.clip;        
        lifeTime = audioClip.length;
        timer = lifeTime;
    }
    protected virtual void Update()
    {
        CheckLifeTime();
    }
    private void CheckLifeTime()
    {
        if(timer < 0)
        {
            timer = lifeTime;
            unUse.Enqueue(use.Dequeue());
            gameObject.SetActive(false);
        }
        timer -= Time.deltaTime;
    }
    public void SetPool(Queue<SePlayer> use, Queue<SePlayer> unUse)
    {
        this.use = use;
        this.unUse = unUse;
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
