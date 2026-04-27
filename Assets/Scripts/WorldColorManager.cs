using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldColorManager : MonoBehaviour
{
    public static WorldColorManager Instance { get; private set; }

    public int totalFragments = 12;  
    public float transitionDuration = 1.2f; 

    private List<SpriteRenderer> colorableSprites = new List<SpriteRenderer>();
    private List<Color> originalColors = new List<Color>();
    private int collectedCount = 0;
    private float currentProgress = 0f;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        GameObject[] colorableObjects = GameObject.FindGameObjectsWithTag("Colorable");

        foreach (var go in colorableObjects)
        {
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                colorableSprites.Add(sr);
                originalColors.Add(sr.color);
                sr.color = Desaturate(sr.color);
            }

            foreach (var childSr in go.GetComponentsInChildren<SpriteRenderer>())
            {
                if (childSr.gameObject == go) continue; 
                colorableSprites.Add(childSr);
                originalColors.Add(childSr.color);
                childSr.color = Desaturate(childSr.color);
            }
        }

        if (colorableSprites.Count == 0)
            Debug.LogWarning("[WorldColorManager] Nenhum objeto com tag 'Colorable' encontrado. " +
                             "Selecione seus tilemaps e decorações e aplique a tag 'Colorable'.");
        else
            Debug.Log($"[WorldColorManager] {colorableSprites.Count} sprites coloráveis encontrados.");
    }

    public void OnFragmentCollected()
    {
        collectedCount++;
        float targetProgress = Mathf.Clamp01((float)collectedCount / totalFragments);
        StartCoroutine(RestoreColor(currentProgress, targetProgress));
        currentProgress = targetProgress;
    }

    IEnumerator RestoreColor(float fromProgress, float toProgress)
    {
        float elapsed = 0f;

        List<Color> fromColors = new List<Color>();
        for (int i = 0; i < colorableSprites.Count; i++)
        {
            if (colorableSprites[i] != null)
                fromColors.Add(colorableSprites[i].color);
            else
                fromColors.Add(Color.white);
        }

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionDuration;

            t = 1f - Mathf.Pow(1f - t, 3f);

            for (int i = 0; i < colorableSprites.Count; i++)
            {
                if (colorableSprites[i] == null) continue;

                Color targetColor = Color.Lerp(
                    Desaturate(originalColors[i]),
                    originalColors[i],
                    toProgress
                );

                colorableSprites[i].color = Color.Lerp(fromColors[i], targetColor, t);
            }

            yield return null;
        }

        for (int i = 0; i < colorableSprites.Count; i++)
        {
            if (colorableSprites[i] == null) continue;
            colorableSprites[i].color = Color.Lerp(
                Desaturate(originalColors[i]),
                originalColors[i],
                toProgress
            );
        }
    }

    Color Desaturate(Color c)
    {
        float gray = c.r * 0.299f + c.g * 0.587f + c.b * 0.114f;

        float r = Mathf.Lerp(c.r, gray, 0.85f) * 0.70f;
        float g = Mathf.Lerp(c.g, gray, 0.85f) * 0.70f;
        float b = Mathf.Lerp(c.b, gray, 0.85f) * 0.75f; 

        return new Color(r, g, b, c.a);
    }
}