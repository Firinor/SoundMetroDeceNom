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

    public void SetUncorrectNote(Delay delay)
    {
        resultImage.enabled = true;
        switch (delay)
        {
            case Delay.Hary:
                resultImage.sprite = Informator.HaryResult;
                break;
            case Delay.Slow:
                resultImage.sprite = Informator.SlowResult;
                break;
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
