using System;
using UnityEngine;

[Serializable]
public abstract class MonoState : IMonoState {
    [field: SerializeField]
    public string Name { get; private set; }

    protected MonoState(string name) { Name = name; }

    public event Action OnEnter;
    public event Action OnExit;
    public event Action OnUpdate;
    public event Action OnLateUpdate;
    public event Action OnFixedUpdate;

    public virtual void Enter() => OnEnter?.Invoke();
    public virtual void Exit() => OnExit?.Invoke();
    public virtual void FixedUpdate() => OnFixedUpdate?.Invoke();
    public virtual void LateUpdate() => OnLateUpdate?.Invoke();
    public virtual void Update() => OnFixedUpdate?.Invoke();
}
