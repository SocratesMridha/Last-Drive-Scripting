using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Script Written By: Socrates Mridha
// Date: 10/10/2018
// Purpose: For the camera to smoothly follow player's car with a Slerp for smooth transitions. 
public class Camera_Follow : MonoBehaviour {

    //The desired position for the camera to be.
    public Transform targetPosition;
    public Transform car; //Car transform for the camera to look at.

    public float smoothedSpeed;

    void FixedUpdate()
    {

        Vector3 desiredPosition = targetPosition.position;
        Vector3 smoothedPosition = Vector3.Slerp(transform.position, desiredPosition, smoothedSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(car);

    }
}
