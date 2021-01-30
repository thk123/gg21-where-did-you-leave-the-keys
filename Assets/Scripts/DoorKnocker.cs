using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DoorKnocker : MonoBehaviour
{
    AudioSource KnockSource;
    public List<AudioClip> Knocks = new List<AudioClip>(3);
    // Start is called before the first frame update
    void Start()
    {
        KnockSource = GetComponent<AudioSource>();
        Debug.Assert(Knocks.Count == 3, "Need three knocks");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Knock(int knockLevel)
    {
        knockLevel = Mathf.Clamp(knockLevel, 0, Knocks.Count);
        KnockSource.clip = Knocks[knockLevel];
        KnockSource.Play();
    }
}
