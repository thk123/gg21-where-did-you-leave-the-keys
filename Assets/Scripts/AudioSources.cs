using UnityEngine;

using UnityEngine.EventSystems;


public class AudioSources : MonoBehaviour {
    public AudioClip GenericSoundSource;
    public AudioClip CardboardSoundSource;
    public AudioClip GlassSoundSource;
    public AudioClip CeramicSoundSource;
    public AudioClip TinSoundSource;
    public AudioClip CutlerySoundSource;

   /* public static AudioClip GenericSound;
    public static AudioClip CardboardSound;
    public static AudioClip GlassSound;
    public static AudioClip CeramicSound;
    public static AudioClip TinSound;
    public static AudioClip CutlerySound;*/

    public static AudioSources Instance { get; private set; }

    void Awake() {
        Instance = this;

  /*      GenericSound = GenericSoundSource;
        CardboardSound = CardboardSoundSource;
        GlassSound = GlassSoundSource;
        CeramicSound = CeramicSoundSource;
        TinSound = TinSoundSource;
        CutlerySound = CutlerySoundSource;*/

    }

    public AudioClip GetAudioClip(AudioTypes audioType)
    {
       
        AudioClip MyClip;

        switch(audioType) {
            case AudioTypes.cardboard:
                MyClip = CardboardSoundSource;
                break;
            case AudioTypes.glass:
                MyClip = GlassSoundSource;
                break;
            case AudioTypes.tin:
                MyClip = TinSoundSource;
                break;
            case AudioTypes.ceramic:
                MyClip = CeramicSoundSource;
                break;
            case AudioTypes.cutlery:
                MyClip = CutlerySoundSource;
                break;
            default:
                MyClip = GenericSoundSource;
                break;
        }

        return MyClip;
    }

}