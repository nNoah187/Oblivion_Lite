using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is from youtbe.com/@CodingWithUnity
 */
public class AnimationOverrider : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        if (gameObject.CompareTag("Player"))
        {
            animator = GetComponentInChildren<Animator>();
        }
        else if (gameObject.CompareTag("Enemy"))
        {
            animator = GetComponent<Animator>();
        }
    }

    public void SetAnimations(AnimatorOverrideController overrideController)
    {
        animator.runtimeAnimatorController = overrideController;
    }
}
