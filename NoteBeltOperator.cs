using UnityEngine;

public class NoteBeltOperator : MonoBehaviour
{
    [SerializeField]
    private NoteOperator[] notes;

    private MicrophonOperator microphonOperator => (MicrophonOperator)CoreHUB.microphonOperator;
    private DiagramOperator diagramOperator => (DiagramOperator)CoreHUB.diagramOperator;

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
        for(int i = 0; i < notes.Length; i++)
        {
            NoteCheckResult result = diagramOperator.MelodySuccessNoteCheck(i);

            switch (result)
            {
                case NoteCheckResult.None:
                    continue;
                case NoteCheckResult.Success:
                    notes[i].SetCorrectNote();
                    break;
                case NoteCheckResult.Fast:
                case NoteCheckResult.Slow:
                    notes[i].SetUncorrectNote(result);
                    break;
                default:
                    continue;
            }
        }
    }

    public void ResetEvent()
    {
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
