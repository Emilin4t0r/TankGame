using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour {

    public GameObject target;
    public float moveSpeed;
    public float lastSpeed;
    public bool speedLocked;
    WheelsControlAssigner tank;

    private void Start() {
        tank = GameObject.Find("Leo2").GetComponent<WheelsControlAssigner>();
    }

    void Update() {

        if (!speedLocked) {
            moveSpeed = Vector3.Distance(transform.position, target.transform.position) / 50;
            lastSpeed = moveSpeed;
        } else {
            moveSpeed = Vector3.Distance(transform.position, target.transform.position) / 50;
            if (moveSpeed > lastSpeed) {
                speedLocked = false;
            } else {
                moveSpeed = lastSpeed;
            }
        }

        if (lastSpeed >= moveSpeed) {
            moveSpeed = lastSpeed;
            speedLocked = true;
        }

        if (Vector3.Distance(transform.position, target.transform.position) < 0.25f) {
            speedLocked = false;
        }

        if (tank.turning) {
            moveSpeed = 0.2f;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed);
    }
}
