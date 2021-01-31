using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioTypes {
    generic, 
    glass, 
    cardboard,
    ceramic,
    tin,
    cutlery
    }

public class ColliderSound : MonoBehaviour
{

    public AudioTypes AudioType = AudioTypes.generic;
    private AudioClip MyClip;
    
    private float WaitCount = 0f;
    bool AudioLoaded = false;
    // Start is called before the first frame update
    void LoadMyAudio()
    {

        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        MyClip = AudioSources.GenericSound;

        switch(AudioType) {
            case AudioTypes.cardboard:
                MyClip = AudioSources.CardboardSound;
                break;
            case AudioTypes.glass:
                MyClip = AudioSources.GlassSound;
                break;
            case AudioTypes.tin:
                MyClip = AudioSources.TinSound;
                break;
            case AudioTypes.ceramic:
                MyClip = AudioSources.CeramicSound;
                break;
            case AudioTypes.cutlery:
                MyClip = AudioSources.CutlerySound;
                break;
        }

        audioSource.clip = MyClip;
        AudioLoaded = true;
        
    }

    void Update() {
        if (!AudioLoaded) {
            WaitCount += Time.deltaTime;
            if (WaitCount>=2f) {
                LoadMyAudio();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 1.5&& AudioLoaded) {
            GetComponent<AudioSource>().Play();
        }
    }
        


        
    
}
