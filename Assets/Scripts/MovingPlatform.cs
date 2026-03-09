using UnityEngine;

// MovingPlatform.cs
// Attach to: any Platform sprite you want to oscillate
// Bonus feature: player is parented to the platform so they ride it smoothly

public class MovingPlatform : MonoBehaviour
{
    [Header("Settings")]
    public bool moveHorizontal = true;  // false = moves vertically
    public float moveDistance = 3f;
    public float moveSpeed = 2f;

    private Vector3 startPos;
    private Vector3 endPos;
    private bool movingToEnd = true;

    void Start()
    {
        startPos = transform.position;
        endPos = moveHorizontal
            ? startPos + Vector3.right * moveDistance
            : startPos + Vector3.up   * moveDistance;
    }

    void Update()
    {
        Vector3 target = movingToEnd ? endPos : startPos;
        transform.position = Vector3.MoveTowards(
            transform.position, target, moveSpeed * Time.deltaTime
        );
        if (Vector3.Distance(transform.position, target) < 0.02f)
            movingToEnd = !movingToEnd;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            other.transform.SetParent(transform);
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            other.transform.SetParent(null);
    }
}
