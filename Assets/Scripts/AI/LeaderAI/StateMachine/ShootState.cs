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
            FindEnemy();
            //Shoot(leader.shootTarget);
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
        leader.transform.LookAt(target.transform);
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
                if (hit.transform.GetComponent<StatePatternLeader>()) {
                    StatePatternLeader enemyLeader = hit.transform.GetComponent<StatePatternLeader>();
                    enemyLeader.health -= 50;
                    if (enemyLeader.health < 1) {
                        leader.enemies.Remove(enemyLeader.GetComponent<Collider>());
                        leader.UpdateGruntEnemyLists(enemyLeader.GetComponent<Collider>());
                        enemyLeader.Die();
                    }
                    Debug.Log(hit.transform.name + " was hit!");
                }
            }
        }
        leader.AnimTimer(1, "ShootAnim");
    }
}
