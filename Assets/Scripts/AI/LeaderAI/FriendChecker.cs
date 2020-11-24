using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendChecker : MonoBehaviour {
    StatePatternLeader parent;

    private void Start() {
        parent = transform.parent.GetComponent<StatePatternLeader>();
    }
    public void OnTriggerEnter(Collider other) {
        if (other.CompareTag(tag)) {            
            parent.TooCloseToFriendly();
        }
    }
}
