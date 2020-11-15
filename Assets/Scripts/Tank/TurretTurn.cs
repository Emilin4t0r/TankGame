using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTurn : MonoBehaviour {
    public Transform target;
    public Transform gun;
    public float maxElevation = 20;
    public float guardElevation;
    float currentElev;

    private Quaternion qTurret;
    private Quaternion qGun;
    private Quaternion qGunStart;

    void Start() {
        qGunStart = gun.transform.localRotation;
        currentElev = maxElevation;
    }

    void Update() {
        float distanceToPlane = Vector3.Dot(transform.up, target.position - transform.position);
        Vector3 planePoint = target.position - transform.up * distanceToPlane;

        qTurret = Quaternion.LookRotation(planePoint - transform.position, transform.up);
        transform.eulerAngles = qTurret.eulerAngles;

        Vector3 v3 = new Vector3(0.0f, distanceToPlane, (planePoint - transform.position).magnitude);
        qGun = Quaternion.LookRotation(v3);

        if (Quaternion.Angle(qGunStart, qGun) <= currentElev)
            gun.localEulerAngles = qGun.eulerAngles;
    }
}
