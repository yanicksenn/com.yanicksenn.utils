using System;
using UnityEngine;
using Util;

public class StateMachine<T> where T : IStateMachineState<T> {
    private T _currentState;
    private bool _activeTransition;

    public T CurrentState => _currentState;
    public event Action<T?, T> OnStateChanged;

    public StateMachine(T initialState) {
        Debug.Assert(initialState != null);
        _currentState = initialState;
        initialState.EnterState(default);
    }

    public void SetCurrentState(T newState) {
        Debug.Assert(newState != null);

        if (_activeTransition) {
            Debug.LogError($"State transitions during {nameof(IStateMachineState<T>.EnterState)} or {nameof(IStateMachineState<T>.ExitState)} not permitted");
            return;
        }

        var previousState = _currentState;
        _currentState?.ExitState(newState);
        _currentState = newState;
        newState.EnterState(previousState);

        OnStateChanged?.Invoke(_currentState, newState);
    }
}
