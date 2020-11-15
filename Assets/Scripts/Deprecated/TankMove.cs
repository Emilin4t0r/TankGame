using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMove : MonoBehaviour {
    public float moveSpeed;
    public float turnSpeed;
    public bool turning;

    private void Update() {
        
        if (Input.GetKey(KeyCode.W)) {
            transform.Translate(new Vector3(0, 0, moveSpeed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.Translate(new Vector3(0, 0, -moveSpeed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(new Vector3(0, -turnSpeed * Time.deltaTime, 0));
            turning = true;
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(new Vector3(0, turnSpeed * Time.deltaTime, 0));
            turning = true;
        }
        if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) {
            turning = false;
        }
    }
}
