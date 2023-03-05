using TMPro;
using UnityEngine;

public enum Difficulty { Easy, Hormal, Hard }
public enum PlayMode { Start, Stop }

public class LogicCore : MonoBehaviour
{
    private PlayMode playMode = PlayMode.Stop;
    private Difficulty difficulty;
    private int BeatsPerMinute = 120;

    [SerializeField]
    private BeatsPerMinuteOperator BeatsPerMinuteOperator;
    [SerializeField]
    private TextMeshProUGUI difficultyText;
    [SerializeField]
    private TextMeshProUGUI startText;
    [SerializeField]
    private ResultOperator resultOperator;

    [SerializeField]
    private NoteBeltOperator noteBeltOperator;
    [SerializeField]
    private DiagramOperator diagramOperator;

    public void ChangeBeats(bool addTemp)
    {
        BeatsPerMinute += 5 * (addTemp? 1: -1);
        if(BeatsPerMinute < 5) { BeatsPerMinute = 5; }
        RefreshTextBPM();
    }

    private void RefreshTextBPM()
    {
        BeatsPerMinuteOperator.RefreshText(BeatsPerMinute);
    }

    public void OnResetButton()
    {
        if (!noteBeltOperator.enabled)
        {
            playMode = PlayMode.Start;
            resultOperator.ResetEvent();
            diagramOperator.ResetEvent();
            noteBeltOperator.SetBPM(BeatsPerMinute);
            noteBeltOperator.enabled = true;
            diagramOperator.enabled = true;
        }
        else
        {
            playMode = PlayMode.Stop;
            noteBeltOperator.enabled = false;
            diagramOperator.enabled = false;

        }
        startText.text = playMode.ToString();
    }

    public void OnDifficultyButton()
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                difficulty = Difficulty.Hormal;
                break;
            case Difficulty.Hormal:
                difficulty = Difficulty.Hard;
                break;
            case Difficulty.Hard:
                difficulty = Difficulty.Easy;
                break;
        }

        RefreshDifficultyText();
    }

    private void RefreshDifficultyText()
    {
        difficultyText.text = difficulty.ToString();
    }
}
