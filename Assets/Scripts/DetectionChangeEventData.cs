using System;
using UnityEngine;

public struct DetectionChangeEventData {
    public GameObject Detector { get; set; }
    public DateTime TimeDetected { get; set; }
    public Vector3 ChangePosition { get; set; }
    public DetectionChange ChangeType { get; set; }
}
