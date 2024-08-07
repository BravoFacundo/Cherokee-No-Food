using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHanging : MonoBehaviour
{
    public LevelManager levelManager;
    private float uniqueOffset;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();

        uniqueOffset = Random.Range(0.0f, 2.0f * Mathf.PI);
    }

    private void Update()
    {
        float windOffset = levelManager.GetWindOffset();
        transform.localRotation = Quaternion.Euler(0, 0, windOffset + uniqueOffset * 5f);
    }
}
