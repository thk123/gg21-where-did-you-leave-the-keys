using UnityEngine;

using UnityEngine.EventSystems;


public class AudioSources : MonoBehaviour {
    public AudioClip GenericSoundSource;
    public AudioClip CardboardSoundSource;
    public AudioClip GlassSoundSource;
    public AudioClip CeramicSoundSource;
    public AudioClip TinSoundSource;
    public AudioClip CutlerySoundSource;

    public static AudioClip GenericSound;
    public static AudioClip CardboardSound;
    public static AudioClip GlassSound;
    public static AudioClip CeramicSound;
    public static AudioClip TinSound;
    public static AudioClip CutlerySound;

    void Awake() {
        GenericSound = GenericSoundSource;
        CardboardSound = CardboardSoundSource;
        GlassSound = GlassSoundSource;
        CeramicSound = CeramicSoundSource;
        TinSound = TinSoundSource;
        TinSound = CutlerySoundSource;

    }

}