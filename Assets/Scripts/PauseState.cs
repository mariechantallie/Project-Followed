using UnityEngine;

public class PauseState : MonoState {
    public PauseState(string name) : base(name) { }

    public override void Enter() {
        base.Enter();
        Time.timeScale = 0f;
    }
}
