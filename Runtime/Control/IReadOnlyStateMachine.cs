using Util;

public interface IReadOnlyStateMachine<T> where T : IStateMachineState<T> {
    T CurrentState { get; }
}
