using JetBrains.Annotations;

namespace Util {
    public interface IStateMachineState<T> where T : IStateMachineState<T> {
        void EnterState([CanBeNull] T previousState) { }
        void ExitState(T newState) { }
    }
}
