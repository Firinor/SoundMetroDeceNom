using UnityEngine;

public class CoreValuesHUBInformator : MonoBehaviour
{
    public PlayMode PlayMode;
    public Difficulty Difficulty;
    public int BeatsPerMinute;
    public int DecibelGate;
    public int Reaction;
    public int SoundShift;

    private void Awake()
    {
        CoreValuesHUB.PlayMode.SetValue(PlayMode);
        CoreValuesHUB.Difficulty.SetValue(Difficulty);
        CoreValuesHUB.BeatsPerMinute.SetValue(BeatsPerMinute);
        CoreValuesHUB.DecibelGate.SetValue(DecibelGate/100f);//%
        CoreValuesHUB.Reaction.SetValue(Reaction);
        CoreValuesHUB.SoundShift.SetValue(SoundShift/100f);//%

        Destroy(this);
    }
}