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

    private void Awake() {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    private void FixedUpdate() {
        if (!isSentry && !shooting) {
            if (currentSlot == null) {
                FindSlot();
            }
            navMeshAgent.destination = currentSlot.transform.position;
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
                    break;
                } else {
                    print("Found no slots from " + slotList.transform.parent);
                    //isSentry = true;
                }
            }
        } else {
            isSentry = true;
        }
    }

    public void Attack(List<Collider> enemies) {
        if (shootTarget == null) {
            shootTarget = enemies[Random.Range(0, enemies.Count)].gameObject;
        }
        shooting = true;
        StartCoroutine("GetNewFiringPos");
        
    }

    void Shoot() {
        transform.LookAt(shootTarget.transform);
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
