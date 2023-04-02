using FirInstanceCell;

public static class CoreValuesHUB
{
    public static PlayMode playMode => PlayMode.GetValue();
    public static Difficulty difficulty => Difficulty.GetValue();

    public static int sampleRate => SampleRate.GetValue();

    public static double melodyPositionInRate => MelodyPositionInRate.GetValue();
    public static double melodyPositionInBeats => MelodyPositionInBeats.GetValue();

    public static double melodyLengthInSeconds => 4 / beatsPerSecond;//4 notes per tact
    public static float melodyLengthInUnits => MelodyLengthInUnits.GetValue();

    public static int beatsPerMinute => BeatsPerMinute.GetValue();
    public static double beatsPerSecond => beatsPerMinute / 60d;
    public static double secondsPerBeat => 60d / beatsPerMinute;
    public static float decibelGate => DecibelGate.GetValue();
    public static int reaction => Reaction.GetValue();
    public static float soundShift => SoundShift.GetValue();

    public static InstanceCell<PlayMode> PlayMode = new InstanceCell<PlayMode>();
    public static InstanceCell<Difficulty> Difficulty = new InstanceCell<Difficulty>();

    public static InstanceCell<int> SampleRate = new InstanceCell<int>();

    public static InstanceCell<double> MelodyPositionInRate = new InstanceCell<double>();
    public static InstanceCell<double> MelodyPositionInBeats = new InstanceCell<double>();
    public static InstanceCell<float> MelodyLengthInUnits = new InstanceCell<float>();

    public static InstanceCell<int> BeatsPerMinute = new InstanceCell<int>();
    public static InstanceCell<float> DecibelGate = new InstanceCell<float>();
    public static InstanceCell<int> Reaction = new InstanceCell<int>();
    public static InstanceCell<float> SoundShift = new InstanceCell<float>();
}