using UnityEngine;

public class ChangeAudioWhenEnabled : MonoBehaviour
{
    public int index;
    private void OnEnable()
    {
        AudioManager.Instance.ChageBGMwithOneWayFade(index);
    }
}
