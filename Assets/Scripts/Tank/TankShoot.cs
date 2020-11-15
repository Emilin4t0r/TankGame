using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShoot : MonoBehaviour {
    public GameObject shot;
    public float shootForce;

    void Start() {
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Shoot();
        }
    }

    void Shoot() {
        GameObject newShot = Instantiate(shot, transform.position, transform.rotation);
        newShot.GetComponent<Rigidbody>().AddForce(newShot.transform.forward * shootForce, ForceMode.Impulse);
        Destroy(newShot, 5f);
    }
}
