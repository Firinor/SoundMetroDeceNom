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
    private float startPosition;
    [SerializeField]
    private float endPosition;

    [SerializeField]
    private float minValue;
    [SerializeField]
    private float maxValue;

    private float decibelGate => CoreValuesHUB.DecibelGate.GetValue();
    private float melodyPosition => CoreValuesHUB.MelodyPositionInSamples.GetValue();
    //private int smooth => CoreValuesHUB.Smooth.GetValue();
    private float oldMelodyPosition = 0;

    private MicrophonOperator microphonOperator => (MicrophonOperator)CoreHUB.MicrophonOperator.GetValue();
    private NoteManager noteManager => (NoteManager)CoreHUB.NoteManager.GetValue();
    //private NoteBeltOperator noteBeltOperator => (NoteBeltOperator)CoreHUB.NoteBeltOperator.GetValue();

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
        if (oldMelodyPosition >= melodyPosition)
            return;

        MoveCursor();
        DrawVolumeLine();
    }

    private void DrawVolumeLine()
    {
        float floatStep = CoreValuesHUB.melodyLengthInSamples / 300f;//%

        int positionStep = (int)floatStep;

        if (positionStep <= 0)
            return;

        Debug.Log($"3oldMelodyPosition {oldMelodyPosition}");
        Debug.Log($"3melodyPosition {melodyPosition}");
        Debug.Log($"3positionStep {positionStep}");

        while (oldMelodyPosition < melodyPosition)
        {
            float xPosition = GetXPosition(oldMelodyPosition);
            //if (noteManager.KeyPosition(oldMelodyPosition))
            //{
            //    DrawZeroPoint(xPosition);
            //}
            //else
            //{
                Debug.Log($"3xPosition {xPosition}");
                float soundValue = microphonOperator.GetLoudness((int)oldMelodyPosition, positionStep);
                Debug.Log($"3soundValue {soundValue}");
                DrawVolumePoint(soundValue, xPosition);
            //}

            oldMelodyPosition += positionStep;
        }
    }

    private void DrawZeroPoint(float xPosition)
    {
        lineRenderer.positionCount++;
        Vector3 nextPosition = new Vector3(xPosition, -390f, 0f);

        lineRenderer.SetPosition(lineRenderer.positionCount - 1, nextPosition);
    }

    private float GetXPosition(float positionInSamples)
    {
        float result = positionInSamples / CoreValuesHUB.melodyLengthInSamples;

        result = Mathf.Lerp(startPosition, endPosition, result);

        return result;
    }

    private void DrawVolumePoint(float soundValue, float xPosition)
    {
        if (soundValue < decibelGate)
            soundValue = 0;

        soundValue = Mathf.Lerp(minValue, maxValue, soundValue);
        lineRenderer.positionCount++;
        Vector3 nextPosition = new Vector3(xPosition, soundValue, 0f);

        lineRenderer.SetPosition(lineRenderer.positionCount - 1, nextPosition);
    }

    private void MoveCursor()
    {
        float delta = (float)melodyPosition / (float)CoreValuesHUB.melodyLengthInSamples;

        if (delta >= 1)
        {
            delta = 0;
            ResetEvent();
        }

        float xPosition = Mathf.Lerp(startPosition, endPosition, delta);

        timeCursor.anchoredPosition = new Vector3(xPosition, 0f, 0f);
    }

    public void ResetEvent()
    {
        timeCursor.anchoredPosition = new Vector3(startPosition, 0f, 0f);
        lineRenderer.positionCount = 0;
        oldMelodyPosition = 0;
        noteManager.NewTact();
    }

    public void SetDecibelGate()
    {
        float decibelValue = Mathf.Lerp(minValue, maxValue, decibelGate);
        decibelCursor.anchoredPosition = new Vector3(0f, decibelValue, 0f);
    }
}
