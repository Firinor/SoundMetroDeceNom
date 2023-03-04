using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;

public enum Difficulty { Easy, Hormal, Hard }

public class LogicCore : MonoBehaviour
{
    private Difficulty difficulty;
    private int BeatsPerMinute = 120;

    [SerializeField]
    private BeatsPerMinuteOperator BeatsPerMinuteOperator;
    [SerializeField]
    private TextMeshProUGUI difficultyText;
    [SerializeField]
    private ResultOperator resultOperator;

    public void ChangeBeats(bool addBPM)
    {
        BeatsPerMinute += 5 * (addBPM? 1: -1);
        if(BeatsPerMinute < 5) { BeatsPerMinute = 5; }
        RefreshTextBPM();
    }

    private void RefreshTextBPM()
    {
        BeatsPerMinuteOperator.RefreshText(BeatsPerMinute);
    }

    public void OnResetButton()
    {
        resultOperator.ResetEvent();
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
