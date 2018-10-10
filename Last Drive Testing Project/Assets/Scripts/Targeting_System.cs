using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Written By: Socrates Mridha
//Date: 10/10/2018
//Purpose: Used for aiming and locking on to other player with rocket.
public class Targeting_System : MonoBehaviour {

    Camera cam;

    //Crosshairs
    public Image crosshair; 
    public Canvas lockedCrosshair;// Worldspace canvas.
    
    //Enemy:
    public GameObject ePlayer;//Enemy player.
    public float eDistance;// Enemy's distance from player.

    //Targeting Veriables:
    public float lockDelay;//How long it takes to lock on to enemy cars.
    public float targetingRange;//Range for the targeting system.
    public float lockingTimer;//Counter on how long the raycast is targeting the enemy.

    public bool targetLocked;//If enemy is locked on.


    void Start () {
        cam = GetComponent<Camera>();
    }
	
	void Update () {
        if (!targetLocked)
        {
            RaycastAim();
            CrosshairFollowMouse();
            CrosshairFollowJoystick();//WORKING PROGRESS.*****

            crosshair.gameObject.SetActive(true);
            lockedCrosshair.gameObject.SetActive(false);
        }
        else
        {
            LockTarget();
            CrosshairLocked();
        }

        if (Input.GetMouseButtonUp(1))//Right mouse click to lose target.
        {
            TargetLost();
        }

    }

    //When enemy car is in range and in line of the raycast.
    void StartLockingTarget()
    {
        eDistance = Vector3.Distance(gameObject.transform.position, ePlayer.transform.position);// Enemy's distance form player.

        //If the enemy car is within the locking range the timer will start.
        if (eDistance < targetingRange)
        {
            lockingTimer += Time.deltaTime;
        }

        //If the timer reaches the targeting system's lock delay targetLocked = true;
        if (lockingTimer > lockDelay)
        {
            targetLocked = true;
        }
    }

    //Lock target after the enemy was targeted for the desired time "lockDelay".
    void LockTarget()
    {
        //Crosshairs:
        lockedCrosshair.gameObject.SetActive(true);
        crosshair.gameObject.SetActive(false);
        crosshair.transform.position = ePlayer.transform.position;

        //Enemy Distance:
        eDistance = Vector3.Distance(gameObject.transform.position, ePlayer.transform.position);// Enemy's distance is still tracked from the player.
        
        //If the locked enemy gets too far from the player, TargetLost function is called.
        if (eDistance > targetingRange)
        {
            TargetLost();
        }
    }

    //Target is no longer locked on.
    void TargetLost()
    {
        //Reseting all values to zero/false/null.
        ePlayer = null;//Enemy.

        eDistance = 0;//Distance.

        lockingTimer = 0;//Locking Counter

        targetLocked = false;
    }

    //Raycast shot from the camera to where the cursor is aiming.
    void RaycastAim()
    {
        //Raycast:
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        //If raycast hits anything.
        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;

            //If the raycart hits any gameobject with a "Player" tag. STILL MIGHT NEED WORK FOR MULTIPLAYER.*****
            if (objectHit.tag == "Player" && objectHit.name != this.gameObject.name)//WORKING CODITION FOR NOT SELECTING PLAYER'S OWN CAR.*****
            {
                ePlayer = objectHit.gameObject;

                StartLockingTarget();

                //Crosshair colors if the player is within or too far from range.
                if (eDistance > targetingRange)
                {
                    crosshair.color = Color.yellow;
                }
                else crosshair.color = Color.green;
            }
            else
            {
                crosshair.color = Color.grey;//Defalut Color
                TargetLost();
            }
        }
    }

    //Crosshair on canvas follows mouse.
    void CrosshairFollowMouse()
    {
        crosshair.transform.position = Input.mousePosition;
    }

    void CrosshairFollowJoystick()//WORKING PROGRESS.*****
    {
        //crosshair transform position using the right joystick axis with an Xbox One Controller.
    }


    //For the crosshair to lock onto the target using worldspace canvas.
    void CrosshairLocked()
    {
        Vector3 eOffset = ePlayer.transform.position;
        lockedCrosshair.transform.position = (eOffset + gameObject.transform.position) / 2; //NEEDS TESTING FOR MULTIPLAYER. *****
    }
}
