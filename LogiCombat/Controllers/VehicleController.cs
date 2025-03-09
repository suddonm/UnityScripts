using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VehicleController : MonoBehaviour, IRoadFollower
{
    NavMeshAgent agent;
    DeliveryController deliveryController;
    RoadNode currentNode;

    public float NodeRadius = 5f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentNode = FindStartNode();
        agent.SetDestination(currentNode.transform.position);
        agent.isStopped = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance > NodeRadius)
        {
            agent.isStopped = false;
        }

        if (agent.remainingDistance <= NodeRadius && !agent.pathPending)
        {
            if (FindNextNode(currentNode) != null)
            {
                agent.SetDestination(FindNextNode(currentNode).transform.position);
            }
            else
            {
                agent.isStopped = true;
            }
        }
    }

    /// <summary>
    /// Find the closest node to the vehicle's current position
    /// </summary>
    /// <returns></returns>
    public RoadNode FindStartNode()
    {
        return RoadManager.Instance.GetClosestNode(transform.position);
    }

    public RoadNode FindNextNode(RoadNode currentNode)
    {
        if (currentNode.Neighbors.Count == 0)
            return null;

        return currentNode.Neighbors[0];
    }
}