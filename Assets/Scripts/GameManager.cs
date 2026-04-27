using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Raízes")]
    public int Roots { get; private set; } = 0;
    public int TotalRoots = 12;

    [Header("Fase")]
    public int currentPhase = 1;   

    public bool IsGameOver { get; private set; } = false;

    [Header("Painéis UI")]
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
        Roots = 0;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);

        UIManager.Instance?.UpdateRoots(Roots, TotalRoots);

    }

    public void CollectRoot(int amount = 1)
    {
        Roots += amount;
        if (Roots < 0) Roots = 0;
        UIManager.Instance?.UpdateRoots(Roots, TotalRoots);
    }

    public void CollectCoin(int amount) => CollectRoot(amount);

    public void EnemyKilled()
    {
        UIManager.Instance?.ShowMessage("Guardião derrotado!", 1.2f);
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
        IsGameOver = false;
        PlayerPrefs.DeleteKey("CheckpointX");
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