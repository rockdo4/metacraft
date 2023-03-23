using UnityEngine;

public class ChangeAudioWhenEnabled : MonoBehaviour
{
    public int index;
    private void OnEnable()
    {
        Logger.Debug(1);
        Logger.Debug(transform.name);
        AudioManager.Instance.ChangeBGMwithFade(index);
    }
}
