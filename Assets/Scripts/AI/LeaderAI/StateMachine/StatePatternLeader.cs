using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePatternLeader : MonoBehaviour {
    public float sightRange;
    public float walkTargetWidth; //the width range in local x-axis to randomize a new walking target position
    public string enemyTag;
    public GameObject patrolArea;
    public float searchDistance; //distance how far the leader can go from the patrol area
    public float accuracy; //Size of accuracy cone when shooting (0.0f - 1.0f) (not recommended to go above 0.2f)

    [HideInInspector]
    public int health;
    [HideInInspector]
    public GameObject gruntSlotList;
    [HideInInspector]
    public List<GameObject> grunts;
    [HideInInspector]
    public List<Collider> enemies;
    [HideInInspector]
    public bool fleeing;
    [HideInInspector]
    public GameObject walkTargetParent;
    [HideInInspector]
    public GameObject walkTargetOrig;
    [HideInInspector]
    public Vector3 walkingTarget;
    [HideInInspector]
    public Vector3 targetArea;
    [HideInInspector]
    public GameObject shootTarget;
    [HideInInspector]
    public ILeaderState currentState;
    [HideInInspector]
    public SearchState searchState;
    [HideInInspector]
    public SentryState sentryState;
    [HideInInspector]
    public ShootState shootState;
    [HideInInspector]
    public FleeState fleeState;



    public bool attackAnimDone;
    public bool shootAnimDone;


    [HideInInspector]
    public UnityEngine.AI.NavMeshAgent navMeshAgent;


    void Awake() {
        searchState = new SearchState(this);
        shootState = new ShootState(this);
        /*sentryState = new SentryState(this);        
        fleeState = new FleeState(this);*/

        gruntSlotList = transform.GetChild(3).gameObject;
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        walkTargetParent = transform.GetChild(0).gameObject;
        walkTargetOrig = walkTargetParent.transform.GetChild(0).gameObject;
    }

    void Start() {
        health = 100;
        shootAnimDone = true;
        currentState = searchState;
    }

    void Update() {
        currentState.UpdateState();
    }

    public void CheckForce() {
        //check amount of leader's soldiers
    }

    public void Die() {
        Destroy(gameObject);
    }

    public void AnimTimer(float time, string animName) {
        StartCoroutine(AnimCor(time, animName));
    }

    IEnumerator AnimCor(float time, string animName) {

        yield return new WaitForSeconds(time);
        switch (animName) {
            case "AttackAnim":
                attackAnimDone = true;
                break;
            case "ShootAnim":
                shootAnimDone = true;
                break;
        }

    }

    public void TooCloseToFriendly() {
        if (currentState == searchState) {
            searchState.TurnAround();
        }
    }

    public void UpdateGruntEnemyLists(Collider removee) {
        if (grunts != null) {
            foreach (GameObject grunt in grunts) {
                if (grunt.GetComponent<GruntController>().enemyList.Contains(removee))
                    grunt.GetComponent<GruntController>().enemyList.Remove(removee);
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
