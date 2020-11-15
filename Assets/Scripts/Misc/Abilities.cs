using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour {
    public enum Ability {Flare, SmokeGrenade, HandGrenade};
    public Ability ability;

    public GameObject flare;
    public GameObject flareSpawner;
    private void Update() {
        if (Input.GetKeyDown(KeyCode.G)) {
            if (ability == Ability.Flare) {
                ShootFlare(5f);
            }
        }
    }
    void ShootFlare(float force) {
        if (GameObject.Find("Flare(Clone)") == null){
            GameObject newFlare = Instantiate(flare, flareSpawner.transform.position, flareSpawner.transform.rotation);
            newFlare.GetComponent<Rigidbody>().AddForce(newFlare.transform.forward * force, ForceMode.Impulse);
            Destroy(newFlare, 10f);
        }
    }
}
