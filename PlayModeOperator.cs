using System;
using UnityEngine;

public class PlayModeOperator : MonoBehaviour
{
    private float playRate = 0;
    private float deltaPlayRate = 0;

    [SerializeField, Min(0)]
    private float notesSoundDelay = 0;
    private float notesDelay => notesSoundDelay/100f;

    public Melody[] melodies;

    [SerializeField]
    private AudioSource audioSource;

    public Action ShiftAction;

    private LogicCore logicCore => (LogicCore)CoreHUB.logicCore;

    private void Awake()
    {
        CoreHUB.PlayModeOperator.SetValue(this);
        ShiftAction += ShiftEvent;
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
            ShiftAction?.Invoke();
            return;
        }

        CoreValuesHUB.MelodyPositionInRate.SetValue(playRate);

        PlayNotes();
    }

    private void PlayNotes()
    {
        foreach (var melody in melodies)
        {
            AudioClip clip = melody.CheckNotes(playRate + notesDelay, deltaPlayRate);
            if(clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }

    public void ShiftEvent()
    {
        playRate -= 1;
        CoreValuesHUB.MelodyPositionInRate.SetValue(playRate);
        ResetMelody();
    }

    private void ResetMelody()
    {
        foreach (var melody in melodies)
        {
            melody.ResetEvent();
        }
    }

    public void ResetEvent()
    {
        playRate = 0;
        CoreValuesHUB.MelodyPositionInRate.SetValue(playRate);
        ResetMelody();
        logicCore.ResetMicrophone();
    }

    public float GetFirstMelodyNotePosition(int index)
    {
        return melodies[0].GetNotePosition(index);
    }
}
