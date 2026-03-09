using UnityEngine;
using UnityEngine.SceneManagement;

// GameManager.cs - Attach to empty GameObject "GameManager"
// Singleton that tracks coins, enemies killed, and game state

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int Coins { get; private set; } = 0;
    public int TotalCoins = 10;
    public bool IsGameOver { get; private set; } = false;

    [Header("UI Panels")]
    public GameObject gameOverPanel;
    public GameObject winPanel;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
    }

    public void CollectCoin(int amount)
    {
        Coins += amount;
        UIManager.Instance?.UpdateCoins(Coins);

        if (Coins >= TotalCoins)
            Win();
    }

    public void EnemyKilled()
    {
        UIManager.Instance?.ShowMessage("Stomp!");
    }

    public void GameOver()
    {
        if (IsGameOver) return;
        IsGameOver = true;
        Time.timeScale = 0f;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    public void Win()
    {
        Time.timeScale = 0f;
        if (winPanel != null) winPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
