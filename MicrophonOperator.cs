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
    [SerializeField]
    private Color failColor;

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

        if (microphoneClip == null)
            image.color = failColor;
        else
            image.gameObject.SetActive(false);
    }

    public float GetLoudness()
    {
        int clipPosition = Microphone.GetPosition(Microphone.devices[0]);

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
