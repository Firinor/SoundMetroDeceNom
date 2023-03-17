using TMPro;
using UnityEngine;

public class LogicCore : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI difficultyText;
    [SerializeField]
    private TextMeshProUGUI startText;

    private ResultOperator resultOperator => (ResultOperator)CoreHUB.ResultOperator.GetValue();
    private NoteBeltOperator noteBeltOperator => (NoteBeltOperator)CoreHUB.NoteBeltOperator.GetValue();
    private DiagramOperator diagramOperator => (DiagramOperator)CoreHUB.DiagramOperator.GetValue();
    private MicrophonOperator microphonOperator => (MicrophonOperator)CoreHUB.MicrophonOperator.GetValue();
    private NoteManager noteManager => (NoteManager)CoreHUB.NoteManager.GetValue();

    private Difficulty difficulty => CoreValuesHUB.Difficulty.GetValue();
    private int beatsPerMinute => CoreValuesHUB.BeatsPerMinute.GetValue();

    private void Awake()
    {
        CoreHUB.LogicCore.SetValue(this);
    }
    private void Start()
    {
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
        startText.text = PlayMode.Start.ToString();
    }

    private void EnabledBeltOperator()
    {
        CoreValuesHUB.PlayMode.SetValue(PlayMode.Start);
        //resultOperator.ResetEvent();
        diagramOperator.ResetEvent();
        noteBeltOperator.enabled = true;
        diagramOperator.enabled = true;
        startText.text = PlayMode.Stop.ToString();
    }

    public void OnDifficultyButton()
    {
        Difficulty difficulty = this.difficulty;
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
        difficultyText.text = difficulty.ToString();
    }

    public void SetCoreValue(CoreValue coreValue, int newValue)
    {
        switch(coreValue)
        {
            case CoreValue.BeatsPerMinute:
                CoreValuesHUB.BeatsPerMinute.SetValue(newValue);
                noteManager.SetNewBeatsPerMinuteInMelody();
                DisabledBeltOperator();
                break;
            case CoreValue.DecibelGate:
                CoreValuesHUB.DecibelGate.SetValue(newValue / 100f);//%
                diagramOperator.SetDecibelGate();
                break;
            case CoreValue.Smooth:
                CoreValuesHUB.Smooth.SetValue(newValue);
                break;
            case CoreValue.Reaction:
                CoreValuesHUB.Reaction.SetValue(newValue);
                noteBeltOperator.ReflectEvent();
                noteManager.GenerateNotesCheckPositions();
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
        noteManager.SetNewBeatsPerMinuteInMelody();
        DisabledBeltOperator();
        diagramOperator.SetDecibelGate();
        noteBeltOperator.ReflectEvent();
        noteManager.GenerateNotesCheckPositions();
    }

    public float GetSoundLengthInSeconds()
    {
        float beatsPerSecond = (float)beatsPerMinute / 60f;
        float soundLength = Melody.NOTES_COUNT / beatsPerSecond;
        return soundLength;//length in seconds
    }
}