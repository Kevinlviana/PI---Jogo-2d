using UnityEngine;

// ?????????????????????????????????????????????????????????????????????????????
// MemoryFragment.cs  — Lost Roots v2  (2ª Entrega)
// Substitui Coin.cs — bob animation, glow pulse, texto de memória, cor do mundo
// Defina uma frase única no campo memoryText de cada instância no Inspector
// ?????????????????????????????????????????????????????????????????????????????

public class MemoryFragment : MonoBehaviour
{
    public int value = 1;

    [TextArea(2, 4)]
    public string memoryText = "Uma lembrança esquecida retorna...";

    public float bobHeight = 0.12f;
    public float bobSpeed = 2.2f;

    public Color baseColor = new Color(1f, 0.85f, 0.2f, 1f);
    public Color glowColor = new Color(1f, 1f, 0.6f, 1f);
    public float pulseSpeed = 2f;

    private Vector3 startPos;
    private SpriteRenderer sr;

    void Start()
    {
        startPos = transform.position;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);

        if (sr != null)
        {
            float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;
            sr.color = Color.Lerp(baseColor, glowColor, t);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager.Instance?.CollectCoin(value);
        UIManager.Instance?.ShowMemoryText(memoryText);
        WorldColorManager.Instance?.OnFragmentCollected();

        Destroy(gameObject);
    }
}