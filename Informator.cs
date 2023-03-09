using UnityEngine;

public class Informator : MonoBehaviour
{
    private static Informator instance;

    [SerializeField]
    private Sprite blackLeftNote;
    public static Sprite BlackLeftNote => instance.blackLeftNote;
    [SerializeField]
    private Sprite blackRightNote;
    public static Sprite BlackRightNote => instance.blackRightNote;
    [SerializeField]
    private Sprite redLeftNote;
    public static Sprite RedLeftNote => instance.redLeftNote;
    [SerializeField]
    private Sprite redRightNote;
    public static Sprite RedRightNote => instance.redRightNote;
    [SerializeField]
    private Sprite greenLeftNote;
    public static Sprite GreenLeftNote => instance.greenLeftNote;
    [SerializeField]
    private Sprite greenRightNote;
    public static Sprite GreenRightNote => instance.greenRightNote;
    [SerializeField]
    private Sprite goodResult;
    public static Sprite GoodResult => instance.goodResult;
    [SerializeField]
    private Sprite haryResult;
    public static Sprite HaryResult => instance.haryResult;
    [SerializeField]
    private Sprite slowResult;
    public static Sprite SlowResult => instance.slowResult;

    [SerializeField]
    private AudioClip startOfTactNote;
    public static AudioClip StartOfTactNote => instance.startOfTactNote;
    [SerializeField]
    private AudioClip defaulteNote;
    public static AudioClip DefaulteNote => instance.defaulteNote;

    public void Awake()
    {
        instance = this;
    }
}
