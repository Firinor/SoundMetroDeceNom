using UnityEngine;

public class NoteManager : MonoBehaviour
{
    private bool isFirstNote = true;
    private Melody melody;

    private bool isNoteReadyToCheck = false;
    private int noteCheckStartPosition = 0;
    private int noteCheckEndPosition = 0;
    private int reactionInSamples = 0;

    [SerializeField]
    private AudioSource audioSource;

    private MicrophonOperator microphonOperator => (MicrophonOperator)CoreHUB.MicrophonOperator.GetValue();
    private NoteBeltOperator noteBeltOperator => (NoteBeltOperator)CoreHUB.NoteBeltOperator.GetValue();

    private float decibelGate => CoreValuesHUB.DecibelGate.GetValue();

    private int reaction => CoreValuesHUB.Reaction.GetValue();
    private int sampleRate => CoreValuesHUB.SampleRate.GetValue();

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
            isNoteReadyToCheck = true;
            //reaction in milliseconds. It should be divided by 1000 to convert in seconds
            reactionInSamples = (int)(reaction / 1000f * sampleRate); 
            noteCheckStartPosition = melody.GetCurrentNotePosition() - reactionInSamples / 2;
            noteCheckEndPosition = noteCheckStartPosition + reactionInSamples;
            melody.NextNote();
        }
    }

    public NoteCheckResult MelodySuccessNoteCheck()
    {
        if(!isNoteReadyToCheck)
            return NoteCheckResult.None;
        
        if (noteCheckEndPosition > microphonOperator.GetMicrophonePosition())
            return NoteCheckResult.None;

        isNoteReadyToCheck = false;

        float soundOnStartValue = microphonOperator.GetLoudness(noteCheckStartPosition, reactionInSamples);
        bool volumeOnStart = soundOnStartValue >= decibelGate;
        
        if (volumeOnStart)
            return NoteCheckResult.Fast;

        float soundOnEndValue = microphonOperator.GetLoudness(noteCheckEndPosition, reactionInSamples);
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
}
