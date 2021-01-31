using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public AudioSource ProximitySource;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(ProximitySource != null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetProximityMute(bool muted)
    {
        ProximitySource.mute = muted;
    }
}
