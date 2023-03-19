using UnityEngine;
using UnityEngine.UI;

public class MicrophonOperator : MonoBehaviour
{
    private AudioClip microphoneClip;
    [SerializeField]
    private Image image;

    private LogicCore logicCore => (LogicCore)CoreHUB.logicCore;

    private void Awake()
    {
        CoreHUB.MicrophonOperator.SetValue(this);
        CoreValuesHUB.SampleRate.SetValue(AudioSettings.outputSampleRate);
    }

    private void Start()
    {
        StartRecording(Microphone.devices[0]);
    }

    public void StartRecording(string microphoneName)
    {
        microphoneClip = Microphone.Start(
            microphoneName,
            loop: true,
            lengthSec: 241,// melody have 4 notes. And min bpm is 1/min = 240sec lenght + 1 second of safe
            AudioSettings.outputSampleRate);

        if (microphoneClip != null)
            image.gameObject.SetActive(false);
    }
    public int GetMicrophonePositionInSamples()
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
        for(int i = 0; i < data.Length; i++)
        {
            data[i] = Mathf.Abs(data[i]);
        }
        return Mathf.Max(data);
    }
}
