using System.Collections.Generic;
using UnityEngine;

public class NoteBeltOperator : MonoBehaviour
{
    [SerializeField]
    private NoteOperator[] notes;
    private List<int> notesToCheck;

    private MicrophonOperator microphonOperator => (MicrophonOperator)CoreHUB.microphonOperator;
    private DiagramOperator diagramOperator => (DiagramOperator)CoreHUB.diagramOperator;
    private PlayModeOperator playModeOperator => (PlayModeOperator)CoreHUB.playModeOperator;

    private void Awake()
    {
        CoreHUB.NoteBeltOperator.SetValue(this);
        enabled = CoreValuesHUB.playMode == PlayMode.Play;

        playModeOperator.ResetAction += ResetEvent;

        notesToCheck = new List<int>();
    }

    private void Update()
    {
        NoteCheck();
    }

    private void NoteCheck()
    {
        for(int i = 0; i < notesToCheck.Count; i++)
        {

            //if(notes[notesToCheck[i]].)

            NoteCheckResult result = diagramOperator.MelodySuccessNoteCheck(notesToCheck[i]);

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
        for (int i = 0; i < notes.Length; i++)
        {
            notes[i].SetReflectPosition();
        }
    }
}
