using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootState : ILeaderState {

    private readonly StatePatternLeader leader;

    public ShootState(StatePatternLeader statePatternLeader) {
        leader = statePatternLeader;
    }

    public void UpdateState() {
        if (leader.shootTarget == null) {            
            for (int i = 0; i < leader.enemies.Count; i++) {
                if (leader.enemies[i] == null) {
                    leader.enemies.Remove(leader.enemies[i]);
                }
            }
            FindEnemy();
        }
        if (leader.shootTarget != null) {
            leader.transform.LookAt(leader.shootTarget.transform);
        }
        if (leader.shootAnimDone) {
            leader.shootAnimDone = false;
            Shoot(leader.shootTarget);
            leader.CheckForce();
        }
    }
    public void ToFleeState() {

    }
    public void ToSearchState() {
        leader.currentState = leader.searchState;
    }
    public void ToSentryState() {

    }
    public void ToShootState() {

    }

    void FindEnemy() {
        if (leader.enemies.Count > 0) {
            
            leader.shootTarget = leader.enemies[Random.Range(0, leader.enemies.Count)].gameObject;
            Debug.Log(leader.name + "Found enemy: " + leader.shootTarget);
        } else {
            ToSearchState();
        }
    }
    void Shoot(GameObject target) {
        if (target != null) {            
            Debug.Log(leader.name + " shot at " + target.name);

            //Accuracy calculations
            Vector3 deviation3D = Random.insideUnitCircle * leader.accuracy;
            Quaternion rot = Quaternion.LookRotation(Vector3.forward + deviation3D);
            Vector3 fwd = leader.transform.rotation * rot * Vector3.forward;

            //Shooting ray
            RaycastHit hit;
            Debug.DrawRay(leader.transform.position, fwd * 100, Color.red, 0.5f);
            if (Physics.Raycast(leader.transform.position, fwd, out hit, 100)) {
                if (hit.transform.tag == leader.enemyTag) {
                    if (hit.transform.GetComponent<StatePatternLeader>()) { //if target is a leader
                        StatePatternLeader enemyLeader = hit.transform.GetComponent<StatePatternLeader>();
                        enemyLeader.health -= 25;
                        if (enemyLeader.health < 1) {
                            leader.RemoveEnemyLeader(enemyLeader);
                        }
                        Debug.Log(hit.transform.name + " was hit!");
                    } else if (hit.transform.GetComponent<GruntController>()) { //if target is a grunt
                        GruntController enemyGrunt = hit.transform.GetComponent<GruntController>();
                        enemyGrunt.health -= 25;
                        if (enemyGrunt.health < 1) {
                            leader.RemoveEnemyGrunt(enemyGrunt);
                        }
                    }
                }
            }
        }
        leader.AnimTimer(1, "ShootAnim");
    }

    
}
