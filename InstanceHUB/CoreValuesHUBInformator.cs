using UnityEngine;

public class CoreValuesHUBInformator : MonoBehaviour
{
    public PlayMode PlayMode;
    public Difficulty Difficulty;
    public int BeatsPerMinute;
    public int DecibelGate;
    public int Smooth;
    public int Reaction;

    private void Awake()
    {
        CoreValuesHUB.PlayMode.SetValue(PlayMode);
        CoreValuesHUB.Difficulty.SetValue(Difficulty);
        CoreValuesHUB.BeatsPerMinute.SetValue(BeatsPerMinute);
        CoreValuesHUB.DecibelGate.SetValue(DecibelGate/100f);//%
        CoreValuesHUB.Smooth.SetValue(Smooth);
        CoreValuesHUB.Reaction.SetValue(Reaction);

        Destroy(this);
    }
}