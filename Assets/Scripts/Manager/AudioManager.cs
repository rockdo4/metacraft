using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : Singleton<AudioManager>
{
    public AudioMixer mixer;

    [Range(-80, 0)]
    public float master   = 0;

    [Range(-80, 0)]
    public float bgm      = 0;

    [Range(-80, 0)]
    public float se       = 0;

    [Range(-80, 0)]
    public float ambience = 0;

    public AudioSource[] uiAudios;
    public AudioSource[] bgms;
    private float[] bgmOriginVolumes;

    public float bgmFadeTime = 2f;
    public float bgmFadeInterval = 1f;

    Coroutine coBgmFadeCoroutine;
    private int currBgmIndex;    

    public int awakeMusicIndex = 0;
    private GameObject bgmHolder;
    private GameObject uiSoundHolder;

    private Dictionary<string, GameObject> audioResourcesHolders;

    public override void Awake()
    {
        base.Awake();
        AudioInstance();
        SaveBGMOriginVolumes();
        PlayBGM(awakeMusicIndex);
        audioResourcesHolders = new Dictionary<string, GameObject>();
    }
    private void AudioInstance()
    {
        bgmHolder = new GameObject("BGMHolder");
        bgmHolder.transform.SetParent(transform);

        uiSoundHolder = new GameObject("UISoundHolder");
        uiSoundHolder.transform.SetParent(transform);

        for (int i = 0; i < bgms.Length; i++)
        {
            bgms[i] = Instantiate(bgms[i], bgmHolder.transform);            
        }

        for (int i = 0; i < uiAudios.Length; i++)
        {
            uiAudios[i] = Instantiate(uiAudios[i], uiSoundHolder.transform);            
        }
    }
    private void SaveBGMOriginVolumes()
    {
        bgmOriginVolumes = new float[bgms.Length];

        for (int i = 0; i < bgms.Length; i++)
        {
            bgmOriginVolumes[i] = bgms[i].volume;
        }
    }
    public void SetBGMVolume(float volume)
    {   
        bgm = volume;
        mixer.SetFloat(nameof(bgm), bgm);
    }
    public void SetSEVolume(float volume) 
    {
        se = volume;
        ambience = volume;

        mixer.SetFloat(nameof(se), se);
        mixer.SetFloat(nameof(ambience), ambience);
    }
    public void PlayUIAudio(int index)
    {        
        uiAudios[index].Play();
    }
    public void ChangeBGMFadeTime(float fadeTime)
    {
        bgmFadeTime = fadeTime;
    }
    public void ChangeBGMFadeInterval(float fadeInterval)
    {
        bgmFadeInterval = fadeInterval;
    }
    public void PlayBGM(int index) 
    {
        StopAllBGM();
        currBgmIndex = index;        
        bgms[index].volume = bgmOriginVolumes[index];
        bgms[index].Play();
        Logger.Debug($"{index}번 트랙 재생");
    }
    public void ChageBGMOnlyFadeOut(int index)
    {
        if (coBgmFadeCoroutine != null)
            StopCoroutine(coBgmFadeCoroutine);
        
        coBgmFadeCoroutine = StartCoroutine(CoBGMOnlyFadeOut(index));
        Logger.Debug($"{index}번 트랙 단방향 페이드 재생");
    }
    public void ChangeBGMFadeCross(int index)
    {
        if(coBgmFadeCoroutine != null)
            StopCoroutine(coBgmFadeCoroutine);

        coBgmFadeCoroutine = StartCoroutine(CoBGMCrossFade(index));
        Logger.Debug($"{index}번 트랙 크로스 페이드 재생");
    }    
    private IEnumerator CoBGMCrossFade(int index)
    {
        float timer = 0f;
          
        float sourBgmVolume = bgms[currBgmIndex].volume;
        float destBgmVolume = bgmOriginVolumes[index];

        float divFadeTime = 1 / bgmFadeTime;
        bool destBgmStartPlay = false;

        while (true)
        {
            timer += Time.fixedDeltaTime;            
         
            bgms[currBgmIndex].volume = Mathf.Lerp(sourBgmVolume, 0, divFadeTime * timer);

            if (timer > bgmFadeInterval && !destBgmStartPlay)
            {                
                bgms[index].Play();
                destBgmStartPlay = true;
            }

            if (destBgmStartPlay)
            {
                bgms[index].volume = Mathf.Lerp(0, destBgmVolume, divFadeTime * (timer - bgmFadeInterval));
            }

            if(timer > bgmFadeInterval + bgmFadeTime)
            {
                if (currBgmIndex.Equals(index))
                    yield break;

                bgms[currBgmIndex].Stop();
                currBgmIndex = index;
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator CoBGMOnlyFadeOut(int index)
    {
        float timer = 0f;

        float sourBgmVolume = bgms[currBgmIndex].volume;

        float divFadeTime = 1 / bgmFadeTime;
        bool destBgmStartPlay = false;

        while (true)
        {
            timer += Time.fixedDeltaTime;

            bgms[currBgmIndex].volume = Mathf.Lerp(sourBgmVolume, 0, divFadeTime * timer);

            if (timer > bgmFadeInterval && !destBgmStartPlay)
            {
                bgms[index].Play();
                bgms[index].volume = bgms[index].volume;
                destBgmStartPlay = true;
            }

            if (timer > bgmFadeTime)
            {
                if (currBgmIndex.Equals(index))
                    yield break;

                bgms[currBgmIndex].Stop();
                currBgmIndex = index;
                yield break;
            }

            yield return null;
        }
    }

    public void StopAllBGM()
    {
        if (coBgmFadeCoroutine != null)
            StopCoroutine(coBgmFadeCoroutine);

        foreach (var bgm in bgms)
        {
            bgm.Stop();            
        }
    }
    public int GetCurrBGMIndex()
    {
        return currBgmIndex;
    }
  
    public Transform GetAudioResourcesHolder(string name)
    {
        if (audioResourcesHolders.ContainsKey(name))
        {
            return audioResourcesHolders[name].transform;
        }
        else
        {
            var value = new GameObject(name);
            value.transform.parent = transform;
            audioResourcesHolders.Add(name, value );
            return value.transform;   
        }
    }
}
