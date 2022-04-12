using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Ghoul : MonoBehaviour {
    [SerializeField] Transform target;
    [field: SerializeField] public List<Transform> PatrolPoints { get; private set; } = new List<Transform>();
    NavMeshAgent agent;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        agent.SetDestination(target.position);
    }

    private IEnumerator Patrol() {
        WaitForSeconds delay = new WaitForSeconds(1f);
        while(true) {
            if(Vector3.Distance(agent.transform.position, target.position) < 1f) {
            }
            yield return delay;
        }
    }
}
