using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;

            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                player.SaveCheckpoint(transform.position);
            }

            Debug.Log("Checkpoint salvo!");

            GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
}