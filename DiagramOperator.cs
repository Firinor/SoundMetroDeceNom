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
    private float startPosition;
    [SerializeField]
    private float endPosition;

    [SerializeField]
    private float minValue;
    [SerializeField]
    private float maxValue;

    private float oldSamplePosition = 0;
    private float oldRatePosition = 0;

    private MicrophonOperator microphonOperator => (MicrophonOperator)CoreHUB.microphonOperator;
    private PlayModeOperator playModeOperator => (PlayModeOperator)CoreHUB.playModeOperator;

    private void Awake()
    {
        CoreHUB.DiagramOperator.SetValue(this);

        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        CoreValuesHUB.MelodyLengthInUnits.SetValue(endPosition - startPosition);

        playModeOperator.ResetAction += ResetEvent;
    }
    private void Update()
    {
        if (oldSamplePosition >= microphonOperator.GetMicrophonePositionInSamples())
            return;

        MoveCursor();
        DrawVolumeLine();
    }

    private void DrawVolumeLine()
    {
        float newRatePosition = CoreValuesHUB.melodyPositionInRate;
        float deltaRate = newRatePosition - oldRatePosition;

        if (deltaRate <= 0)
            return;

        int positionInSamples = microphonOperator.GetMicrophonePositionInSamples();
        float deltaSample = positionInSamples - oldSamplePosition;
        if (deltaSample <= 0)
            return;

        float deltaStepsCount = deltaRate * stepsCount;
        if (deltaStepsCount < 1)
            return;

        float sampleStep = deltaSample / deltaStepsCount;
        float rateStep = deltaRate / deltaStepsCount;

        //Debug.Log($"deltaRate = {deltaRate}; deltaSample = {deltaSample}; sampleStep = {sampleStep}; rateStep = {rateStep}; deltaStepsCount = {deltaStepsCount};");

        if (deltaStepsCount <= 0 || sampleStep <= 0 || rateStep <= 0)
            return;

        while (oldRatePosition < newRatePosition)
        {
            float xPosition = GetXPosition(oldRatePosition);

            float soundValue = microphonOperator.GetLoudness((int)oldSamplePosition, (int)sampleStep + 1);
            DrawVolumePoint(soundValue, xPosition, oldRatePosition);
            volumePoints.Add(new Vector3(soundValue, oldRatePosition, 0f));

            oldSamplePosition += sampleStep;
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
        lineRenderer.positionCount = 0;

        oldSamplePosition = 0;
        oldRatePosition = 0;
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
