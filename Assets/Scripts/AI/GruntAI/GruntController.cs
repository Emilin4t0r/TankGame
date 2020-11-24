using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntController : MonoBehaviour {

    public float leaderScanRadius;
    public string enemyTag;

    GameObject currentSlot;
    GameObject shootTarget;
    bool shooting;
    bool isSentry;
    public UnityEngine.AI.NavMeshAgent navMeshAgent;
    public List<Collider> enemyList = new List<Collider>();

    private void Awake() {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    private void FixedUpdate() {
        if (!isSentry && !shooting) {
            if (currentSlot == null) {
                FindSlot();
            } else {
                navMeshAgent.destination = currentSlot.transform.position;
            }
        }
        if (shooting) {
            Shoot();
        }
    }

    void FindSlot() {
        Collider[] colls = Physics.OverlapSphere(transform.position, leaderScanRadius);
        List<Collider> friendlySlotLists = new List<Collider>();
        foreach (Collider col in colls) {
            if (col.CompareTag("SlotList") && col.transform.parent.CompareTag(tag)) {
                friendlySlotLists.Add(col);
            }
        }
        if (friendlySlotLists.Count > 0) {
            foreach (Collider slotList in friendlySlotLists) {
                GameObject newSlot = slotList.GetComponent<GruntSlotList>().GetEmptyGruntSlot();
                if (newSlot != null) {
                    currentSlot = newSlot;
                    newSlot.GetComponent<GruntSlot>().myGrunt = gameObject;
                    StatePatternLeader currentLeader = slotList.transform.parent.GetComponent<StatePatternLeader>();
                    if (currentLeader.currentState == currentLeader.shootState) {
                        currentLeader.searchState.Attack(); //if the new leader is shooting at something, tell its new grunts to shoot as well
                    }
                    break;
                }
            }
            if (currentSlot == null) {
                isSentry = true;
                print("joopajoo");
            }
        } else {
            isSentry = true;
        }
    }

    public void Attack(List<Collider> enemies) {
        enemyList = enemies;
        if (shootTarget == null) {
            FindShootTarget();
        }
        shooting = true;
        StartCoroutine("GetNewFiringPos");

    }

    void Shoot() {
        if (shootTarget != null) {
            transform.LookAt(shootTarget.transform);
        } else {
            FindShootTarget();
        }
    }

    bool CanSeeEnemy() {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * 100);
        if (Physics.Raycast(transform.position, transform.forward, out hit)) {
            if (hit.transform.CompareTag(enemyTag)) {
                return true;
            } else {
                return false;
            }
        }
        return false;
    }

    void FindShootTarget() {
        if (enemyList.Count > 0) {
            int rand = Random.Range(0, enemyList.Count);
            if (enemyList[rand] != null) {
                shootTarget = enemyList[rand].gameObject;
            } else {
                enemyList.Remove(enemyList[rand]);
                if (enemyList.Count == 0) {
                    shooting = false;
                } else {
                    FindShootTarget();
                }
            }
        } else {
            shooting = false;
        }
    }

    IEnumerator GetNewFiringPos() {
        do {
            navMeshAgent.destination = new Vector3(transform.position.x + Random.Range(-2f, 2f), 1, transform.position.z + Random.Range(-2f, 2f));
            yield return new WaitForSeconds(2);
        } while (!CanSeeEnemy());
    }


    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, leaderScanRadius);
    }
}
