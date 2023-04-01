using System;
using System.Collections.Generic;
using UnityEngine;

public class Melody
{
    public const int NOTES_COUNT = 8;
    public const int NOTES_PER_TACT = 4;

    public Note[] melody;
    public List<AudioClip> resultClips;
    public AudioSource AudioSource;

    private int nextIndex;

    public Melody()
    {
        resultClips = new List<AudioClip>();
        SetNewDefaultMelody();
    }

    public void SetNewDefaultMelody()
    {
        melody = new Note[NOTES_COUNT];

        float eighthShare = (float)NOTES_PER_TACT / (float)NOTES_COUNT;
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

    internal AudioClip CheckNotes(double songPosition)
    {
        if(nextIndex >= melody.Length)
            return null;

        resultClips.Clear();

        if (melody[nextIndex].position < songPosition)
        {
            nextIndex++;
            Debuger.noteCount++;
            return melody[nextIndex-1].clip;
        }

        return null;
    }

    //private AudioClip CheckNotesFormTwoTakts(double cursorPosition, double deltaRate)
    //{
    //    double start = cursorPosition - deltaRate;
    //    double start2 = start + 1;
    //    double end = cursorPosition;

    //    resultClips.Clear();
    //    for (int i = 0; i < melody.Length; i++)
    //    {
    //        if (melody[i].position < start)
    //            continue;

    //        if (!melody[i].isPlayed && melody[i].position < end)
    //        {
    //            melody[i].isPlayed = true;
    //            //Debug.Log($"TIME({Time.time}) cursorPosition {cursorPosition}, deltaRate {deltaRate}, melody[{i}] position {melody[i].position}");
    //            resultClips.Add(melody[i].clip);
    //            Debuger.noteCount++;
    //        }
    //        //notes of the previous tact
    //        if (!melody[i].isPlayed && melody[i].position >= start2)
    //        {
    //            resultClips.Add(melody[i].clip);
    //            Debuger.noteCount++;
    //        }
    //    }

    //    if (resultClips.Count > 0)
    //    {
    //        return resultClips.ToArray();
    //    }

    //    return null;
    //}

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