using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintCol : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print(other.transform.name);
    }
}
