using System;
using UnityEngine;

public class Melody
{
    public const int NOTES_COUNT = 8;
    public const int NOTES_PER_TACT = 4;

    public Note[] melody;
    public AudioSource AudioSource;

    //private int notesPerTact = 4;

    public Melody()
    {
        SetNewDefaultMelody();
    }

    public void SetNewDefaultMelody()
    {
        melody = new Note[NOTES_COUNT];

        float eighthShare = 1 / (float)Duration.EIGHTH;
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
                isPlayed = false,
                duration = Duration.EIGHTH,
                position = positionShift + eighthShare * i,
                noteTipe = NoteTipe.Note,
                clip = clip
            };
        }
    }

    internal AudioClip CheckNotes(float cursorPosition, float deltaRate)
    {
        float start = cursorPosition - deltaRate;
        float end = cursorPosition;

        for(int i = 0; i < melody.Length; i++)
        {
            if(!melody[i].isPlayed && melody[i].position >= start && melody[i].position < end)
            {
                melody[i].isPlayed = true;
                return melody[i].clip;
            }
        }

        return null;
    }

    public Note GetNote(int noteIndex)
    {
        return melody[noteIndex];
    }

    internal float GetCurrentNotePosition(int noteIndex)
    {
        return GetNotePosition(noteIndex);
    }

    internal float GetNotePosition(int noteIndex)
    {
        return melody[noteIndex].position;
    }

    internal void ResetEvent()
    {
        for (int i = 0; i < melody.Length; i++)
        {
            melody[i].isPlayed = false;
        }
    }

    public class Note
    {
        public bool isPlayed;
        public int tact;
        public float position;
        public NoteTipe noteTipe;
        public Duration duration;
        public AudioClip clip;
    }
}