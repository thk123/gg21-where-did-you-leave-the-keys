using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHinger : MonoBehaviour
{

    enum State
    {
        Closed,
        Opening,
        Open, 
        Closing
    }
    // Start is called before the first frame update

    public float OpenAngle = 90.0f;
    public AnimationCurve EasingCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
    public float OpeningTime = 1.0f;
    float t;


    State DoorState;
    void Start()
    {
        t = 0.0f;
        DoorState = State.Closed;
    }

    public IEnumerator DoDoorSwing(float pauseTime)
    {
        ToggleDoor();
        yield return new WaitUntil(() => DoorState == State.Open);
        yield return new WaitForSeconds(pauseTime);
        ToggleDoor();
        yield return new WaitUntil(() => DoorState == State.Closed);
    }

    public void ToggleDoor()
    {
        if(DoorState == State.Closed)
        {
            DoorState = State.Opening;
            t = 0.0f;
        }
        else if(DoorState == State.Open)
        {
            t = 1.0f;
            DoorState = State.Closing;
        }
        else
        {
            Debug.LogWarning("tried to open/close door whilst it was already opening or closing");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(DoorState == State.Opening)
        {
            t += Time.deltaTime;
            if(t >= 1.0f)
            {
                t = 1.0f;
                DoorState = State.Open;
            }
        }
        else if(DoorState == State.Closing)
        {
            t -= Time.deltaTime;
            if(t <= 0.0f)
            {
                t = 0.0f;
                DoorState = State.Closed;
            }
        }
        float TargetAngle = EasingCurve.Evaluate(t) * OpenAngle;
        transform.eulerAngles = new Vector3(0.0f, TargetAngle, 0.0f);
    }

    bool IsMoving()
    {
        return DoorState == State.Opening || DoorState == State.Closing;
    }
}
