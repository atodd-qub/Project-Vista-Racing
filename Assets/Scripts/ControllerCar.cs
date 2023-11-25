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
    public bool isDrifting;
    private bool isCarGrounded;
    public float accelerateForce;
    private float accelerateBaseForce = 0f;
    public float brakeForce;
    private float brakeBaseForce = -5f;
    private float maxAccelerateForce = 210f;
    private float accelerateThreshold = 160f;

    private float turnInput;
    public float turnSpeed;
    public float airDrag;
    public float groundDrag;
    public float alignToGroundTime;

    //private float steerDirection;
    private float driftTime;
    private bool driftLeft = false;
    private bool driftRight = false;
    private float outwardsDriftForce = 500;
    private float driftDirection;
    private bool isSliding = false;

    public Transform leftDrift;
    public Transform rightDrift;

    
    public Rigidbody sphereRB;
    public Rigidbody carRB;

    public LayerMask groundLayer;
    public Animation carAnim;

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
                accelerateForce += 2;
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

        // drifting
        if (isDrifting && isCarGrounded)
        {
            //transform.GetComponent<Animator>().SetTrigger("Hop");
            //carAnim.Play("Hop");
            if (turnInput < 0)
            {
                driftLeft = true;
                driftRight = false;
            }
            else if (turnInput > 0)
            {
                driftLeft = false;
                driftRight = true;
            }

            if (accelerateForce > 40 && turnInput != 0)
            {
                //TO DO: particle effects
                // drift in correct direction
                if (driftLeft && !driftRight)
                {
                    driftDirection = turnInput < 0 ? -1.5f : -0.5f;
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, -20f, 0), 8f * Time.deltaTime);

                    if (isCarGrounded)
                    {
                        sphereRB.AddForce(transform.right * outwardsDriftForce * Time.deltaTime, ForceMode.Acceleration);
                    }
                }
                else if (driftRight && !driftLeft)
                {
                    driftDirection = turnInput < 0 ? 1.5f : 0.5f;
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 20f, 0), 8f * Time.deltaTime);

                    if (isCarGrounded)
                    {
                        sphereRB.AddForce(transform.right * -outwardsDriftForce * Time.deltaTime, ForceMode.Acceleration);
                    }
                }
                else
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0f, 0), 8f * Time.deltaTime);
                }

            }
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
