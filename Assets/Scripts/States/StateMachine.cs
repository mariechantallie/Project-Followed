using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StateMachine {
    [field: SerializeField]
    public List<IMonoState> States { get; private set; } = new List<IMonoState>();
    [field: SerializeField]
    public IMonoState CurrentState { get; private set; } = null;

    public event Action<StateTransision> OnStateChange;

    public StateTransision ChangeState(IMonoState toState) {
        var result = new StateTransision(CurrentState, toState);
        CurrentState?.Exit();
        CurrentState = toState;
        OnStateChange?.Invoke(result);
        CurrentState.Enter();
        return result;
    }

    public void AddState(IMonoState toAdd) {
        if(!States.Contains(toAdd)) {
            States.Add(toAdd);
        }
    }

    public void AddStates(params IMonoState[] toAdd) {
        foreach(IMonoState state in toAdd) {
            AddState(state);
        }
    }

    public void RemoveState(IMonoState toRemove) {
        States.Remove(toRemove);
    }

    public void RemoveStates(params IMonoState[] toRemove) {
        foreach(IMonoState state in toRemove) {
            while(States.Contains(state)) {
                RemoveState(state);
            }
        }
    }
}
