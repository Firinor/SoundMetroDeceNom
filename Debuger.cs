using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Debuger : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    private float min = float.MaxValue;
    private float max = float.MinValue;

    public void Reset()
    {
        min = float.MaxValue; max = float.MinValue;
    }

    void Update()
    {
        if(min > Time.deltaTime)
        {
            min = Time.deltaTime;
        }

        if(max < Time.deltaTime)
        {
            max = Time.deltaTime;
        }

        text.text = $"min {min}; max {max}!;";
    }
}
