using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PopupNewLevel : MonoBehaviour
{
    [Header("Configuration")]
    public string text;
    public float fillDuration = 2f;
    public float textFadeDuration = 2f;
    public AudioClip fadeInSound;

    [Header("Local References")]
    [SerializeField] private Image brushImg;
    [SerializeField] private TMP_Text popupText;
    [SerializeField] private AudioSource audioSource;

    private void Awake() => SetValues();

    private void SetValues()
    {
        brushImg.fillAmount = 0;
        popupText.text = text;
        popupText.color = Color.clear;
    }

    private IEnumerator Start()
    {
        StartCoroutine(FillImage());
        audioSource.PlayOneShot(fadeInSound);
        yield return new WaitForSeconds(fillDuration);
        StartCoroutine(FadeInText());
    }

    private IEnumerator FillImage()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fillDuration)
        {
            elapsedTime += Time.deltaTime;
            brushImg.fillAmount = Mathf.Clamp01(elapsedTime / fillDuration);
            yield return null;
        }
        brushImg.fillAmount = 1f;
    }

    private IEnumerator FadeInText()
    {
        popupText.text = text;
        float elapsedTime = 0f;
        while (elapsedTime < textFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / textFadeDuration);
            Color color = new(t, t, t, t);
            popupText.color = color;
            yield return null;
        }
        popupText.color = Color.white;
    }
}
