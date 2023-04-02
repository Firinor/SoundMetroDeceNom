using UnityEngine;

public class Debuger : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip clip1;

    [SerializeField]
    private AudioClip clip2;

    [SerializeField]
    private double timer;
    [SerializeField]
    private double BPS;
    [SerializeField]
    private double BPM;
    private double oldAudioTime = 0;
    private double currentTimer;
    public static double totalTimer;
    public static double totalTimer2;

    private int noteIndex;
    public static int noteCount;


    //[SerializeField]
    //private TextMeshProUGUI text;
    //private float min = float.MaxValue;
    //private float max = float.MinValue;

    //[SerializeField]
    //private float resetTimer = 5f;
    //private float timer = 0f;

    //void Awake()
    //{
    //    oldAudioTime = AudioSettings.dspTime;
    //    Application.targetFrameRate = 0;
    //}
    //public void ResetEvent()
    //{
    //    timer = 0f;
    //    min = float.MaxValue; max = float.MinValue;
    //}

    //public void SetSpeed(Slider speed)
    //{
    //    timer = speed.value;
    //    BPS = 1 / timer;
    //    BPM = BPS * 60;
    //}

    void Update()
    {
        //double delta = AudioSettings.dspTime - oldAudioTime;

        //if (delta > 0)
        //{
        //    totalTimer += delta;
        //    currentTimer += delta;
        //    oldAudioTime = AudioSettings.dspTime;
        //}

        //if (currentTimer > timer)
        //{
        //    noteCount++;
        //    Debug.Log("Time left : " + totalTimer + "Notes : " + noteCount);
        //    currentTimer -= timer;

        //    if (noteIndex == 0)
        //    {
        //        audioSource.clip = clip1;

        //        audioSource.Play();
        //    }
        //    else
        //    {
        //        audioSource.clip = clip2;
        //        audioSource.Play();
        //    }
        //    noteIndex++;
        //    if(noteIndex == 8)
        //    {
        //        noteIndex = 0;
        //    }
        //}


            //timer += Time.deltaTime;

            //if (timer >= resetTimer)
            //{
            //    ResetEvent();
            //}

            //if (min > Time.deltaTime)
            //{
            //    min = Time.deltaTime;
            //}

            //if(max < Time.deltaTime)
            //{
            //    max = Time.deltaTime;
            //}

            //text.text = $"FPS: {1/ min} / {1 / max};" + Environment.NewLine +
            //    $" min {min}; max {max}!;";
    }
}
