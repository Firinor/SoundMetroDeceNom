using TMPro;
using UnityEngine;

public class LogicCore : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI difficultyText;
    [SerializeField]
    private TextMeshProUGUI startText;

    private ResultOperator resultOperator => (ResultOperator)CoreHUB.resultOperator;
    private NoteBeltOperator noteBeltOperator => (NoteBeltOperator)CoreHUB.noteBeltOperator;
    private DiagramOperator diagramOperator => (DiagramOperator)CoreHUB.diagramOperator;
    private MicrophonOperator microphonOperator => (MicrophonOperator)CoreHUB.microphonOperator;
    private PlayModeOperator playModeOperator => (PlayModeOperator)CoreHUB.playModeOperator;

    private void Awake()
    {
        CoreHUB.LogicCore.SetValue(this);
    }
    private void Start()
    {

        playModeOperator.melodies = new Melody[1] { new Melody() };
        ResetEvent();
    }

    public void OnResetButton()
    {
        if (noteBeltOperator.enabled)
            DisabledBeltOperator();
        else
            EnabledBeltOperator();
    }

    private void DisabledBeltOperator()
    {
        CoreValuesHUB.PlayMode.SetValue(PlayMode.Stop);
        noteBeltOperator.enabled = false;
        diagramOperator.enabled = false;
        playModeOperator.enabled = false;
        startText.text = PlayMode.Play.ToString();
    }

    private void EnabledBeltOperator()
    {
        CoreValuesHUB.PlayMode.SetValue(PlayMode.Play);
        noteBeltOperator.enabled = true;
        diagramOperator.enabled = true;
        playModeOperator.enabled = true;
        noteBeltOperator.ResetEvent();
        diagramOperator.ResetEvent();
        playModeOperator.ResetEvent();
        startText.text = PlayMode.Stop.ToString();
    }

    public void OnDifficultyButton()
    {
        Difficulty difficulty = CoreValuesHUB.difficulty;
        switch (difficulty)
        {
            case Difficulty.Easy:
                CoreValuesHUB.Difficulty.SetValue(Difficulty.Hormal);
                break;
            case Difficulty.Hormal:
                CoreValuesHUB.Difficulty.SetValue(Difficulty.Hard);
                break;
            case Difficulty.Hard:
                CoreValuesHUB.Difficulty.SetValue(Difficulty.Easy);
                break;
        }

        RefreshDifficultyText();
    }

    private void RefreshDifficultyText()
    {
        difficultyText.text = CoreValuesHUB.difficulty.ToString();
    }

    public void SetCoreValue(CoreValue coreValue, int newValue)
    {
        switch(coreValue)
        {
            case CoreValue.BeatsPerMinute:
                CoreValuesHUB.BeatsPerMinute.SetValue(newValue);
                noteBeltOperator.ReflectEvent();
                break;
            case CoreValue.DecibelGate:
                CoreValuesHUB.DecibelGate.SetValue(newValue / 100f);//%
                diagramOperator.SetDecibelGate();
                break;
            case CoreValue.Reaction:
                CoreValuesHUB.Reaction.SetValue(newValue);
                noteBeltOperator.ReflectEvent();
                break;
        }
    }

    public void ResetMicrophone()
    {
        string microphoneName = Microphone.devices[0];
        microphonOperator.StartRecording(microphoneName);
    }
    public void ResetEvent()
    {
        playModeOperator.ResetEvent();
        diagramOperator.ResetEvent();
        diagramOperator.SetDecibelGate();
        noteBeltOperator.ResetEvent();
        noteBeltOperator.ReflectEvent();
    }
}