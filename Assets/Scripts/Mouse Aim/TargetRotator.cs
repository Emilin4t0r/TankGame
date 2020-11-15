using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRotator : MonoBehaviour {

    public GameObject targetRotatorSpot;

    float ySpeed;
    float xSpeed;
    public float maxSpeed;
    public bool isFreeLooking;
    Transform trans;

    Vector3 nextAimSpot;    

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        trans = transform;
        switch (Screen.width) {
            case 2048:
                maxSpeed = 0.8f;
                break;
            case 2560:
                maxSpeed = 1f;
                break;
            default:
                maxSpeed = 0.6f;
                break;
        }
    }

    void Update() {        
        if (!isFreeLooking) {
            MoveTurret();
        }
        if (Input.GetKey(KeyCode.C)) {
            isFreeLooking = true;
        } else {
            isFreeLooking = false;
        }
        trans.position = targetRotatorSpot.transform.position;
    } 
    
    void MoveTurret() {
        ySpeed = -Input.GetAxis("Mouse Y");
        xSpeed = Input.GetAxis("Mouse X");
        if (xSpeed > maxSpeed) {
            xSpeed = maxSpeed;
        }
        if (xSpeed < -maxSpeed) {
            xSpeed = -maxSpeed;
        }

        if (trans.eulerAngles.x < 340 && trans.eulerAngles.x > 180 && ySpeed < 0) {
            ySpeed = 0;
        } else if (trans.eulerAngles.x > 20 && trans.eulerAngles.x < 180 && ySpeed > 0) {
            ySpeed = 0;
        }
        nextAimSpot = trans.eulerAngles + new Vector3(ySpeed, xSpeed, 0);
        trans.eulerAngles = nextAimSpot;        
    }
}
