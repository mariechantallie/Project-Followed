using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Events/DetectionChangeEvent")]
public class DetectionChangeEvent : ScriptableObject {
    public event Action<DetectionChangeEventData> OnDetectionChange;

    public void Raise(DetectionChangeEventData eventData) {
        OnDetectionChange?.Invoke(eventData);
    }
}
