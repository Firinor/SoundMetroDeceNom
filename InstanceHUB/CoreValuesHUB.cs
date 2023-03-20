using FirInstanceCell;

public static class CoreValuesHUB
{
    public static PlayMode playMode => PlayMode.GetValue();
    public static Difficulty difficulty => Difficulty.GetValue();

    public static int sampleRate => SampleRate.GetValue();

    public static float melodyPositionInRate => MelodyPositionInRate.GetValue();

    public static float melodyLengthInSeconds => 4 / beatsPerSecond;//4 notes per tact
    public static float melodyLengthInUnits => MelodyLengthInUnits.GetValue();

    public static int beatsPerMinute => BeatsPerMinute.GetValue();
    public static float beatsPerSecond => beatsPerMinute / 60f;
    public static float decibelGate => DecibelGate.GetValue();
    public static int reaction => Reaction.GetValue();

    public static InstanceCell<PlayMode> PlayMode = new InstanceCell<PlayMode>();
    public static InstanceCell<Difficulty> Difficulty = new InstanceCell<Difficulty>();

    public static InstanceCell<int> SampleRate = new InstanceCell<int>();

    public static InstanceCell<float> MelodyPositionInRate = new InstanceCell<float>();

    public static InstanceCell<float> MelodyLengthInUnits = new InstanceCell<float>();

    public static InstanceCell<int> BeatsPerMinute = new InstanceCell<int>();
    public static InstanceCell<float> DecibelGate = new InstanceCell<float>();
    public static InstanceCell<int> Reaction = new InstanceCell<int>();
}