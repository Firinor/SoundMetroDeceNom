using UnityEngine;
using UnityEngine.UI;

public class MicrophonOperator : MonoBehaviour
{
    private int sampleWindow = 64;
    private int oldClipPosition;
    private float oldResult;
    private AudioClip microphoneClip;
    [SerializeField]
    private LogicCore core;
    [SerializeField]
    private Image image;

    void Start()
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
            lengthSec: core.GetSoundLength(),
            AudioSettings.outputSampleRate);

        if (microphoneClip != null)
            image.gameObject.SetActive(false);
    }
    public int GetPosition()
    {
        return Microphone.GetPosition(Microphone.devices[0]);
    }

    public float GetLoudness()
    {
        int clipPosition = GetPosition();

        if(oldClipPosition == clipPosition)
            return oldResult;

        oldClipPosition = clipPosition;

        int startPosition = clipPosition - sampleWindow;

        if(startPosition < 0)
            return 0;

        float[] data = new float[sampleWindow];
        microphoneClip.GetData(data, startPosition);
        
        oldResult = Mathf.Max(data);
        return oldResult;
    }
}
