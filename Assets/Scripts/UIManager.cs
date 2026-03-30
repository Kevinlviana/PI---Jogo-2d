using UnityEngine;
using TMPro;
using System.Collections;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("HUD")]
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI messageText;

    [Header("Game Over / Win")]
    public TextMeshProUGUI finalCoinText;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        UpdateCoins(0);
        UpdateLives(3);
        if (messageText != null) messageText.text = "";
    }

    public void UpdateCoins(int coins)
    {
        if (coinText != null)
            coinText.text = "Coins: " + coins;

        if (finalCoinText != null)
            finalCoinText.text = "Coins collected: " + coins;
    }

    public void UpdateLives(int lives)
    {
        if (livesText != null)
            livesText.text = "Lives: " + lives;
    }

    public void ShowMessage(string msg, float duration = 1.2f)
    {
        if (messageText == null) return;
        StopAllCoroutines();
        StartCoroutine(ShowMessageCoroutine(msg, duration));
    }

    IEnumerator ShowMessageCoroutine(string msg, float duration)
    {
        messageText.text = msg;
        yield return new WaitForSeconds(duration);
        messageText.text = "";
    }

}
