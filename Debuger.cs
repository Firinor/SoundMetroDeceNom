using System;
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
    [SerializeField]
    private float resetTimer = 5f;
    private float timer = 0f;

    void Awake()
    {
        Application.targetFrameRate = 60;
    }
    public void ResetEvent()
    {
        timer = 0f;
        min = float.MaxValue; max = float.MinValue;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= resetTimer)
        {
            ResetEvent();
        }

        if (min > Time.deltaTime)
        {
            min = Time.deltaTime;
        }

        if(max < Time.deltaTime)
        {
            max = Time.deltaTime;
        }

        text.text = $"FPS: {Application.targetFrameRate};" + Environment.NewLine +
            $" min {min}; max {max}!;";
    }
}
