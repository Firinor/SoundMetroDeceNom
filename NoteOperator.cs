using UnityEngine;
using UnityEngine.UI;

public class NoteOperator : MonoBehaviour
{
    private const float  DISTANCE_BETWEEN_NOTES = 70.5f;

    [SerializeField]
    private Image noteImage;
    [SerializeField]
    private Image resultImage;
    [SerializeField]
    private NoteSide side;

    [SerializeField]
    private RectTransform silenseCursor;
    [SerializeField]
    private RectTransform startCursor;
    [SerializeField]
    private RectTransform endCursor;
    [SerializeField]
    private Image silenseCursorImage;
    [SerializeField]
    private Image startCursorImage;
    [SerializeField]
    private Image endCursorImage;

    private int beatsPerMinute => CoreValuesHUB.BeatsPerMinute.GetValue();
    private int reaction => CoreValuesHUB.Reaction.GetValue();

    public void ResetNote()
    {
        resultImage.enabled= false;
        switch(side)
        {
            case NoteSide.Left:
                noteImage.sprite = Informator.BlackLeftNote;
                break;
            case NoteSide.Right:
                noteImage.sprite = Informator.BlackRightNote;
                break;
        }

        Difficulty difficulty = CoreValuesHUB.Difficulty.GetValue();

        if(difficulty != Difficulty.Easy)
        {
            silenseCursorImage.enabled= false;
            startCursorImage.enabled= false;
            endCursorImage.enabled= false;
        }
        else
        {
            silenseCursorImage.enabled = true;
            startCursorImage.enabled = true;
            endCursorImage.enabled = true;
        }
    }

    public void SetReflectPosition()
    {
        float reactionInSecondLength = (reaction / 1000f) * (beatsPerMinute / 60f) * DISTANCE_BETWEEN_NOTES;
        
        float noteCheckStartPosition = -reactionInSecondLength / 2;
        float noteCheckSilensePosition = noteCheckStartPosition - reactionInSecondLength;
        float noteCheckEndPosition = noteCheckStartPosition + reactionInSecondLength;

        MoveCursor(silenseCursor, noteCheckSilensePosition);
        MoveCursor(startCursor, noteCheckStartPosition);
        MoveCursor(endCursor, noteCheckEndPosition);
    }

    private void MoveCursor(RectTransform cursor, float value)
    {
        cursor.anchoredPosition = new Vector3(value, cursor.anchoredPosition.y, 0f);
    }

    public void SetCorrectNote()
    {
        resultImage.enabled = true;
        resultImage.sprite = Informator.GoodResult;
        switch (side)
        {
            case NoteSide.Left:
                noteImage.sprite = Informator.GreenLeftNote;
                break;
            case NoteSide.Right:
                noteImage.sprite = Informator.GreenRightNote;
                break;
        }
    }

    public void SetUncorrectNote(NoteCheckResult noteCheckResult)
    {
        resultImage.enabled = true;
        switch (noteCheckResult)
        {
            case NoteCheckResult.Fast:
                resultImage.sprite = Informator.HaryResult;
                break;
            case NoteCheckResult.Slow:
                resultImage.sprite = Informator.SlowResult;
                break;
            default:
                return;
        }
        switch (side)
        {
            case NoteSide.Left:
                noteImage.sprite = Informator.RedLeftNote;
                break;
            case NoteSide.Right:
                noteImage.sprite = Informator.RedRightNote;
                break;
        }
    }
}
