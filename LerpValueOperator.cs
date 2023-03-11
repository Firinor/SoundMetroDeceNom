using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LerpValueOperator : MonoBehaviour
{
    [SerializeField]
    private float step = 1;
    [SerializeField]
    private float max = 500;

    [SerializeField]
    private LogicCore logicCore;
    [SerializeField]
    private CoreValue coreValue;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private string conditionalLetterDesignation;

    public void RefreshText(int newValue)
    {
        text.text = newValue + conditionalLetterDesignation;
    }

    public void ChangeValue(bool addTemp)
    {
        slider.value += step * (addTemp ? 1 : -1) / max;
        //if(beatsPerMinute < beatsStep) { beatsPerMinute = beatsStep; }
        //if (beatsPerMinute > beatsPerMinuteMax) { beatsPerMinute = beatsPerMinuteMax; }
        //RefreshTextBPM();
    }

    public void ChangeValue()
    {
        int newValue = (int)Mathf.Lerp(step, max, slider.value);
        logicCore.SetCoreValue(coreValue, newValue);
        RefreshText(newValue);
    }
}
