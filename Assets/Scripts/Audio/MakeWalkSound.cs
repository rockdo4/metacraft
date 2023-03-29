using UnityEngine;

public class MakeWalkSound : MonoBehaviour
{
    //[Range(0f, 1f)]
    //public float minVolume;
    //[Range(0f, 1f)]
    //public float maxVolume;
    //[Range(0f, 1f)]
    //public float minPitch;
    //[Range(0f, 1f)]
    //public float maxPitch;

    //public AudioSource audioSource;
    public AudioSource[] audioSources;
    public Transform audioSourcesHolder;
    int audioNumCount;

    private void Awake()
    {
        for(int i =0; i < audioSources.Length; i++)
        {
            audioSources[i] = Instantiate(audioSources[i], audioSourcesHolder);
        }

        audioNumCount = audioSources.Length;
    }

    public void PlayFootStep()
    {
        //audioSource.volume = Random.Range(minVolume, maxVolume);
        //audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSources[Random.Range(0, audioNumCount)].Play();
    }

}
