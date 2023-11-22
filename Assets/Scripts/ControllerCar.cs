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
    public bool isAccelerating;
    public bool isBraking;
    private bool isDrifting;
    private bool isCarGrounded;
    public float accelerateForce;
    private float accelerateBaseForce = 0f;
    public float brakeForce;
    private float brakeBaseForce = -5f;
    private float maxAccelerateForce = 180f;
    private float accelerateThreshold = 150f;

    private float turnInput;
    public float turnSpeed;
    public float airDrag;
    public float groundDrag;
    public float alignToGroundTime;
    
    public Rigidbody sphereRB;
    public Rigidbody carRB;

    public LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        // deatch sphere rigidbody from car
        sphereRB.transform.parent = null;
        carRB.transform.parent = null;
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
        // accelerateForce = isAccelerating ? 100f : 0f;
        brakeForce = isBraking ? -50f : 0f;

        // set car position to sphere
        transform.position = sphereRB.transform.position;

        // set cars rotation
        if (isAccelerating)
        {
            float newRotation = turnInput * turnSpeed * Time.deltaTime;
            transform.Rotate(0, newRotation, 0, Space.World);

            // check if card is less than top speed
            if (accelerateForce < maxAccelerateForce && accelerateForce < accelerateThreshold)
            {
                accelerateForce += 1;
            }
            // accelerate slower if getting closer to top speed
            else if (accelerateForce < maxAccelerateForce)
            {
                accelerateForce += 0.5f;
            }
        } 
        else if (isBraking || accelerateForce > 0)
        {
            //float newRotation = turnInput * turnSpeed * Time.deltaTime * -1;
            float newRotation = turnInput * turnSpeed * Time.deltaTime;
            transform.Rotate(0, newRotation, 0, Space.World);

            // decelerate
            accelerateForce += -1;
        }

        // limit reverse speed
        if (accelerateForce <= 0)
        {
            accelerateForce = 0;
        }


        // check for deacceleration
        //if (!isAccelerating && accelerateForce > 0)
        //{
        //    accelerateForce += -1;
        //}

        
        

        // raycast ground check
        RaycastHit hit;
        isCarGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);

        // rotate car to be parallel to ground
        Quaternion toRotateto = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotateto, alignToGroundTime * Time.deltaTime);

        if (isCarGrounded)
        {
            sphereRB.drag = groundDrag;
        }
        else
        {
            sphereRB.drag = airDrag;
        }

        // add the current acceleration force
        if (isCarGrounded)
        {
            // move car
            sphereRB.AddForce(transform.forward * accelerateForce);
        }
        else
        {
            // add extra gravity if car is in air
            sphereRB.AddForce(transform.up * -30f);
        }

        // check if brake force is needed
        if (isBraking)
        {
            sphereRB.AddForce(transform.forward * brakeForce);
        }

        carRB.MoveRotation(transform.rotation);

    }
}
