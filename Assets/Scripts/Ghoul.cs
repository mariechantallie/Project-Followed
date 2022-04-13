using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Ghoul : MonoBehaviour {
    const float DistanceToTargetCheckDelay = 0.1f;

    [SerializeField] Transform target;
    [SerializeField] PatrolPath path;
    [SerializeField] float patrolDelay = 1f;

    public bool IsPatrolling { get; private set; } = true;

    NavMeshAgent agent;
    IEnumerator patrolCoroutine;

    void Awake() {
        agent = GetComponent<NavMeshAgent>();
        patrolCoroutine = Patrol();
    }

    void Start() {
        StartCoroutine(patrolCoroutine);
    }

    bool IsAtPatrolPoint => (!agent.pathPending && !agent.hasPath && IsAgentCloseToTarget);
    bool IsAgentCloseToTarget => agent.remainingDistance <= agent.stoppingDistance;

    private IEnumerator Patrol() {
        var patrolReachedDelay = new WaitForSeconds(patrolDelay);
        var distanceCheckDelay = new WaitForSeconds(DistanceToTargetCheckDelay);

        while(true) {
            if(IsPatrolling) {
                agent.SetDestination(path.CurrentPoint.position);
                if(IsAtPatrolPoint) {
                    yield return patrolReachedDelay;
                    path.NextPosition();
                }
                yield return distanceCheckDelay;
            }
        }
    }
}
