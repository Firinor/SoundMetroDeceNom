using System;
using UnityEngine;

public class PlayModeOperator : MonoBehaviour
{
    private double playRate = 0;
    private double deltaPlayRate = 0;

    private double timer;
    private double BPS;
    private double BPM;
    private double oldAudioTime = 0;
    private double currentTimer;

    [SerializeField, Min(0)]
    private double notesSoundDelay = 0;
    private double notesDelay => -notesSoundDelay/100d;

    public Melody[] melodies;

    public Action ShiftAction;

    private LogicCore logicCore => (LogicCore)CoreHUB.logicCore;
    private AudioOutputOperator audioOperator => (AudioOutputOperator)CoreHUB.audioOperator;

    private void Awake()
    {
        CoreHUB.PlayModeOperator.SetValue(this);
        ShiftAction += ShiftEvent;
    }

    private void Start()
    {
        ResetEvent();
    }

    void Update()
    {
        if (melodies == null || melodies.Length == 0)
            return;

        double deltaTime = AudioSettings.dspTime - oldAudioTime;
        if (deltaTime <= 0)
            return;
        Debuger.totalTimer += deltaTime;

        deltaTime *= CoreValuesHUB.beatsPerSecond;

        oldAudioTime = AudioSettings.dspTime;

        deltaPlayRate = deltaTime / (double)Melody.NOTES_PER_TACT;
        playRate += deltaPlayRate;

        if (playRate >= 1)
        {
            ShiftAction?.Invoke();
            return;
        }

        if(Debuger.totalTimer >= 30)
        {
            Debug.Log($"totalTimer {Debuger.totalTimer}; noteCount {Debuger.noteCount}");
            Debuger.totalTimer -= 30;
            Debuger.noteCount = 0;
        }

        CoreValuesHUB.MelodyPositionInRate.SetValue(playRate);

        PlayNotes();
    }

    private void PlayNotes()
    {
        foreach (var melody in melodies)
        {
            AudioClip[] clip = melody.CheckNotes(playRate + notesDelay, deltaPlayRate);
            if(clip != null)
            {
                audioOperator.PlayClip(clip);
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
        oldAudioTime = AudioSettings.dspTime;
        playRate = 0;
        Debuger.totalTimer = 0;
        Debuger.noteCount = 0;
        CoreValuesHUB.MelodyPositionInRate.SetValue(playRate);
        ResetMelody();
        logicCore.ResetMicrophone();
    }

    public float GetFirstMelodyNotePosition(int index)
    {
        return melodies[0].GetNotePosition(index);
    }
}
