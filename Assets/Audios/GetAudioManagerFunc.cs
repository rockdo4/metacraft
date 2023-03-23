using UnityEditor.Purchasing;
using UnityEngine;

public class GetAudioManagerFunc : MonoBehaviour
{
    public void PlayBGM(int index)
    {
        AudioManager.Instance.PlayBGM(index);
    }
    public void PlayBGMUsingFadeCross(int index) 
    {
        AudioManager.Instance.ChangeBGMFadeCross(index);
    }
    public void PlayBGMUsingOnlyFadeOut(int index)
    {
        AudioManager.Instance.ChageBGMOnlyFadeOut(index);
    }
    public void PlayUI(int index)
    {        
        AudioManager.Instance.PlayUIAudio(index);
    }
}
