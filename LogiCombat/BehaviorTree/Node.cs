using System;
using UnityEngine;

namespace BehaviorTree
{
    [Serializable]
    public abstract class Node : ScriptableObject
    {
        public enum State
        {
            Success,
            Failure,
            Running
        }

        public State state;

        private State _state = State.Running;

        private bool _started;

        protected abstract void OnStart();

        protected abstract void OnStop();

        protected abstract State OnUpdate();

        public State Update()
        {
            if (!_started)
            {
                OnStart();
                _started = true;
            }

            _state = OnUpdate();

            // if the state is running the state is not failure or not success (in case I decide to add other states latter).
            if (_state != State.Failure && _state != State.Success) return _state;
            OnStop();
            _started = false;
            return _state;
        }
    }
}