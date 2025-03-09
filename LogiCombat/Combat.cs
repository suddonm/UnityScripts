using UnityEngine;

public class Combat : MonoBehaviour
{
    public int attackDamage = 10; // Damage dealt per attack
    public float attackRange = 2f; // Range of attacks
    public float attackCooldown = 2f; // Time between attacks
    public LayerMask targetLayer; // Define which layers can be attacked

    private float nextAttackTime = 0f;

    private void Update()
    {
        // Automatically attack if cooldown has passed
        if (Time.time >= nextAttackTime)
        {
            AutoAttack();
        }
    }

    private Collider GetNearestTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, targetLayer);
        Collider nearest = null;
        float nearestDistance = float.MaxValue;

        foreach (Collider hit in hits)
        {
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (distance < nearestDistance)
            {
                nearest = hit;
                nearestDistance = distance;
            }
        }

        return nearest;
    }

    private void AutoAttack()
    {
        Collider nearestTarget = GetNearestTarget();
        if (nearestTarget != null)
        {
            Health targetHealth = nearestTarget.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(attackDamage);
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the attack range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
