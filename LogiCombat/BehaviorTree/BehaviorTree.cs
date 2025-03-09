using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class BehaviorTree : ScriptableObject
    {
        public string TreeName;

        protected Node RootNode;

        private Node.State treeState = Node.State.Running;

        public BehaviorTree() { }

        public BehaviorTree Init()
        {
            return this;
        }

        public Node.State Update()
        {
            treeState = RootNode.Update();

            return treeState;
        }
    }
}