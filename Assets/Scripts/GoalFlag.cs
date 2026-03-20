using UnityEngine;

public class GoalFlag : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance?.Win();
        }
    }
}