using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class UnitController : MonoBehaviour
{
    // Unit stats
    public int health;
    public int maxHealth;

    public int defense;
        
    public int initiative;  // For determining turn order if needed

    // Boolean for whether unit can take a bonus action
    public bool hasBonusAction;

    // Events or turn tracking
    public bool hasMoved;
    public bool hasAttacked;
    public bool hasBonusActionAvailable;

    public UnitState state;

    private bool isSelected = false; // To track if the unit is selected

    private Outline unit_outline;

    public SquadController squad;

    [SerializeField]
    public Weapon weapon;

    [SerializeField]
    public Armor armor;



    public enum UnitState
    {
        Idle = 0,
        Attacking = 1,
        Moving = 2
    }

    void Awake()
    {
        unit_outline = gameObject.AddComponent<Outline>();

        unit_outline.OutlineMode = Outline.Mode.OutlineHidden;
        unit_outline.OutlineColor = Color.green;
        unit_outline.OutlineWidth = 2f;
    }

    // Unity method called when the game starts
    public void Start()
    {
        health = maxHealth;
        hasMoved = false;
        hasAttacked = false;
        hasBonusActionAvailable = hasBonusAction;
    }

    #region UnitMode

    public void SetAttackMode()
    {
        if (hasAttacked)
        {
            Debug.Log($"State: {gameObject.name} - Already Attacked");
        }

        state = UnitState.Attacking;

        Debug.Log($"State: {gameObject.name} - Attacking");
    }

    public void SetMoveMode()
    {        
        if (hasMoved)
        {            
            Debug.Log($"State: {gameObject.name} - Already Moved");
            
            return;
        }

        state = UnitState.Moving;

        Debug.Log($"State: {gameObject.name} - Moving");
    }

    public void SetIdleMode()
    {
        state = UnitState.Idle;
        Debug.Log($"State: {gameObject.name} - Idle");
    }

    #endregion

    public void ShowOutline()
    {
        unit_outline.OutlineMode = Outline.Mode.OutlineAll;
    }

    public void HideOutline()
    {
        unit_outline.OutlineMode = Outline.Mode.OutlineHidden;
    }

    // Method to select or deselect the unit
    //public ISelectable Select()
    //{
    //    squad.Select();

    //    return this;
    //    // Optional: Change unit appearance to indicate selection, e.g. highlight
    //}

    //public void Deselect()
    //{
    //    squad.Deselect();
    //}

    // Basic movement function
    public virtual void Move(Vector3 targetPosition)
    {
        //if (hasMoved)
        //{
        //    Debug.Log($"{gameObject.name} has already moved this turn.");
        //    return;
        //}

        //Vector3Int cellPosition = GameManager.Instance.gridLayout.WorldToCell(targetPosition);

        //if (MoveAreaSpaces.Contains(cellPosition))
        //{
        //    transform.position = GameManager.Instance.tilemap.GetCellCenterWorld(cellPosition);
        //    hasMoved = true;
        //    state = UnitState.Idle;
        //    Debug.Log($"{gameObject.name} moved to {cellPosition}");
        //    HideMoveRange();
        //}
        //else
        //{
        //    Debug.Log("Target is out of range.");
        //}
    }

    // Basic attack function
    public virtual void Attack(UnitController target)
    {
        //Roll to hit (similar to D&D style)
        int attackRoll = UnityEngine.Random.Range(1, 21);  // D20 roll
        if (attackRoll + weapon.weapon_damage >= (target.defense + target.armor.armor_defense))
        {
            //Apply damage
            target.TakeDamage(weapon.weapon_damage);
            Debug.Log($"{gameObject.name} attacked {target.gameObject.name} for {weapon.weapon_damage} damage.");

        }
        else
        {
            Debug.Log($"{gameObject.name} missed the attack on {target.gameObject.name}.");
        }
    }

    // Function to handle taking damage
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    // Death function
    public virtual void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        // Handle unit death (e.g., remove from the game, play animation, etc.)
        Destroy(gameObject);
    }

    // Function to handle bonus actions
    public virtual void BonusAction()
    {
        if (!hasBonusActionAvailable)
        {
            Debug.Log($"{gameObject.name} cannot take a bonus action.");
            return;
        }

        Debug.Log($"{gameObject.name} used its bonus action.");
        // Implement bonus action effects
        hasBonusActionAvailable = false;
    }

    // Reset actions at the start of each turn
    public virtual void ResetTurn()
    {
        hasMoved = false;
        hasAttacked = false;
        hasBonusActionAvailable = hasBonusAction;

        state = UnitState.Idle;
    }

    public void OnActionComplete()
    {
        GameManager.Instance.OnUnitActionComplete(); // Notify GameController that this unit has completed its action
    }

    public bool IsAlive()
    {
        // Return whether the unit is alive (e.g., has health remaining)
        return health > 0;
    }

    public void PerformAction()
    {
        // Example of a unit performing an action (move, attack, etc.)
        // Once done, call OnActionComplete
        OnActionComplete();
    }




}