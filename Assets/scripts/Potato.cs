using UnityEngine;

public class Potato : Character
{
    public bool isSpawning = true;
    public bool isCarrot = false;

    protected override void Move()
    {
        if (isSpawning)
            return;

        animator.SetBool("walking", true);
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    protected override void CheckForCharacters()
    {
        if (isSpawning)
            return;

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        float colliderOffset = collider.size.x / 2 + 0.1f;

        Vector2 rayStart = transform.position + (transform.right * colliderOffset) - (transform.up * collider.size.y * 0.4f); // Adjust colliderOffset as needed
        Debug.DrawLine(rayStart, rayStart + Vector2.right * 0.1f, Color.red, 1.0f);

        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.right, 0.1f);
        isAttacking = false; // Reset attacking status

        if (hit.collider != null && hit.collider.gameObject != gameObject) // Check it's not hitting itself
        {
            if (hit.collider.gameObject.CompareTag("Batman"))
            {
                Batman batman = hit.collider.gameObject.GetComponent<Batman>();
                if (batman.isGhost) {
                    if (!isCarrot) {
                        animator.SetBool("attacking", false);
                        animator.SetBool("walking", true);
                        Move();
                        return;
                    }
                }
                isAttacking = true;
                animator.SetBool("walking", false);
                animator.SetBool("attacking", true);
                batman.TakeDamage(damage * Time.deltaTime);
            }
            else if (hit.collider.gameObject.CompareTag("Potato"))
            {
                animator.SetBool("walking", false);
                rb.velocity = Vector2.zero;
            }
        }
        else
        {
            animator.SetBool("attacking", false);
            animator.SetBool("walking", true);
            Move();
        }
    }


    // When 2d colliding
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isSpawning = false;
        }
    }


    public override void Update()
    {
        base.Update();  // Call to base class Update to handle moving and checking if not attacking

        // Additional logic to handle spawning state
        if (isSpawning)
        {
            // Add logic to transition out of spawning state if needed
            // For example, after an animation finishes or a timer runs out
        }
    }

    public override void TakeDamage(float amount)
    {
        if (isSpawning)
            return;

        base.TakeDamage(amount);
    }
}
