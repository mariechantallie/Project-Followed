using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameStateChangeEvent", menuName = "ScriptableObjects/Events/GameStateChangeEvent")]
public class GameStateChangeEvent : ScriptableObject {
    public event Action<StateTransision> OnGameStateChange;

    public void Raise(StateTransision transision) {
        OnGameStateChange?.Invoke(transision);
    }
}
