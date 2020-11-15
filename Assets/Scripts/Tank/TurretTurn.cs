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

    private Transform trans;

    void Start() {
        qGunStart = gun.transform.localRotation;
        currentElev = maxElevation;
        trans = transform;
    }

    void Update() {
        float distanceToPlane = Vector3.Dot(trans.up, target.position - trans.position);
        Vector3 planePoint = target.position - trans.up * distanceToPlane;

        qTurret = Quaternion.LookRotation(planePoint - trans.position, trans.up);
        trans.eulerAngles = qTurret.eulerAngles;

        Vector3 v3 = new Vector3(0.0f, distanceToPlane, (planePoint - trans.position).magnitude);
        qGun = Quaternion.LookRotation(v3);

        if (Quaternion.Angle(qGunStart, qGun) <= currentElev)
            gun.localEulerAngles = qGun.eulerAngles;
    }
}
