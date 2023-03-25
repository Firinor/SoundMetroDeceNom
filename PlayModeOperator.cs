using System;
using UnityEngine;

public class PlayModeOperator : MonoBehaviour
{
    private float playRate = 0;
    private float deltaPlayRate = 0;
    public Melody[] melodies;

    [SerializeField]
    private AudioSource audioSource;

    public Action ResetAction;

    private LogicCore logicCore => (LogicCore)CoreHUB.logicCore;

    private void Awake()
    {
        CoreHUB.PlayModeOperator.SetValue(this);
        ResetAction += ResetEvent;
    }

    void Update()
    {
        if (melodies == null || melodies.Length == 0)
            return;

        float deltaTime = Time.deltaTime * CoreValuesHUB.beatsPerSecond;
        deltaPlayRate = deltaTime / Melody.NOTES_PER_TACT;
        playRate += deltaPlayRate;

        if (playRate >= 1)
        {
            ResetAction?.Invoke();
            return;
        }

        CoreValuesHUB.MelodyPositionInRate.SetValue(playRate);

        PlayNotes();
    }

    private void PlayNotes()
    {
        foreach (var melody in melodies)
        {
            AudioClip clip = melody.CheckNotes(playRate, deltaPlayRate);
            if(clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }

    public void ResetEvent()
    {
        playRate -= 1;
        CoreValuesHUB.MelodyPositionInRate.SetValue(playRate);

        foreach (var melody in melodies)
        {
            melody.ResetEvent();
        }

        logicCore.ResetMicrophone();
    }

    public float GetFirstMelodyNotePosition(int index)
    {
        return melodies[0].GetNotePosition(index);
    }
}
