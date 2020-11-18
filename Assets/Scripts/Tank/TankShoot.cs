using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShoot : MonoBehaviour {
    public GameObject explosion;
    public float velocity;
    public float fireRate;
    float nextFireTime;
    Transform trans;

    void Start() {
        trans = transform;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time > nextFireTime) {
            StartCoroutine("Shoot");
            nextFireTime = Time.time + fireRate;
        }
    }

    IEnumerator Shoot() {
        ViewChanger.Instance.ShakeCurrentCam(10, 5, 0, 1);
        float waitTime;
        RaycastHit hit;
        Debug.DrawRay(trans.position, trans.forward * 500, Color.white, 1f);
        if (Physics.Raycast(trans.position, trans.forward, out hit)) {
            waitTime = Vector3.Distance(trans.position, hit.point) / velocity;
            yield return new WaitForSeconds(waitTime);
            GameObject explsn = Instantiate(explosion, hit.point, hit.transform.rotation);
            Destroy(explsn, 1f);
        }
    }

    private void OnGUI() {
        if (Time.time > nextFireTime) {
            GUI.Label(new Rect(10, 200, 500, 100), "Ready To Fire!");
        } else {
            GUI.Label(new Rect(10, 200, 500, 100), "Loading Gun..." + (nextFireTime - Time.time).ToString("F0"));
        }
    }
}
