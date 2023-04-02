using UnityEngine;

public class SouthSilverActive : MonoBehaviour
{
    public AudioSource charge;
    public AudioSource fire;
    public AudioSource land;

    public float fireDelay = 2.6f;
    public float landDelay = 3f;

    private string playFireSound = nameof(PlayFireSound);
    private string playLandSound = nameof(PlayLandSound);

    private void Awake()
    {
        var audioSourcesHolder = AudioManager.Instance.GetAudioResourcesHolder(transform.parent.name);

        charge = Instantiate(charge, audioSourcesHolder);
        fire= Instantiate(fire, audioSourcesHolder);
        land= Instantiate(land, audioSourcesHolder);
    }

    private void SSilverActive()
    {
        charge.Play();
        Invoke(playFireSound, fireDelay);
        Invoke(playLandSound, landDelay);
    }
    private void PlayLandSound()
    {
        land.Play();
    }
    private void PlayFireSound()
    {
        fire.Play();
    }
}
