using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class UnlockableDoor : MonoBehaviour
{
    AudioSource UnlockEffect;
    public DoorHinger Hinge;

    public bool IsUnlocked

    {
        get;
        private set;
    }

    // Start is called before the first frame update
    void Start()
    {
        UnlockEffect = GetComponent<AudioSource>();
        IsUnlocked = false;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Lock()
    {
        IsUnlocked = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Key>() != null)
        {
            Unlock();
            GameObject.Destroy(other);
        }
    }

    private void Unlock()
    {
        IsUnlocked = true;
        UnlockEffect.Play();
        StartCoroutine(Hinge.DoDoorSwing(0.5f));
    }
}
