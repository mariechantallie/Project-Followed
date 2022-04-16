using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] KeyCode pauseToggleKey = KeyCode.Escape;
    [field: SerializeField] public StateMachine GameState { get; private set; } = new StateMachine();
    [field: SerializeField] public GameStateChangeEvent GameStateChange { get; private set; }

    IMonoState pausedState = new PauseState("Paused");
    IMonoState unpausedState = new UnpausedState("Unpaused");

    void Start() {
        GameState.AddState(pausedState);
        GameState.AddState(unpausedState);
        GameState.ChangeState(unpausedState);
    }

    void Update() {
        if(Input.GetKeyDown(pauseToggleKey)) {
            var toState = (GameState.CurrentState == pausedState) ? unpausedState : pausedState;
            var transision = GameState.ChangeState(toState);
            GameStateChange.Raise(transision);
        }
    }
}
