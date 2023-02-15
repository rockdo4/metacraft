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


    //�̱��� ��ӹ޾����� awake �����Ҷ� ����
    #region �߰��ϰ� ���� Ÿ�� �ؿ��ٰ� SetFloat �ۼ��� Audio Mixer �� ������� ���� �߰�
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
