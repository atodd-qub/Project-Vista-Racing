using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCar : MonoBehaviour
{
    // Get input
    // Use input to move sphere
    // Set Cars position to sphere

    public KeyCode brakeKey;
    public KeyCode accelerateKey;
    public KeyCode driftKey;
    private bool isAccelerating;
    private bool isBraking;
    private bool isDrifting;
    public float accelerateForce;
    public float brakeForce;

    private float turnInput;
    public float turnSpeed;
    
    public Rigidbody sphereRB;

    // Start is called before the first frame update
    void Start()
    {
        // deatch sphere rigidbody from car
        sphereRB.transform.parent = null;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        
    }

    private void FixedUpdate()
    {
        // get inputs
        isAccelerating = Input.GetKey(accelerateKey);
        isBraking = Input.GetKey(brakeKey);
        isDrifting = Input.GetKey(driftKey);
        turnInput = Input.GetAxisRaw("Horizontal");

        // check if car is accelerating/braking
        accelerateForce = isAccelerating ? 100f : 0f;
        brakeForce = isBraking ? -50f : 0f;

        // set car position to sphere
        transform.position = sphereRB.transform.position;

        // set cars rotation
        if (isAccelerating)
        {
            float newRotation = turnInput * turnSpeed * Time.deltaTime;
            transform.Rotate(0, newRotation, 0, Space.World);
        }
        else if (isBraking)
        {
            float newRotation = turnInput * turnSpeed * Time.deltaTime * -1;
            transform.Rotate(0, newRotation, 0, Space.World);
        }

        sphereRB.AddForce(transform.forward * accelerateForce);
        
        if (isBraking)
        {
            sphereRB.AddForce(transform.forward * brakeForce);
        }
    }
}
