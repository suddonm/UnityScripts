using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.Tilemaps;

public class HexGridPosition : MonoBehaviour
{
    public PositionState DefaultState { get; set; } = PositionState.Available;

    public PositionState CurrentState { get; set; } = PositionState.Available;

    public enum PositionState
    {
        Impassable = -1,
        Available = 0,        
        Occupied = 1,
        Any = 3
    }

    public void Hide()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    public void Show()
    {
        GetComponent<MeshRenderer>().enabled = true;
    }
}