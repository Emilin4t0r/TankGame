using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMG : MonoBehaviour {
    public GameObject mGShot;
    public float shootForce;
    public float fireRate;
    float nextFireTime;
    Transform trans;
    private void Start() {
        trans = transform;
    }
    void Update() {
        if (Input.GetKey(KeyCode.Mouse1) && Time.time > nextFireTime) {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot() {
        RaycastHit hit;
        Debug.DrawRay(trans.position, trans.forward * 50, Color.yellow);
        if (Physics.Raycast(trans.position, trans.forward, out hit)) {
            //if hit enemy { kill enemy after 0.2 seconds (to account for pseudobullet speed) }
        }
        GameObject newShot = Instantiate(mGShot, trans.position, trans.rotation);
        newShot.GetComponent<Rigidbody>().AddForce(newShot.transform.forward * shootForce, ForceMode.Impulse);
        Destroy(newShot, 2f);
    }
}
