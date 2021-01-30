using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    Rigidbody GrabbedItem;
    public float MaxDistance = 100.0f;
    public float DeadDistance = 1.0f;
    public Vector3 HandOffset;
    public AnimationCurve ForceCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 100.0f);
    // Start is called before the first frame update
    void Start()
    {
    }

    Ray lastRay = new Ray();

    Vector3 HandPosition
    {
        get => transform.position + HandOffset;
    }
    

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
        if(IsGrabbing())
        {
            var pullDirection = HandPosition - GrabbedItem.transform.position;
            var dist2 = pullDirection.sqrMagnitude;
            if(dist2 > DeadDistance * DeadDistance)
            {
                float range = MaxDistance - DeadDistance;
                // 1 = max distance,  0 = dead distance
                float t = Mathf.Lerp(0.0f, 1.0f, (dist2 - DeadDistance) / range);
                float forceStrengt = ForceCurve.Evaluate(t);
                GrabbedItem.AddForce(pullDirection.normalized * forceStrengt);
            }
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

    float oldDrag;

    private void Drop()
    {
        GrabbedItem.drag = oldDrag;
        GrabbedItem.freezeRotation = false;
        GrabbedItem.useGravity = true;
        GrabbedItem = null;
    }


    private void Grab(Rigidbody itemToGrab)
    {
        GrabbedItem = itemToGrab;
        GrabbedItem.useGravity = false;
        GrabbedItem.freezeRotation = true;
        oldDrag = GrabbedItem.drag;
        GrabbedItem.drag = 10.0f;
    }

    bool IsGrabbing()
    {
        return GrabbedItem != null;
    }
}
