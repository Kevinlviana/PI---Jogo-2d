using UnityEngine;

public class LuckBox : MonoBehaviour
{
    [Header("Drop Chances (total = 100)")]
    [Range(0, 100)] public int coinChance = 70;
    [Range(0, 100)] public int healthChance = 30;

    [Header("Prefabs")]
    public GameObject coinPrefab;
    public GameObject healthPrefab;

    [Header("Visuals")]
    public Sprite activeSprite;
    public Sprite usedSprite;
    public Color usedColor = new Color(0.5f, 0.5f, 0.5f, 1f);

    [Header("Bounce")]
    public float bounceHeight = 0.3f;
    public float bounceSpeed = 10f;

    private bool isUsed = false;
    private SpriteRenderer sr;

    private Vector3 startPos;
    private bool isBouncing = false;
    private float bounceTimer = 0f;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        startPos = transform.position;

        if (activeSprite != null)
            sr.sprite = activeSprite;
    }

    void Update()
    {
        if (!isBouncing) return;

        bounceTimer += Time.deltaTime * bounceSpeed;

        float yOffset = Mathf.Sin(bounceTimer * Mathf.PI) * bounceHeight;
        transform.position = startPos + Vector3.up * yOffset;

        if (bounceTimer >= 1f)
        {
            transform.position = startPos;
            isBouncing = false;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (isUsed) return;
        if (!other.gameObject.CompareTag("Player")) return;

        foreach (ContactPoint2D contact in other.contacts)
        {
            if (contact.point.y < transform.position.y - 0.2f)
            {
                Activate();
                return;
            }
        }
    }

    void Activate()
    {
        isUsed = true;

        isBouncing = true;
        bounceTimer = 0f;

        if (usedSprite != null)
            sr.sprite = usedSprite;

        sr.color = usedColor;

        SpawnDrop();
    }

    void SpawnDrop()
    {
        Vector3 spawnPos = transform.position + Vector3.up * 1f;

        int roll = Random.Range(0, 100);

        if (roll < coinChance)
        {
            if (coinPrefab != null)
                Instantiate(coinPrefab, spawnPos, Quaternion.identity);

            GameManager.Instance?.CollectCoin(1);
        }
        else
        {
            if (healthPrefab != null)
                Instantiate(healthPrefab, spawnPos, Quaternion.identity);
        }
    }
}