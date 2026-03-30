using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float patrolDistance = 5f;

    [Header("Ground Check")]
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private int direction = 1;
    private Vector3 startPos;
    private bool started = false;
    private float flipCooldown = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.freezeRotation = true;
        rb.gravityScale = 3f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void FixedUpdate()
    {
        if (!started)
        {
            startPos = transform.position;
            started = true;
            return;
        }

        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        flipCooldown -= Time.fixedDeltaTime;
        if (flipCooldown > 0f) return;

        Vector2 checkPos = new Vector2(
            transform.position.x + direction * 0.6f,  
            transform.position.y - 0.6f               
        );

       
        bool groundAhead = Physics2D.OverlapBox(checkPos, new Vector2(0.1f, 0.2f), 0f, groundLayer);

        if (!groundAhead)
        {
            Flip();
            return;
        }

        if (Mathf.Abs(transform.position.x - startPos.x) > patrolDistance)
        {
            Flip();
        }
    }

    void Flip()
    {
        direction *= -1;
        sr.flipX = (direction == -1);
        flipCooldown = 0.5f;  
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        bool playerAbove = other.transform.position.y > transform.position.y + 0.3f;

        if (playerAbove)
        {
            Rigidbody2D playerRb = other.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 8f);
            GameManager.Instance?.EnemyKilled();
            Destroy(gameObject);
        }
        else
        {
            other.gameObject.GetComponent<PlayerHealth>()?.TakeHit();
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector2 checkPos = new Vector2(
            transform.position.x + direction * 0.6f,
            transform.position.y - 0.6f
        );
        bool hit = Physics2D.OverlapBox(checkPos, new Vector2(0.1f, 0.2f), 0f, groundLayer);
        Gizmos.color = hit ? Color.green : Color.red;
        Gizmos.DrawWireCube(checkPos, new Vector2(0.1f, 0.2f));

        if (Application.isPlaying && started)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(
                new Vector3(startPos.x - patrolDistance, transform.position.y, 0),
                new Vector3(startPos.x + patrolDistance, transform.position.y, 0)
            );
        }
    }
}