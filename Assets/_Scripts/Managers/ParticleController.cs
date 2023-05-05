using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [Header("References")]
    private Camera cam;

    [Header("Prefabs")]
    [SerializeField] private GameObject impactExplosion;    

    private void Awake()
    {
        cam = Camera.main;
    }
    
    public void ImpactExplosion(Vector3 spawnPosition, Quaternion spawnRotation)
    {
        GameObject newImpactExplosion = Instantiate(impactExplosion, spawnPosition, spawnRotation);
        newImpactExplosion.GetComponent<LookAtCamera>().target = cam.transform;
    }
    
}
