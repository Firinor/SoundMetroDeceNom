using System;
using UnityEngine;
using UnityEngine.UI;

public class MicrophonOperator : MonoBehaviour
{
    private int sampleWindow = 64;
    private int oldClipPosition;
    private float oldResult;
    private AudioClip microphoneClip;
    [SerializeField]
    private Image image;

    private LogicCore logicCore => (LogicCore)CoreHUB.LogicCore.GetValue();

    private void Awake()
    {
        CoreHUB.MicrophonOperator.SetValue(this);
        CoreValuesHUB.SampleRate.SetValue(AudioSettings.outputSampleRate);
    }

    private void Start()
    {
        StartRecording(Microphone.devices[0]);
    }

    public void SetSmooth(int smooth)
    {
        sampleWindow = smooth;
    }

    public void StartRecording(string microphoneName)
    {
        microphoneClip = Microphone.Start(
            microphoneName,
            loop: true,
            lengthSec: logicCore.GetSoundLength(),
            AudioSettings.outputSampleRate);

        if (microphoneClip != null)
            image.gameObject.SetActive(false);
    }
    public int GetMicrophonePosition()
    {
        return Microphone.GetPosition(Microphone.devices[0]);
    }

    public float GetLoudness(int clipPosition, int sampleWindow)
    {
        int startPosition = clipPosition - sampleWindow;

        if (startPosition < 0)
            startPosition = 0;

        float[] data = new float[sampleWindow];
        microphoneClip.GetData(data, startPosition);

        oldResult = Mathf.Max(data);
        return oldResult;
    }

    public float GetLoudness()
    {
        int clipPosition = GetMicrophonePosition();

        if(oldClipPosition == clipPosition)
            return oldResult;

        oldClipPosition = clipPosition;

        int startPosition = clipPosition - sampleWindow;

        if(startPosition < 0)
            startPosition = 0;

        float[] data = new float[sampleWindow];
        microphoneClip.GetData(data, startPosition);
        
        oldResult = Mathf.Max(data);
        return oldResult;
    }
}
