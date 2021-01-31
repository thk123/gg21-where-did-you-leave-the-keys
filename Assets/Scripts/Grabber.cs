using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    struct SavedProperties
    {
        float Drag;
        Dictionary<Renderer, Material[]> SavedMaterial;
        bool UseGravity;
        bool FreezeRotation;

        public SavedProperties(Rigidbody body)
        {
            Drag = body.drag;
            UseGravity = body.useGravity;
            SavedMaterial = body.GetComponentsInChildren<Renderer>().ToDictionary(r => r, r => r.materials);
            FreezeRotation = body.freezeRotation;
        }

        public void Restor(Rigidbody body)
        {
            body.drag = Drag;
            body.useGravity = UseGravity;
            body.freezeRotation = FreezeRotation;
            var materials = SavedMaterial;
            ApplyToAllRenderers(body, (renderer) =>
            {
                
                renderer.materials = materials[renderer];
            });
        }
    }

    Rigidbody GrabbedItem;
    public float MaxDistance = 100.0f;
    public float DeadDistance = 1.0f;
    public Vector3 HandOffset;
    public AnimationCurve ForceCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 100.0f);
    public float GrabForceMuliplier = 1.0f;

    public LayerMask NoGrabLayer;

    public Material HoldMaterial;
    public Material LookAtMaterial;
    // Start is called before the first frame update
    void Start()
    {
        LookAtMaterial = LookAtMaterial ?? HoldMaterial;
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
                float forceStrengt = ForceCurve.Evaluate(t) * GrabForceMuliplier;
                GrabbedItem.AddForce(pullDirection.normalized * forceStrengt);
            }
        }




        if (!IsGrabbing())
        {
            RaycastHit info;
            var midpoint = new Vector3(Screen.width / 2, Screen.height / 2, 0.0f);
            lastRay = Camera.main.ScreenPointToRay(midpoint);
            if (Physics.Raycast(lastRay, out info, MaxDistance, ~NoGrabLayer))
            {
                if (info.rigidbody != null && info.collider.gameObject.tag != "NoGrab")
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        StopLookingAt();
                        Grab(info.rigidbody);
                    }
                    else if (CurrentlyLookingAt != info.rigidbody)
                    {

                        LookAt(info.rigidbody);
                    }
                }
                else
                {
                    StopLookingAt();
                }
            }
            else
            {
                StopLookingAt();
            }
        }     
    }

    Rigidbody CurrentlyLookingAt = null;

    private void LookAt(Rigidbody lookingAt)
    {
        StopLookingAt();
        ApplyToAllRenderers(lookingAt, (renderer) =>
        {
            renderer.materials = renderer.materials.Concat(new Material[] { LookAtMaterial }).ToArray();
        });
        CurrentlyLookingAt = lookingAt;
    }

    private void StopLookingAt()
    {
        if (CurrentlyLookingAt != null)
        {
            ApplyToAllRenderers(CurrentlyLookingAt, (renderer) =>
            {
                renderer.materials = renderer.materials.Take(renderer.materials.Length - 1).ToArray();
            });
            CurrentlyLookingAt = null;
        }
        
    }

    private static void ApplyToAllRenderers(Rigidbody rigidBody, Action<Renderer> action)
    {
        var renderers = rigidBody.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            action(renderer);
            
        }
    }

    SavedProperties? savedProps = null;

    private void Drop()
    {
        savedProps.Value.Restor(GrabbedItem);
        
        GrabbedItem.GetComponent<Key>()?.SetProximityMute(false);
        GrabbedItem = null;
    }


    private void Grab(Rigidbody itemToGrab)
    {
        GrabbedItem = itemToGrab;
        savedProps = new Grabber.SavedProperties(itemToGrab);
        ApplyToAllRenderers(GrabbedItem, (renderer) =>
        {
            renderer.material = HoldMaterial;
        });
        GrabbedItem.useGravity = false;
        GrabbedItem.freezeRotation = true;
        GrabbedItem.drag = 10.0f;

        itemToGrab.GetComponent<PickupableSFX>()?.PickUp();
        itemToGrab.GetComponent<Key>()?.SetProximityMute(true);
    }

    bool IsGrabbing()
    {
        return GrabbedItem != null;
    }
}
