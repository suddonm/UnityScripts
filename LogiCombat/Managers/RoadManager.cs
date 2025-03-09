using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public static RoadManager Instance { get; private set; }

    private List<RoadNode> roadNodes = new List<RoadNode>();

    private void Awake()
    {
        Instance = this;
        roadNodes.AddRange(FindObjectsOfType<RoadNode>());
    }

    public void Start()
    {
        
    }

    public List<RoadNode> GetAllRoadNodes()
    {
        return roadNodes;
    }

    public RoadNode GetClosestNode(Vector3 position)
    {
        RoadNode closestNode = null;
        float closestDistance = float.MaxValue;
        foreach (RoadNode node in roadNodes)
        {
            float distance = Vector3.Distance(node.transform.position, position);
            if (distance < closestDistance)
            {
                closestNode = node;
                closestDistance = distance;
            }
        }

        return closestNode;
    }

    public List<RoadNode> BuildPathToDestination(RoadNode startNode, RoadNode destinationNode)
    {
        List<RoadNode> path = new List<RoadNode>();
        RoadNode currentNode = startNode;
        while (currentNode != destinationNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Neighbors[0];
        }
        path.Add(destinationNode);
        return path;
    }
}