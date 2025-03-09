using System.Collections;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class EnemyAIController: MonoBehaviour
{
    // States for the AI
    public enum AIState
    {
        Idle,
        Patrolling,
        Chasing,
        Attacking,
        Securing,
        Covering,
        Searching // After combat, the AI can search for threats
    }

    public AIState currentState = AIState.Idle;

    public bool IsHostile;

    [Header("Patrol Settings")]
    public PatrolPoint[] patrolPoints; // Array of patrol points
    private int currentPatrolIndex = 0;
    private float stateTimer = 0f;

    private float decisionInterval = 5f; // Time before making a new decision

    [Header("Suspicion Settings")]
    public float suspicionDuration = 5f; // Time to stay in high suspicion state

    [Header("Detection Settings")]
    public float detectionRadius = 10f; // Radius for detecting suspicious activity
    public LayerMask detectionMask; // Layers the guard can detect

    [Header("Combat Settings")]
    public Cover coverPoint; // Optional cover point
    public Transform target; // The player or intruder
    public float attackRange = 20f; // Range for attacking the target
    public float stoppingDistance = 2f; // Distance to stop
    public bool IsInCover;

    public float walkSpeed = 1f;
    public float runSpeed = 2f;

    public float decisionDelay = 0.5f;
    public float decisionTimer = 0f;

    private NavMeshAgent agent;
    private Animator animator;
    private Unit unit;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        unit = GetComponent<Unit>();

        unit.unitStats.weapon.ResetWeapon();
        currentState = AIState.Idle;
        animator.SetFloat("Speed", 0f);
        IsHostile = false;
        IsInCover = false;
    }

    private void Update()
    {
        if (decisionTimer < decisionDelay)
        {
            decisionTimer += Time.deltaTime;
            return;
        }

        switch (currentState)
        {
            case AIState.Idle:
                HandleIdleState();
                break;
            case AIState.Patrolling:
                HandlePatrollingState();
                break;
            case AIState.Chasing:
                HandleChasingState();
                break;
            case AIState.Attacking:
                HandleAttackingState();
                break;
            case AIState.Covering:
                HandleCoveringState();
                break;
            case AIState.Securing:
                HandleSecuringState();
                break;
        }

        decisionTimer = 0f;
        UpdateAnimation();
    }

    private void HandleCoveringState()
    {
        agent.SetDestination(coverPoint.transform.position);
        transform.LookAt(coverPoint.transform);
        agent.isStopped = false;
        agent.speed = runSpeed;
        animator.SetFloat("Speed", runSpeed);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            agent.isStopped = true;
            IsInCover = true;
            animator.SetFloat("Speed", 0f);
            ChangeState(AIState.Attacking);
        }
    }

    private void HandleSecuringState()
    {
        IsHostile = true;

        ChangeState(AIState.Idle);
    }

    private void UpdateAnimation()
    {
        // Update the animator
        animator.SetBool("IsHostile", IsHostile);
    }

    private void ChangeState(AIState newState)
    {
        currentState = newState;
        stateTimer = 0f; // Reset the timer when entering a new state
        Debug.Log($"{this.name} - State changed to: {newState}");
    }

    // Idle State
    private void HandleIdleState()
    {
        IsHostile = false;
        DetectHostileActivity();

        stateTimer += Time.deltaTime;
        if (stateTimer >= decisionInterval)
        {
            MakeIdleDecision();
        }
    }

    private void MakeIdleDecision()
    {
        // 50% chance to patrol, 50% chance to keep waiting
        if (Random.value > 0.5f)
        {
            ChangeState(AIState.Patrolling);
        }
        else
        {
            ChangeState(AIState.Idle);
        }
    }

    // Patrolling State
    private void HandlePatrollingState()
    {
        IsHostile = false;
        IsInCover = false;
        agent.speed = walkSpeed;
        DetectHostileActivity();

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            agent.isStopped = true;
            animator.SetFloat("Speed", 0f);
            MakePatrolDecision();
        }
    }

    private void MakePatrolDecision()
    {
        // 50% chance to keep patrolling, 50% chance to wait
        if (Random.value > 0.5f)
        {
            ChangeState(AIState.Idle);
        }
        else
        {
            // Continue patrolling
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].transform.position);
            agent.isStopped = false;
            agent.speed = walkSpeed;
            animator.SetFloat("Speed", walkSpeed);

            Debug.Log($"Moving to patrol point {currentPatrolIndex}");

            IsInCover = false;
        }
    }

    // Chasing State
    private void HandleChasingState()
    {
        IsHostile = true;

        if (Vector3.Distance(transform.position, target.position) <= attackRange)
        {
            ChangeState(AIState.Attacking);
        }
        else if (Vector3.Distance(transform.position, target.position) > detectionRadius)
        {
            // Lost the player; return to patrol
            ChangeState(AIState.Patrolling);
        }
        else
        {
            agent.SetDestination(target.position);
            transform.LookAt(agent.nextPosition);
            agent.speed = runSpeed;
            agent.isStopped = false;
            animator.SetFloat("Speed", runSpeed);
        }
    }

    // Attacking State
    private void HandleAttackingState()
    {
        IsHostile = true;

        // if there is still a target
        if (target != null)
        {
            if (!IsInCover &&
                coverPoint != null)
            {
                // Find cover
                ChangeState(AIState.Covering);
            }

            else if(Vector3.Distance(transform.position, target.position) > attackRange)
            {
                // if the target moves out of range, chase them
                ChangeState(AIState.Chasing);
            }
            else
            {
                // Rotate to face the player
                agent.SetDestination(this.transform.position);
                agent.isStopped = true;
                agent.speed = 0f;
                animator.SetFloat("Speed", 0f);

                transform.LookAt(target);
                Attack();
            }
        }
        else
        {
            ChangeState(AIState.Securing);
        }
    }

    // Call this when the guard should become hostile
    public void BecomeHostile(Transform newTarget)
    {
        currentState = AIState.Attacking;
        target = newTarget;
    }

    void Attack()
    {
        if (unit.unitStats.weapon.CanFire())
        {
            // Placeholder for attack logic
            Debug.Log($"{this.name} Attacking {target.name}!");

            unit.unitStats.weapon.Fire();
            FireAtTarget();
        }
    }

    void FireAtTarget()
    {
        float accuracy = unit.unitStats.GetEffectiveAccuracy();
        if (Random.value <= accuracy)
        {
            Debug.Log($"{gameObject.name} hit the target!");
            target.GetComponent<Unit>()?.TakeDamage(unit.unitStats.weapon.damage);
        }
        else
        {
            Debug.Log($"{gameObject.name} missed the target!");
        }
    }
    

    // Detect suspicious activity (player or noises)
    private void DetectHostileActivity()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, detectionMask);
        foreach (var hit in hits)
        {
            var character = hit.GetComponent<RTSCharacterController>();
            if (character != null)
            {
                // Character detected
                // Check if the character is hostile
                if (character.IsHostile)
                {
                    target = hit.transform;
                    ChangeState(AIState.Attacking);
                    IsHostile = true;
                    break;
                }
            }
        }

        IsHostile = false;
    }

    #region Gizmos

    /// <summary>
    /// Debug helper: Draw patrol point connections in the editor.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (patrolPoints.Length > 0)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                // Draw connections between patrol points
                Gizmos.DrawLine(patrolPoints[i].transform.position, patrolPoints[(i + 1) % patrolPoints.Length].transform.position);
            }
        }
    }

    #endregion
}
