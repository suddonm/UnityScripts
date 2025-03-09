using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public delegate void OnHealthChanged(int currentHealth, int maxHealth);
    public event OnHealthChanged onHealthChanged;

    public delegate void OnDeath();
    public event OnDeath onDeath;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        onHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        onDeath?.Invoke();
        Destroy(gameObject); // Destroy the object upon death (can be replaced with disabling or animations)
    }
}
