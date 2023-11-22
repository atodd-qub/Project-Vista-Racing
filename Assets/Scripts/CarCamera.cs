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
        var targetPosition = player.transform.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, 20f * Time.deltaTime);

        //Vector3 targetPosition = player.transform.position + offset;
        //transform.position = Vector3.Lerp(transform.position, targetPosition, 20f * Time.deltaTime);

        // rotation
        //var direction = player.transform.position - transform.position;
        //var rotation = Quaternion.LookRotation(direction, Vector3.up);
        //rotation.Set(-169.0f, rotation.y, rotation.z, rotation.w);
        //transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        var targetRotation = player.transform.rotation * Quaternion.Euler(10, 0, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
