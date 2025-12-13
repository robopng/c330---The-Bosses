using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 50; // change when needed
    int currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // TODO: Play animation here!
        Destroy(gameObject); // not immediately?
    }

}
