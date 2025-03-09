using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class WorkSequence : SequenceNode
    {
        public WorkSequence Init(NPCController npcController)
        {
            _children = new List<Node>
            {
                ScriptableObject.CreateInstance<MoveNode>().Init(npcController.WorkplaceLocation.position, npcController.NavMeshAgent),
                ScriptableObject.CreateInstance<WaitNode>().Init(npcController.Workplace.workStart, npcController.Workplace.workEnd)
            };

            return this;
        }
    }
}