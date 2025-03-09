using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<UnitController> allUnits = new List<UnitController>(); // Add all units in the game

    private int currentUnitIndex = 0;

    public void StartTurn()
    {
        if (allUnits.Count == 0)
            return;

        // Reset the current unit’s turn
        allUnits[currentUnitIndex].ResetTurn();
        Debug.Log($"{allUnits[currentUnitIndex].gameObject.name}'s turn started.");
    }

    public void EndTurn()
    {
        // Move to the next unit in the list
        currentUnitIndex = (currentUnitIndex + 1) % allUnits.Count;
        StartTurn();
    }
}
