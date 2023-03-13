using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteOperator : MonoBehaviour
{
    [SerializeField]
    private Image noteImage;
    [SerializeField]
    private Image resultImage;
    [SerializeField]
    private NoteSide side;

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
