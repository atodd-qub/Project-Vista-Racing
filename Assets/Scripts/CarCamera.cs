using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCamera : MonoBehaviour
{
    public Camera currentCamera;
    public GameObject player;
    public Vector3 offset; // offset for main camera
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // translation
        //transform.position = player.transform.position + offset;
        var targetPosition = player.transform.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, 20f * Time.deltaTime);

        // rotation
        var direction = player.transform.position - transform.position;
        var rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        //transform.LookAt(player.transform, Vector3.up);
        //transform.rotation 
    }
}
