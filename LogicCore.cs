using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum NoteSide { Left, Right }
public enum Delay { Hary, Slow }
public enum CoreValue { BeatsPerMinute, DecibelGate }
public enum Difficulty { Easy, Hormal, Hard }
public enum PlayMode { Start, Stop }

public class LogicCore : MonoBehaviour
{
    private PlayMode playMode = PlayMode.Stop;
    private Difficulty difficulty;
    private int beatsPerMinute = 120;
    private int decibelGate = 10;

    [SerializeField]
    private LerpValueOperator BeatsPerMinuteOperator;
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

    public void OnResetButton()
    {
        if (!noteBeltOperator.enabled)
        {
            playMode = PlayMode.Start;
            resultOperator.ResetEvent();
            diagramOperator.ResetEvent();
            noteBeltOperator.enabled = true;
            diagramOperator.enabled = true;
            noteBeltOperator.ResetEvent();
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

    internal void SetCoreValue(CoreValue coreValue, int newValue)
    {
        switch(coreValue)
        {
            case CoreValue.BeatsPerMinute:
                beatsPerMinute = newValue;
                noteBeltOperator.SetBPM(beatsPerMinute);
                diagramOperator.SetBPM(beatsPerMinute);
                break;
            case CoreValue.DecibelGate:
                decibelGate = newValue;
                break;
        }
    }
}
