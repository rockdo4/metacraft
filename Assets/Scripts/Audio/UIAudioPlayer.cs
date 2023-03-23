using UnityEngine;

public class UIAudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] audioClips = new AudioClip[0];
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void Play()
    {
        audioSource.Play();
    }

    public void Play(int index)
    {
        audioSource.PlayOneShot(audioClips[index]);
    }
}
