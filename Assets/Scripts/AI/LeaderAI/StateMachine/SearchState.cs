using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : ILeaderState {

    private readonly StatePatternLeader leader;
    bool shouldStop;   
    public SearchState(StatePatternLeader statePatternLeader) {
        leader = statePatternLeader;
    }

    public void UpdateState() {
        Search();

        if (leader.attackAnimDone) {
            leader.attackAnimDone = false;
            shouldStop = false;
            ToShootState();
        }
        leader.CheckForce();
    }
    #region Imethods
    public void ToFleeState() {

    }
    public void ToSearchState() {

    }
    public void ToSentryState() {

    }
    #endregion Imethods


    public void ToShootState() {
        leader.currentState = leader.shootState;
    }
    void Search() {
        leader.navMeshAgent.isStopped = false;
        if (leader.walkingTarget == Vector3.zero) {
            GetNewWalkingTarget();
        }
        if (!shouldStop) {
            leader.navMeshAgent.destination = leader.walkingTarget;
        }               

        // Leader has arrived to destination.
        if (leader.navMeshAgent.remainingDistance <= leader.navMeshAgent.stoppingDistance && !leader.navMeshAgent.pathPending && !shouldStop) {              
            GetNewWalkingTarget();
            Look();
        }
    }

    public void GetNewWalkingTarget() {        
        leader.walkingTarget = new Vector3(leader.walkTargetOrig.transform.position.x + Random.Range(-leader.walkTargetWidth, leader.walkTargetWidth),
            1,
            leader.walkTargetOrig.transform.position.z + Random.Range(-leader.walkTargetWidth, leader.walkTargetWidth)
            );
        if (Vector3.Distance(leader.transform.position, leader.patrolArea.transform.position) > leader.searchDistance) {
            TurnAround();
            leader.walkingTarget = new Vector3(leader.walkTargetOrig.transform.position.x + Random.Range(-leader.walkTargetWidth, leader.walkTargetWidth),
            1,
            leader.walkTargetOrig.transform.position.z + Random.Range(-leader.walkTargetWidth, leader.walkTargetWidth)
            );
        }
    }

    public void TurnAround() {
        //leader.transform.LookAt(leader.patrolArea.transform.position); //maybe smooth snap-turning by rotating a parent of walk target instead of leader  
        leader.walkTargetParent.GetComponent<WalkTargetParent>().TurnAround();
    }

    void Look() {
        Collider[] colliders = Physics.OverlapSphere(leader.transform.position, leader.sightRange); 
        foreach (Collider col in colliders) {
            if (col.CompareTag(leader.enemyTag)) {//If if see one enemy, 
                colliders = Physics.OverlapSphere(col.transform.position, 10f);//make new scan for enemies around it
            }
        }
        leader.enemies = new List<Collider>();
        foreach (Collider col in colliders) {
            if (col.CompareTag(leader.enemyTag)) { //add enemies in are to list
                leader.enemies.Add(col);
            }
        }
        if (leader.enemies.Count > 0) {
            leader.targetArea = leader.enemies[0].transform.position;
            shouldStop = true; //Stop moving
            Attack();
        }
    }

    public void Attack() {
        Debug.Log(leader.name + " giving attack command to grunts!");
        leader.grunts = leader.gruntSlotList.GetComponent<GruntSlotList>().GetGrunts();
        if (leader.grunts != null) {
            foreach (GameObject grunt in leader.grunts) {
                grunt.GetComponent<GruntController>().Attack(leader.enemies);
            }
        }
        leader.AnimTimer(1, "AttackAnim");
    }
}
