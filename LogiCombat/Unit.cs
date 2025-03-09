using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitStats unitStats; // Reference to the ScriptableObject
    private int currentHealth;
    private Weapon equippedWeapon;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (unitStats == null)
        {
            Debug.LogError("UnitStats not assigned!");
            return;
        }

        currentHealth = unitStats.maxHealth;
        equippedWeapon = unitStats.weapon;
        Debug.Log($"{unitStats.unitName} initialized with {currentHealth} HP and weapon {equippedWeapon.weaponName}.");
    }

    public bool TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
            return true;
        }
        return false;
    }

    private void Die()
    {
        Debug.Log($"{unitStats.unitName} has died.");
        Destroy(gameObject);
    }

    public float GetEffectiveAccuracy()
    {
        return unitStats.GetEffectiveAccuracy();
    }
}
