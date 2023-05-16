using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterFrameNumber : MonoBehaviour
{
    [SerializeField] private int frames;
    void Start()
    {
        float time = frames / 60;
        Destroy(gameObject, time);
    }
}
