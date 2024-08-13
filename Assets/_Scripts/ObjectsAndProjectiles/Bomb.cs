using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float totalTimeToExplode = 5f;
    [SerializeField] private float minBlinkInterval = 0.1f;
    [SerializeField] private float pitchIncreaseAmount = 0.05f;
    public bool canBeStopped = true;

    [SerializeField] private float shieldBlinkInterval = 0.1f;
    [SerializeField] private float shieldMinAlpha = 0.5f;

    private float currentBlinkInterval;
    private float blinkTimer;

    private bool increasingAlpha = true;
    private bool isCountingDown = false;
    private bool hasExploded = false;

    private Color bombColor;
    private Color shieldColor;

    [Header("Debug")]
    [SerializeField] private bool debugStartBomb;
    [SerializeField] private bool debugStopBomb;

    [Header("Local References")]
    [SerializeField] private SpriteRenderer bombOnSpriteRenderer;
    [SerializeField] private SpriteRenderer shieldSpriteRenderer;
    [SerializeField] private AudioSource audioSource;

    [Header("Sound References")]
    [SerializeField] private AudioClip bombBeepSound;
    [SerializeField] private AudioClip bombDefuseSound;

    [Header("References")]
    [SerializeField] private ParticleManager particleManager;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        bombColor = bombOnSpriteRenderer.color;
        shieldColor = shieldSpriteRenderer.color;

        InitializeShield();
        StartBombCount();
    }

    private void InitializeShield()
    {
        if (canBeStopped) shieldSpriteRenderer.enabled = false;
        else
        {
            shieldColor.a = shieldMinAlpha;
            shieldSpriteRenderer.color = shieldColor;
        }
    }

    public void StartBombCount()
    {
        currentBlinkInterval = totalTimeToExplode / 10f;
        blinkTimer = 0f;
        isCountingDown = true;
    }

    public void StopBombCount()
    {
        if (canBeStopped && isCountingDown && !hasExploded)
        {
            isCountingDown = false;
            audioSource.clip = bombDefuseSound;
            audioSource.Play();
            Debug.Log("La bomba ha sido detenida.");
        }
    }

    private void Update()
    {
        if (!isCountingDown || hasExploded) return;

        blinkTimer += Time.deltaTime;

        if (blinkTimer >= currentBlinkInterval)
        {
            HandleBlink();
            HandleSound();
        }

        UpdateLightsAlpha();
        UpdateShieldAlpha();

        totalTimeToExplode -= Time.deltaTime;
        if (totalTimeToExplode <= 0f && !hasExploded) Explode();

        Debug_Bomb();
    }

    private void HandleBlink()
    {
        blinkTimer = 0f;
        increasingAlpha = !increasingAlpha;

        currentBlinkInterval = Mathf.Max(currentBlinkInterval * 0.9f, minBlinkInterval);
    }

    private void HandleSound()
    {
        if (increasingAlpha)
        {
            audioSource.clip = bombBeepSound;
            audioSource.pitch += pitchIncreaseAmount;
            audioSource.Play();
        }
    }

    private void UpdateLightsAlpha()
    {
        float alpha = Mathf.Lerp(0f, 1f, blinkTimer / currentBlinkInterval);
        bombColor.a = increasingAlpha ? alpha : 1f - alpha;
        bombOnSpriteRenderer.color = bombColor;
    }

    private void UpdateShieldAlpha()
    {
        if (!canBeStopped)
        {
            float shieldAlpha = Mathf.PingPong(Time.time / shieldBlinkInterval, 1f - shieldMinAlpha) + shieldMinAlpha;
            shieldColor.a = shieldAlpha;
            shieldSpriteRenderer.color = shieldColor;
        }
    }

    private void Explode()
    {
        hasExploded = true;
        particleManager.ImpactExplosion(transform.position + new Vector3(0, 0, -.2f), transform.rotation);
        Debug.Log("¡Boom! La bomba ha explotado.");
    }

    private void Debug_Bomb()
    {
        if (debugStopBomb)
        {
            StopBombCount();
            debugStopBomb = false;
        }        
    }
}
