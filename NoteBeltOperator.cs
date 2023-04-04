using UnityEngine;

public class NoteBeltOperator : MonoBehaviour
{
    [SerializeField]
    private NoteOperator[] notes;
    private int noteToCheck = 0;

    private DiagramOperator diagramOperator => (DiagramOperator)CoreHUB.diagramOperator;
    private PlayModeOperator playModeOperator => (PlayModeOperator)CoreHUB.playModeOperator;

    private float reactionInRate;
    private float startPosition = 0;
    private float endPosition = 0;

    private void Awake()
    {
        CoreHUB.NoteBeltOperator.SetValue(this);
        enabled = CoreValuesHUB.playMode == PlayMode.Play;
    }

    private void Update()
    {
        NoteCheck();
    }

    private void NoteCheck()
    {
        float notePosition = playModeOperator.GetFirstMelodyNotePositionInRate(noteToCheck);

        if (!isNoteReadyToCheck(notePosition))
            return;

        float noteRatePosition = notePosition / (float)Melody.LENGTH_IN_NOTES;

        NoteCheckResult result = MelodySuccessNoteCheck(noteRatePosition);

        switch (result)
        {
            case NoteCheckResult.None:
                break;
            case NoteCheckResult.Success:
                notes[noteToCheck].SetCorrectNote();
                noteToCheck++;
                break;
            case NoteCheckResult.Fast:
            case NoteCheckResult.Slow:
                notes[noteToCheck].SetUncorrectNote(result);
                noteToCheck++;
                break;
            default:
                break;
        }

        if(noteToCheck >= notes.Length)
            noteToCheck -= notes.Length;
    }

    private bool isNoteReadyToCheck(float notePosition)
    {
        float noteCheckEndPosition = notePosition + endPosition;

        return CoreValuesHUB.cursorPositionInRate * (double)Melody.LENGTH_IN_NOTES > noteCheckEndPosition;
    }

    public NoteCheckResult MelodySuccessNoteCheck(float notePosition)
    {
        
        float noteCheckStartPosition = notePosition + startPosition;
        float soundOnStartValue = diagramOperator.GetLoudness(noteCheckStartPosition - reactionInRate / 2, noteCheckStartPosition);

        if (soundOnStartValue < 0)
            return NoteCheckResult.None;

        //Debug.Log($"notePosition {notePosition}; reactionInRate {reactionInRate}; noteCheckStartPosition {noteCheckStartPosition};" +
        //    $" noteCheckSilencePosition {noteCheckStartPosition - reactionInRate / 2};");

        bool isStartSound = soundOnStartValue >= CoreValuesHUB.decibelGate;

        if (isStartSound)
            return NoteCheckResult.Fast;

        float noteCheckEndPosition = notePosition + endPosition;
        float soundOnEndValue = diagramOperator.GetLoudness(noteCheckStartPosition, noteCheckEndPosition);

        //Debug.Log($"noteCheckEndPosition {noteCheckEndPosition}; soundOnEndValue {soundOnEndValue};");

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
        noteToCheck = 0;

        for (int i = 0; i < notes.Length; i++)
        {
            notes[i].ResetNote();
        }
    }

    public void ReflectEvent()
    {
        float melodyUnitsInSecond = CoreValuesHUB.melodyLengthInUnits / (float)CoreValuesHUB.melodyLengthInSeconds;
        //reaction in milliseconds. It should be divided by 1000 to convert in seconds
        float reactionInUnit = melodyUnitsInSecond * (CoreValuesHUB.reaction / 1000f);
        reactionInRate = (CoreValuesHUB.reaction / 1000f) / (float)CoreValuesHUB.melodyLengthInSeconds;

        //Debug.Log($"CoreValuesHUB.melodyLengthInSeconds {CoreValuesHUB.melodyLengthInSeconds}; reactionInUnit {reactionInUnit};" +
        //    $"reactionInRate {reactionInRate}");
        for (int i = 0; i < notes.Length; i++)
        {
            notes[i].SetReflectPosition(reactionInUnit);
        }

        startPosition = -reactionInRate / 2;
        endPosition = startPosition + reactionInRate;
    }
}
