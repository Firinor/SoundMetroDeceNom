using System;
using UnityEngine;

public class DiagramOperator : MonoBehaviour
{
    [SerializeField]
    private RectTransform timeCursor;
    [SerializeField]
    private RectTransform decibelCursor;
    [SerializeField]
    private LineRenderer lineRenderer;
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
    //private PlayModeOperator playModeOperator => (PlayModeOperator)CoreHUB.playModeOperator;

    private void Awake()
    {
        CoreHUB.DiagramOperator.SetValue(this);

        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        CoreValuesHUB.MelodyStartPosition.SetValue(startPosition);
        CoreValuesHUB.MelodyEndPosition.SetValue(endPosition);
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

        float deltaStepsCount = deltaRate * stepsCount;
        float sampleStep = deltaSample / deltaStepsCount;
        float rateStep = deltaRate / deltaStepsCount;

        while (oldRatePosition < newRatePosition)
        {
            float xPosition = GetXPosition(oldRatePosition);

            float soundValue = microphonOperator.GetLoudness((int)oldSamplePosition, (int)sampleStep + 1);
            DrawVolumePoint(soundValue, xPosition);

            oldSamplePosition += sampleStep;
            oldRatePosition += rateStep;
        }
    }

    private float GetXPosition(float positionInRate)
    {
        return Mathf.Lerp(startPosition, endPosition, positionInRate);
    }

    private void DrawVolumePoint(float soundValue, float xPosition)
    {
        if (soundValue < CoreValuesHUB.decibelGate)
            soundValue = 0;

        soundValue = Mathf.Lerp(minValue, maxValue, soundValue);
        lineRenderer.positionCount++;
        Vector3 nextPosition = new Vector3(xPosition, soundValue, 0f);

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
        timeCursor.anchoredPosition = new Vector3(startPosition, 0f, 0f);
        lineRenderer.positionCount = 0;
        oldSamplePosition = 0;
        oldRatePosition = 0;
    }

    public void SetDecibelGate()
    {
        float decibelValue = Mathf.Lerp(minValue, maxValue, CoreValuesHUB.decibelGate);
        decibelCursor.anchoredPosition = new Vector3(0f, decibelValue, 0f);
    }

    public NoteCheckResult MelodySuccessNoteCheck(int i)
    {
        return NoteCheckResult.None;
    }
}
