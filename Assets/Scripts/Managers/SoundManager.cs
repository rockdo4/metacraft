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
    public static List<LinkedList<(SePlayer use, SePlayer unuse)>> sePlayersPool;
    public int SePlayersCount { get; set; } = 0;
    public int SePlayersMaxCount = 30;

    //싱글톤 상속받았으니 awake 구현할때 조심    
    private void Start()
    {
        //sePlayersPool = new List<LinkedList<(SePlayer use, SePlayer unuse)>>((int));
    }
    private void Update()
    {        
        MixerControl();
    }
    public void MixerControl()
    {
        //매 업데이트에서 체크하고 싶지 않음
        mixer.SetFloat(nameof(master), master);
        mixer.SetFloat(nameof(bgm), bgm);
        mixer.SetFloat(nameof(se), se);
    }
}
