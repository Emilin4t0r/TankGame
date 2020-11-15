using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierShoot : MonoBehaviour {
    public GameObject projectile;
    public float shootForce;

    void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            Discharge();
        }
    }

    void Discharge() {
        GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
        newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * shootForce, ForceMode.Impulse);
        Destroy(newProjectile, 5f);
    }
}
