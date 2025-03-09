using System.Collections.Generic;
using System;

namespace BehaviorTree
{
    public class SelectorNode : Node
    {
        protected List<Node> _children;

        private int _currentChildIndex = 0;

        public SelectorNode Init(List<Node> children)
        {
            _children = children;
            return this;
        }

        protected override void OnStart()
        {
            _currentChildIndex = new Random().Next(0, _children.Count);
        }

        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            return _children[_currentChildIndex].Update() switch
            {
                State.Failure => State.Failure,
                State.Running => State.Running,
                State.Success => State.Success,
                _ => throw new ArgumentOutOfRangeException()
            };
        }           
    }
}