using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAnimationOnJump : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponentInParent<Enemy>().animator;
    }

    public void StopAnimation()
    {
        if (!GetComponentInParent<Enemy>().isGrounded) animator.speed = 0f;
    }

}
