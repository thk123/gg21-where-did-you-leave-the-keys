using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    AudioSource musicPlayer;
    public List<AudioClip> MusicPieces;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(MusicPieces.Count > 0, "Must be at least one piece of music");
        musicPlayer = GetComponent<AudioSource>();
        musicPlayer.clip = MusicPieces[0];
        musicPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMusicIntensity(int level)
    {
        level = Mathf.Clamp(level, 0, MusicPieces.Count - 1);
        float pos = musicPlayer.time;
        musicPlayer.clip = MusicPieces[level];
        musicPlayer.time = pos;
        musicPlayer.Play();
    }
}
