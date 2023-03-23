using UnityEngine;

public class GetAudioManagerFunc : MonoBehaviour
{
    public void PlayBGM(int index)
    {
        AudioManager.Instance.PlayBGM(index);
    }
    public void PlayBGMUsingFade(int index) 
    {
        AudioManager.Instance.ChangeBGMwithFade(index);
    }
    public void PlayUI(int index)
    {        
        AudioManager.Instance.PlayUIAudio(index);
    }
}
