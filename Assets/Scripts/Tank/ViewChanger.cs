using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class ViewChanger : MonoBehaviour
{
    public static ViewChanger Instance;

    public GameObject mainCamera;
    public GameObject sightCamera;
    public GameObject aimer;
    public MeshRenderer gunMesh;
    public GameObject currentCamera;

    public GameObject normalAimer;
    public GameObject sightAimer;

    private void Start() {
        Instance = this;
        currentCamera = mainCamera;
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            ToggleCamera();
        }
    }
    void ToggleCamera() {
        if (mainCamera.activeSelf == true) { //TO SIGHT MODE
            mainCamera.SetActive(false);
            sightCamera.SetActive(true);
            aimer.SetActive(true);
            gunMesh.enabled = false;
            normalAimer.SetActive(false);
            sightAimer.SetActive(true);
            currentCamera = sightCamera;
        } else {
            sightCamera.SetActive(false); //TO 3RD PERSON
            mainCamera.SetActive(true);
            gunMesh.enabled = true;
            aimer.SetActive(false);
            normalAimer.SetActive(true);
            sightAimer.SetActive(false);
            currentCamera = mainCamera;
        }
    }

    public void ShakeCurrentCam(float mag, float rough, float fadeIn, float fadeOut) {
        CameraShaker.GetInstance(currentCamera.name).ShakeOnce(mag, rough, fadeIn, fadeOut);
    }
}
