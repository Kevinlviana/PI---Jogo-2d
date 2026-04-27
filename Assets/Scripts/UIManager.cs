using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public TextMeshProUGUI rootsText;

    public Image[] heartIcons;
    public Sprite heartFull;
    public Sprite heartEmpty;

    public TextMeshProUGUI messageText;
    public TextMeshProUGUI memoryText;

    public TextMeshProUGUI finalRootsText;

    public Image fadePanel;

    private Coroutine messageCoroutine;
    private Coroutine memoryCoroutine;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        UpdateRoots(0, GameManager.Instance != null ? GameManager.Instance.TotalRoots : 12);
        UpdateLives(3);
        if (messageText != null) messageText.text = "";
        if (memoryText != null) { memoryText.text = ""; SetAlpha(memoryText, 0f); }
        if (fadePanel != null) SetImageAlpha(fadePanel, 0f);
    }

    public void UpdateRoots(int current, int total)
    {
        if (rootsText != null) rootsText.text = $"Raízes: {current} / {total}";
        if (finalRootsText != null) finalRootsText.text = $"Raízes coletadas: {current} / {total}";
    }

    public void UpdateLives(int lives)
    {
        if (heartIcons == null) return;
        for (int i = 0; i < heartIcons.Length; i++)
        {
            if (heartIcons[i] == null) continue;
            heartIcons[i].sprite = (i < lives) ? heartFull : heartEmpty;
        }
    }

    public void ShowMessage(string msg, float duration = 1.2f)
    {
        if (messageText == null) return;
        if (messageCoroutine != null) StopCoroutine(messageCoroutine);
        messageCoroutine = StartCoroutine(MessageCoroutine(msg, duration));
    }

    IEnumerator MessageCoroutine(string msg, float duration)
    {
        messageText.text = msg;
        yield return new WaitForSeconds(duration);
        messageText.text = "";
    }

    public void ShowMemoryText(string text, float holdDuration = 2f, float fadeDuration = 0.5f)
    {
        if (memoryText == null) return;
        if (memoryCoroutine != null) StopCoroutine(memoryCoroutine);
        memoryCoroutine = StartCoroutine(MemoryTextCoroutine(text, holdDuration, fadeDuration));
    }

    IEnumerator MemoryTextCoroutine(string text, float holdDuration, float fadeDuration)
    {
        memoryText.text = text;
        yield return StartCoroutine(FadeText(memoryText, 0f, 1f, fadeDuration));
        yield return new WaitForSeconds(holdDuration);
        yield return StartCoroutine(FadeText(memoryText, 1f, 0f, fadeDuration));
        memoryText.text = "";
    }

    IEnumerator FadeText(TextMeshProUGUI tmp, float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            SetAlpha(tmp, Mathf.Lerp(from, to, elapsed / duration));
            yield return null;
        }
        SetAlpha(tmp, to);
    }

    public void FadeIn(float duration = 0.4f) => StartCoroutine(FadePanel(1f, 0f, duration));
    public void FadeOut(float duration = 0.4f) => StartCoroutine(FadePanel(0f, 1f, duration));

    IEnumerator FadePanel(float from, float to, float duration)
    {
        if (fadePanel == null) yield break;
        fadePanel.gameObject.SetActive(true);
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            SetImageAlpha(fadePanel, Mathf.Lerp(from, to, elapsed / duration));
            yield return null;
        }
        SetImageAlpha(fadePanel, to);
        if (to == 0f) fadePanel.gameObject.SetActive(false);
    }

    void SetAlpha(TextMeshProUGUI tmp, float a)
    { Color c = tmp.color; c.a = a; tmp.color = c; }

    void SetImageAlpha(Image img, float a)
    { Color c = img.color; c.a = a; img.color = c; }
}