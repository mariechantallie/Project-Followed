using System;

public abstract class State : IState {
    public string Name { get; private set; }

    public event Action OnEnter;
    public event Action OnExit;

    public State(string name) {
        Name = name;
    }

    public void Enter() => OnEnter?.Invoke();
    public void Exit() => OnExit?.Invoke();
}
