using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    struct SavedProperties
    {
        float Drag;
        Material Material;
        bool UseGravity;
        bool FreezeRotation;

        public SavedProperties(Rigidbody body)
        {
            Drag = body.drag;
            UseGravity = body.useGravity;
            Material = body.gameObject.GetComponentInChildren<MeshRenderer>()?.material;
            FreezeRotation = body.freezeRotation;
        }

        public void Restor(Rigidbody body)
        {
            body.drag = Drag;
            body.useGravity = UseGravity;
            body.freezeRotation = FreezeRotation;
            if (body.gameObject.GetComponentInChildren<MeshRenderer>() != null)
            {
                body.gameObject.GetComponentInChildren<MeshRenderer>().material = Material;
            }
        }
    }

    Rigidbody GrabbedItem;
    public float MaxDistance = 100.0f;
    public float DeadDistance = 1.0f;
    public Vector3 HandOffset;
    public AnimationCurve ForceCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 100.0f);

    public Material HoldMaterial;
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

    SavedProperties? savedProps = null;

    private void Drop()
    {
        savedProps.Value.Restor(GrabbedItem);
        GrabbedItem = null;
    }


    private void Grab(Rigidbody itemToGrab)
    {
        GrabbedItem = itemToGrab;
        savedProps = new Grabber.SavedProperties(itemToGrab);
        GrabbedItem.gameObject.GetComponentInChildren<MeshRenderer>().material = HoldMaterial;
        GrabbedItem.useGravity = false;
        GrabbedItem.freezeRotation = true;
        GrabbedItem.drag = 10.0f;
    }

    bool IsGrabbing()
    {
        return GrabbedItem != null;
    }
}
