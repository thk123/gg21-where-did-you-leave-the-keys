using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CentralDoor : MonoBehaviour
{
    public DoorHinger Hinge;

    public bool IsUnlocked

    {
        get;
        private set;
    }

    // Start is called before the first frame update
    void Start()
    {
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

    public void Unlock()
    {
        IsUnlocked = true;
        StartCoroutine(Hinge.DoDoorSwing(0.5f));
    }
}
