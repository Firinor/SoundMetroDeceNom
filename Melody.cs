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
    public NotePosition[] melody;
    private float[] notesValues;
    private float cursorPosition;
    private int notePosition;

    public Melody()
    {
        melody = new NotePosition[8];
        for(int i = 0; i < melody.Length; i++)
        {
            melody[i] = new NotePosition
            {
                noteTipe = NoteTipe.Note,
                duration = Duration.EIGHTH
            };
        }
        GenetareValues();
    }

    private void GenetareValues()
    {
        float melodyCursorPosition = 0;

        notesValues = new float[melody.Length];
        for (int i = 0; i < melody.Length; i++)
        {
            notesValues[i] = melodyCursorPosition;
            melodyCursorPosition += 1 / (float)melody[i].duration;
        }
    }

    internal bool isOnNote(float cursorPosition)
    {
        cursorPosition = Mathf.Clamp(cursorPosition, 0, 1);

        if(notePosition < notesValues.Length && cursorPosition > notesValues[notePosition])
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
