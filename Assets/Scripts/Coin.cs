using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Settings")]
    public int value = 1;

    [Header("Animation")]
    public float bobHeight = 0.15f;
    public float bobSpeed = 2f;

    private Vector3 startPos;
    private bool collected = false;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * bobSpeed) * bobHeight;

        transform.position = startPos + new Vector3(0f, newY, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;

            GameManager.Instance?.CollectCoin(value);

            Destroy(gameObject);
        }
    }
}