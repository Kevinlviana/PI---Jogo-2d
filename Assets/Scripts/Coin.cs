using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Settings")]
    public int value = 1;

    [Header("Animation")]
    public float bobHeight = 0.15f;
    public float bobSpeed = 2f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance?.CollectCoin(value);
            Destroy(gameObject);
        }
    }
}
