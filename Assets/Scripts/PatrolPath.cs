using UnityEngine;

[ExecuteInEditMode]
public class PatrolPath : MonoBehaviour {
    [field: SerializeField] public Color PointColor { get; private set; } = Color.green;
    [field: SerializeField] public float PointSize { get; private set; } = 1.0f;
    [field: SerializeField] public Color PathColor { get; private set; } = Color.yellow;
    [field: SerializeField] public Vector3 Offset { get; private set; } = Vector3.zero;
    [SerializeField] bool drawGizmos = true;

    public Transform CurrentPoint { get; private set; }

    Transform cachedTransform;
    Transform currentPoint;
    Transform lastPoint;
    int currentIndex = -1;
    int currentDirection = 1;

    void Awake() {
        cachedTransform = transform;
        CurrentPoint = cachedTransform.GetChild(0);
    }

    void OnDrawGizmos() {
        if(!drawGizmos) { return; }

        int childCount = cachedTransform.childCount;
        lastPoint = cachedTransform.GetChild(childCount - 1);

        for(int i = 0; i < childCount; i++) {
            currentPoint = cachedTransform.GetChild(i);

            Gizmos.color = CurrentPointColor(i);
            Gizmos.DrawSphere(OffsetPosition(currentPoint.position), PointSize);

            Gizmos.color = PathColor;
            Gizmos.DrawLine(OffsetPosition(currentPoint.position), OffsetPosition(lastPoint.position));

            lastPoint = currentPoint;
        }
    }

    Vector3 OffsetPosition(Vector3 position) => position + Offset;

    Color CurrentPointColor(int index) {
        if(index == 0) { return Color.green; }
        if(index == cachedTransform.childCount - 1) { return Color.red; }
        return PointColor;
    }

    public Transform NextPosition() {
        if(currentIndex <= 0) { currentDirection = 1; }
        if(currentIndex == cachedTransform.childCount - 1) { currentDirection = -1; }
        currentIndex += currentDirection;
        CurrentPoint = cachedTransform.GetChild(currentIndex);
        return CurrentPoint;
    }
}
