using System;
using UnityEngine;

public class DiagramOperator : MonoBehaviour
{
    [SerializeField]
    private RectTransform timeCursor;
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private float lineWidth;
    [SerializeField]
    private MicrophonOperator microphon;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float startPosition;
    [SerializeField]
    private float endPosition;

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

    private void MoveCursor()
    {
        float delta = Time.deltaTime * speed;

        Vector3 oldPosition = timeCursor.anchoredPosition;

        if (oldPosition.x > endPosition)
            ResetEvent();

        timeCursor.anchoredPosition = new Vector3(timeCursor.anchoredPosition.x + delta, 0f, 0f);
    }

    public void ResetEvent()
    {
        timeCursor.anchoredPosition = new Vector3(startPosition, 0f, 0f);
        lineRenderer.positionCount = 0;
    }
}
