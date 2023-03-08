using System;
using UnityEngine;

public class DiagramOperator : MonoBehaviour
{
    [SerializeField]
    private RectTransform timeCursor;
    [SerializeField]
    private float yCursorPosition;
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private float lineWidth;
    [SerializeField]
    private MicrophonOperator microphon;

    private int beatsPerSecond = 2;
    private float tactDelta;
    [SerializeField]
    private float startPosition;
    [SerializeField]
    private float endPosition;
    [SerializeField]
    private float notesInTact = 4;

    [SerializeField]
    private float minValue;
    [SerializeField]
    private float maxValue;

    [SerializeField]
    private float thrashold = 0.1f;

    private void Awake()
    {
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        tactDelta = (endPosition - startPosition)/notesInTact;
    }
    private void Update()
    {
        MoveCursor();
        DrawVolumeLine();
    }

    private void DrawVolumeLine()
    {
        float soundValue = microphon.GetLoudness();

        if(soundValue < thrashold)
            soundValue = 0;

        soundValue = Mathf.Lerp(minValue, maxValue, soundValue);
        lineRenderer.positionCount++;
        Vector3 nextPosition = new Vector3(timeCursor.anchoredPosition.x, soundValue, 0f);
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, nextPosition);
    }

    public void SetBPM(int beatsPerMinute)
    {
        beatsPerSecond = beatsPerMinute / 60;
    }
    private void MoveCursor()
    {
        //120 bpm 4/4
        //2 bps = 1/2 tact
        //2 såñ= 1 tact
        float delta = Time.deltaTime * tactDelta * beatsPerSecond;

        Vector3 oldPosition = timeCursor.anchoredPosition;

        if (oldPosition.x > endPosition)
            ResetEvent();

        timeCursor.anchoredPosition = new Vector3(timeCursor.anchoredPosition.x + delta, yCursorPosition, 0f);
    }

    public void ResetEvent()
    {
        timeCursor.anchoredPosition = new Vector3(startPosition, yCursorPosition, 0f);
        lineRenderer.positionCount = 0;
    }
}
