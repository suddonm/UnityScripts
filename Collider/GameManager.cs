using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;
using static UnitController;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton instance
    public UIManager UIManager;

    public InfoPanelController InfoPanelController;

    public SpawnController SpawnController;

    public Camera MainCamera { get; private set; }

    public List<SquadController> player1Squads;
    public List<SquadController> player2Squads;

    private int currentPlayer = 1; // 1 for Player 1, 2 for Player 2

    public SquadController SelectedSquad;    

    private bool isGamePaused = false;
    private bool isGameOver = false;

    public HexGridController HexGridController;

    public GameState gameState;

    public enum GameState
    {
        Idle = 0,
        Moving = 1,
        Attacking = 2,
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy extra instances if one already exists
        }
    }

    void Start()
    {
        MainCamera = Camera.main;

        player1Squads = new List<SquadController>();
        player2Squads = new List<SquadController>();

        //start the game, set the move order to player 1
        currentPlayer = 1;

        //TODO: Add new code for spawning
        SpawnController = GetComponent<SpawnController>();
        HexGridPosition pos = HexGridController.FindClosestAvailablePosition(Vector3.zero);
        SquadController squad1 = SpawnController.SpawnSquad("Squad 1", pos);
        pos.CurrentState = HexGridPosition.PositionState.Occupied;
        player1Squads.Add(squad1);

        // Start Player 1's turn
        StartTurn();
    }

    void DeselectAll()
    {
        SetIdleMode();

        if (SelectedSquad != null)
        {
            SelectedSquad.Deselect();
        }
    }

    void Update()
    {
        //TODO:deselect unit if nothing is hit
        if (Input.GetMouseButtonDown(1))
        {
            DeselectAll();
        }

        // Left click to select units
        if (Input.GetMouseButtonDown(0))
        {
            if (SelectedSquad != null)
            {
                switch (gameState)
                {
                    case GameState.Idle:
                        break;
                    case GameState.Moving:
                        if (Input.GetMouseButtonDown(0))
                        {
                            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
                            RaycastHit hit;

                            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
                            {
                                SelectedSquad.Move(hit.point); // Move unit to clicked location
                            }
                        }
                        break;
                    case GameState.Attacking:
                        if (Input.GetMouseButtonDown(0))
                        {
                            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
                            RaycastHit hit;

                            if (Physics.Raycast(ray, out hit))
                            {
                                SquadController squad = hit.collider.GetComponent<SquadController>();

                                if (squad != null)
                                {
                                    SelectedSquad.Attack(squad);
                                }
                            }
                        }
                        break;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        var target = hit.collider.GetComponent<ISelectable>();

                        if (target != null)
                        {
                            target.Select();
                        }
                    }
                }
            }
        }
    }

    //public void DeselectUnit()
    //{
    //    if (SelectedUnit != null)
    //    {
    //        SelectedUnit.Deselect();
    //        SelectedUnit = null;
    //        UIManager.HideUnitActions();
    //    }
    //}

    private void StartTurn()
    {
        // Check if game is over
        if (isGameOver)
        {
            Debug.Log("Game Over!");
            return;
        }

        Debug.Log("Player " + currentPlayer + "'s turn!");
        InfoPanelController.TMP_PlayerTurn.text = $"Player Turn: {currentPlayer}";

        // Set up the units for the current player
        if (currentPlayer == 1)
        {            
            EnablePlayerSquads(player1Squads, true);
            EnablePlayerSquads(player2Squads, false);            
        }
        else
        {
            EnablePlayerSquads(player1Squads, false);
            EnablePlayerSquads(player2Squads, true);

            //TODO: AI controller
            //StartCoroutine(TakeAITurn());
            TakeAITurn();
        }

        // Select the first unit to act
        //SelectNextUnit();
    }

    public void SetMoveMode()
    {
        if (SelectedSquad != null)
        {
            SelectedSquad.SetMoveMode();
            gameState = GameState.Moving;
        }
    }

    public void SetAttackMode()
    {
        if (SelectedSquad != null)
        {
            SelectedSquad.SetAttackMode();
            gameState = GameState.Attacking;
        }   
    }

    public void SetIdleMode()
    {
        if (SelectedSquad != null)
        {
            SelectedSquad.SetIdleMode();
            gameState = GameState.Idle;
        }
    }

    void TakeAITurn()
    {
        Debug.Log("AI Turn");

        //UnitController unit = player2Units[0];

        //if (unit != null)
        //{
            
        //    unit.Move(new Vector3(unit.transform.position.x + Random.Range((unit.moveRange * -1) / 2, unit.moveRange / 2), 
        //                          unit.transform.position.y + Random.Range((unit.moveRange * -1) / 2, unit.moveRange / 2), 
        //                          unit.transform.position.z));            
        //}

        Debug.Log("End of AI Turn");
        EndTurn();
    }

    public void EndTurn()
    {
        // End current player's turn
        Debug.Log("Player " + currentPlayer + " has ended their turn.");

        ResetPlayerSquads(player1Squads);
        ResetPlayerSquads(player2Squads);

        // Switch players
        currentPlayer = (currentPlayer == 1) ? 2 : 1;

        // Start the next player's turn
        StartTurn();
    }

    public void OnUnitActionComplete()
    {
        // When the current unit finishes its action, move to the next unit
        //currentUnitIndex++;
        //SelectNextUnit();
    }

    private void EnablePlayerSquads(List<SquadController> squads, bool enable)
    {
        foreach (SquadController squad in squads)
        {
            squad.enabled = enable;

            squad.CalculateMoveArea();
        }
    }

    private void ResetPlayerSquads(List<SquadController> squads)
    {
        foreach (SquadController squad in squads)
        {
            squad.ResetTurn();
        }
    }

    public void CheckForGameOver()
    {
        //// Check if all units from either side are destroyed
        //if (AreAllUnitsDead(player1Units))
        //{
        //    isGameOver = true;
        //    Debug.Log("Player 2 wins!");
        //}
        //else if (AreAllUnitsDead(player2Units))
        //{
        //    isGameOver = true;
        //    Debug.Log("Player 1 wins!");
        //}
    }

    private bool AreAllUnitsDead(UnitController[] units)
    {
        foreach (UnitController unit in units)
        {
            if (unit.IsAlive())
            {
                return false;
            }
        }
        return true;
    }

    public int GetCurrentPlayer()
    {
        return currentPlayer;
    }
}
