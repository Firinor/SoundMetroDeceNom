using System;
using UnityEngine;

public class PlayModeOperator : MonoBehaviour
{
    private double timer;
    private double songPosition;
    private double songPosInBeats;
    private double oldAudioTime = 0;
    private double cursorPosition;
    private int loopCount = 0;

    public Melody[] melodies;

    public Action ShiftAction;

    private LogicCore logicCore => (LogicCore)CoreHUB.logicCore;
    private AudioOutputOperator audioOperator => (AudioOutputOperator)CoreHUB.audioOperator;

    private void Awake()
    {
        CoreHUB.PlayModeOperator.SetValue(this);
        ShiftAction += NextTact;
    }

    private void Start()
    {
        ResetEvent();
    }

    void Update()
    {
        Debuger.totalTimer2 += Time.deltaTime;

        if (melodies == null || melodies.Length == 0)
            return;

        songPosition = (float)(AudioSettings.dspTime - oldAudioTime);

        if (songPosition <= 0)
            return;
        Debuger.totalTimer = songPosition;

        songPosInBeats = songPosition / CoreValuesHUB.secondsPerBeat;
        SetCursorPositionCoreValue();

        Debug.Log($"coreValuesHUB.CursorPositionInRate {CoreValuesHUB.cursorPositionInRate}");
        songPosInBeats = songPosInBeats - CoreValuesHUB.soundShift;

        if (songPosInBeats >= Melody.LENGTH_IN_NOTES * (1 + loopCount))
        {
            ShiftAction?.Invoke();
            return;
        }

        //if(Debuger.totalTimer >= 30)
        //{
        //    Debug.Log($"songPosition {songPosition}; totalTimer {Debuger.totalTimer};" +
        //        $" totalTimer {Debuger.totalTimer2}; noteCount {Debuger.noteCount}");
        //    Debuger.totalTimer -= 30;
        //    Debuger.totalTimer2 -= 30;
        //    Debuger.noteCount = 0;
        //}

        SetMelodyCoreValues();

        PlayNotes();
    }

    private void SetCursorPositionCoreValue()
    {
        cursorPosition = songPosInBeats / (double)Melody.LENGTH_IN_NOTES - loopCount;

        if (cursorPosition < 0)
            cursorPosition += 1d;
        if (cursorPosition > 1)
            cursorPosition -= 1d;

        CoreValuesHUB.CursorPositionInRate.SetValue(cursorPosition);
    }

    private void SetMelodyCoreValues()
    {
        double MelodyPositionInBeats = songPosInBeats - Melody.LENGTH_IN_NOTES * loopCount;
        CoreValuesHUB.MelodyPositionInBeats.SetValue(MelodyPositionInBeats);
        CoreValuesHUB.MelodyPositionInRate.SetValue(MelodyPositionInBeats / (float)Melody.LENGTH_IN_NOTES);
    }

    private void PlayNotes()
    {
        foreach (Melody melody in melodies)
        {
            double songPosition = CoreValuesHUB.melodyPositionInBeats;

            AudioClip clip = melody.CheckNote(songPosition);
            if(clip != null)
            {
                audioOperator.PlayClip(clip);
            }
        }
    }

    public void NextTact()
    {
        loopCount ++;
        SetMelodyCoreValues();
        foreach (Melody melody in melodies)
        {
            melody.MelodyNextTact();
        }
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
        loopCount = 0;
        CoreValuesHUB.MelodyPositionInRate.SetValue(0);
        Debuger.totalTimer = 0;
        Debuger.totalTimer2 = 0;
        Debuger.noteCount = 0;
        ResetMelody();
        logicCore.ResetMicrophone();
    }

    public float GetFirstMelodyNotePosition(int index)
    {
        return melodies[0].GetNotePosition(index) - CoreValuesHUB.soundShift;
    }
}
