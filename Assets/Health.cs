using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 10f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        // Notify the health bar about the change
        if (OnHealthChanged != null)
            OnHealthChanged(currentHealth / maxHealth);
    }

    public delegate void HealthChange(float healthPercentage);
    public event HealthChange OnHealthChanged;
}
