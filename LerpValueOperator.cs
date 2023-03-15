using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LerpValueOperator : MonoBehaviour
{
    [SerializeField]
    private float min = 1;
    [SerializeField]
    private float step = 1;
    [SerializeField]
    private float max = 500;

    [SerializeField]
    private CoreValue coreValue;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private string conditionalLetterDesignation;

    private LogicCore logicCore => (LogicCore)CoreHUB.LogicCore.GetValue();

    private void Awake()
    {
        SetSliderMinMax(min, max);
    }

    public void SetSliderMinMax(float min, float max)
    {
        slider.minValue = min;
        slider.maxValue = max;
    }

    public void RefreshText(int newValue)
    {
        text.text = newValue + conditionalLetterDesignation;
    }

    public void ChangeValue(bool isAddition)
    {
        slider.value += isAddition ? step : -step;
        //if(beatsPerMinute < beatsStep) { beatsPerMinute = beatsStep; }
        //if (beatsPerMinute > beatsPerMinuteMax) { beatsPerMinute = beatsPerMinuteMax; }
        //RefreshTextBPM();
    }

    public void ChangeValue()
    {
        int newValue = (int)slider.value;
        logicCore.SetCoreValue(coreValue, newValue);
        RefreshText(newValue);
    }
}
