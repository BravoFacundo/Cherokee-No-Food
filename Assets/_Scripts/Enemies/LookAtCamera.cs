using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Transform target;
    public bool LockY = true;
    
    void Start()
    {
        target = Camera.main.gameObject.transform;
    }    

    void Update()
    {
        //Rotate transform towards the designated target
        transform.LookAt(target);

        //Lock rotation on Y axis if its needed
        if (LockY) transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
}
