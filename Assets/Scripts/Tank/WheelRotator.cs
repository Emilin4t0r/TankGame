using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotator : MonoBehaviour {
    WheelsControlAssigner tankSpeed;
    float speed;

    private void Start() {
        tankSpeed = GameObject.Find("Leo2").GetComponent<WheelsControlAssigner>();
    }
    private void FixedUpdate() {
        speed = tankSpeed.transform.InverseTransformDirection(tankSpeed.rb.velocity).z;
        transform.Rotate(new Vector3(0, speed, 0));
    }
}
