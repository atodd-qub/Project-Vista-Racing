using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject playerCar;
    public Rigidbody carRB;
    private float speed = 12.0f;
    private float turnSpeed = 40.0f;
    private float acceleration = 1;
    private float topSpeed = 100;
    private float horizontalInput;
    public KeyCode accelerateKey;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Check for player input (accelerate, brake, drift)
        // Accelerate
        //If accelrate key is held down, AddRelativeForce ? 
        if (Input.GetKeyDown(accelerateKey))
        {
            Debug.Log("K held down");
            //transform.Translate(Vector3.forward * Time.deltaTime * acceleration);
            carRB.AddRelativeForce(transform.forward * (speed * acceleration));
            acceleration += 1;
        }
        else
        {
            if (acceleration > 1)
            {
                acceleration -= 1;
            }
        }
    }

}
