using System;
using System.Collections.Generic;

namespace BehaviorTree
{
    public class SequenceNode : Node
    {
        protected List<Node> _children;

        private int _currentChildIndex = 0;

        public SequenceNode Init(List<Node> children)
        {
            _children = children;
            return this;
        }
        
        protected override void OnStart()
        {
            _currentChildIndex = 0;
        }

        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            return _children[_currentChildIndex]!.Update() switch
            {                
                State.Failure => State.Failure,
                State.Running => State.Running,
                State.Success => ++_currentChildIndex < _children.Count? State.Running : State.Success,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}