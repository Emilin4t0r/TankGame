using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelsControlAssigner : MonoBehaviour {
    public WheelCollider rWheel;
    public WheelCollider lWheel;
    WheelCollider[] rWheels;
    WheelCollider[] lWheels;
    public float motorForce;
    public float steerForce; //braking when turning; 
    public float brakeForce;
    public float topSpd;
    public float topReverseSpd;
    public float topTurnSpd;
    float speedCap;
    public bool reversing;
    public float currentSpeed;

    public float fricLo, fricHi;
    WheelFrictionCurve lCurve, rCurve;

    public Rigidbody rb;

    private void Start() {
        rWheels = ConvertToWC(GameObject.FindGameObjectsWithTag("RWheel"));
        lWheels = ConvertToWC(GameObject.FindGameObjectsWithTag("LWheel"));
        speedCap = topSpd;
        rb.centerOfMass = new Vector3(0, 1, -2); //Resetting center of mass lower to avoid tipping over
    }

    void Update() {

        /*---THIS SCRIPT FIRST DOES ALL CALCULATIONS FOR A SINGLE WHEEL OF ONE 
         *   SIDE AND THEN ASSIGNS SAID CALCULATIONS TO ALL WHEELS OF SAME SIDE---*/

        currentSpeed = rb.velocity.magnitude;
        
        //if no input is given, reset values
        lWheel.brakeTorque = 0;
        lWheel.motorTorque = 0;
        lCurve = lWheel.sidewaysFriction;
        lCurve.stiffness = fricHi;
        lWheel.sidewaysFriction = lCurve;

        rWheel.brakeTorque = 0;
        rWheel.motorTorque = 0;
        rCurve = rWheel.sidewaysFriction;
        rCurve.stiffness = fricHi;
        rWheel.sidewaysFriction = rCurve;


        //DRIVING FORWARD
        if (Input.GetKey(KeyCode.W)) {
            lWheel.motorTorque = motorForce;
            rWheel.motorTorque = motorForce;
            speedCap = topSpd;
        }

        //BRAKING
        if (Input.GetKey(KeyCode.S)) {
            lWheel.motorTorque = -motorForce;
            rWheel.motorTorque = -motorForce;
            speedCap = topReverseSpd;
            if (currentSpeed > topReverseSpd) {
                lWheel.brakeTorque = brakeForce;
                rWheel.brakeTorque = brakeForce;
            }
        }

        //SPEED LIMITER
        if (Mathf.Abs(currentSpeed) > speedCap) {
            lWheel.motorTorque = 0;
            rWheel.motorTorque = 0;
        }

        //TURNING LEFT
        if (Input.GetKey(KeyCode.A)) {
            rWheel.motorTorque = steerForce;
            rCurve = rWheel.sidewaysFriction;
            rCurve.stiffness = fricHi;
            rWheel.sidewaysFriction = rCurve;

            lWheel.motorTorque = -steerForce;
            lCurve = lWheel.sidewaysFriction;
            lCurve.stiffness = fricLo;
            lWheel.sidewaysFriction = lCurve;
        } else if (Input.GetKey(KeyCode.D)) { //TURNING RIGHT
            lWheel.motorTorque = steerForce;
            lCurve = lWheel.sidewaysFriction;
            lCurve.stiffness = fricHi;
            lWheel.sidewaysFriction = lCurve;

            rWheel.motorTorque = -steerForce;
            rCurve = rWheel.sidewaysFriction;
            rCurve.stiffness = fricLo;
            rWheel.sidewaysFriction = rCurve;
        }

        //TURN SPEED LIMITER
        if (rb.angularVelocity.magnitude > topTurnSpd) {
            lWheel.motorTorque = 0;
            rWheel.motorTorque = 0;
        }

        //HANDBRAKE
        if (Input.GetKey(KeyCode.Space) && currentSpeed > 0f) {
            lWheel.brakeTorque = brakeForce;
            rWheel.brakeTorque = brakeForce;
        }

        /*--------------------------DEBUGGING-------------------------------*/
        //TANK RESETTING WITH 'R' (FOR DEBUG PURPOSES)
        if (Input.GetKeyDown(KeyCode.R)) {
            transform.position = new Vector3(-10, 1, 0);
            transform.eulerAngles = new Vector3(0, 0, 0);
            lWheel.motorTorque = 0;
            rWheel.motorTorque = 0;
            rb.velocity = new Vector3(0, 0, 0);
        }
        /*--------------------------DEBUGGING-------------------------------*/

        //ASSIGN SIGNLE WHEEL CALCULATIONS TO ALL WHEELS OF SAME SIDE
        foreach (WheelCollider left in lWheels) {
            left.motorTorque = lWheel.motorTorque;
            left.sidewaysFriction = lCurve;
            left.brakeTorque = lWheel.brakeTorque;
            ApplyLocalPositionToVisuals(left);
        }
        foreach (WheelCollider right in rWheels) {
            right.motorTorque = rWheel.motorTorque;
            right.sidewaysFriction = rCurve;
            right.brakeTorque = rWheel.brakeTorque;
            ApplyLocalPositionToVisuals(right);
        }
    }

    WheelCollider[] ConvertToWC(GameObject[] array) {
        WheelCollider[] wheels = new WheelCollider[array.Length];
        for (int i = 0; i < array.Length; i++) {
            wheels[i] = array[i].GetComponent<WheelCollider>();
        }
        return wheels;
    }

    public void ApplyLocalPositionToVisuals(WheelCollider collider) {
        if (collider.transform.childCount == 0) {
            return;
        }
        Transform visualWheel = collider.transform.GetChild(0);
        Vector3 position;
        collider.GetWorldPose(out position, out _);
        visualWheel.transform.position = position;
    }

    private void OnGUI() {
        GUI.Label(new Rect(10, 10, 500, 100), "Tank Controls DEBUG Build. Press ALT+F4 to close.");
        GUI.Label(new Rect(10, 30, 500, 100), "WASD To move, Space for handbrake.");
        GUI.Label(new Rect(10, 50, 500, 100), "Mouse aims, LMB to fire main gun, RMB to fire machine gun");
        GUI.Label(new Rect(10, 70, 500, 100), "R to reset tank");
        GUI.Label(new Rect(10, 90, 150, 100), "Speed: " + transform.InverseTransformDirection(rb.velocity).z.ToString("F0"));
    }
}
