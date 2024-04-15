using UnityEngine;

public class Batman : Character
{
    public int worth = 1;  // Specific to Batman
    public bool isGhost = false;
    protected override void Move()
    {
        animator.SetBool("walking", true);
        rb.velocity = new Vector2(-speed, rb.velocity.y);
    }

    protected override void CheckForCharacters()
    {
        // Get the offset from the collider edge
        float colliderOffset = GetComponent<BoxCollider2D>().size.x / 2 + 0.1f;

        // Start the ray on the left side of the collider
        Vector2 rayStart = transform.position - transform.right * colliderOffset; // Adjust colliderOffset as needed
        Debug.DrawLine(rayStart, rayStart + Vector2.left * 0.1f, Color.red, 1.0f);

        // Cast a ray to the left
        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.left, 0.1f);
        isAttacking = false; // Reset attacking status

        if (hit.collider != null && hit.collider.gameObject != gameObject) // Check it's not hitting itself
        {
            if (hit.collider.gameObject.CompareTag("Potato"))
            {
                Potato potato = hit.collider.gameObject.GetComponent<Potato>();
                if (isGhost) {
                    if (!potato.isCarrot) {
                        animator.SetBool("attacking", false);
                        animator.SetBool("walking", true);
                        Move();
                        return;
                    }
                }
                isAttacking = true;
                animator.SetBool("walking", false);
                animator.SetBool("attacking", true);
                potato.TakeDamage(damage * Time.deltaTime);
            }
            else if (hit.collider.gameObject.CompareTag("Barn"))
            {
                isAttacking = true;
                animator.SetBool("walking", false);
                animator.SetBool("attacking", true);
                hit.collider.gameObject.GetComponent<Barn>().TakeDamage(damage * Time.deltaTime);
            }
            else if (hit.collider.gameObject.CompareTag("Batman"))
            {
                animator.SetBool("walking", false);
                rb.velocity = Vector2.zero;
            }
            else
            {
                animator.SetBool("attacking", false);
                animator.SetBool("walking", true);
                Move();
            }
        }
        else
        {
            animator.SetBool("attacking", false);
            animator.SetBool("walking", true);
            Move();
        }
    }

    public override void Die()
    {
        CoinManager.Instance.AddCoins(worth);
        Spawner spawner = FindObjectOfType<Spawner>();
        spawner.OnEnemyDeath();
        Destroy(gameObject);
    }

}
