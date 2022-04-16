using System;

public interface IMonoState : IState {
    public event Action OnUpdate;
    public event Action OnLateUpdate;
    public event Action OnFixedUpdate;

    void Update();
    void LateUpdate();
    void FixedUpdate();
}
