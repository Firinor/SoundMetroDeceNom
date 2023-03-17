using System;
using UnityEngine;

public class Melody
{
    public const int NOTES_COUNT = 8;

    public NotePosition[] melody;

    private float cursorPosition;

    private int noteIndex;
    private int notesPerTact = 4;

    public Melody(int beatsPerMinute)
    {
        SetNewBeatsPerMinute(beatsPerMinute);
    }

    public void SetNewBeatsPerMinute(int beatsPerMinute)
    {
        melody = new NotePosition[8];

        float beatsPerSecond = beatsPerMinute / 60f;
        float tactLength = notesPerTact / beatsPerSecond;

        int melodyLength = (int)(AudioSettings.outputSampleRate * tactLength);

        CoreValuesHUB.MelodyLengthInSamples.SetValue(melodyLength);
        CoreValuesHUB.MelodyLengthInSec.SetValue(tactLength);

        int melodyCursorSamplePosition = (int)(melodyLength / (float)Duration.EIGHTH);
        int positionShift = melodyCursorSamplePosition / 3;

        for (int i = 0; i < melody.Length; i++)
        {
            melody[i] = new NotePosition
            {
                duration = Duration.EIGHTH,
                position = positionShift + melodyCursorSamplePosition * i,
                noteTipe = NoteTipe.Note
            };
            Debug.Log($"note {i} position {melody[i].position}");
        }
    }

    internal bool isOnNote(float cursorPosition)
    {
        if(noteIndex < melody.Length && cursorPosition >= melody[noteIndex].position)
        {
            //Debug.Log(notePosition);
            return true;
        }
        
        return false;
    }

    public int GetNote()
    {
        return noteIndex;
    }

    public void NextNote()
    {
        noteIndex++;
    }

    public void ResetNotePosition()
    {
        noteIndex = 0;
    }

    internal int GetCurrentNotePosition()
    {
        return GetNotePosition(noteIndex);
    }

    internal int GetNotePosition(int noteIndex)
    {
        return melody[noteIndex].position;
    }

    public class NotePosition
    {
        public int tact;
        public int position;
        public NoteTipe noteTipe;
        public Duration duration;
    }
}