using FirInstanceCell;

public static class CoreValuesHUB
{
    public static InstanceCell<PlayMode> PlayMode = new InstanceCell<PlayMode>();
    public static InstanceCell<Difficulty> Difficulty = new InstanceCell<Difficulty>();

    public static InstanceCell<int> SampleRate = new InstanceCell<int>();

    public static InstanceCell<float> MelodyPosition = new InstanceCell<float>();
    public static InstanceCell<float> MelodyLength = new InstanceCell<float>();

    public static InstanceCell<int> BeatsPerMinute = new InstanceCell<int>();
    public static InstanceCell<float> DecibelGate = new InstanceCell<float>();
    public static InstanceCell<int> Smooth = new InstanceCell<int>();
    public static InstanceCell<int> Reaction = new InstanceCell<int>();
}