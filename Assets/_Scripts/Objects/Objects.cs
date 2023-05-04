using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    private void Start()
    {
        var target = Camera.main.gameObject.transform;
        transform.LookAt(target);

        transform.Rotate(0,0, 40, Space.World);
    }
}
