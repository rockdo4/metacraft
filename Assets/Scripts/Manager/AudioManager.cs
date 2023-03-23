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

    private string currBgmName;

    public int awakeMusicIndex;
    private GameObject bgmHolder;
    private GameObject uiHolder;

    private Dictionary<string, AudioSource> bgmSources;

    public override void Awake()
    {
        base.Awake();
        AudioInstance();
        SaveBGMOriginVolumes();
        PlayBGM(awakeMusicIndex);
    }
    //private void BGMListToDictionary()
    //{
    //    foreach(var bgm in bgms)
    //    {
    //        bgmSources.Add(bgm.clip.name, bgm);
    //    }
    //}

    private void Update()
    {
        MixerControl();
    }
    private void AudioInstance()
    {
        bgmHolder = new GameObject("BGMHolder");
        bgmHolder.transform.SetParent(transform);

        uiHolder = new GameObject("UIHolder");
        uiHolder.transform.SetParent(transform);

        for (int i = 0; i < bgms.Length; i++)
        {
            bgms[i] = Instantiate(bgms[i], bgmHolder.transform);
            DontDestroyOnLoad(bgms[i]);
        }

        for (int i = 0; i < uiAudios.Length; i++)
        {
            uiAudios[i] = Instantiate(uiAudios[i], uiHolder.transform);
            DontDestroyOnLoad(uiAudios[i]);
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
    public void MixerControl()
    {
        //매 업데이트에서 체크하고 싶지 않음
        mixer.SetFloat(nameof(master), master);
        mixer.SetFloat(nameof(bgm), bgm);
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
    }
    public void ChangeBGMwithFade(int index)
    {
        if(coBgmFadeCoroutine != null)
            StopCoroutine(coBgmFadeCoroutine);

        coBgmFadeCoroutine = StartCoroutine(CoBGMFadeCoroutine(index));
    }    
    private IEnumerator CoBGMFadeCoroutine(int index)
    {
        float timer = 0f;
          
        float sourBgmVolume = bgms[currBgmIndex].volume;
        float destBgmVolume = bgms[index].volume;

        float divFadeTime = 1 / bgmFadeTime;
        bool destBgmStartPlay = false;

        while (true)
        {
            timer += Time.deltaTime;
         
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

    public void StopAllBGM()
    {
        foreach(var bgm in bgms)
        {
            bgm.Stop();            
        }
    }
}
