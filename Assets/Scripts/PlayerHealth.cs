using UnityEngine;
using System.Collections;

// PlayerHealth.cs
// Attach to: Player GameObject

public class PlayerHealth : MonoBehaviour
{
    [Header("Stats")]
    public int maxLives = 3;
    public float invincibilityDuration = 1.5f;

    private int currentLives;
    private bool isInvincible = false;
    private SpriteRenderer sr;
    private PlayerController controller;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        controller = GetComponent<PlayerController>();
        currentLives = maxLives;
    }

    void Start()
    {
        UIManager.Instance?.UpdateLives(currentLives);
    }

    // Call from EnemyPatrol or spike triggers
    public void TakeHit()
    {
        if (isInvincible) return;

        currentLives--;
        UIManager.Instance?.UpdateLives(currentLives);

        if (currentLives <= 0)
        {
            controller.Die();
        }
        else
        {
            StartCoroutine(InvincibilityFrames());
        }
    }

    // Brief invincibility + blink so player doesnt die instantly on next frame
    IEnumerator InvincibilityFrames()
    {
        isInvincible = true;
        float elapsed = 0f;

        while (elapsed < invincibilityDuration)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        sr.enabled = true;
        isInvincible = false;
    }
}
