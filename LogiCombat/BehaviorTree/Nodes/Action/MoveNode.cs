using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTree
{
    [Serializable]    
    public class MoveNode : ActionNode
    {
        private Vector3 _targetPosition;        
        private NavMeshAgent _navMeshAgent;


        public ActionNode Init(Vector3 targetPosition, NavMeshAgent navMeshAgent)
        {            
            _targetPosition = targetPosition;
            _navMeshAgent = navMeshAgent;

            return this;
        }

        protected override void OnStart()
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_targetPosition);
        }

        protected override void OnStop()
        {
            _navMeshAgent.isStopped = true;
        }

        protected override State OnUpdate()
        {
            if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance >= 0.5f)
            {
                return State.Running;
            }

            return State.Success;
        }
    }
}