using System;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    private bool isFirstNote = true;
    private Melody melody;
    private int oldPosition = 0;

    private int[] noteCheckStartPosition;
    private int[] noteCheckEndPosition;
    private int reactionInSamples = 0;

    [SerializeField]
    private AudioSource audioSource;

    private MicrophonOperator microphonOperator => (MicrophonOperator)CoreHUB.MicrophonOperator.GetValue();
    private NoteBeltOperator noteBeltOperator => (NoteBeltOperator)CoreHUB.NoteBeltOperator.GetValue();

    private float decibelGate => CoreValuesHUB.DecibelGate.GetValue();

    private void Awake()
    {
        CoreHUB.NoteManager.SetValue(this);
        melody = new Melody(CoreValuesHUB.beatsPerMinute);
    }

    public void SetNewBeatsPerMinuteInMelody()
    {
        melody.SetNewBeatsPerMinute(CoreValuesHUB.beatsPerMinute);
    }


    private void Update()
    {
        MelodySoundByPositionCheck(CoreValuesHUB.melodyPositionInSamples);
    }

    private void MelodySoundByPositionCheck(float cursorPosition)
    {
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

            melody.NextNote();
        }
    }

    public void GenerateNotesCheckPositions()
    {
        noteCheckStartPosition = new int[Melody.NOTES_COUNT];
        noteCheckEndPosition = new int[Melody.NOTES_COUNT];

        //reaction in milliseconds. It should be divided by 1000 to convert in seconds
        reactionInSamples = (int)(CoreValuesHUB.reaction / 1000f * CoreValuesHUB.sampleRate);
        Debug.Log($"1reactionInSamples {reactionInSamples}");

        for(int i = 0; i < noteCheckStartPosition.Length; i++)
        {
            noteCheckStartPosition[i] = melody.GetNotePosition(i) - reactionInSamples / 2;
            noteCheckEndPosition[i] = noteCheckStartPosition[i] + reactionInSamples;
        }
        
    }

    public NoteCheckResult MelodySuccessNoteCheck(int noteIndex)
    {
        if (noteCheckEndPosition[noteIndex] > microphonOperator.GetMicrophonePositionInSamples())
            return NoteCheckResult.None;
        
        Debug.Log($"2noteIndex {noteIndex}");
        Debug.Log($"2noteCheckEndPosition {noteCheckEndPosition[noteIndex]}");
        Debug.Log($"2microphonePositionInSamples {microphonOperator.GetMicrophonePositionInSamples()}");

        float soundOnStartValue = microphonOperator.GetLoudness(noteCheckStartPosition[noteIndex], reactionInSamples / 2);
        float soundOnEndValue = microphonOperator.GetLoudness(noteCheckEndPosition[noteIndex], reactionInSamples);

        Debug.Log($"2soundOnStartValue {soundOnStartValue}");
        Debug.Log($"2soundOnEndValue {soundOnEndValue}");
        Debug.Log($"2melody {melody.GetNote()}");
        bool volumeOnStart = soundOnStartValue >= decibelGate;
        
        if (volumeOnStart)
            return NoteCheckResult.Fast;

        
        bool volumeOnEnd = soundOnEndValue >= decibelGate;
        
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

    internal bool KeyPosition(float samplePosition)
    {
        for (int i = 0; i < Microphone.devices.Length;)
        {
            //    if()
            //        return true;
            //    if ()
            //        return true;
        }
        //oldPosition;

        return false;
    }
}
