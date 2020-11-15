using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeLook : MonoBehaviour {
    Vector2 rotation = new Vector2(0, 0);
    bool needsToReset;
    public float sensitivity = 1;
    TargetRotator targetRotator;
    Transform trans;

    private void Start() {
        targetRotator = GameObject.Find("TargetRotator").GetComponent<TargetRotator>();
        trans = transform;
    }

    void Update() {
        if (targetRotator.isFreeLooking) {
            Look();
        } else if (needsToReset) {
            ResetAngle();
        }
    }

    void Look() {
        rotation.y += Input.GetAxis("Mouse X");
        rotation.x += -Input.GetAxis("Mouse Y");
        trans.localEulerAngles = rotation * sensitivity;
        if (!needsToReset)
            needsToReset = true;
    }

    void ResetAngle() {
        trans.localEulerAngles = Vector3.zero;
        trans.localPosition = Vector3.zero;
        rotation = Vector2.zero;
        needsToReset = false;
    }
}
