using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer mixer;

    [Range(-80, 0)]
    public float master = 0;

    [Range(-80, 0)]
    public float bgm = 0;

    [Range(-80, 0)]
    public float se = 0;

    [Range(-80, 0)]
    public float ambience = 0;
    public void MixerControl()
    {
        //매 업데이트에서 체크하고 싶지 않음
        mixer.SetFloat(nameof(master), master);
        mixer.SetFloat(nameof(bgm), bgm);
        mixer.SetFloat(nameof(se), se);
        mixer.SetFloat(nameof(ambience), ambience);
    }

}
