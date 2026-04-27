using UnityEngine;


public class GoalFlag : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager gm = GameManager.Instance;
        if (gm == null) return;

        if (gm.Roots >= gm.TotalRoots)
        {
            gm.Win();
        }
        else
        {
            int remaining = gm.TotalRoots - gm.Roots;
            string msg = remaining == 1 ? "Falta 1 Raiz..." : $"Faltam {remaining} Raízes...";
            UIManager.Instance?.ShowMessage(msg, 2f);
        }
    }
}