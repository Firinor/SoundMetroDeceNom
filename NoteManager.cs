using UnityEngine;

public class NoteManager : MonoBehaviour
{
    private bool isFirstNote = true;
    private Melody melody = new Melody();
    [SerializeField]
    private AudioSource audioSource;

    public void MelodyCheck(float cursorPosition)
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
    }

    public void NewTact()
    {
        melody.ResetNotePosition();
        isFirstNote = true;
    }
}
