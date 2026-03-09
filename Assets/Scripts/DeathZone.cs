using UnityEngine;

// DeathZone.cs
// Attach to: a wide invisible trigger collider below the level
// When the player falls off platforms, this kills them instantly

public class DeathZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                // Remove all remaining lives instantly (fell in a pit)
                ph.TakeHit();
                ph.TakeHit();
                ph.TakeHit();
            }
        }
    }
}
