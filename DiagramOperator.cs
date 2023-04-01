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

    //volumePoints.x is sound value; volumePoints.y is position rate;
    private List<Vector3> volumePoints;
    //We start drawing diatram from step 200
    private int loudnessIndex = 200;
    [SerializeField]
    private Vector3 startPoint;

    [SerializeField]
    private float lineWidth;
    [SerializeField]
    private float desiredStepsCount = 500;

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
        LineRendererInitialization();

        CoreValuesHUB.MelodyLengthInUnits.SetValue(endPosition - startPosition);

        playModeOperator.ShiftAction += ResetEvent;
    }

    private void LineRendererInitialization()
    {
        volumePoints = new List<Vector3>();

        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        lineRenderer.positionCount = loudnessIndex;
        for (int i =0; i < loudnessIndex; i++)
        {
            lineRenderer.SetPosition(i, startPoint);
        }
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
        float newRatePosition = (float)CoreValuesHUB.melodyPositionInRate;
        float deltaRate = newRatePosition - oldRatePosition;

        if (deltaRate <= 0)
            return;

        float deltaStepsCount = deltaRate * desiredStepsCount;

        if (deltaStepsCount < 1)
            return;

        float sampleStep = deltaSample / deltaStepsCount;
        float rateStep = deltaRate / deltaStepsCount;

        Debug.Log($"deltaRate = {deltaRate}; deltaSample = {deltaSample}; sampleStep = {sampleStep};" +
            $" rateStep = {rateStep}; deltaStepsCount = {deltaStepsCount}; CoreValuesHUB.melodyPositionInRate = {CoreValuesHUB.melodyPositionInRate};");

        if (deltaStepsCount <= 0 || sampleStep <= 0 || rateStep <= 0)
            return;

        deltaSample = -deltaSample;
        while (deltaSample < 0)
        {
            float xPosition = GetXPosition(oldRatePosition);

            float soundValue = microphonOperator.GetLoudness((int)deltaSample, (int)sampleStep);
            Debug.Log($"soundValue = {soundValue}; xPosition = {xPosition}; oldRatePosition = {oldRatePosition};" +
                $" deltaSample = {deltaSample};");

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
        //if (CoreValuesHUB.melodyPositionInRate >= 1)
        //{
        //    ResetEvent();
        //}

        float xPosition = Mathf.Lerp(startPosition, endPosition, (float)CoreValuesHUB.melodyPositionInRate);

        timeCursor.anchoredPosition = new Vector3(xPosition, 0f, 0f);
    }

    public void ResetEvent()
    {
        Debug.Log("ResetEvent()");
        timeCursor.anchoredPosition = new Vector3(startPosition, 0f, 0f);

        ShiftLineRenderer();

        lineRenderer.positionCount = loudnessIndex;
        oldRatePosition = 0;
    }

    private void ShiftLineRenderer()
    {
        int j = 0;
        Vector3 shiftPosition = new Vector3(-CoreValuesHUB.melodyLengthInUnits, 0, -1f);//"z" is a rate value.

        for (int i = 0; i < loudnessIndex; i++)
        {
            int index = lineRenderer.positionCount - loudnessIndex + i;
            lineRenderer.SetPosition(j, lineRenderer.GetPosition(index) + shiftPosition);
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
        float maxValue = 0;
        for (int i = 0; i < volumePoints.Count; i++)
        {
            if (volumePoints[i].y >= noteCheckStartPosition
                && volumePoints[i].y <= noteCheckEndPosition
                && maxValue < volumePoints[i].x)
            {
                maxValue = volumePoints[i].x;
            }
        }

        return maxValue;
    }

    public float GetPositionInRate()
    {
        float maxValue = 0;
        for (int i = 0; i < volumePoints.Count; i++)
        {
            if (maxValue < volumePoints[i].y)
            {
                maxValue = volumePoints[i].y;
            }
        }

        return maxValue;
    }
}
