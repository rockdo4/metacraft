using UnityEngine;

public class MakeWalkSound : MonoBehaviour
{
    public AudioSource[] audioSources;
    int audioNumCount;

    private void Awake()
    {
        var audioSourcesHolder = AudioManager.Instance.GetAudioResourcesHolder(name);

        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i] = Instantiate(audioSources[i], audioSourcesHolder);
        }

        audioNumCount = audioSources.Length;
    }

    public void PlayFootStep()
    {
        audioSources[Random.Range(0, audioNumCount)].Play();
    }

}
