using System;
using System.Collections.Generic;

namespace EnemyFSM.Core
{
    // Generic FSM. TStateID is usually an enum, TContext is the data bag passed to states.
    public class StateMachine<TStateID, TContext>
    {
        private readonly Dictionary<TStateID, IState<TContext>> _states = new();
        private IState<TContext> _currentState;

        public TStateID CurrentStateID { get; private set; }

        // Fires after every successful transition with the new state ID.
        public event Action<TStateID> OnStateChanged;

        // Register a state once during setup.
        public void RegisterState(TStateID id, IState<TContext> state) =>
            _states[id] = state;

        // Calls Exit on current state then Enter on the next. Silent no-op if ID not registered.
        public void TransitionTo(TStateID id, TContext ctx)
        {
            if (!_states.TryGetValue(id, out var next)) return;

            _currentState?.Exit(ctx);
            CurrentStateID = id;
            _currentState  = next;
            _currentState.Enter(ctx);

            OnStateChanged?.Invoke(id);
        }

        // Call this every Update.
        public void Tick(TContext ctx) => _currentState?.Tick(ctx);
    }
}
