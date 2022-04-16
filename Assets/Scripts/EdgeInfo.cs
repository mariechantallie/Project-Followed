// Ganked & Refactored from Sebastian Lague: https://www.youtube.com/watch?v=rQG9aUWarwE

using UnityEngine;

public struct EdgeInfo {
    public Vector3 PointA { get; private set; }
    public Vector3 PointB { get; private set; }

    public EdgeInfo(Vector3 pointA, Vector3 pointB) {
        PointA = pointA;
        PointB = pointB;
    }
}
