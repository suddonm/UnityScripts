using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathUtils : MonoBehaviour
{
    

    public static RoadNode FindRoute(RoadNode start, RoadNode end)
    {
        RoadNode current = start;
        RoadNode path = current;

        List<RoadNode> openList = new List<RoadNode>();
        HashSet<RoadNode> closedList = new HashSet<RoadNode>();

        openList.Add(start);        
        while (openList.Count > 0)
        {
            RoadNode tmp = openList.OrderBy(node => FindDistance(node, end)).First();

            current.Neighbors = new List<RoadNode>() { tmp };

            if (tmp == end)
            {                
                break;
            }

            openList.Remove(tmp);
            closedList.Add(tmp);

            foreach (RoadNode node in tmp.Neighbors)
            {
                if (closedList.Contains(node))
                    continue;

                if (!openList.Contains(node))
                {
                    openList.Add(node);
                }
            }
        }

        return path;
    }

    public static float FindDistance(RoadNode a, RoadNode b)
    {
        return Vector3.Distance(a.transform.position, b.transform.position);
    }
}
