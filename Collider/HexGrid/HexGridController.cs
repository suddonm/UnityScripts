using System;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

[RequireComponent(typeof(GridLayout))]
public class HexGridController : MonoBehaviour
{
    private int GRID_HEIGHT = 64;

    public Dictionary<Vector3Int, HexGridPosition> Positions;

    public HexGridPosition PositionPrefab;

    private Tilemap tilemap;
    private GridLayout gridLayout;

    public int DefaultWidth = 100;
    public int DefaultHeight = 100;

    public void Awake()
    {
        Positions = new Dictionary<Vector3Int, HexGridPosition>();
        tilemap = GetComponentInChildren<Tilemap>();
        gridLayout = GetComponent<GridLayout>();

        Positions = ComputeGridPositions(DefaultWidth, DefaultHeight);
        GRID_HEIGHT = (int)this.transform.position.y;
    }

    private Dictionary<Vector3Int, HexGridPosition> ComputeGridPositions(int width, int height)
    {
        Dictionary<Vector3Int, HexGridPosition> dicPositions = new Dictionary<Vector3Int, HexGridPosition>();
        Vector3Int gridSpace = gridLayout.WorldToCell(tilemap.transform.position);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int tempSpace = new Vector3Int(gridSpace.x + x, gridSpace.y + y, gridSpace.z);

                RaycastHit hit;
                LayerMask layerMask = LayerMask.GetMask("Terrain");

                if (Physics.Raycast(tilemap.CellToWorld(tempSpace), tilemap.transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
                {
                    Debug.DrawRay(tilemap.CellToWorld(tempSpace), tilemap.transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);

                    var posPrefab = Instantiate<HexGridPosition>(PositionPrefab);

                    posPrefab.transform.SetParent(tilemap.transform, false);
                    posPrefab.transform.position = hit.point;
                    posPrefab.name = string.Format("PositionCube_{0}_{1}_{2}", tempSpace.x, tempSpace.y, tempSpace.z);

                    var impassableObjects = GameObject.FindGameObjectsWithTag("Impassable");
                    foreach (var obj in impassableObjects)
                    {
                        //TODO: Figure out how to make the impassable box colliders interact
                        if (obj.GetComponent<BoxCollider>().bounds.Intersects(posPrefab.GetComponent<BoxCollider>().bounds))
                        {
                            posPrefab.DefaultState = HexGridPosition.PositionState.Impassable;
                            posPrefab.CurrentState = HexGridPosition.PositionState.Impassable;
                        }
                    }

                    posPrefab.Hide();

                    dicPositions.Add(tempSpace, posPrefab);
                }
            }
        }

        return dicPositions;
    }

    public HexGridPosition FindClosestPosition(Vector3 pos, HexGridPosition.PositionState state = HexGridPosition.PositionState.Available)
    {
        //TODO: put in position state

        Vector3Int cellPos = tilemap.WorldToCell(new Vector3(pos.x, GRID_HEIGHT, pos.z));
        return Positions[cellPos];
    }
    public HexGridPosition FindClosestAvailablePosition(Vector3 pos)
    {
        return FindClosestPosition(pos, HexGridPosition.PositionState.Available);
    }

    public List<HexGridPosition> GetAvailablePositionsInRange(Vector3 pos, int range)
    {
        return GetPositionsInRange(pos, range, HexGridPosition.PositionState.Available);
    }

    public List<HexGridPosition> GetPositionsInRange(Vector3 pos, int range, HexGridPosition.PositionState state = HexGridPosition.PositionState.Any)
    {
        List<HexGridPosition> positions = new List<HexGridPosition>();
        Vector3Int cellPos = tilemap.WorldToCell(new Vector3(pos.x, pos.y, pos.z));

        for (int x = cellPos.x - range; x <= cellPos.x + range; x++)
        {
            for (int y = cellPos.y - range; y <= cellPos.y + range; y++)
            {
                var newPos = new Vector3Int(x, y, 0);
                
                if (Positions.ContainsKey(newPos) &&
                    Vector3Int.Distance(cellPos, newPos) < range)
                {
                    //TODO: find a better way to do position state
                    if (state == HexGridPosition.PositionState.Any ||
                        Positions[newPos].CurrentState == state)
                    {
                        positions.Add(Positions[newPos]);
                    }                    
                }
            }
        }

        return positions;
    }


}