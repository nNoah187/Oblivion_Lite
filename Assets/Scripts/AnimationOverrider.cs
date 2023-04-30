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
        animator = GetComponentInChildren<Animator>();
    }

    public void SetAnimations(AnimatorOverrideController overrideController)
    {
        animator.runtimeAnimatorController = overrideController;
    }
}
