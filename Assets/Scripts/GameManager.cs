using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;

        Coins = 0; 

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);

        UIManager.Instance?.UpdateCoins(Coins);
    }

    public void CollectCoin(int amount)
    {
        Coins += amount;

        if (Coins < 0) Coins = 0;

        UIManager.Instance?.UpdateCoins(Coins);
    }

    public void EnemyKilled()
    {
        UIManager.Instance?.ShowMessage("Enemy defeated!");
    }

    public void GameOver()
    {
        if (IsGameOver) return;

        IsGameOver = true;
        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void Win()
    {
        Time.timeScale = 0f;

        if (winPanel != null)
            winPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        IsGameOver = false;

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