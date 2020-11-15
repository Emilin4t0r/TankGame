using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aimer : MonoBehaviour {
    public GameObject aimSpot;
    float dist;

    void Update() {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * 50, Color.green);
        if (Physics.Raycast(transform.position, transform.forward, out hit)) {
            if (hit.transform.gameObject.layer == 8) {
                aimSpot.transform.position = hit.point;
                dist = Vector3.Distance(transform.position, hit.point) / 100;
                aimSpot.transform.localScale = new Vector3(dist, dist, dist);
            }
        } else {
            aimSpot.transform.localPosition = new Vector3(0, 0, 100);
            aimSpot.transform.localScale = new Vector3(1, 1, 1);
        }
        
    }
}
