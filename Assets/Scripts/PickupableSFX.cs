using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableSFX : MonoBehaviour
{
    public AudioSource PickUpSFX;
    public AudioSource CollisionSFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (CollisionSFX != null)
        {
            CollisionSFX.Play();
        }
    }

    public void PickUp()
    {
        if(PickUpSFX != null)
        {
            PickUpSFX.Play();
        }
    }
}
