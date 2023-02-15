using Chapter.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    public AudioMixer mixer;

    [Range(-80, 0)]
    public float master = 0;

    [Range(-80, 0)]
    public float bgm = 0;

    [Range(-80, 0)]
    public float se = 0;


    //싱글톤 상속받았으니 awake 구현할때 조심
    #region 추가하고 싶은 타입 밑에다가 SetFloat 작성후 Audio Mixer 에 변수명과 같이 추가
    public void MixerControl()
    {        
        mixer.SetFloat(nameof(master), master);
        mixer.SetFloat(nameof(bgm), bgm);
        mixer.SetFloat(nameof(se), se);
    }
    #endregion

    private void Update()
    {        
        MixerControl();
    }

}
