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
        } else {
            Shoot(leader.shootTarget);
        }
        leader.CheckForce();
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
        } else {
            ToSearchState();
        }
    }
    void Shoot(GameObject target) {
        //Debug.Log(leader.name + " Shooting at " + target.name);
        leader.transform.LookAt(target.transform);
    }
}
