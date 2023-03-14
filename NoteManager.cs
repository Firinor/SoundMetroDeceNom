using System;
using UnityEngine;
using static Melody;

public class NoteManager : MonoBehaviour
{
    private bool isFirstNote = true;
    private Melody melody;

    private bool isNoteReadyToCheck = false;
    private int noteCheckStartPosition = 0;
    private int noteCheckEndPosition = 0;

    [SerializeField]
    private AudioSource audioSource;

    private MicrophonOperator microphonOperator => (MicrophonOperator)CoreHUB.MicrophonOperator.GetValue();
    private NoteBeltOperator noteBeltOperator => (NoteBeltOperator)CoreHUB.NoteBeltOperator.GetValue();

    private float decibelGate => CoreValuesHUB.DecibelGate.GetValue();

    private int reaction => CoreValuesHUB.Reaction.GetValue();

    private void Awake()
    {
        CoreHUB.NoteManager.SetValue(this);
        melody = new Melody(CoreValuesHUB.BeatsPerMinute.GetValue());
    }

    public void SetNewBeatsPerMinuteInMelody(int beatsPerMinute)
    {
        melody.SetNewBeatsPerMinute(beatsPerMinute);
    }


    private void Update()
    {
        MelodySoundByPositionCheck(CoreValuesHUB.MelodyPosition.GetValue());
    }

    private void MelodySoundByPositionCheck(float cursorPosition)
    {
        //Debug.Log(cursorPosition);
        if (melody.isOnNote(cursorPosition))
        {
            //Debug.Log("mic " + microphonOperator.GetMicrophonePosition());
            //Debug.Log("cur " + cursorPosition + " " + noteIndex);
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

            isNoteReadyToCheck = true;
            noteCheckStartPosition = melody.GetCurrentNotePosition();
            Debug.Log("noteCheckStartPosition " + noteCheckStartPosition);
            noteCheckEndPosition = noteCheckStartPosition + reaction;
            Debug.Log("noteCheckEndPosition " + noteCheckEndPosition);
            melody.NextNote();
        }
    }

    public NoteCheckResult MelodySuccessNoteCheck(int cursorPosition)
    {
        if(!isNoteReadyToCheck)
            return NoteCheckResult.None;

        if (microphonOperator.GetMicrophonePosition() < noteCheckEndPosition)
            return NoteCheckResult.None;

        Debug.Log("cursorPosition " + cursorPosition);

        isNoteReadyToCheck = false;

        float soundOnStartValue = microphonOperator.GetLoudness(cursorPosition, reaction);
        bool volumeOnStart = soundOnStartValue >= decibelGate;
        Debug.Log("soundOnStartValue " + soundOnStartValue);
        
        if (volumeOnStart)
            return NoteCheckResult.Fast;

        float soundOnEndValue = microphonOperator.GetLoudness(cursorPosition, reaction);
        bool volumeOnEnd = soundOnEndValue >= decibelGate;
        Debug.Log("soundOnEndValue " + soundOnEndValue);
        if (!volumeOnEnd)
            return NoteCheckResult.Slow;

        //All checks have been passed
        return NoteCheckResult.Success;
    }

    public void NewTact()
    {
        melody.ResetNotePosition();
        noteBeltOperator.ResetEvent();
        microphonOperator.StartRecording(Microphone.devices[0]);
        isFirstNote = true;
    }
}
