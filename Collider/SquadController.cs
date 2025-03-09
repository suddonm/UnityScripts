using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
using static UnitController;

public class SquadController : MonoBehaviour, ISelectable, IMoveable
{
    public List<UnitController> units = new List<UnitController>();

    private Camera mainCamera;

    private bool isSelected = false;

    private List<HexGridPosition> MoveAreaPositions;
    private List<HexGridPosition> AttackAreaPositions;

    private List<Vector3Int> SquadAreaSpaces;

    // Events or turn tracking
    public bool hasMoved;
    public bool hasAttacked;
    public bool hasBonusActionAvailable;
    public int moveRange;
    public int attackRange;

    public int squadArea;

    public enum SquadState
    {
        Idle = 0,
        Attacking = 1,
        Moving = 2
    }

    public SquadState state;

    public HexGridPosition Position { get; internal set; }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        MoveAreaPositions = new List<HexGridPosition>();
        AttackAreaPositions = new List<HexGridPosition>();

        if (Position == null)
        {
            Position = GameManager.Instance.HexGridController.FindClosestPosition(this.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Mode

    public void SetAttackMode()
    {
        if (hasAttacked)
        {
            Debug.Log($"State: {gameObject.name} - Already Attacked");
        }

        CalculateAttackArea();
        DisplayAttackRange();
        state = SquadState.Attacking;

        Debug.Log($"State: {gameObject.name} - Attacking");
    }

    public void SetMoveMode()
    {
        if (hasMoved)
        {
            Debug.Log($"State: {gameObject.name} - Already Moved");

            return;
        }

        CalculateMoveArea();
        DisplayMoveRange();
        state = SquadState.Moving;

        Debug.Log($"State: {gameObject.name} - Moving");
    }

    public void SetIdleMode()
    {
        state = SquadState.Idle;

        HideMoveRange();
        HideAttackRange();

        Debug.Log($"State: {gameObject.name} - Idle");
    }

    #endregion

    public void DisplayMoveRange()
    {
        foreach (var pos in MoveAreaPositions)
        {
            pos.Show();
        }
    }

    // Method to hide the move range when the player is done moving
    public void HideMoveRange()
    {
        foreach (var pos in MoveAreaPositions)
        {
            pos.Hide();
        }
    }

    public void DisplayAttackRange()
    {
        foreach (var pos in AttackAreaPositions)
        {
            pos.Show();
        }
    }

    public void HideAttackRange()
    {
        foreach (var pos in AttackAreaPositions)
        {
            pos.Hide();
        }
    }

    public virtual void Move(Vector3 targetPosition)
    {
        if (hasMoved)
        {
            Debug.Log($"{gameObject.name} has already moved this turn.");
            return;
        }

        HexGridPosition pos = GameManager.Instance.HexGridController.FindClosestPosition(targetPosition);
        if (MoveAreaPositions.Contains(pos))
        {
            //set old position available
            HexGridPosition oldPos = GameManager.Instance.HexGridController.FindClosestPosition(transform.position);
            oldPos.CurrentState = oldPos.DefaultState;

            transform.position = pos.transform.position;
            hasMoved = true;

            SetIdleMode();

            //set new position occupied
            pos.CurrentState = HexGridPosition.PositionState.Occupied;

            Debug.Log($"{gameObject.name} moved to {pos.name}");

            HideMoveRange();
        }
        else
        {
            Debug.Log("Target is out of range.");
        }
    }

    public void Attack(SquadController target)
    {
        if (hasAttacked)
        {
            Debug.Log($"{gameObject.name} has already attacked this turn.");
            return;
        }

        if (AttackAreaPositions.Contains(target.Position))
        {
            foreach (var unit in units)
            {
                if (target.units.Count > 0)
                {
                    int targetUnit = UnityEngine.Random.Range(0, target.units.Count - 1);

                    unit.Attack(target.units[targetUnit]);
                }
                else
                {
                    Debug.Log("Target has no units to attack");
                }
            }

            hasAttacked = true;
            SetIdleMode();            
        }
        else
        {
            Debug.Log($"{target.gameObject.name} is out of range");
        }
    }

    private void SetPosition(HexGridPosition pos)
    {
        Position = pos;
    }

    // Method to select or deselect the squad
    public ISelectable Select()
    {
        isSelected = true;
        
        Debug.Log($"Selected: {gameObject.name}");

        mainCamera.GetComponent<RTSCameraController>().SetTarget(this.transform);
        GameManager.Instance.SelectedSquad = this;

        ShowOutline();        

        foreach (var unit in units)
        {
            unit.ShowOutline();
        }

        return this;
    }

    public void Deselect()
    {
        isSelected = false;
        GameManager.Instance.SelectedSquad = null;

        SetIdleMode();

        foreach (var unit in units)
        {
            unit.HideOutline();            
        }
    }

    private void ShowOutline()
    {
        //TODO
    }

    private void HideOutline()
    {
        //TODO
    }

    public void CalculateMoveArea()
    {
        MoveAreaPositions = GameManager.Instance.HexGridController.GetAvailablePositionsInRange(this.transform.position, moveRange);
    }

    private void CalculateAttackArea()
    {
        AttackAreaPositions= GameManager.Instance.HexGridController.GetPositionsInRange(this.transform.position, attackRange);
    }

    public void CalculateSquadArea()
    {
        //SquadAreaSpaces = new List<Vector3Int>();

        //Vector3Int gridSpace = GameManager.Instance.gridLayout.WorldToCell(transform.position);

        //for (int i = squadArea * -1; i <= squadArea; i++)
        //{
        //    for (int y = squadArea * -1; y <= squadArea; y++)
        //    {
        //        Vector3Int squadSpace = new Vector3Int(gridSpace.x + i, gridSpace.y + y, gridSpace.z);
        //        if (Vector3Int.Distance(gridSpace, squadSpace) < squadArea)
        //        {
        //            SquadAreaSpaces.Add(squadSpace);
        //        }
        //    }
        //}
        throw new NotImplementedException();
    }

    internal void ResetTurn()
    {
        hasMoved = false;
        hasAttacked = false;
        //hasBonusActionAvailable = true; TODO: Figure this out
        Deselect();

        foreach (var unit in units)
        {
            unit.ResetTurn();
        }
    }
}
