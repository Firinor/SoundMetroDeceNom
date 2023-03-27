using UnityEngine;

public class AudioOutputOperator : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSources;

    private void Awake()
    {
        CoreHUB.AudioOperator.SetValue(this);
    }

    public void PlayClip(AudioClip clip)
    {
        audioSources.PlayOneShot(clip);
    }
}
