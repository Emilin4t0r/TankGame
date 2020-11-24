using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkTargetParent : MonoBehaviour
{
    StatePatternLeader parentLeader;

    private void Start() {
        parentLeader = transform.parent.GetComponent<StatePatternLeader>();
    }
    public void TurnAround() {
        StartCoroutine("DoTurnAround");
    }

    IEnumerator DoTurnAround() {
        transform.LookAt(parentLeader.patrolArea.transform.position);
        yield return new WaitForSeconds(1);
        transform.localEulerAngles = Vector3.zero;
    }
}
