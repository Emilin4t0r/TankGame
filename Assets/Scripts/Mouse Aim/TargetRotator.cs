using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRotator : MonoBehaviour {

    public GameObject targetRotatorSpot;

    float ySpeed;
    float xSpeed;
    public float maxSpeed;

    Vector3 nextAimSpot;    

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
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
        ySpeed = -Input.GetAxis("Mouse Y");
        xSpeed = Input.GetAxis("Mouse X");
        if (xSpeed > maxSpeed) {
            xSpeed = maxSpeed;
        }
        if (xSpeed < -maxSpeed) {
            xSpeed = -maxSpeed;
        }

        if (transform.eulerAngles.x < 340 && transform.eulerAngles.x > 180 && ySpeed < 0) {
            ySpeed = 0;
        } else if (transform.eulerAngles.x > 40 && transform.eulerAngles.x < 180 && ySpeed > 0) {
            ySpeed = 0;
        }
        nextAimSpot = transform.eulerAngles + new Vector3(ySpeed, xSpeed, 0);
        transform.eulerAngles = nextAimSpot;
        transform.position = targetRotatorSpot.transform.position;
    }    
}
