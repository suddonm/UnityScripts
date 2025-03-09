using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadNode : MonoBehaviour
{
    public enum Type
    {
        Start,
        Road,
        Depot,
        Checkpoint
    }

    public Type type = Type.Road;
    public List<RoadNode> Neighbors;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.5f);

        foreach (RoadNode neighbor in Neighbors)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, neighbor.transform.position);
        }
    }
}
