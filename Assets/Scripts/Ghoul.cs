using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Ghoul : MonoBehaviour {
    const float DistanceToTargetCheckDelay = 0.1f;

    [SerializeField] Transform target;
    [SerializeField] Transform head;
    [SerializeField] PatrolPath path;
    [SerializeField] float patrolDelay = 1f;
    [SerializeField] float headSwivelRate = 30f;
    [SerializeField] float headSwivelDegree = 30f;
    [SerializeField] float headSwivelPause = 0.5f;
    [SerializeField] DetectionChangeEvent detectionChangeEvent;
    [field: SerializeField] public StateMachine GhoulState { get; private set; } = new StateMachine();

    public bool IsPatrolling { get; private set; } = true;
    public IMonoState Hunting { get; private set; }
    public IMonoState Patrolling { get; private set; }

    NavMeshAgent agent;
    IEnumerator patrolCoroutine;
    IEnumerator huntCoroutine;
    IEnumerator swivelHeadCoroutine;

    #region MonoBehaviour

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();

        patrolCoroutine = PatrolCoroutine();
        huntCoroutine = HuntCoroutine();
        swivelHeadCoroutine = SwivelHeadCoroutine();

        Hunting = new HuntingState(this);
        Patrolling = new PatrollingState(this);

        GhoulState.AddStates(Hunting, Patrolling);
    }
    private void Start() {
        GhoulState.ChangeState(Patrolling);
    }

    private void Update() {
    }

    private void OnEnable() {
        HandleSubscriptions();
    }

    private void OnDisable() {
        HandleUnSubscriptions();
    }

    #endregion MonoBehaviour

    private bool IsAtPatrolPoint => (!agent.pathPending && !agent.hasPath && IsAgentCloseToTarget);
    private bool IsAgentCloseToTarget => agent.remainingDistance <= agent.stoppingDistance;

    private void AdjustSeeking(DetectionChangeEventData data) {
        if(data.ChangeType == DetectionChange.Detected) {
            GhoulState.ChangeState(Hunting);
        }
        if(data.ChangeType == DetectionChange.Lost) {
            GhoulState.ChangeState(Patrolling);
        }
    }

    public void Patrol() {
        if(huntCoroutine is not null) {
            StopCoroutine(huntCoroutine);
        }
        if(patrolCoroutine is not null) {
            StartCoroutine(patrolCoroutine);
            StartCoroutine(swivelHeadCoroutine);
        }
    }

    public void Hunt() {
        if(patrolCoroutine is not null) {
            StopCoroutine(patrolCoroutine);
            StopCoroutine(swivelHeadCoroutine);
        }
        if(huntCoroutine is not null) {
            StartCoroutine(huntCoroutine);
        }
    }

    private void HandleSubscriptions() {
        detectionChangeEvent.OnDetectionChange += AdjustSeeking;
    }

    private void HandleUnSubscriptions() {
        detectionChangeEvent.OnDetectionChange -= AdjustSeeking;
    }

    private IEnumerator HuntCoroutine() {
        while(true) {
            agent.SetDestination(target.position);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator PatrolCoroutine() {
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

    private IEnumerator SwivelHeadCoroutine() {
        bool isRotatingRight = true;
        var pause = new WaitForSeconds(headSwivelPause);

        while(true) {
            if(isRotatingRight) {
                head.rotation = Quaternion.RotateTowards(head.rotation, HeadSwivelRight, Time.deltaTime * headSwivelRate);
                if(head.rotation == HeadSwivelRight) {
                    isRotatingRight = false;
                    yield return pause;
                }
            } else {
                head.rotation = Quaternion.RotateTowards(head.rotation, HeadSwivelLeft, Time.deltaTime * headSwivelRate);
                if(head.rotation == HeadSwivelLeft) {
                    isRotatingRight = true;
                    yield return pause;
                }
            }
            yield return null;
        }
    }

    private Quaternion HeadSwivelRight => Quaternion.Euler(0f, headSwivelDegree, 0f) * transform.rotation;
    private Quaternion HeadSwivelLeft => Quaternion.Euler(0f, -headSwivelDegree, 0f) * transform.rotation;
}
