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

    private MicrophonOperator microphonOperator => (MicrophonOperator)CoreHUB.MicrophonOperator.GetValue();
    private NoteManager noteManager => (NoteManager)CoreHUB.NoteManager.GetValue();
    private NoteBeltOperator noteBeltOperator => (NoteBeltOperator)CoreHUB.NoteBeltOperator.GetValue();

    private void Awake()
    {
        CoreHUB.DiagramOperator.SetValue(this);

        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }
    private void Update()
    {
        MoveCursor();
        DrawVolumeLine();
    }

    private void DrawVolumeLine()
    {
        float soundValue = microphonOperator.GetLoudness();

        if(soundValue < decibelGate)
            soundValue = 0;

        soundValue = Mathf.Lerp(minValue, maxValue, soundValue);
        lineRenderer.positionCount++;
        Vector3 nextPosition = new Vector3(timeCursor.anchoredPosition.x, soundValue, 0f);
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, nextPosition);
    }

    private void MoveCursor()
    {
        float delta = CoreValuesHUB.MelodyPosition.GetValue() / CoreValuesHUB.MelodyLength.GetValue();

        if (delta > 1)
        {
            delta = 0;
            NewTact();
        }

        float xPosition = Mathf.Lerp(startPosition, endPosition, delta);

        timeCursor.anchoredPosition = new Vector3(xPosition, 0f, 0f);
    }

    public void NewTact()
    {
        timeCursor.anchoredPosition = new Vector3(startPosition, 0f, 0f);
        lineRenderer.positionCount = 0;
        noteManager.NewTact();
    }

    public void SetDecibelGate()
    {
        float decibelValue = Mathf.Lerp(minValue, maxValue, decibelGate);
        decibelCursor.anchoredPosition = new Vector3(0f, decibelValue, 0f);
    }
}
