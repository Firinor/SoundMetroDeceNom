using UnityEngine;

public class PlayModeOperator : MonoBehaviour
{
    private float playRate = 0;
    public Melody[] melodies;

    [SerializeField]
    private AudioSource audioSource;

    private void Awake()
    {
        CoreHUB.PlayModeOperator.SetValue(this);
    }

    void Update()
    {
        if (melodies == null || melodies.Length == 0)
            return;

        float deltaTime = Time.deltaTime * CoreValuesHUB.beatsPerSecond;
        playRate += deltaTime / Melody.NOTES_PER_TACT;

        if (playRate >= 1)
        {
            ResetEvent();
            return;
        }

        CoreValuesHUB.MelodyPositionInRate.SetValue(playRate);

        PlayNotes();
    }

    private void PlayNotes()
    {
        foreach (var melody in melodies)
        {
            AudioClip clip = melody.CheckNotes(playRate, Time.deltaTime);
            if(clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }

    public void ResetEvent()
    {
        playRate = 0;
        CoreValuesHUB.MelodyPositionInRate.SetValue(playRate);
    }
}
