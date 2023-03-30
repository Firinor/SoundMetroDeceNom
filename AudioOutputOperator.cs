using UnityEngine;
using System.Media;

public class AudioOutputOperator : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSources;

    private SoundPlayer soundPlayer;

    private void Awake()
    {
        CoreHUB.AudioOperator.SetValue(this);
    }

    public void PlayClip(AudioClip clip)
    {

        audioSources.PlayOneShot(clip);
    }
}
