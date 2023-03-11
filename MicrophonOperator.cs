using UnityEngine;

public class MicrophonOperator : MonoBehaviour
{
    private int sampleWindow = 64;
    private int oldClipPosition;
    private float oldResult;
    private AudioClip microphoneClip;

    void Start()
    {
        StartRecording();
    }

    public void SetSmooth(int smooth)
    {
        sampleWindow = smooth;
    }

    private void StartRecording()
    {
        string microphoneName = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneName, loop: true, lengthSec: 61, AudioSettings.outputSampleRate);
    }

    public float GetLoudness()
    {
        int clipPosition = Microphone.GetPosition(Microphone.devices[0]);

        Debug.Log(clipPosition);

        if(oldClipPosition == clipPosition)
            return oldResult;

        int startPosition = clipPosition - sampleWindow;

        if(startPosition < 0)
            return 0;

        float[] data = new float[sampleWindow];
        microphoneClip.GetData(data, startPosition);

        oldResult = Mathf.Max(data);
        return oldResult;
    }
}
