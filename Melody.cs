using UnityEngine;

public class Melody
{
    public const int NOTES_COUNT = 8;
    public const int LENGTH_IN_NOTES = 4;

    public Note[] melody;
    public AudioSource AudioSource;

    private int nextIndex;

    public Melody()
    {
        SetNewDefaultMelody();
    }

    public void SetNewDefaultMelody()
    {
        melody = new Note[NOTES_COUNT];

        float eighthShare = (float)LENGTH_IN_NOTES / (float)NOTES_COUNT;
        float positionShift = eighthShare / 2f;//half

        for (int i = 0; i < melody.Length; i++)
        {
            AudioClip clip;
            if (i == 0)
                clip = Informator.StartOfTactNote;
            else
                clip = Informator.DefaultNote;

            melody[i] = new Note
            {
                duration = Duration.EIGHTH,
                position = positionShift + eighthShare * i,
                clip = clip
            };
        }
    }

    internal AudioClip CheckNote(double songPosition)
    {
        AudioClip result = null;

        if (nextIndex < melody.Length && songPosition > melody[nextIndex].position)
        {
            result = melody[nextIndex].clip;

            nextIndex++;
        }

        return result;
    }

    internal float GetNotePosition(int noteIndex)
    {
        return melody[noteIndex].position;
    }

    internal void ResetEvent()
    {
        SetNewDefaultMelody();
    }

    internal void MelodyNextTact()
    {
        nextIndex = 0;
    }

    public class Note
    {
        public int tact;
        public float position;
        public Duration duration;
        public AudioClip clip;
    }
}