using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aimer : MonoBehaviour {
    public GameObject aimSpot;
    Transform aimTrans;
    float dist;

    private void Start() {
        aimTrans = aimSpot.transform;
    }

    void FixedUpdate() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit)) {
            if (hit.transform.gameObject.layer == 8) {
                aimTrans.position = hit.point;
                dist = Vector3.Distance(transform.position, hit.point) / 100;
                aimTrans.localScale = new Vector3(dist, dist, dist);
            }
        } else {
            aimTrans.localPosition = new Vector3(0, 0, 100);
            aimTrans.localScale = new Vector3(1, 1, 1);
        }
        
    }
}
