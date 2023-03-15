using UnityEngine;

public class NoteBeltOperator : MonoBehaviour
{
    [SerializeField]
    private NoteOperator[] notes;

    private int noteIndex = 0;
    private float melodyPosition = 0;

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
        MoveCursorPosition(Time.deltaTime);
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

    private void MoveCursorPosition(float deltaTime)
    {
        //melodyPosition += deltaTime * semplesRate;//
        CoreValuesHUB.MelodyPosition.SetValue(microphonOperator.GetMicrophonePosition());
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
        melodyPosition = 0;
        CoreValuesHUB.MelodyPosition.SetValue(melodyPosition);
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
