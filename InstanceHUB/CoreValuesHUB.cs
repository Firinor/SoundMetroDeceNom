using FirInstanceCell;

public static class CoreValuesHUB
{
    public static PlayMode playMode => PlayMode.GetValue();
    public static Difficulty difficulty => Difficulty.GetValue();

    public static int sampleRate => SampleRate.GetValue();

    public static float melodyPositionInRate => MelodyPositionInRate.GetValue();
    public static float melodyPositionInSeconds => MelodyPositionInSeconds.GetValue();
    public static int melodyPositionInSamples => MelodyPositionInSamples.GetValue();
    public static float melodyLengthInSec => MelodyLengthInSec.GetValue();
    public static int melodyLengthInSamples => MelodyLengthInSamples.GetValue();

    public static float melodyStartPosition => MelodyStartPosition.GetValue();
    public static float melodyEndPosition => MelodyEndPosition.GetValue();

    public static int beatsPerMinute => BeatsPerMinute.GetValue();
    public static float decibelGate => DecibelGate.GetValue();
    public static int smooth => Smooth.GetValue();
    public static int reaction => Reaction.GetValue();

    public static InstanceCell<PlayMode> PlayMode = new InstanceCell<PlayMode>();
    public static InstanceCell<Difficulty> Difficulty = new InstanceCell<Difficulty>();

    public static InstanceCell<int> SampleRate = new InstanceCell<int>();

    public static InstanceCell<float> MelodyPositionInRate = new InstanceCell<float>();
    public static InstanceCell<float> MelodyPositionInSeconds = new InstanceCell<float>();
    public static InstanceCell<int> MelodyPositionInSamples = new InstanceCell<int>();

    public static InstanceCell<float> MelodyLengthInSec = new InstanceCell<float>();
    public static InstanceCell<int> MelodyLengthInSamples = new InstanceCell<int>();

    public static InstanceCell<float> MelodyStartPosition = new InstanceCell<float>();
    public static InstanceCell<float> MelodyEndPosition = new InstanceCell<float>();

    public static InstanceCell<int> BeatsPerMinute = new InstanceCell<int>();
    public static InstanceCell<float> DecibelGate = new InstanceCell<float>();
    public static InstanceCell<int> Smooth = new InstanceCell<int>();
    public static InstanceCell<int> Reaction = new InstanceCell<int>();
}