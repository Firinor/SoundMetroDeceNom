using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MicrophonOperator : MonoBehaviour
{
    private AudioClip microphoneClip;
    [SerializeField]
    private Image image;

    private int currentSampleCursor = 0;
    private int recordingLength = 0;

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
        int lengthSec = 10;

        microphoneClip = Microphone.Start(
            microphoneName,
            loop: true,
            lengthSec,// melody have 4 notes. And min bpm is 1/min = 240sec lenght + 1 second of safe
            AudioSettings.outputSampleRate);

        if (microphoneClip != null)
            image.gameObject.SetActive(false);

        recordingLength = lengthSec * AudioSettings.outputSampleRate;
    }
    public int GetMicrophonePositionInSamples()
    {
        return Microphone.GetPosition(Microphone.devices[0]);
    }
    public int GetMicrophoneDeltaInSamples()
    {
        int position = GetMicrophonePositionInSamples();

        int result = position - currentSampleCursor;

        if (result < 0)
            result += recordingLength;

        currentSampleCursor = position;
        
        return result;
    }

    public float GetLoudness(int minusPosition, int sampleWindow)
    {
        if(minusPosition >= recordingLength)
            return 0f;

        int startPosition = currentSampleCursor + minusPosition - sampleWindow;

        if (startPosition >= recordingLength)
            return 0f;

        float[] redata = null;
        float[] data = null;

        if (startPosition < 0)
        {
            if (startPosition + recordingLength < 0)
                return 0f;

            sampleWindow += startPosition;

            redata = new float[-startPosition];
            microphoneClip.GetData(redata, startPosition + recordingLength);

            startPosition = 0;
        }

        if (sampleWindow > 0)
        {
            data = new float[sampleWindow];
            microphoneClip.GetData(data, startPosition);
        }

        if (redata != null)
        {
            if (data != null)
            {
                data = redata.Concat(data).ToArray();
            }
            else
            {
                data = redata;
            }
        }

        for (int i = 0; i < data.Length; i++)
        {
            data[i] = Mathf.Abs(data[i]);
        }
        return Mathf.Max(data);
    }
}
