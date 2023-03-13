using UnityEngine;

public enum NoteCheckResult { None, Correct, Fast, Slow}
public class NoteManager : MonoBehaviour
{
    private bool isFirstNote = true;
    private Melody melody = new Melody();
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private NoteBeltOperator beltOperator;

    public NoteCheckResult MelodyCheck(float cursorPosition, int noteIndex)
    {
        //Debug.Log(cursorPosition);
        if (melody.isOnNote(cursorPosition))
        {
            AudioClip clip;
            if (isFirstNote)
            {
                clip = Informator.StartOfTactNote;
                isFirstNote = false;
            }
            else
            {
                clip = Informator.DefaulteNote;
            }

            audioSource.clip = clip;
            audioSource.Play();
        }
        return NoteCheckResult.None;
    }

    public void NewTact()
    {
        melody.ResetNotePosition();
        isFirstNote = true;
    }
}
