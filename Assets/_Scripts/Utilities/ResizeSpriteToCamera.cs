using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeSpriteToCamera : MonoBehaviour
{
    [SerializeField] private Sprite spriteSize;
    [SerializeField] private bool initialSprite;

    void Start()
    {
        Camera cam = Camera.main;
        float depth = transform.position.z;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (!initialSprite) spriteRenderer.sprite = spriteSize;

        float halfFovRadians = cam.fieldOfView * Mathf.Deg2Rad / 2f;
        float visibleHeightAtDepth = depth * Mathf.Tan(halfFovRadians) * 2f;
        float spriteHeight = spriteRenderer.sprite.rect.height / spriteRenderer.sprite.pixelsPerUnit;
        float scaleFactor = visibleHeightAtDepth / spriteHeight;
        spriteRenderer.transform.localScale = Vector3.one * scaleFactor;
    }
}
