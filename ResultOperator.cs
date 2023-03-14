using System;
using TMPro;
using UnityEngine;

public class ResultOperator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI hightVolume;
    [SerializeField]
    private TextMeshProUGUI midVolume;
    [SerializeField]
    private TextMeshProUGUI quietVolume;

    [SerializeField]
    private TextMeshProUGUI hightTemp;
    [SerializeField]
    private TextMeshProUGUI midTemp;
    [SerializeField]
    private TextMeshProUGUI lowTemp;

    private void Awake()
    {
        CoreHUB.ResultOperator.SetValue(this);
    }

    public void HightVolume()
    {
        Add(hightVolume);
    }
    public void MidVolume()
    {
        Add(midVolume);
    }
    public void QuietVolume()
    {
        Add(quietVolume);
    }
    public void HightTemp()
    {
        Add(hightTemp);
    }
    public void MidTemp()
    {
        Add(midTemp);
    }
    public void LowTemp()
    {
        Add(lowTemp);
    }

    private void Add(TextMeshProUGUI targetText)
    {
        int newValue = Int32.Parse(targetText.text) + 1;
        targetText.text = newValue.ToString();
    }

    public void ResetEvent()
    {
        hightVolume.text = "0";
        midVolume.text = "0";
        quietVolume.text = "0";

        hightTemp.text = "0";
        midTemp.text = "0";
        lowTemp.text = "0";
    }
}
