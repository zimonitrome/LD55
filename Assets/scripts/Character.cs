using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public float health = 3f;
    public float speed = 1f;
    public float damage = 1f;
    public GameObject healthBar;
    public float maxHealth;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected bool isAttacking = false;

    protected abstract void Move();
    protected abstract void CheckForCharacters();

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        maxHealth = health;
    }

    public virtual void Update()
    {
        if (!isAttacking)
        {
            Move();
        }
        CheckForCharacters();
    }

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        healthBar.transform.localScale = new Vector3(health / maxHealth, 1, 1);
        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
