using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Rigidbody selfBody;
    Vector3 currentVelocity;
    public float MovementSpeed = 10.0f;
    public float MaxMoveSpeed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        selfBody = GetComponent<Rigidbody>();
        currentVelocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 acceleration = Vector3.zero;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            acceleration.x -= MovementSpeed;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            acceleration.x += MovementSpeed;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            acceleration.z += MovementSpeed;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            acceleration.z -= MovementSpeed;
        }

        selfBody.AddForce(acceleration, ForceMode.Acceleration);
    }
}
