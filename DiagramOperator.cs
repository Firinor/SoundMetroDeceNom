using UnityEngine;

public class DiagramOperator : MonoBehaviour
{
    [SerializeField]
    private NoteManager noteManager;

    [SerializeField]
    private RectTransform timeCursor;
    [SerializeField]
    private RectTransform decibelCursor;
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
    private float tactLenght;
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
    private float decibelGate = 0.1f;

    private void Awake()
    {
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        tactLenght = endPosition - startPosition;
        tactDelta = tactLenght / notesInTact;
    }
    private void Update()
    {
        MoveCursor();
        DrawVolumeLine();
    }

    private void DrawVolumeLine()
    {
        float soundValue = microphon.GetLoudness();

        if(soundValue < decibelGate)
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

        if (timeCursor.anchoredPosition.x >= endPosition)
            MoveToStartEvent();

        timeCursor.anchoredPosition = new Vector3(timeCursor.anchoredPosition.x + delta, yCursorPosition, 0f);
        float cursorPosition = (timeCursor.anchoredPosition.x - startPosition) / tactLenght;// 0 .. 1
        noteManager.MelodyCheck(cursorPosition);
    }

    public void MoveToStartEvent()
    {
        timeCursor.anchoredPosition = new Vector3(timeCursor.anchoredPosition.x - tactLenght, yCursorPosition, 0f);
        ResetToZero();
    }
    public void ResetEvent()
    {
        timeCursor.anchoredPosition = new Vector3(startPosition, yCursorPosition, 0f);
        ResetToZero();
    }

    private void ResetToZero()
    {
        lineRenderer.positionCount = 0;
        noteManager.NewTact();
    }

    public void SetDecibelGate(float newDecibelGate)
    {
        decibelGate = newDecibelGate / 100f;//%

        float decibelValue = Mathf.Lerp(minValue, maxValue, decibelGate);
        decibelCursor.anchoredPosition = new Vector3(0f, decibelValue, 0f);
    }
}
