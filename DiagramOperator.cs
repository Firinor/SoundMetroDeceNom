using System;
using System.Collections.Generic;
using UnityEngine;

public class DiagramOperator : MonoBehaviour
{
    [SerializeField]
    private RectTransform timeCursor;
    [SerializeField]
    private RectTransform decibelCursor;
    [SerializeField]
    private LineRenderer lineRenderer;
    //volumePoints.x is sound value; volumePoints.y is position rate
    private List<Vector3> volumePoints = new List<Vector3>();
    [SerializeField]
    private float lineWidth;
    [SerializeField]
    private float stepsCount = 500;

    [SerializeField]
    private float leftEdgePosition;
    [SerializeField]
    private float startPosition;
    [SerializeField]
    private float endPosition;

    [SerializeField]
    private float minValue;
    [SerializeField]
    private float maxValue;

    private float oldRatePosition = 0;

    private MicrophonOperator microphonOperator => (MicrophonOperator)CoreHUB.microphonOperator;
    private PlayModeOperator playModeOperator => (PlayModeOperator)CoreHUB.playModeOperator;

    private void Awake()
    {
        CoreHUB.DiagramOperator.SetValue(this);

        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        CoreValuesHUB.MelodyLengthInUnits.SetValue(endPosition - startPosition);

        playModeOperator.ShiftAction += ResetEvent;
    }
    private void Update()
    {
        float deltaSample = microphonOperator.GetMicrophoneDeltaInSamples();
        if (deltaSample <= 0)
            return;

        MoveCursor();
        DrawVolumeLine(deltaSample);
    }

    private void DrawVolumeLine(float deltaSample)
    {
        float newRatePosition = CoreValuesHUB.melodyPositionInRate;
        float deltaRate = newRatePosition - oldRatePosition;

        if (deltaRate <= 0)
            return;

        float deltaStepsCount = deltaRate * stepsCount;

        if (deltaStepsCount < 1)
            return;

        float sampleStep = deltaSample / deltaStepsCount;
        float rateStep = deltaRate / deltaStepsCount;

        //Debug.Log($"deltaRate = {deltaRate}; deltaSample = {deltaSample}; sampleStep = {sampleStep}; rateStep = {rateStep}; deltaStepsCount = {deltaStepsCount};");

        if (deltaStepsCount <= 0 || sampleStep <= 0 || rateStep <= 0)
            return;

        deltaSample *= -1;
        while (deltaSample < 0)
        {
            float xPosition = GetXPosition(oldRatePosition);

            float soundValue = microphonOperator.GetLoudness((int)deltaSample, (int)sampleStep);
            DrawVolumePoint(soundValue, xPosition, oldRatePosition);
            volumePoints.Add(new Vector3(soundValue, oldRatePosition, 0f));

            deltaSample += sampleStep;
            oldRatePosition += rateStep;
        }
    }

    private float GetXPosition(float positionInRate)
    {
        return Mathf.Lerp(startPosition, endPosition, positionInRate);
    }

    private void DrawVolumePoint(float soundValue, float xPosition, float ratePosition)
    {
        if (soundValue < CoreValuesHUB.decibelGate)
            soundValue = 0;

        soundValue = Mathf.Lerp(minValue, maxValue, soundValue);
        lineRenderer.positionCount++;
        Vector3 nextPosition = new Vector3(xPosition, soundValue, ratePosition);

        lineRenderer.SetPosition(lineRenderer.positionCount - 1, nextPosition);
    }

    private void MoveCursor()
    {
        if (CoreValuesHUB.melodyPositionInRate >= 1)
        {
            ResetEvent();
        }

        float xPosition = Mathf.Lerp(startPosition, endPosition, CoreValuesHUB.melodyPositionInRate);

        timeCursor.anchoredPosition = new Vector3(xPosition, 0f, 0f);
    }

    public void ResetEvent()
    {
        //TO DO DEBUG RESET EVENT
        timeCursor.anchoredPosition = new Vector3(startPosition, 0f, 0f);

        volumePoints.Clear();
        //lineRenderer.positionCount = 0;
        ShiftLineRenderer();

        oldRatePosition = 0;
    }

    private void ShiftLineRenderer()
    {
        float edge = leftEdgePosition + CoreValuesHUB.melodyLengthInUnits;
        int j = 0;
        Vector3 shiftPosition = new Vector3(-CoreValuesHUB.melodyLengthInUnits, 0, -1f);//"z" is a rate value.

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            if (lineRenderer.GetPosition(i).x < edge)
                continue;

            lineRenderer.SetPosition(j, lineRenderer.GetPosition(i) + shiftPosition);
            j++;
        }

        lineRenderer.positionCount = j;
    }

    public void SetDecibelGate()
    {
        float decibelValue = Mathf.Lerp(minValue, maxValue, CoreValuesHUB.decibelGate);
        decibelCursor.anchoredPosition = new Vector3(0f, decibelValue, 0f);
    }

    public float GetLoudness(float noteCheckStartPosition, float noteCheckEndPosition)
    {
        List<float> loudness = new List<float>();

        for(int i = 0; i < volumePoints.Count; i++)
        {
            if (volumePoints[i].y >= noteCheckStartPosition
                && volumePoints[i].y <= noteCheckEndPosition)
            {
                loudness.Add(volumePoints[i].x);
            }
        }

        if(loudness.Count == 0)
        {
            return -1f;
        }

        return Mathf.Max(loudness.ToArray());
    }

    public float GetPositionInRate()
    {
        if(volumePoints.Count == 0)
            return 0f;

        return volumePoints[volumePoints.Count - 1].y;
    }
}
