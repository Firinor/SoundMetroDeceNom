using System;
using System.Collections.Generic;
using UnityEngine;

public class NoteBeltOperator : MonoBehaviour
{
    [SerializeField]
    private NoteOperator[] notes;
    private List<int> notesToCheck = new List<int>();

    private MicrophonOperator microphonOperator => (MicrophonOperator)CoreHUB.microphonOperator;
    private DiagramOperator diagramOperator => (DiagramOperator)CoreHUB.diagramOperator;
    private PlayModeOperator playModeOperator => (PlayModeOperator)CoreHUB.playModeOperator;

    private float reactionInRate;
    private float startPosition = 0;
    private float endPosition = 0;

    private void Awake()
    {
        CoreHUB.NoteBeltOperator.SetValue(this);
        enabled = CoreValuesHUB.playMode == PlayMode.Play;

        playModeOperator.ShiftAction += ResetEvent;
    }

    private void Update()
    {
        NoteCheck();
    }

    private void NoteCheck()
    {
        for(int i = 0; i < notesToCheck.Count; i++)
        {
            float notePosition = playModeOperator.GetFirstMelodyNotePosition(notesToCheck[i]);

            if (!isNoteReadyToCheck(notePosition))
                break;

            NoteCheckResult result = MelodySuccessNoteCheck(notePosition);

            switch (result)
            {
                case NoteCheckResult.None:
                    continue;
                case NoteCheckResult.Success:
                    notes[notesToCheck[i]].SetCorrectNote();
                    notesToCheck.Remove(notesToCheck[i]);
                    break;
                case NoteCheckResult.Fast:
                case NoteCheckResult.Slow:
                    notes[notesToCheck[i]].SetUncorrectNote(result);
                    notesToCheck.Remove(notesToCheck[i]);
                    break;
                default:
                    continue;
            }
        }
    }

    private bool isNoteReadyToCheck(float notePosition)
    {
        float noteCheckEndPosition = notePosition + endPosition;

        return diagramOperator.GetPositionInRate() > noteCheckEndPosition;
    }

    public NoteCheckResult MelodySuccessNoteCheck(float notePosition)
    {
        float noteCheckStartPosition = notePosition + startPosition;
        float soundOnStartValue = diagramOperator.GetLoudness(noteCheckStartPosition - reactionInRate / 2, noteCheckStartPosition);

        if (soundOnStartValue < 0)
            return NoteCheckResult.None;

        bool isStartSound = soundOnStartValue >= CoreValuesHUB.decibelGate;

        if (isStartSound)
            return NoteCheckResult.Fast;

        float noteCheckEndPosition = notePosition + endPosition;
        float soundOnEndValue = diagramOperator.GetLoudness(noteCheckStartPosition, noteCheckEndPosition);

        if (soundOnEndValue < 0)
            return NoteCheckResult.None;

        bool isEndSound = soundOnEndValue >= CoreValuesHUB.decibelGate;

        if (!isEndSound)
            return NoteCheckResult.Slow;

        //All checks have been passed
        return NoteCheckResult.Success;
    }

    public void ResetEvent()
    {
        notesToCheck.Clear();

        for (int i = 0; i < notes.Length; i++)
        {
            notes[i].ResetNote();
            notesToCheck.Add(i);
        }
    }

    public void ReflectEvent()
    {
        float melodyUnitsInSecond = CoreValuesHUB.melodyLengthInUnits / (float)CoreValuesHUB.melodyLengthInSeconds;
        //reaction in milliseconds. It should be divided by 1000 to convert in seconds
        float reactionInUnit = melodyUnitsInSecond * (CoreValuesHUB.reaction / 1000f);
        reactionInRate = (CoreValuesHUB.reaction / 1000f) / (float)CoreValuesHUB.melodyLengthInSeconds;

        for (int i = 0; i < notes.Length; i++)
        {
            notes[i].SetReflectPosition(reactionInUnit);
        }

        startPosition = -reactionInRate / 2;
        endPosition = startPosition + reactionInRate;
    }
}
