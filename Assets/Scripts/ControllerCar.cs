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
    public bool isCarGrounded;
    public bool isCarOnGrass;
    public float accelerateForce;
    private float accelerateBaseForce = 0f;
    public float brakeForce;
    private float brakeBaseForce = -5f;
    public float maxAccelerateForce;
    private float accelerateThreshold = 160f;

    public float turnInput;//temp pub
    public float turnSpeed;
    public float airDrag;
    public float groundDrag;
    public float alignToGroundTime;
    private float steerAmount;

    //private float steerDirection;
    private float driftTime;
    public bool driftLeft = false;//temp pub
    public bool driftRight = false;//temp pub
    private float outwardsDriftForce = 500;
    public float driftDirection;//temp pub
    private bool driftDirLock;

    public Transform leftDrift;
    public Transform rightDrift;

    
    public Rigidbody sphereRB;
    public Rigidbody carRB;

    public LayerMask groundLayer;
    public LayerMask grassLayer;

    private AudioSource carAudio;
    public AudioClip revSound;
    public AudioClip deaccelerateSound;
    public AudioClip driftSound;
    public AudioClip boostSoundOne;
    public AudioClip boostSoundTwo;
    public AudioClip boostSoundThree;
    private float engineSoundPitch;
    public float minPitch;
    public float maxPitch;

    public float currentSpeed;
    private float minSpeed = 0;
    private float maxSpeed = 48.2f;

    public GameObject[] wheelsToRotate;
    public float rotationSpeed;
    private Animator anim;

    public TrailRenderer[] trails;

    public ParticleSystem[] driftParticles;
    public Material sparks;
    public Color drift1;
    public Color drift2;
    public Color drift3;
    public ParticleSystem[] boostParticles;

    // Start is called before the first frame update
    void Start()
    {
        // deatch sphere rigidbody from car
        sphereRB.transform.parent = null;
        carRB.transform.parent = null;

        // get components
        carAudio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
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

        // drifting
        if (isDrifting && isCarGrounded)
        {
            //transform.GetComponent<Animator>().SetTrigger("Hop");
            //carAnim.Play("Hop");
            if (turnInput < 0 && !driftDirLock)
            {
                driftLeft = true;
                driftRight = false;
                driftDirLock = true;
            }
            else if (turnInput > 0 && !driftDirLock)
            {
                driftLeft = false;
                driftRight = true;
                driftDirLock = true;
            }

            if (accelerateForce > 40)
            {
                // start drift timer
                driftTime += Time.deltaTime;

                // drift in correct direction
                if (driftLeft && !driftRight)
                {
                    driftDirection = turnInput < 0 ? -1.5f : -0.5f;
                    if (turnInput == 1) //if holding other direction
                    {
                        driftDirection = -0.3f;
                    }
                    transform.GetChild(0).localRotation = Quaternion.Lerp(transform.GetChild(0).localRotation, Quaternion.Euler(0, -20f, 0), 8f * Time.deltaTime);

                    if (isCarGrounded)
                    {
                        sphereRB.AddForce(transform.right * outwardsDriftForce * Time.deltaTime, ForceMode.Acceleration);
                    }

                    carAudio.PlayOneShot(driftSound, 0.35f);
                    ToggleTrails(true);
                }
                else if (driftRight && !driftLeft)
                {
                    driftDirection = turnInput > 0 ? 1.5f : 0.5f;
                    if (turnInput == -1) //if holding other direction
                    {
                        driftDirection = 0.3f;
                    }
                    transform.GetChild(0).localRotation = Quaternion.Lerp(transform.GetChild(0).localRotation, Quaternion.Euler(0, 20f, 0), 8f * Time.deltaTime);

                    if (isCarGrounded)
                    {
                        sphereRB.AddForce(transform.right * -outwardsDriftForce * Time.deltaTime, ForceMode.Acceleration);
                    }

                    carAudio.PlayOneShot(driftSound, 0.4f);
                    ToggleTrails(true);
                    
                }
                else
                {
                    transform.GetChild(0).localRotation = Quaternion.Lerp(transform.GetChild(0).localRotation, Quaternion.Euler(0, 0f, 0), 8f * Time.deltaTime);
                }

                //TO DO: particle effects
                if (driftTime >= 1.5 && driftTime < 3)
                {
                    //TO DO
                    foreach (var driftParticle in driftParticles)
                    {
                        ParticleSystem.MainModule driftMain = driftParticle.main;
                        driftMain.startColor = drift1;
                        sparks.SetColor("_EmissionColor", drift1 * 2.2f);
                        driftMain.startSize = 0.2f;

                        if(!driftParticle.isPlaying)
                        {
                            driftParticle.Play();
                        }
                    }
                }
                if (driftTime >= 3 && driftTime < 6)
                {
                    // TO DO
                    foreach (var driftParticle in driftParticles)
                    {
                        ParticleSystem.MainModule driftMain = driftParticle.main;
                        driftMain.startColor = drift2;
                        sparks.SetColor("_EmissionColor", drift2 * 2.2f);
                        driftMain.startSize = 0.3f;
                        driftParticle.Play();
                    }
                }
                if (driftTime >= 6)
                {
                    // TO DO
                    foreach (var driftParticle in driftParticles)
                    {
                        ParticleSystem.MainModule driftMain = driftParticle.main;
                        driftMain.startColor = drift3;
                        sparks.SetColor("_EmissionColor", drift3 * 2.2f);
                        driftMain.startSize = 0.4f;
                        driftParticle.Play();
                    }
                }

            }
        }
        else if (!isDrifting || accelerateForce < 40)
        {
            // if player is no longer drifting...
            // or car is going too slow to drift...
            // give boost
            if (driftTime >= 1.5 && driftTime < 3)
            {
                // TO DO: Particles

                // boost
                Debug.Log("driftTime: " + driftTime);
                StartCoroutine(Boost(50, 0.5f));
                carAudio.PlayOneShot(boostSoundOne, 0.95f);
            }
            if (driftTime >= 3 && driftTime < 6)
            {
                // TO DO: Particles

                // boost
                Debug.Log("driftTime: " + driftTime);
                StartCoroutine(Boost(100, 0.75f));
                carAudio.PlayOneShot(boostSoundTwo, 0.95f);
            }
            if (driftTime >= 6)
            {
                // TO DO: Particles

                // boost
                Debug.Log("driftTime: " + driftTime);
                StartCoroutine(Boost(140, 1f));
                carAudio.PlayOneShot(boostSoundThree, 0.95f);
            }

            // reset everything
            transform.GetChild(0).localRotation = Quaternion.Lerp(transform.GetChild(0).localRotation, Quaternion.Euler(0, 0f, 0), 8f * Time.deltaTime);
            driftDirection = 0;
            driftDirLock = false;
            driftLeft = false;
            driftRight = false;
            driftTime = 0;
            ToggleTrails(false);
            foreach (var driftParticle in driftParticles)
            {
                driftParticle.Stop();
            }
        }

        // raycast ground check
        RaycastHit hit;
        isCarGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);
        isCarOnGrass = Physics.Raycast(transform.position, -transform.up, out hit, 1f, grassLayer);

        // set max speed based on cars location
        if (isCarGrounded)
        {
            maxAccelerateForce = 210f;
        }
        else
        {
            maxAccelerateForce = 65f;
        }

        // set acceleration force
        if (isAccelerating)
        {
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
            // decelerate
            accelerateForce += -1;

            // play sfx
            //carAudio.Stop();
        }

        if (isCarOnGrass && accelerateForce > maxAccelerateForce)
        {
            // decelerate if on grass
            accelerateForce += -5;
        }

        // set car's rotation
        float newRotation;
        if (isDrifting)
        {
            newRotation = driftDirection * turnSpeed * Time.deltaTime;
        }
        else
        {
            newRotation = turnInput * turnSpeed * Time.deltaTime;
        }
        transform.Rotate(0, newRotation, 0, Space.Self);
        //since handling is supposed to be stronger when car is moving slower, we adjust steerAmount depending on the real speed of the car, and then rotate the car on its y axis with steerAmount
        /*if (isDrifting)
        {
            steerAmount = accelerateForce > 30 ? accelerateForce / 4 * driftDirection : steerAmount = accelerateForce / 1.5f * driftDirection;
        }
        else
        {
            steerAmount = accelerateForce > 30 ? accelerateForce / 4 * turnInput : steerAmount = accelerateForce / 1.5f * turnInput;
        }
        steerDirVect = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + steerAmount, transform.eulerAngles.z);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, steerDirVect, 3 * Time.deltaTime);
        */

        // limit reverse speed
        if (accelerateForce <= 0)
        {
            accelerateForce = 0;
        }

        // rotate car to be parallel to ground
        Quaternion toRotateto = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotateto, alignToGroundTime * Time.deltaTime);

        if (isCarGrounded || isCarOnGrass)
        {
            sphereRB.drag = groundDrag;
        }
        else
        {
            sphereRB.drag = airDrag;
        }

        // add the current acceleration force
        if (isCarGrounded || isCarOnGrass)
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

        // move rb roation of car model to match overall rotation
        carRB.MoveRotation(transform.rotation);

        // play sfx
        if(accelerateForce > 0)
        {
            EngineSound();
            SpinWheels();
        }
    }

    private IEnumerator Boost(float boostForce, float boostTime)
    {
        while(boostTime > 0)
        {
            sphereRB.AddForce(transform.forward * boostForce);
            boostTime -= Time.deltaTime;
            foreach(var boost in boostParticles)
            {
                boost.Play();
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private void EngineSound()
    {
        currentSpeed = carRB.velocity.magnitude;
        
        
        engineSoundPitch = carRB.velocity.magnitude / 60f;

        if (currentSpeed < maxSpeed)
        {
            carAudio.pitch = minPitch;
        }

        if (currentSpeed > minSpeed && currentSpeed < maxSpeed)
        {
            carAudio.pitch = minPitch + engineSoundPitch;
        }

        if (currentSpeed > maxSpeed)
        {
            carAudio.pitch = maxPitch;
        }
    }

    private void SpinWheels()
    {
        foreach (var wheel in wheelsToRotate)
        {
            wheel.transform.Rotate(Time.deltaTime  * rotationSpeed, 0, 0, Space.Self);
        }

        if (turnInput > 0)
        {
            // turning right
            anim.SetBool("goingLeft", false);
            anim.SetBool("goingRight", true);
        }
        else if (turnInput < 0)
        {
            // turning left
            anim.SetBool("goingLeft", true);
            anim.SetBool("goingRight", false);
        }
        else
        {
            // straight
            anim.SetBool("goingLeft", false);
            anim.SetBool("goingRight", false);
        }
    }

    private void ToggleTrails(bool toggle)
    {
        foreach (var trail in trails)
        {
            trail.emitting = toggle;
        }
    }
}