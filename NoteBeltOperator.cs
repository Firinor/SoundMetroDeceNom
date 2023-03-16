using UnityEngine;

public class NoteBeltOperator : MonoBehaviour
{
    [SerializeField]
    private NoteOperator[] notes;

    private int noteIndex = 0;

    [SerializeField]
    private float distance;
    [SerializeField]
    private float startPosition;
    [SerializeField]
    private float endPosition;

    private int semplesRate => CoreValuesHUB.SampleRate.GetValue();

    private MicrophonOperator microphonOperator => (MicrophonOperator)CoreHUB.MicrophonOperator.GetValue();
    private NoteManager noteManager => (NoteManager)CoreHUB.NoteManager.GetValue();

    private void Awake()
    {
        CoreHUB.NoteBeltOperator.SetValue(this);
        enabled = CoreValuesHUB.PlayMode.GetValue() == PlayMode.Start;
    }

    private void Update()
    {
        MoveCursorPosition();
        NoteCheck();
        //float delta = Time.deltaTime * distance * BPM;

        //for(int i = 0; i < notes.Length; i++)
        //{
        //    Vector3 oldPosition = notes[i].anchoredPosition;
        //    if(oldPosition.x > endPosition)
        //    {
        //        notes[i].anchoredPosition = new Vector3(oldPosition.x - delta, oldPosition.y, 0f);
        //    }
        //    else
        //    {
        //        ResetEvent(i);
        //    }
        //}
    }

    private void MoveCursorPosition()
    {
        int melodyPositionInSamples = microphonOperator.GetMicrophonePositionInSamples();
        CoreValuesHUB.MelodyPositionInSamples.SetValue(melodyPositionInSamples);

        float melodyPositionInPercentages = melodyPositionInSamples / CoreValuesHUB.melodyLengthInSamples;
        CoreValuesHUB.MelodyPositionInRate.SetValue(melodyPositionInPercentages);

        float melodyPositionInSeconds = melodyPositionInPercentages * CoreValuesHUB.melodyLengthInSec;
        CoreValuesHUB.MelodyPositionInSeconds.SetValue(melodyPositionInSeconds);
    }

    private void NoteCheck()
    {
        if (noteIndex >= notes.Length)
            return;

        NoteCheckResult result = noteManager.MelodySuccessNoteCheck();

        switch (result)
        {
            case NoteCheckResult.None:
                return;
            case NoteCheckResult.Success:
                notes[noteIndex].SetCorrectNote();
                noteIndex++;
                break;
            case NoteCheckResult.Fast:
            case NoteCheckResult.Slow:
                notes[noteIndex].SetUncorrectNote(result);
                noteIndex++;
                break;
            default:
                return;
        }
    }

    public void ResetEvent()
    {
        noteIndex = 0;
        int melodyPosition = 0;
        CoreValuesHUB.MelodyPositionInSamples.SetValue(melodyPosition);
        CoreValuesHUB.MelodyPositionInSeconds.SetValue(melodyPosition);
        CoreValuesHUB.MelodyPositionInRate.SetValue(melodyPosition);
        for (int i = 0; i < notes.Length; i++)
        {
            notes[i].ResetNote();
        }
    }

    public void ReflectEvent()
    {
        for (int i = 0; i < notes.Length; i++)
        {
            notes[i].SetReflectPosition();
        }
    }
}
