using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Visual")]
    public SpriteRenderer flagRenderer;
    public Sprite activeSprite;   
    public Sprite inactiveSprite; 
    private bool isActive = false;

    void Start()
    {
        if (flagRenderer != null && inactiveSprite != null)
            flagRenderer.sprite = inactiveSprite;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (isActive) return;

        isActive = true;

        other.GetComponent<PlayerController>()?.SaveCheckpoint(transform.position);

        if (flagRenderer != null && activeSprite != null)
            flagRenderer.sprite = activeSprite;

        UIManager.Instance?.ShowMessage("Checkpoint!", 1.5f);

    }
}