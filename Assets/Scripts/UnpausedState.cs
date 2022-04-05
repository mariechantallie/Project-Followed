using UnityEngine;

public class UnpausedState : MonoState {
    public UnpausedState(string name) : base(name) { }

    public override void Enter() {
        base.Enter();
        Time.timeScale = 1.0f;
    }
}
