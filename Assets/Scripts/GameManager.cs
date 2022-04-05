using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] KeyCode pauseToggleKey = KeyCode.Escape;
    [field: SerializeField]
    public StateMachine GameState { get; private set; } = new StateMachine();
    IMonoState pausedState = new PauseState("Paused");
    IMonoState unpausedState = new UnpausedState("Unpaused");

    private void Start() {
        GameState.AddState(pausedState);
        GameState.AddState(unpausedState);
        GameState.ChangeState(unpausedState);
    }

    private void Update() {
        if(Input.GetKeyDown(pauseToggleKey)) {
            var toState = (GameState.CurrentState == pausedState) ? unpausedState : pausedState;
            GameState.ChangeState(toState);
        }
    }
}
