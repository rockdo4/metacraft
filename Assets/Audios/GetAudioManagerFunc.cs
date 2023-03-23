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
    public void PlayBGMUsingOneWayFade(int index)
    {
        AudioManager.Instance.ChageBGMwithOneWayFade(index);
    }
    public void PlayUI(int index)
    {        
        AudioManager.Instance.PlayUIAudio(index);
    }
}
