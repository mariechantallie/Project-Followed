// Ganked & Refactored from Sebastian Lague: https://www.youtube.com/watch?v=rQG9aUWarwE

using UnityEngine;

public struct ViewCastInfo {
    public bool Hit { get; private set; }
    public Vector3 Point { get; private set; }
    public float Distance { get; private set; }
    public float Angle { get; private set; }

    public ViewCastInfo(bool hit, Vector3 point, float distance, float angle) {
        Hit = hit;
        Point = point;
        Distance = distance;
        Angle = angle;
    }
}
