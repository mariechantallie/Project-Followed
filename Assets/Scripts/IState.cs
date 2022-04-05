using System;

public interface IState {
    public string Name { get; }

    public event Action OnEnter;
    public event Action OnExit;

    void Enter();
    void Exit();
}
