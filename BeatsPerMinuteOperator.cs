using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BeatsPerMinuteOperator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    public void RefreshText(int newBPM)
    {
        text.text = newBPM + " bpm";
    }
}
