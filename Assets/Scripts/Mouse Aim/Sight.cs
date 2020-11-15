using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour {
    public GameObject gun;
    void FixedUpdate() {
        transform.localEulerAngles = new Vector3(gun.transform.localEulerAngles.x, 0, 0);
    }
}
