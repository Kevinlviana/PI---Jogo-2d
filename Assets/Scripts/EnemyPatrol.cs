using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyPatrol : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2.5f;
    public float patrolDistance = 4f;

    [Header("Detection")]
    public float groundAheadDist = 0.5f;
    public float wallCheckDist = 0.3f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private int direction = 1;
    private Vector3 startPos;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.freezeRotation = true;
        rb.gravityScale = 3f;
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
        Vector2 edgeCheck = new Vector2(transform.position.x + direction * 0.4f, transform.position.y);
        bool groundAhead = Physics2D.Raycast(edgeCheck, Vector2.down, groundAheadDist + 0.2f, groundLayer);
        bool wallAhead = Physics2D.Raycast(transform.position, new Vector2(direction, 0), wallCheckDist, groundLayer);
        if (!groundAhead || wallAhead) Flip();
        if (Mathf.Abs(transform.position.x - startPos.x) > patrolDistance) Flip();
    }

    void Flip()
    {
        direction *= -1;
        sr.flipX = (direction == -1);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        bool playerAbove = other.transform.position.y > transform.position.y + 0.3f;
        if (playerAbove)
        {
            Rigidbody2D playerRb = other.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null) playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 8f);
            GameManager.Instance?.EnemyKilled();
            Destroy(gameObject);
        }
        else
        {
            PlayerHealth ph = other.gameObject.GetComponent<PlayerHealth>();
            ph?.TakeHit();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 edgeOrigin = new Vector2(transform.position.x + direction * 0.4f, transform.position.y);
        Gizmos.DrawRay(edgeOrigin, Vector2.down * (groundAheadDist + 0.2f));
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, new Vector3(direction, 0) * wallCheckDist);
    }
}