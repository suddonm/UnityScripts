using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class NPCTree : BehaviorTree
    {
        protected NPCController _npcController;

        public NPCTree Init(NPCController npcController)
        {
            _npcController = npcController;
            RootNode = ScriptableObject.CreateInstance<SelectorNode>().Init(new List<Node>()
            {
                ScriptableObject.CreateInstance<WorkSequence>().Init(npcController)
            });
            return this;
        }
    }
}

