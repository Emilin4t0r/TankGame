using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntController : MonoBehaviour {

    public float leaderScanRadius;
    public string enemyTag;
    public float accuracy;

    [HideInInspector]
    public int health;
    float nextShootTime = 0;
    StatePatternLeader currentLeader;
    GameObject currentSlot;
    public GameObject shootTarget;
    bool shooting;
    bool isSentry;
    public UnityEngine.AI.NavMeshAgent navMeshAgent;
    public List<Collider> enemyList = new List<Collider>();

    private void Awake() {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        health = 100;
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
            if (shootTarget != null) {
                transform.LookAt(shootTarget.transform);
            }
            if (shootTarget != null && Time.time > nextShootTime) {                
                Shoot();
            }
            if (shootTarget == null) {
                FindShootTarget();
            }
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
                    currentLeader = slotList.transform.parent.GetComponent<StatePatternLeader>();
                    if (currentLeader.currentState == currentLeader.shootState) {
                        currentLeader.searchState.Attack(); //if the new leader is shooting at something, tell its new grunts to shoot as well
                    }
                    break;
                }
            }
            if (currentSlot == null) {
                isSentry = true;
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
            
            //Accuracy calculations
            Vector3 deviation3D = Random.insideUnitCircle * accuracy;
            Quaternion rot = Quaternion.LookRotation(Vector3.forward + deviation3D);
            Vector3 fwd = transform.rotation * rot * Vector3.forward;

            //Shooting ray
            RaycastHit hit;
            Debug.DrawRay(transform.position, fwd * 100, Color.red, 0.5f);
            if (Physics.Raycast(transform.position, fwd, out hit, 100)) {
                if (hit.transform.tag == enemyTag) {
                    if (hit.transform.GetComponent<StatePatternLeader>()) { //if target is a leader
                        StatePatternLeader enemyLeader = hit.transform.GetComponent<StatePatternLeader>();
                        enemyLeader.health -= 10;
                        if (enemyLeader.health < 1) {
                            if (currentLeader != null) {
                                currentLeader.RemoveEnemyLeader(enemyLeader);
                            } else {
                                enemyList.Remove(enemyLeader.GetComponent<Collider>());
                                enemyLeader.Die();
                            }
                        }
                    } else if (hit.transform.GetComponent<GruntController>()) { //if target is a grunt
                        GruntController enemyGrunt = hit.transform.GetComponent<GruntController>();
                        enemyGrunt.health -= 10;
                        if (enemyGrunt.health < 1) {
                            if (currentLeader != null && enemyList.Contains(enemyGrunt.GetComponent<Collider>())) {
                                currentLeader.RemoveEnemyGrunt(enemyGrunt);
                            } else {
                                enemyList.Remove(enemyGrunt.GetComponent<Collider>());
                                enemyGrunt.Die();
                            }
                        }
                    }
                }
            }
        }
        nextShootTime = Time.time + 1;
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
        } while (!CanSeeEnemy() && shooting);
    }

    public void Die() {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, leaderScanRadius);
    }
}
