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
    void Start()
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = AudioSources.Instance.GetAudioClip(AudioType);
        
    }

    void Update() {
        if (!AudioLoaded) {
            WaitCount += Time.deltaTime;
            if (WaitCount>=2f) {
                AudioLoaded = true;
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
