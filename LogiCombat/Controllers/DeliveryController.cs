using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.AI;

public class DeliveryController : MonoBehaviour
{
    public Depot[] depots;
    public int currentDepot = 0;

    public RoadNode Depot;

    // Start is called before the first frame update


    private void OnDrawGizmosSelected()
    {
        //foreach (Depot depot in depots)
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawWireSphere(depot.transform.position, 1f);
        //}

        //foreach (var path in agent.path.corners)
        //{
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawSphere(path, 0.1f);
        //}
    }
}
