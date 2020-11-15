using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMG : MonoBehaviour {
    public GameObject mGShot;
    public float shootForce;
    public float fireRate;
    float nextFireTime;
    void Update() {
        if (Input.GetKey(KeyCode.Mouse1) && Time.time > nextFireTime) {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot() {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * 50, Color.yellow);
        if (Physics.Raycast(transform.position, transform.forward, out hit)) {
            //if hit enemy { kill enemy after 0.2 seconds (to account for pseudobullet speed) }
        }
        GameObject newShot = Instantiate(mGShot, transform.position, transform.rotation);
        newShot.GetComponent<Rigidbody>().AddForce(newShot.transform.forward * shootForce, ForceMode.Impulse);
        Destroy(newShot, 2f);
    }
}
