using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private GameObject currentBackground;
    
    [Header("Configuration")]
    [SerializeField] private float transitionDuration;
    [SerializeField] private Transform backgroundParent;

    [Header("Prefabs")]
    [SerializeField] private List<GameObject> backgroundsPrefabs;

    private void Start()
    {
        currentBackground = backgroundParent.GetChild(0).gameObject;
        //ChangeLevelBackground(2);
    }

    public void ChangeLevelBackground(int level)
    {
        level = Mathf.Clamp(level, 1, 4);
        GameObject newScene = Instantiate(backgroundsPrefabs[level], currentBackground.transform.position, Quaternion.identity, backgroundParent);

        SetAlpha(newScene, 0f);
        StartCoroutine(FadeInNewBackground(newScene));
    }
    private void SetAlpha(GameObject newBackground, float alpha)
    {
        SpriteRenderer[] sprites = newBackground.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in sprites)
        {
            Color color = sprite.color;
            color.a = alpha;
            sprite.color = color;
        }
    }
    private IEnumerator FadeInNewBackground(GameObject newBackground)
    {
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / transitionDuration);
            SetAlpha(newBackground, alpha);
            yield return null;
        }
        SetAlpha(newBackground, 1f);

        Destroy(currentBackground);
        currentBackground = newBackground;
    }
}
