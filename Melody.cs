using UnityEngine;

public enum NoteTipe { Note, Pause }

public enum Duration
{
    WHOLE = 1,
    HALF = 2,
    QUARTER = 4,
    EIGHTH = 8,
    SIXTEENTH = 16,
}

public class Melody
{
    public const int NOTES_COUNT = 8;

    public NotePosition[] melody;

    private float cursorPosition;

    private int notePosition;
    private int beatsPerMinute;
    private int notesPerTact = 4;

    public Melody()
    {
        melody = new NotePosition[8];

        float beatsPerSecond = beatsPerMinute / 60;
        float tactLength = notesPerTact / beatsPerSecond;

        int melodyCursorSamplePosition =  (int)(AudioSettings.outputSampleRate * tactLength / (float)Duration.EIGHTH);

        for (int i = 0; i < melody.Length; i++)
        {
            melody[i] = new NotePosition
            {
                duration = Duration.EIGHTH,
                position = melodyCursorSamplePosition * (i+1),
                noteTipe = NoteTipe.Note,
            };
        }
    }

    internal bool isOnNote(float cursorPosition)
    {
        if(notePosition < melody.Length && cursorPosition > melody[notePosition].position)
        {
            notePosition++;
            return true;
        }
        
        return false;
    }

    public void ResetNotePosition()
    {
        notePosition = 0;
    }

    public class NotePosition
    {
        public int tact;
        public int position;
        public NoteTipe noteTipe;
        public Duration duration;
    }
}
