using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    Rigidbody CurrentlyGrabbedItem;
    public float MaxDistance = 100.0f;
    public Vector3 HandOffset;
    // Start is called before the first frame update
    void Start()
    {
        CurrentlyGrabbedItem = null;
    }

    Ray lastRay = new Ray();
    

    private void OnDrawGizmos()
    {
        if(lastRay.direction != Vector3.zero)
        {
            Gizmos.DrawLine(lastRay.origin, lastRay.origin + (lastRay.direction * MaxDistance));
            lastRay = new Ray();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(IsGrabbing() && !Input.GetMouseButton(0))
        {
            Drop();
        }
        if(Input.GetMouseButtonDown(0))
        {
            if(!IsGrabbing())
            {
                RaycastHit info;
                lastRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(lastRay, out info, MaxDistance))
                {
                    
                    if(info.rigidbody != null)
                    {
                        Grab(info.rigidbody);
                    }
                }
            }

        }
        
    }

    private void Drop()
    {
        CurrentlyGrabbedItem.transform.parent = null;
        CurrentlyGrabbedItem.isKinematic = false;
        CurrentlyGrabbedItem = null;
    }

    private void Grab(Rigidbody itemToGrab)
    {
        itemToGrab.isKinematic = true;
        itemToGrab.MovePosition(transform.position + HandOffset);
        itemToGrab.transform.parent = transform;
        itemToGrab.transform.SetParent(transform, true);
        CurrentlyGrabbedItem = itemToGrab;
    }

    bool IsGrabbing()
    {
        return CurrentlyGrabbedItem != null;
    }
}
