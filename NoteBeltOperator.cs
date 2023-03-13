using UnityEngine;

public class NoteBeltOperator : MonoBehaviour
{
    [SerializeField]
    private NoteOperator[] notes;
    [SerializeField]
    private MicrophonOperator microphon;
    [SerializeField]
    private NoteManager noteManager;

    private int noteIndex = 0;

    [SerializeField]
    private float distance;
    [SerializeField]
    private float startPosition;
    [SerializeField]
    private float endPosition;
    
    void Update()
    {
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

    private void NoteCheck()
    {
        int soundPosition = microphon.GetPosition();

        NoteCheckResult result = noteManager.MelodyCheck(soundPosition, noteIndex);

        switch (result)
        {
            case NoteCheckResult.None:
                return;
            case NoteCheckResult.Correct:
                notes[noteIndex].SetCorrectNote();
                break;
            case NoteCheckResult.Fast:
            case NoteCheckResult.Slow:
                notes[noteIndex].SetUncorrectNote(result);
                break;
            default:
                return;
        }
        noteIndex++;
    }

    public void ResetEvent()
    {
        noteIndex = 0;
        for (int i = 0; i < notes.Length; i++)
        {
            notes[i].ResetNote();
        }
    }
}
