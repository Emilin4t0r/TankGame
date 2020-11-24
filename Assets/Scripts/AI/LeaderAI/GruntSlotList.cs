using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntSlotList : MonoBehaviour {

    public GameObject[] gruntSlots;

    private void Awake() {
        gruntSlots = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) {
            gruntSlots[i] = transform.GetChild(i).gameObject;
        }
    }
    public GameObject GetEmptyGruntSlot() {
        foreach (GameObject slot in gruntSlots) {
            if (slot.GetComponent<GruntSlot>().myGrunt == null) {
                return slot;
            }
        }
        return null;
    }

    public List<GameObject> GetGrunts() {
        List<GameObject> grunts = new List<GameObject>();
        foreach (GameObject slot in gruntSlots) {
            if (slot.GetComponent<GruntSlot>().myGrunt != null) {
                grunts.Add(slot.GetComponent<GruntSlot>().myGrunt);
            }
        }
        if (grunts.Count > 0) {
            return grunts;
        } else {
            return null;
        }
    }
}
