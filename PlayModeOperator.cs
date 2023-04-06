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
    private int loopCountToDiagram = 0;

    public Melody[] melodies;
    private AudioClip clip;

    public Action ShiftAction;

    private LogicCore logicCore => (LogicCore)CoreHUB.logicCore;
    private AudioOutputOperator audioOperator => (AudioOutputOperator)CoreHUB.audioOperator;
    private DiagramOperator diagramOperator => (DiagramOperator)CoreHUB.diagramOperator;
    private NoteBeltOperator noteBeltOperator => (NoteBeltOperator)CoreHUB.noteBeltOperator;

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
        if (melodies == null || melodies.Length == 0)
            return;

        songPosition = (float)(AudioSettings.dspTime - oldAudioTime);

        if (songPosition <= 0)
            return;

        songPosInBeats = songPosition / CoreValuesHUB.secondsPerBeat;
        SetCursorPositionCoreValue();

        songPosInBeats = songPosInBeats - CoreValuesHUB.soundShift;

        if (songPosInBeats >= Melody.LENGTH_IN_NOTES * (1 + loopCount))
        {
            ShiftAction?.Invoke();
            return;
        }

        SetMelodyCoreValues();

        PlayNotes();
    }

    private void SetCursorPositionCoreValue()
    {
        cursorPosition = songPosInBeats / (double)Melody.LENGTH_IN_NOTES - loopCountToDiagram;

        if (cursorPosition < 0)
        {
            cursorPosition += 1d;
        }

        if (cursorPosition > 1)
        {
            loopCountToDiagram++;
            diagramOperator.ResetEvent();
            noteBeltOperator.ResetEvent();
            cursorPosition -= 1d;
        }

        CoreValuesHUB.CursorPositionInRate.SetValue(cursorPosition);
    }

    private void SetMelodyCoreValues()
    {
        double MelodyPositionInBeats = songPosInBeats - Melody.LENGTH_IN_NOTES * loopCount;
        CoreValuesHUB.MelodyPositionInBeats.SetValue(MelodyPositionInBeats);
        //CoreValuesHUB.MelodyPositionInRate.SetValue(MelodyPositionInBeats / (float)Melody.LENGTH_IN_NOTES);
    }

    private void PlayNotes()
    {
        foreach (Melody melody in melodies)
        {
            double songPosition = CoreValuesHUB.melodyPositionInBeats;

            clip = melody.CheckNote(songPosition);
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
        loopCountToDiagram = 0;
        ResetMelody();
        logicCore.ResetMicrophone();
    }

    public float GetFirstMelodyNotePositionInRate(int index)
    {
        return melodies[0].GetNotePosition(index);
    }
}
