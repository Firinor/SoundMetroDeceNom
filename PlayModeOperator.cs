using System;
using UnityEngine;

public class PlayModeOperator : MonoBehaviour
{
    private double timer;
    private double songPosition;
    private double songPosInBeats;
    private double oldAudioTime = 0;
    private double currentTimer;
    private int loopCount = 0;

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
        Debuger.totalTimer2 += Time.deltaTime;

        if (melodies == null || melodies.Length == 0)
            return;

        songPosition = (float)(AudioSettings.dspTime - oldAudioTime);

        if (songPosition <= 0)
            return;
        Debuger.totalTimer = songPosition;

        songPosInBeats = songPosition / CoreValuesHUB.secondsPerBeat;

        if (songPosInBeats >= Melody.NOTES_PER_TACT * (1+loopCount))
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

        double MelodyPositionInBeats = songPosInBeats - Melody.NOTES_PER_TACT * loopCount;
        CoreValuesHUB.MelodyPositionInBeats.SetValue(MelodyPositionInBeats);
        CoreValuesHUB.MelodyPositionInRate.SetValue(MelodyPositionInBeats / (float)Melody.NOTES_PER_TACT);

        PlayNotes();
    }

    private void PlayNotes()
    {
        foreach (var melody in melodies)
        {
            AudioClip clip = melody.CheckNotes(CoreValuesHUB.melodyPositionInBeats - CoreValuesHUB.soundShift);
            if(clip != null)
            {
                audioOperator.PlayClip(clip);
            }
        }
    }

    public void ShiftEvent()
    {
        loopCount ++;
        CoreValuesHUB.MelodyPositionInRate.SetValue((songPosInBeats - Melody.NOTES_PER_TACT * loopCount) / (float)Melody.NOTES_PER_TACT);
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
        return melodies[0].GetNotePosition(index);
    }
}
