using UnityEngine;
//using System.Media;

public class AudioOutputOperator : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSources;

    //private SoundPlayer soundPlayer;

    private void Awake()
    {
        CoreHUB.AudioOperator.SetValue(this);
    }

    public void PlayClip(AudioClip[] clips)
    {
        foreach (AudioClip clip in clips)
        {
            audioSources.PlayOneShot(clip);
        }
    }

    public void PlayClip(AudioClip clip)
    {

        audioSources.clip = clip;
        audioSources.Play();
    }
}
