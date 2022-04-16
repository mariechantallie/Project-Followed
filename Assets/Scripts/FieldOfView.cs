// Math part stolen from from Sebastian Lague: https://www.youtube.com/watch?v=rQG9aUWarwE
// Math Make Brain Hurt...

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FieldOfView : MonoBehaviour {
    private const float SearchForTargetDelay = .2f;

    [SerializeField] GameObject observer;
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstacleMask;
    [Range(0, 360)][SerializeField] float viewAngle;
    [field: SerializeField] public float ViewRadius { get; private set; }
    [SerializeField] float meshResolution;
    [SerializeField] int edgeResolveIterations;
    [SerializeField] float edgeDstThreshold;
    [SerializeField] MeshFilter viewMeshFilter;
    [SerializeField] DetectionChangeEvent detectionChangeEvent;
    [field: SerializeField] public bool IsInView { get; private set; } = false;
    [field: SerializeField] public bool IsInRange { get; private set; } = false;

    Mesh viewMesh;
    IEnumerator findTargetsWithDelayCoroutine;
    Transform cachedTransform;
    bool lastInViewStatus = false;

    public float ViewAngle {
        get { return viewAngle; }
        private set { viewAngle = Mathf.Clamp(value, 0.0f, 360.0f); }
    }

    [field: SerializeField]
    public GameObject VisibleTarget { get; private set; } = null;

    #region MonoBehaviour

    void Awake() {
        cachedTransform = transform;
        findTargetsWithDelayCoroutine = FindTargetsWithDelay(SearchForTargetDelay);
    }

    void Start() {
        viewMesh = new Mesh() { name = "View Mesh" };
        viewMeshFilter.mesh = viewMesh;
        observer ??= gameObject;
    }

    void OnEnable() {
        if(findTargetsWithDelayCoroutine != null) {
            StartCoroutine(findTargetsWithDelayCoroutine);
        }
    }

    void OnDisable() {
        if(findTargetsWithDelayCoroutine != null) {
            StopCoroutine(findTargetsWithDelayCoroutine);
        }
    }

    void LateUpdate() => DrawFieldOfView();

    #endregion MonoBehaviour

    IEnumerator FindTargetsWithDelay(float delay) {
        while(true) {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets() {
        Collider[] targetsInViewRadius;
        Transform target = null;
        Vector3 dirToTarget;
        float dstToTarget;

        IsInRange = false;
        IsInView = false;

        targetsInViewRadius = Physics.OverlapSphere(cachedTransform.position, ViewRadius, targetMask);

        if(targetsInViewRadius.Length > 0) {
            target = targetsInViewRadius[0].transform;
            dirToTarget = (target.position - cachedTransform.position).normalized;

            IsInRange = Vector3.Angle(cachedTransform.forward, dirToTarget) < HalfViewAngle;

            if(IsInRange) {
                dstToTarget = Vector3.Distance(cachedTransform.position, target.position);
                IsInView = !Physics.Raycast(cachedTransform.position, dirToTarget, dstToTarget, obstacleMask);
                VisibleTarget = target.gameObject;
            }
        }

        if(IsInView != lastInViewStatus) {
            lastInViewStatus = IsInView;

            var changeData = new DetectionChangeEventData() {
                Detector = observer,
                TimeDetected = DateTime.Now,
                ChangePosition = VisibleTarget.transform.position,
                ChangeType = ((IsInView) ? DetectionChange.Detected : DetectionChange.Lost)
            };
            detectionChangeEvent.Raise(changeData);
        }

        if(!IsInView) { VisibleTarget = null; }
    }

    void DrawFieldOfView() {
        int stepCount = Mathf.RoundToInt(ViewAngle * meshResolution);
        float stepAngleSize = ViewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo newViewCast;
        ViewCastInfo oldViewCast = new ViewCastInfo();
        float angle;
        int vertexCount;
        Vector3[] vertices;
        int[] triangles;
        EdgeInfo edge;

        for(int i = 0; i <= stepCount; i++) {
            angle = cachedTransform.eulerAngles.y - HalfViewAngle + stepAngleSize * i;
            newViewCast = ViewCast(angle);

            if(i > 0) {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.Distance - newViewCast.Distance) > edgeDstThreshold;
                if(oldViewCast.Hit != newViewCast.Hit || (oldViewCast.Hit && newViewCast.Hit && edgeDstThresholdExceeded)) {
                    edge = FindEdge(oldViewCast, newViewCast);
                    if(edge.PointA != Vector3.zero) {
                        viewPoints.Add(edge.PointA);
                    }
                    if(edge.PointB != Vector3.zero) {
                        viewPoints.Add(edge.PointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.Point);
            oldViewCast = newViewCast;
        }

        vertexCount = viewPoints.Count + 1;
        vertices = new Vector3[vertexCount];
        triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for(int i = 0; i < vertexCount - 1; i++) {
            vertices[i + 1] = cachedTransform.InverseTransformPoint(viewPoints[i]);

            if(i < vertexCount - 2) {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();

        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast) {
        float minAngle = minViewCast.Angle;
        float maxAngle = maxViewCast.Angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;
        float angle;
        ViewCastInfo newViewCast;
        bool edgeDstThresholdExceeded;

        for(int i = 0; i < edgeResolveIterations; i++) {
            angle = (minAngle + maxAngle) / 2;
            newViewCast = ViewCast(angle);

            edgeDstThresholdExceeded = Mathf.Abs(minViewCast.Distance - newViewCast.Distance) > edgeDstThreshold;
            if(newViewCast.Hit == minViewCast.Hit && !edgeDstThresholdExceeded) {
                minAngle = angle;
                minPoint = newViewCast.Point;
            } else {
                maxAngle = angle;
                maxPoint = newViewCast.Point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    ViewCastInfo ViewCast(float globalAngle) {
        Vector3 dir = DirectionFromAngle(globalAngle, true);
        RaycastHit hit;

        if(Physics.Raycast(cachedTransform.position, dir, out hit, ViewRadius, obstacleMask)) {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        } else {
            return new ViewCastInfo(false, cachedTransform.position + dir * ViewRadius, ViewRadius, globalAngle);
        }
    }

    public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if(!angleIsGlobal) {
            angleInDegrees += cachedTransform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    float HalfViewAngle => ViewAngle * 0.5f;
}
