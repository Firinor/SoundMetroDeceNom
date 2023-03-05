using UnityEngine;

public class MicrophonOperator : MonoBehaviour
{
    [SerializeField]
    private int sampleWindow;
    private AudioClip microphoneClip;

    void Start()
    {
        StartRecording();
    }

    private void StartRecording()
    {
        string microphoneName = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneName, loop: true, 20, AudioSettings.outputSampleRate);
    }

    public float GetLoudness()
    {
        int clipPosition = Microphone.GetPosition(Microphone.devices[0]);

        int startPosition = clipPosition - sampleWindow;
        if(startPosition < 0)
            return 0;

        float[] data = new float[sampleWindow];
        microphoneClip.GetData(data, startPosition);

        float totalLoudness = 0;

        for(int i = 0; i < data.Length; i++)
        {
            totalLoudness += Mathf.Abs(data[i]);
        }

        return totalLoudness / sampleWindow;
    }
}
