using System;
using TMPro;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public enum NoteSide { Left, Right }
public enum Delay { Hary, Slow }
public enum CoreValue { BeatsPerMinute, DecibelGate, Smooth, Reaction }
public enum Difficulty { Easy, Hormal, Hard }
public enum PlayMode { Start, Stop }

public class LogicCore : MonoBehaviour
{
    private PlayMode playMode = PlayMode.Stop;
    private Difficulty difficulty;
    private int beatsPerMinute = 120;
    private int decibelGate = 10;
    private int smooth = 64;
    private int reaction = 300;

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
    [SerializeField]
    private MicrophonOperator microphonOperator;

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

    public void SetCoreValue(CoreValue coreValue, int newValue)
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
                diagramOperator.SetDecibelGate(decibelGate);
                break;
            case CoreValue.Smooth:
                smooth = newValue;
                microphonOperator.SetSmooth(smooth);
                break;
            case CoreValue.Reaction:
                reaction = newValue;

                break;
        }
    }

    public void ResetMicrophone()
    {
        string microphoneName = Microphone.devices[0];
        microphonOperator.StartRecording(microphoneName);
    }

    public int GetSoundLength()
    {
        float beatsPerSecond = (float)beatsPerMinute / 60f;
        float soundLength = Melody.NOTES_COUNT / beatsPerSecond;
        int extraSecondOfSafe = 1;
        return (int)soundLength + extraSecondOfSafe;//length in seconds
    }
}
